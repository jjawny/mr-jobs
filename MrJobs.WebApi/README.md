# How to run?
1. `cp appsettings.json appsettings.Development.json` (safely gitignored)
2. Create an AzAppRego (for auth)
   - No platform needed to start with
   - app rego > Manage > API permissions > grant MS Graph User.Read
3. Create a scope via app rego > Manage > Expose an API > create a new scope for admins and users called "API.Access"
4. Populate fields:
   - `{{AUTH_CLIENT_ID}}` app rego > Overview > copy client ID
   - `{{AUTH_TENANT_ID}}` app rego > Overview > copy tenant ID
   - `{{AUTH_AUTHORITY}}` app rego > Overview > Endpoints > copy authority URL (the one with the tenant ID usually)
   - `{{AUTH_AUDIENCE}}` app rego > Manage > Expose an API > copy Application ID URI
   - `{{SYSTEM_API_KEY}}` you decide! or just use `openssl rand -base64 32`
5. `dotnet run` or use C# Dev Kit

## Use the [.http file](./MrJobs.WebApi.http)
1. Login to Azure `az login --tenant <APP REGO TENANT ID> --scope <API.Access SCOPE>`
2. Obtain a JWT `az account get-access-token --resource <APP REGO CLIENT ID> --scope <API.Access SCOPE>`
3. Variables:
   - **@JWT** use the Azure JWT
   - **@SystemApiKey** use your custom API key
