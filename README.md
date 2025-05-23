# Debrief

## MrJobs.WebApi
- A bare-minimum ASP.NET Web API
- Hosted on an AzWebApp
- Has Azure auth (via AzAppRego)
- Has a build & deploy pipeline (CI/CD)

## MrJobs.WebJob.DotNet
- A console app which makes a single HTTP GET call to the cloud-hosted Web API
## MrJobs.WebJob.PowerShell
- Exact same functionality as the .NET version
- Unlike the pre-compiled console app (using Azure NuGet packages for Managed Identity), the PowerShell script needs to import the equivalent (Az.Accounts module) but this isn't always guarenteed to be available in the Web Job's runtime
- We _COULD_ save the module locally, including it in the package we deploy, but **speed** and **reliability** is everything, especially during incidents
- So use an API key instead (showcasing injecting secrets via CI/CD pipelines)
## Internal CRON job (within Web API)
- TODO: kinda shitty but here anyway