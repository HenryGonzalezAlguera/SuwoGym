"""Verificación estática sin dependencias del workflow Azure manual.

No ejecuta Azure ni despliega; pretende detectar eliminaciones accidentales
de las barreras de publicación en revisiones posteriores.
"""
from pathlib import Path

ruta = Path(__file__).resolve().parents[2] / ".github/workflows/azure-staging-manual.yml"
script = ruta.read_text(encoding="utf-8")
checks = {
    "Solo despacho manual": "\n  workflow_dispatch:" in script,
    "Sin despliegue por push": "\n  push:" not in script,
    "Rama main y confirmación escrita": "github.ref == 'refs/heads/main' && inputs.confirm == 'DEPLOY_STAGING'" in script,
    "Entorno restringido staging": "\n    environment: staging" in script,
    "Autenticación OIDC": "uses: azure/login@v2" in script and "id-token: write" in script,
    "No utiliza publish profiles": "publish-profile:" not in script,
    "No crea recursos": "az group create" not in script and "az webapp create" not in script,
    "No ejecuta SQL": "sqlpackage" not in script.lower(),
    "Comprobación de salud": "/health/$endpoint" in script,
    "Doble destino separado": "AZURE_WEBAPP_WEB" in script and "AZURE_WEBAPP_API" in script,
    "Grupo existente limitado": '"rg-stockflow-demo"' in script,
    "Plan Linux F1 concreto": '"asp-stockflow-free"' in script and '[[ "$tier" == "F1" && "$linux" == "true" ]]' in script,
    "Sólo rutas SUWÓ": 'suwogym-stg-' in script and '"$app" != *stockflow*' in script,
    "Destinos exactos SUWÓ": '[[ "$AZURE_WEBAPP_API" == "suwogym-stg-api-hga" ]]' in script and '[[ "$AZURE_WEBAPP_WEB" == "suwogym-stg-web-hga" ]]' in script,
    "Resuelve dominios regionales dinámicos": "defaultHostName" in script and '"https://${host}/health/${endpoint}"' in script,
    "Verifica plan de ambas apps": 'serverFarmId' in script,
    "Variable del plan compartido": 'vars.AZURE_APP_SERVICE_PLAN' in script,
}
for nombre, correcto in checks.items():
    if not correcto:
        raise SystemExit("FALLO: " + nombre)
    print("[OK] " + nombre)

# La comprobación previa nunca debe sustituir un despliegue manual ni publicar recursos.
preflight = (ruta.parent / "azure-staging-preflight.yml").read_text(encoding="utf-8")
prechecks = {
    "Preflight solo manual": "\\n  workflow_dispatch:" in preflight and "\\n  push:" not in preflight,
    "Preflight main y confirmación": "github.ref == 'refs/heads/main' && inputs.confirm == 'CHECK_STAGING'" in preflight,
    "Preflight OIDC y staging": "uses: azure/login@v2" in preflight and "environment: staging" in preflight,
    "Preflight restringe API y WEB": '[[ "$AZURE_WEBAPP_API" == "suwogym-stg-api-hga" ]]' in preflight and '[[ "$AZURE_WEBAPP_WEB" == "suwogym-stg-web-hga" ]]' in preflight,
    "Preflight solo lectura": all(forbidden not in preflight for forbidden in (
        "azure/webapps-deploy", "az webapp deploy", "az webapp create", "az webapp delete",
        "az appservice plan update", "az role assignment create", "sqlpackage",
        "az webapp config appsettings list",
    )),
}
for nombre, correcto in prechecks.items():
    if not correcto:
        raise SystemExit("FALLO: " + nombre)
    print("[OK] " + nombre)
