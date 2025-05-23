# Run
1. Auth locally: `az login --scope <WEB_API_SCOPE>`
3. `cp appsettings.json appsettings.Development.json` (safely gitignored)
4. Populate fields:
   - `{{WEB_API_HOST}}` the Web API to ping (localhost or see AzWebApp)
   - `{{WEB_API_APP_REGO_BASE_SCOPE}}` the AzAppRego's base scope: app rego > Expose an API > copy Application ID URI

# For Managed Identity to work
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