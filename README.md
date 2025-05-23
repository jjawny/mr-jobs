# Debrief

## MrJobs.WebApi
A bare-minimum ASP.NET Web API, hosted on an AzWebApp, using Azure auth (via AzAppRego), has a build & deploy pipeline (CI/CD)

## MrJobs.WebJob.DotNet
A console app that makes a single HTTP GET call to the cloud-hosted Web API once deployed as a AzWebJob, via its own manually triggered pipeline
 
## MrJobs.WebJob.PowerShell
This one is purely a PowerShell example using the custom API key, (I just assume this will work), no CI/CD pipeline has been setup, see the [.NET Web Job](./MrJobs.WebJob.DotNet/) for a live one

## Internal CRON job (within Web API)
Kinda an anti-pattern, when devs run locally this auto-runs and could affect shared dev/staging data without users knowing it, also unpredictable, better to stick with the usually HTTP endpoints
