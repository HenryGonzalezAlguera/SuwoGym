> **ACTUALIZACIÓN 8C — NOVIEMBRE 2026:** El usuario tiene un plan
> **Linux F1** existente (`asp-stockflow-free` en `rg-stockflow-demo`,
> región Central US). La guía Windows/B1 siguiente queda como propuesta
> histórica y **no debe seguirse** para la publicación gratuita.
> Utilizar [FASE8C_AZURE_LINUX_F1.md](FASE8C_AZURE_LINUX_F1.md)
> antes de crear recursos. En particular, usar `/home/suwo-keys`,
> no una ruta `D:\home\...`, y el grupo y plan ya existentes.

# Fase 8B — Azure Staging: despliegue manual

## Alcance
Se preparan procedimientos para publicar SUWÓ GYM (API + WEB .NET 10)
en Microsoft Azure App Service **Windows**, usando **Azure SQL Database**
nueva y solo datos ficticios. **No se han provisionado recursos**.
El workflow solo se ejecuta manualmente desde main con confirmación
DEPLOY_STAGING; nunca con push o pull_request. No crea recursos ni DB.

## Costos y decisión de plataforma

| Recurso | Opción inicial | Costo orientativo |
|---|---|---|
| Plan Windows App Service | Un solo plan B1; una instancia | USD 54,75/mes (lista referencial; verificar región y oferta) |
| WEB .NET 10 | App Service separado en ese mismo plan | Comparte costo del plan |
| API .NET 10 | Segundo App Service en ese plan | Comparte costo del plan |
| Azure SQL | Nueva SuwoGym_Staging; modalidad gratuita si es elegible | Gratis dentro de cuota; cargos según opción de exceso |
| HTTPS | Subdominios azurewebsites.net | No requiere comprar dominio inicialmente |

Azure SQL Free contempla 100.000 vCore-segundos, 32 GB de datos y
32 GB de backup por mes; hasta 10 bases por suscripción. Configurar
**Auto-pause until next month** si se agota la cuota para evitar
cobros no deseados. Al agotarse queda sin servicio hasta el
próximo mes, no dispone de SLA y las conexiones/health checks pueden
impedir el auto-pause. Una base free debe ser **nueva**, no restaurada.
Una instancia B1 se factura por el plan incluso si hay dos apps
compartiéndola. F1 gratuita también existe para pequeñas pruebas,
pero tiene cuotas y no Always On. No es una opción para disponibilidad
continua. Egreso, registros, almacenamiento extra, región y otros
servicios pueden sumar costos.

Cotizar en https://azure.microsoft.com/en-us/pricing/calculator/
Documentación:
https://azure.microsoft.com/en-us/pricing/details/app-service/windows/
https://learn.microsoft.com/en-us/azure/azure-sql/database/free-tier-faq

## Preparación de recursos (manual, con aprobación de costo)

En https://portal.azure.com:
1. Revisar suscripción, límites, créditos y crear presupuesto/alertas
   ANTES de contratar recursos.
2. Crear grupo ejemplo **rg-suwogym-stg**, plan Windows App Service
   B1 (1 instancia), .NET 10, HTTPS Only, TLS mínimo 1.2, Always On.
3. Crear en el MISMO plan dos apps con nombres globalmente únicos,
   por ejemplo **suwogym-stg-web-UNICO** y **suwogym-stg-api-UNICO**.
   Los dos nombres y el grupo deben contener **stg**.
4. Crear servidor lógico Azure SQL de staging (por ejemplo
   sql-suwogym-stg-UNICO.database.windows.net) y base NUEVA llamada
   exactamente **SuwoGym_Staging**. Elegir tier y límite de gasto
   conscientemente; no conectar a SuwoGym ni SuwoGym_Test_7B.
5. Configurar acceso SQL de mínimos permisos: no usar sa/db_owner para
   la API. Restringir firewall a redes de administración y salidas
   necesarias de App Service o usar conectividad privada si procede.
   No habilitar acceso irrestricto desde todos los servicios Azure.
6. Staging nunca utiliza datos reales de clientes, membresías o pagos.

## Migración segura del esquema: solo estructura

No ejecutar los scripts locales 01, 02, 07 u 08 sin una adaptación
controlada: los scripts locales pueden crear bases, usar USE [SuwoGym]
o insertar usuarios predefinidos. Azure SQL requiere un esquema
compatible y un despliegue apuntando explícitamente a SuwoGym_Staging.

Ruta recomendada:
1. Extraer un **DACPAC solo de esquema** desde la base local aislada
   **SuwoGym_Test_7B**, sin incluir datos ni credenciales.
2. Revisar compatibilidad de objetos con Azure SQL Database y el
   plan de cambios (SqlPackage DeployReport/Script) antes de publicar.
3. Desplegar el esquema DACPAC **solo** a la base Azure SuwoGym_Staging.
4. En una ventana conectada a esta base, comprobar SELECT DB_NAME().
5. Ejecutar **Database/10_Staging_Catalogos_Minimos.sql**, que aborta
   si no se conecta a SuwoGym_Staging, o si hay usuarios o pagos.
   Crea únicamente los roles 1, 2 y 3.
6. Esperado: 0 usuarios, 0 pagos, 3 roles. Después crear cuentas
   ficticias desde la interfaz siguiendo el flujo normal.

Un BACPAC exporta datos y NO debe usarse para copiar información real.
Guía DACPAC: https://learn.microsoft.com/en-us/sql/tools/sqlpackage/sqlpackage

## Variables de entorno en App Service

