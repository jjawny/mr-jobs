# How to run?
The integration tests require [appsettings.Testing.json](../MrJobs.WebApi/appsettings.Testing.json) to be populated w all the same auth settings from appsettings.json + an additional Client Secret. Why? Because we can't test Managed Identity outside of Azure or test interactive user flow w/o a user, so we will fallback to testing w a Service Principal. This is called 'Client Credentials flow' and our Service Principal will be the AzAppRego itself. Setup:

1. From AzAppRego:
- Copy the client ID
- Click on Expose an API > Add a client application
- Assign (paste) the client ID for the "API.Access" scope

  🏁 Now the SP has permission to call the API.

2. From AzAppRego
- Click on API permissions > Add a permission > My APIs
- Choose the same AzAppRego
- Assign it to an app role (the same role as the web job is fine)

  🏁 Now the Service Principal has the same RBAC claims as the web job.


3. From AzAppRego
- Click on Certificates & secrets
- Create a new client secret and copy it into appsettings.Testing.json

  🏁 This is the "password" the Service Principal
