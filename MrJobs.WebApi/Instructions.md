# 🏠 How to run locally?
1. Create an AzAppRego (for OAuth)  
   No platform needed to start with (add redirect + SPA settings later when turning into a fullstack app)
2. Add perms for API to read user profiles: AzAppRego > API permissions > Add a permission > MS Graph > 'profile' and 'User.Read'
3. Create a scope: AzAppRego > Expose an API > Add a scope for admins and users called "API.Access"
4. `cp appsettings.json appsettings.Development.json` (safely gitignored)
5. Populate fields:
   - `{{AUTH_CLIENT_ID}}` AzAppRego > Overview > copy client ID
   - `{{AUTH_TENANT_ID}}` AzAppRego > Overview > copy tenant ID
   - `{{AUTH_AUTHORITY}}` AzAppRego > Overview > Endpoints > copy authority URL (usually the URL w the tenant ID to restrict access for those within the enterprise)
   - `{{AUTH_AUDIENCE}}` app rego > Manage > Expose an API > copy Application ID URI
   - `{{SYSTEM_API_KEY}}` you decide! or just use `openssl rand -base64 32`
6. `dotnet run`

# 🛩️ How to test?
Use the [.http file](./MrJobs.WebApi.http)
1. Login `az login --tenant <AzAppRego tenant ID> --scope <"API.Access" scope>`
2. Obtain a JWT `az account get-access-token --resource <AzAppRego client ID> --scope <"API.Access" scope>`
3. Variables:
   - **@JWT** paste the Azure JWT
   - **@SystemApiKey** paste your custom API key
