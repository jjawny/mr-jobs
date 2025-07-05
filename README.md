# Debrief

🏃💨 Speedrunning CRON jobs x Web APIs via Azure infra

TODO: Log the best time here
TODO: add in pacing stuff (server-side only in .NET) asp.net rate limiting, do we even do debouncing here? etc throttling n queing or is this a separate speedrun (or maybe just demo about scalability w a worker running concurrently)
TODO: Detail the speedrun steps w individual times? something like
1. Provision infra
2. Add auth
3. Az RBAC roles (app role x entra group) + route
4. test managed identity locally
5. pipelines
6. web job etc

## 🌐 MrJobs.WebApi
- A bare-minimum ASP.NET Web API:
- Hosted via an AzWebApp (on an AzAppServicePlan)
- Using build n deploy piplines (CI/CD) auto-triggered
- Has 2 types of auth protection for routes:
  - MSAL via AzAppRego (for users n Az services)
  - Custom API key (backup strat)

## 🎮 MrJobs.WebJob.DotNet
- A console app
- Obtains a JWT access token from the AzAppRego via Managed Identity
- Makes a single HTTP request to the Web API authing w the JWT
- Has CI/CD pipelines (manually-triggered)
- View Kudu logs after deploying to confirm success
- TODO: Instructions to test Managed Identity locally via Az CLI 

## 🐚 MrJobs.WebJob.PowerShell
- A PowerShell example (alt strat of console app)
- Makes a single HTTP request to the Web API authing w the custom API key
- No CI/CD pipelines have been setup for this guy
- See the [MrJobs.WebJob.DotNet](./MrJobs.WebJob.DotNet/) for a live example

## 🐧 TODO: MrJobs.WebJob.Shell (example for linux/unix systems)

## ⚙️ Internal (self-hosted) BG task
- ❌ Kinda an anti-pattern...
- ❌ When devs run locally, these internal jobs will auto-run, possibly mutating shared dev/staging data unintentionally
- ❌ Adds complexity a dev's mental model of the app
- ✅ Using ext web jobs means we can say that CRON jobs are just another vanilla HTTP route!
- ✅ Using ext web jobs means we can control the job (change the timer/pause/manually invoke the biz logic) all w/o touching the main app

## ☁️ TODO: Instructions on using Az Bicep to provision infra using code

## 💡 TODO: Add Az app insights endpoint to demo types of telemetry as related to Az speedrun
