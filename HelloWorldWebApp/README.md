# HelloWorldWebApp

Minimal ASP.NET Core site that returns a styled "Hello World" HTML page plus a `/health` endpoint. Use it to verify Azure App Service deployments before publishing the main Blazor WebApp.

## Run locally

```powershell
cd HelloWorldWebApp
dotnet run
```

Visit `http://localhost:5000` for the UI or `http://localhost:5000/health` for JSON health output.

## Deploy to Azure App Service

1. Publish the project:
   ```powershell
   dotnet publish HelloWorldWebApp/HelloWorldWebApp.csproj -c Release -o publish
   ```
2. Deploy the `publish` folder to the target App Service (zip deploy, FTP, or Azure DevOps).
3. Browse to the site URL; you should see the "🚀 CRC Hello World" page.
4. After confirming the site works, swap in the main Blazor WebApp using the same deployment pipeline.

