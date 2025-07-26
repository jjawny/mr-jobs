# 🏠 How to run locally?
1. Auth locally: `az login --scope <WEB_API_BASE_SCOPE like api://client ID/.default> --tenant <AZ APP REGO TENANT ID>`
2. Populate [appsettings.json](./appsettings.json):
   - `{{WEB_API_ROUTE}}` the Web API route to ping (e.g. `/poke`) either https://localhost or see AzWebApp's domain
   - `{{WEB_API_BASE_SCOPE}}` the AzAppRego's base scope: app rego > Expose an API > copy Application ID URI
4. `dotnet run`

# ☁️ Managed Identity setup
1. Turn on Managed Identity for the AzWebApp (which has web jobs): web app > Settings > System assigned ON > copy the object ID for later
2. Get the object ID for the app rego: app rego > enterprise app > copy object ID for later
3. Create a new role in the app rego: app rego > Manage > App roles > create an application app role "WebJobs" > copy the role ID for later
4. Using the 3 copied values, assign the Managed Identity to the role
   ```bash
   az rest --method POST \
   --url "https://graph.microsoft.com/v1.0/servicePrincipals/<OBJECT ID of the WEB APP's system assigned MANAGED IDENTITY>/appRoleAssignments" \
   --body "{
      \"principalId\": \"<OBJECT ID of the WEB APP's system assigned MANAGED IDENTITY>\",
      \"resourceId\": \"<OBJECT ID of the APP REGO's ENTERPRISE APP>\",
      \"appRoleId\": \"<ROLE ID of the APP REGO's role>\"
   }"
   ```
   Confirm with
   ```bash
   az rest --method GET \
      --url "https://graph.microsoft.com/v1.0/servicePrincipals/<OBJECT ID of the WEB APP's system assigned MANAGED IDENTITY>/appRoleAssignments"
   ```
5. Configure [settings.job](./settings.job) (invoke manually or on a CRON timer)
6. Deploy