Configurarlas en Portal → App Service → Settings → Environment
variables → App settings. Dos guiones bajos __ corresponden a : en
configuración .NET. **NO usar la pestaña Connection strings como
sustituto**, pues el código utiliza ConnectionStringsApi personalizado.

API (sustituir ejemplos, nunca almacenar secretos en Git):
- ASPNETCORE_ENVIRONMENT = Staging
- AllowedHosts = suwogym-stg-api-UNICO.azurewebsites.net
- Jwt__Issuer = SuwoGym.API
- Jwt__Audience = SuwoGym.Web
- Jwt__SecretKey = valor NUEVO, aleatorio y exclusivo de Azure (>=32 bytes)
- ConnectionStringsApi__DefaultConnection = cadena SQL Azure con Encrypt=True;TrustServerCertificate=False y usuario de permisos mínimos
- Correos__Correo = remitente SMTP ficticio/de prueba
- Correos__AppPassword = credencial SMTP almacenada como secreto
- Recuperacion__UrlBaseWeb = https://suwogym-stg-web-UNICO.azurewebsites.net

WEB:
- ASPNETCORE_ENVIRONMENT = Staging
- AllowedHosts = suwogym-stg-web-UNICO.azurewebsites.net
- ConnectionStringsWeb__urlApi = https://suwogym-stg-api-UNICO.azurewebsites.net
- Deployment__DataProtectionKeysPath = D:\home\site\suwo-keys

Antes de iniciar la WEB, **crear D:\home\site\suwo-keys** en su
App Service Windows (Advanced Tools/Kudu, Debug console) en el
volumen persistente, fuera de wwwroot y con permisos restringidos.
La validación de fase 8A impide arrancar WEB si esa carpeta no existe.
Las claves en archivo NO están cifradas por Data Protection en reposo;
en producción emplear cifrado por Key Vault u otro mecanismo apropiado.

Las URL localhost del repositorio quedan anuladas por las variables
Staging; los User Secrets locales no se copian al servidor.

## GitHub Actions: OIDC y aprobación

En GitHub → Settings → Environments crear **staging**, y configurar
**required reviewers** cuando la política del repositorio lo permita.
No asumir que la mera existencia del environment requiere aprobación.

En Microsoft Entra crear una identidad para el deployment con permisos
mínimos sobre el **grupo de recursos staging**, NO toda la suscripción.
Añadir federated credential OIDC con el subject exacto:

    repo:HenryGonzalezAlguera/SuwoGym:environment:staging

En el environment staging configurar estos **Secrets**:
- AZURE_CLIENT_ID
- AZURE_TENANT_ID
- AZURE_SUBSCRIPTION_ID

Y estas **Variables**:
- AZURE_RESOURCE_GROUP
- AZURE_WEBAPP_API
- AZURE_WEBAPP_WEB

Solo cuando los recursos y configuraciones anteriores estén completos:
GitHub → Actions → SUWÓ GYM | Azure staging manual → Run workflow →
rama main → escribir **DEPLOY_STAGING**. El workflow restaura,
compila, prueba, publica la API y WEB, y revisa ambos endpoints
/health/live y /health/ready. No provisiona SQL ni aplica migraciones.
Si health check falla, el workflow queda rojo y NO hay rollback
automático; diagnosticar antes de repetir.

Documentación Microsoft:
https://learn.microsoft.com/en-us/azure/app-service/deploy-github-actions

## Preflight y pruebas con PowerShell

El script de auditoría requiere Azure CLI autenticado, solo usa consultas
de lectura y NO modifica recursos. Abrir terminal en la raíz del repo:

~~~powershell
.\tools\azure\Test-AzureStagingPreflight.ps1 -ResourceGroup "rg-suwogym-stg" -WebApp "suwogym-stg-web-UNICO" -ApiApp "suwogym-stg-api-UNICO" -SqlServer "sql-suwogym-stg-UNICO" -SqlDatabase "SuwoGym_Staging"
~~~

Tras la publicación remota:

~~~powershell
.\tools\azure\Test-AzureStagingHealth.ps1 -WebApp "suwogym-stg-web-UNICO" -ApiApp "suwogym-stg-api-UNICO"
~~~

Cambiar UNICO por nombres de Azure realmente disponibles.
No compartir cadenas SQL, claves JWT ni contraseñas SMTP en capturas.

## Riesgos pendientes para una versión pública real

- Las sesiones MVC están en memoria: utilizar solo una instancia WEB.
  Reinicios cierran sesiones; para escalado se requiere almacenamiento
  distribuido (Redis/SQL) y coordinación de Data Protection.
- Data Protection en archivos no cifra claves en reposo de forma
  automática; usar proveedor con cifrado antes de producción.
- Rate limiting por IP detrás de reverse proxies necesita configurar
  ForwardedHeaders con proxies/redes de confianza; nunca confiar en
  X-Forwarded-For sin restricciones.
- La API no debe quedar abierta a tráfico no controlado sin controles
  de red, seguridad y autenticación revisados.
- Validar el esquema DACPAC sobre Azure SQL y las operaciones
  WEB→API→SQL con cuentas ficticias; el CI solo ejecuta tests sin DB.
- No reutilizar JWT o SMTP de desarrollo en el entorno remoto.
- Revisar advertencias de compilación, métricas, costes, acceso, logs
  y procedimiento de backup/restore y rollback.
- La disponibilidad de Azure SQL Free puede perderse temporalmente
  al llegar al límite mensual.

**Cierre 8B**: workflow y scripts listos, revisión CI aprobada.
**Siguiente 8C**: provisionamiento autorizado y primera publicación
con presupuesto revisado, sin datos reales.
