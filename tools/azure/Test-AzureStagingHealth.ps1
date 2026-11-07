# SUWÓ GYM — comprobaciones HTTPS en staging; no usa contraseñas.
[CmdletBinding()]
param(
  [Parameter(Mandatory)][string]$WebApp,
  [Parameter(Mandatory)][string]$ApiApp
)
$ErrorActionPreference = 'Stop'
foreach ($nombre in @($WebApp,$ApiApp)) {
  if ($nombre -notmatch '^[a-zA-Z0-9-]+$' -or $nombre -notmatch 'stg') {
    throw 'Solo se permiten nombres de aplicaciones Staging (stg).'
  }
}
$fallos = 0
foreach ($nombre in @($ApiApp,$WebApp)) {
  foreach ($tipo in @('live','ready')) {
    $url = "https://$nombre.azurewebsites.net/health/$tipo"
    try {
      $resultado = Invoke-WebRequest -Uri $url -UseBasicParsing -TimeoutSec 15
      $exito = ($resultado.StatusCode -eq 200 -and $resultado.Content.Trim() -eq 'Healthy')
      if ($exito) {
        Write-Host "[OK] $nombre /health/$tipo"
      } else {
        Write-Warning "[FALLO] $nombre /health/$tipo (respuesta inesperada)"
        $fallos++
      }
    }
    catch {
      Write-Warning "[FALLO] $nombre /health/$tipo (sin respuesta saludable)"
      $fallos++
    }
  }
}
if ($fallos -gt 0) { exit 1 }
Write-Host 'Todos los endpoints de salud de Staging están correctos.'
