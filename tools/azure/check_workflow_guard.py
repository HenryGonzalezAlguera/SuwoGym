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
}
for nombre, correcto in checks.items():
    if not correcto:
        raise SystemExit("FALLO: " + nombre)
    print("[OK] " + nombre)
