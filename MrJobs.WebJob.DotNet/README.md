# Use
1. Auth locally: `az login --scope <app rego scope like API.Access OR User.Read TODO: clean up>`
3. `cp appsettings.json appsettings.Development.json` (safely gitignored)
4. Populate fields:
   - `{{Api:Host}}` the Web API to ping (see Azure Web App)
   - `{{Api:Scope}}` the access token's scope (see Azure App Rego)
