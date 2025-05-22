$systemApiKey = "{{SystemRoutes:ApiKey}}"
$systemApiKeyHeader = "x-system-api-key"
$timeoutSec = 30
$maxRetries = 3
$retryCount = 0

while ($retryCount -lt $maxRetries) {
  try {
    $response = Invoke-RestMethod -Uri "https://{{SystemRoutes:Host}}/access-via-custom-api-key"
                                  -Method Get
                                  -Headers @{ $systemApiKeyHeader = $systemApiKey }
                                  -TimeoutSec $timeoutSec

    Write-Output: "Success, API response: $response"
  } catch {
    Write-Host "Error occurred: $_"
    Start-Sleep -Seconds 5
    $retryCount++
  }
}
