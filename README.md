# Mr Jobs 🏃🏻💨

Speedrunning CRON jobs x Web APIs hosted on Azure infra

## 🌐 MrJobs.WebApi
- A bare-minimum ASP.NET Web API
- Hosted on an AzWebApp (on an AzAppServicePlan)
- Built n deployed via a CI/CD pipline
- Uses 2 types of auth:
  1. MSAL via AzAppRego (for users n Az service principals)
  2. Custom API key (for the wider-system(s))

## 🌐🧪 MrJobs.WebApi.Tests
- Integration tests for both auth strats
- E2E from Azure (fetch JWT) to in-mem ASP.NET Web API framework (use JWT to pass auth middlewares/protected routes)
- Testing MSAL auth using a Client Credentials Flow (client secret)

## 🎮 MrJobs.WebJob.DotNet
- A console app
- Obtains a JWT access token from the AzAppRego via Managed Identity
- Makes a single HTTP request to the Web API authing w the JWT
- Has CI/CD pipelines (manually-triggered)
- View Kudu logs after deploying to confirm success

## 🐚 MrJobs.WebJob.PowerShell
- A PowerShell example (alt strat of console app)
- Makes a single HTTP request to the Web API authing w the custom API key
- No CI/CD pipelines have been setup for this guy
- See the [MrJobs.WebJob.DotNet](./MrJobs.WebJob.DotNet/) for a live example

## ⚙️ Internal (self-hosted) job
- ❌ Kinda an anti-pattern...
- ❌ When devs run locally, these internal jobs will auto-run, possibly mutating shared dev/staging data unintentionally
- ❌ Adds complexity a dev's mental model of the app
- ✅ Using ext web jobs means we can say that CRON jobs are just another vanilla HTTP route!
- ✅ Using ext web jobs means we can control the job (change the timer/pause/manually invoke the biz logic) all w/o touching the main app

## ☁️ IaC
TODO: add Azure bicep instructions
