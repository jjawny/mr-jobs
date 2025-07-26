# Before you deploy
## ⚙️ GH Actions Variables
### Repository-wide
- `APPROVERS` the GH usernames comma-separated
- `AZURE_RESOURCE_GROUP` the RG's name w the AzWebApp
- `AZURE_WEBAPP` the AzWebApp's name
- `AZURE_WEBAPP_RUNTIME_STACK` the runtime stack (e.g. "dotnet:9")  
  see available using `az webapp list-runtimes --os-type windows`
- `BUILD_DOTNET_VERSION` the .NET version to build with (e.g. "9.x")
- `WEB_API_PROJECT_PATH` the path to the web api (e.g., "./MrJobs.WebApi/MrJobs.WebApi.csproj")
- `WEB_JOB_PROJECT_PATH` the path to the web job (e.g., "./MrJobs.WebJob.DotNet/MrJobs.WebJob.DotNet.csproj")

## 🤫 GH Actions Secrets
### Repository-wide
- `AZUREAPPSERVICE_CLIENTID...`, `AZUREAPPSERVICE_SUBSCRIPTIONID...`, `AZUREAPPSERVICE_TENANTID...` added after linking AzWebApp (AzWebApp > Deployment > Deployment Center) to GH repo

### Production Environment
For Web API's Azure OAuth:
- `AUTH_AUTHORITY` the AzAppRego > Overview > Endpoints > Authority URL
- `AUTH_AUDIENCE` the AzAppRego > Expose an API > Application ID URI (e.g. "api://client ID")
- `AUTH_TENANT_ID` the AzAppRego tenant ID
- `AUTH_CLIENT_ID` the AzAppRego client ID

For custom API key access:
- `SYSTEM_API_KEY` the API key for system(s) to use

For integration tests:
- TODO:

For web job:
- `WEB_API_ROUTE` the fully-qualified endpoint to trigger
