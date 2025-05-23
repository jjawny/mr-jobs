# Debrief

## MrJobs.WebApi
A bare-minimum ASP.NET Web API, hosted in an AzWebApp, using azure auth via AzAppRego, has a build & deploy pipeline (CI/CD), has 2 core routes (one protected by azure auth, one protected by a custom API key)

## MrJobs.WebJob.DotNet
A console app that obtains a JWT (access token) from the AzAppRego via Managed Identity, then makes a single HTTP request to the the Web API's azure auth route using the JWT, has a build & deploy pipeline (must be triggered manually)
 
## MrJobs.WebJob.PowerShell
A PowerShell example of an AzWebJob using the custom API key, no CI/CD pipeline has been setup, see the [MrJobs.WebJob.DotNet](./MrJobs.WebJob.DotNet/) for a live example

## Internal CRON jobs (within Web API)
Kinda an anti-pattern, when devs run locally, these internal jobs will auto-run; possibly affecting shared dev/staging data unintentionally, so it's unpredictable and also harder to maintain, adds complexity to the app mental model, there's huge gains just having the job's business logic be another HTTP route that anyone can hit (users, Windows Task Schedular, AzWebJob, ...)
