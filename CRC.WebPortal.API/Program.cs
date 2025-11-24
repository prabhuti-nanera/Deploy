using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using CRC.WebPortal.Application;
using CRC.WebPortal.Infrastructure;
using CRC.WebPortal.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp",
        policy => policy
            .WithOrigins(
                "http://localhost:5000", "https://localhost:5000",
                "http://localhost:5105", "https://localhost:5105",
                "http://localhost:5201", "https://localhost:5201",
                "http://localhost:5202", "https://localhost:5202",
                "http://localhost:5301", "https://localhost:5301",
                "http://localhost:5205", "https://localhost:5205",
                "https://localhost:7205",
                "http://localhost:8082", "https://localhost:8082",
                "http://localhost:8083", "https://localhost:8083",
                "http://localhost:8084", "https://localhost:8084",
                "http://127.0.0.1:57525", "http://127.0.0.1:57526",
                "https://crc-system-webapp.azurewebsites.net"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// Add controllers and API explorer for Swagger
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CRC WebPortal API", Version = "v1" });
    
    // Handle circular references and complex types
    c.SchemaGeneratorOptions.SchemaIdSelector = type => type.FullName?.Replace("+", ".");
    c.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
    
    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// Add Application Layer
builder.Services.AddApplication();

// Add Infrastructure Layer
builder.Services.AddInfrastructure(configuration);

// Add DataSeeder as a service
builder.Services.AddScoped<DataSeeder>();

// Configure JWT Authentication
var jwtSettings = configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
{
    throw new InvalidOperationException("JWT configuration is missing required values.");
}

var key = Encoding.ASCII.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            // Don't challenge on preflight requests
            if (context.Request.Method == "OPTIONS")
            {
                context.HandleResponse();
                context.Response.StatusCode = 200;
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }
    };
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// CORS must be the first middleware to handle preflight requests properly
app.UseCors("AllowBlazorApp");

// Only redirect to HTTPS in production to avoid CORS preflight issues in development
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRC WebPortal API v1"));
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Health check endpoint
app.MapGet("/", () => "CRC WebPortal API is running!");

app.MapGet("/db-check", async (IServiceProvider sp) =>
{
    await using var scope = sp.CreateAsyncScope();
    var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var canConnect = await ctx.Database.CanConnectAsync();
    var conn = ctx.Database.GetDbConnection();
    return new
    {
        CanConnect = canConnect,
        Database = conn.Database,
        DataSource = conn.DataSource,
        Schema = "crc_webportal"
    };
});


app.MapControllers();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
        
        // Seed the database
        var seeder = services.GetRequiredService<DataSeeder>();
        await seeder.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database. Continuing without seeding...");
        // Continue running the API even if seeding fails
    }
}

await app.RunAsync();
