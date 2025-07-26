# 🏠 How to run locally?

The integration tests require [appsettings.Tests.json](../MrJobs.WebApi/appsettings.Tests.json), `cp appsettings.Tests.Example.json appsettings.Tests.json`

Populate w all the same auth settings from appsettings.json + an additional Client Secret. Why? Because we can't test Managed Identity outside of Azure or test interactive user flow w/o a user, so we will fallback to testing a Service Principal. This is called 'Client Credentials flow' where our Service Principal will be the AzAppRego itself and the password is the secret.

### Setup AzAppRego to auth against itself:
- Click on Expose an API > Add a client application
- Paste the AzAppRego's client ID and assign to the "API.Access" scope
- 🏁 Now the SP has permission to call the API
- Click on API permissions > Add a permission > My APIs
- Choose the same AzAppRego
- Assign it to an app role (the same role as the web job is fine)
- 🏁 Now the Service Principal has the same RBAC claims as the web job
- Click on Certificates & secrets
- Create a new client secret and copy it into appsettings.Tests.json
- 🏁 This is the "password" the Service Principal
