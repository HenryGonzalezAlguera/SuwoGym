# SUWÓ GYM — lectura SOLAMENTE. No crea, modifica ni elimina recursos.
# Requiere Azure CLI y sesión az login en una cuenta con permisos de lectura.
[CmdletBinding()]
param(
  [Parameter(Mandatory)][string]$ResourceGroup,
  [Parameter(Mandatory)][string]$WebApp,
  [Parameter(Mandatory)][string]$ApiApp,
  [Parameter(Mandatory)][string]$SqlServer,
  [Parameter(Mandatory)][string]$SqlDatabase
)
$ErrorActionPreference = 'Stop'
foreach ($item in @($ResourceGroup, $WebApp, $ApiApp, $SqlServer)) {
  if ($item -notmatch '^[a-zA-Z0-9-]+$' -or $item -notmatch 'stg') {
    throw 'BLOQUEADO: todos los recursos deben identificarse como Staging (stg).'
  }
}
if ($SqlDatabase -ne 'SuwoGym_Staging') {
  throw 'BLOQUEADO: solo se inspecciona la base SuwoGym_Staging.'
}
if ($WebApp -eq $ApiApp) { throw 'WEB y API no pueden ser el mismo App Service.' }
if (-not (Get-Command az -ErrorAction SilentlyContinue)) {
  throw 'No se encontró Azure CLI. Instalarlo e iniciar az login.'
}
& az account show --query id -o tsv --only-show-errors 1>$null
if ($LASTEXITCODE -ne 0) { throw 'Azure CLI no tiene una sesión iniciada.' }

function ConsultaAzure([string[]]$Argumentos) {
  $salida = & az @Argumentos --only-show-errors 2>$null
  if ($LASTEXITCODE -ne 0) { throw 'No se pudo comprobar el recurso. Revisar permisos, nombre y suscripción.' }
  return $salida
}
$null = ConsultaAzure -Argumentos @('group','show','-n',$ResourceGroup,'--query','name','-o','tsv')
$web = ConsultaAzure -Argumentos @('webapp','show','-g',$ResourceGroup,'-n',$WebApp,'--query','serverFarmId','-o','tsv')
$api = ConsultaAzure -Argumentos @('webapp','show','-g',$ResourceGroup,'-n',$ApiApp,'--query','serverFarmId','-o','tsv')
if ($web -ne $api) {
  throw 'WEB y API deben compartir un plan App Service en la primera prueba.'
}
$plan = ConsultaAzure -Argumentos @('appservice','plan','show','--ids',$web,'--query','sku.name','-o','tsv')
$null = ConsultaAzure -Argumentos @('sql','server','show','-g',$ResourceGroup,'-n',$SqlServer,'--query','name','-o','tsv')
$sql = ConsultaAzure -Argumentos @('sql','db','show','-g',$ResourceGroup,'-s',$SqlServer,'-n',$SqlDatabase,'--query','status','-o','tsv')
Write-Host 'PRECHECK OK: recursos Staging identificados, sin cambios.'
Write-Host "Plan App Service: $plan; SQL: $sql"
Write-Host 'Pendiente revisar en Portal: secretos, certificados, firewall y aprobación de presupuesto.'
