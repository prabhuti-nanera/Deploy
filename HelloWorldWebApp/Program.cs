using System.Net;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Simple health endpoint for Azure monitoring
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }));

// Primary endpoint served at /api/hello. In Azure/IIS the root request is
// rewritten to this route by web.config so we can verify the rewrite logic.
const string HtmlTemplate = """
    <!DOCTYPE html>
    <html lang="en">
    <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <title>Hello from CRC</title>
        <style>
            body {
                font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif;
                background: #0f172a;
                color: #f8fafc;
                margin: 0;
                display: flex;
                align-items: center;
                justify-content: center;
                min-height: 100vh;
            }
            .card {
                background: rgba(15, 23, 42, 0.85);
                border: 1px solid rgba(148, 163, 184, 0.3);
                border-radius: 16px;
                padding: 32px 48px;
                text-align: center;
                box-shadow: 0 20px 30px rgba(15, 23, 42, 0.5);
            }
            h1 {
                margin-bottom: 8px;
                font-size: 2.5rem;
            }
            p {
                margin: 0;
                opacity: 0.8;
            }
        </style>
    </head>
    <body>
        <main class="card">
            <h1> CRC Hello World</h1>
            <p>Your Azure deployment pipeline is working!</p>
            <p><strong>Environment:</strong> {{ENVIRONMENT}}</p>
            <p><strong>Message from web.config:</strong> {{MESSAGE}}</p>
        </main>
    </body>
    </html>
    """;
app.MapGet("/api/hello", () =>
{
    var message = Environment.GetEnvironmentVariable("HELLOWORLD_MESSAGE")
                  ?? "Welcome to the local developer build.";

    var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                  ?? "Unknown";

    var html = HtmlTemplate
        .Replace("{{ENVIRONMENT}}", WebUtility.HtmlEncode(envName))
        .Replace("{{MESSAGE}}", WebUtility.HtmlEncode(message));

    return Results.Content(html, "text/html");
});

// Prevent browser console noise for missing resources in simple demo host
app.MapGet("/favicon.ico", () => Results.StatusCode(StatusCodes.Status204NoContent));

app.MapGet(
    "/.well-known/appspecific/com.chrome.devtools.json",
    () => Results.Content("{}", "application/json"));

app.MapGet(
    "/_framework/aspnetcore-browser-refresh.js",
    () => Results.Content("console.info('Browser refresh disabled.');", "application/javascript"));

app.Run();

