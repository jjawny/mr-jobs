# Debrief

## MrJobs.WebApi
- A bare-minimum ASP.NET Web API with one route, hosted in Azure
## MrJobs.WebJob.DotNet
- A console app which makes a single HTTP GET call to the cloud-hosted Web API
## MrJobs.WebJob.PowerShell
- Exact same functionality as the .NET version
- Unlike the pre-compiled console app (using Azure NuGet packages for Managed Identity), the PowerShell script needs to import the equivalent (Az.Accounts module) but this isn't always guarenteed to be available in the Web Job's runtime
- We _COULD_ save the module locally, including it in the package we deploy, but **speed** and **reliability** is everything, especially during incidents
- So use an API key instead (showcasing injecting secrets via CI/CD pipelines)
## CI/CD
- All of the above are deployed via CI/CD build/release pipelines, auto-triggered on pushes to main, injecting secrets, approvals gates, etc

## Internal CRON job version
- TODO: kinda shitty but here anyway