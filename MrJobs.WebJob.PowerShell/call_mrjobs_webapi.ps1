$systemApiKey = "{{SYSTEM_API_KEY}}"
$headers = @{}
$headers["x-system-api-key"] = $systemApiKey
$headers["Accept"] = "application/json"
$timeoutSec = 30
$maxRetries = 3
$retryCount = 0

while ($retryCount -lt $maxRetries) {
  try {
    $response = Invoke-RestMethod -Uri "http://localhost:5138/poke/using-api-key" `
                                  -Method Get `
                                  -Headers $headers `
                                  -TimeoutSec $timeoutSec

    Write-Host "Success:"
    $response | ConvertTo-Json -Depth 5
    break
  } catch {
    Write-Host "Error: $_"
    Start-Sleep -Seconds 5
    $retryCount++
  }
}
