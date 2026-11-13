# Fase 8C — Publicación de demostración en Azure Linux F1 compartido

## Estado comprobado el 13 de noviembre de 2026

- **API**: `suwogym-stg-api-hga` — `https://suwogym-stg-api-hga-hsdmctgddbbxgpf4.centralus-01.azurewebsites.net`.
- **WEB**: `suwogym-stg-web-hga` — `https://suwogym-stg-web-hga-emfxdjcfccabdfh2.centralus-01.azurewebsites.net`.
- Ambas apps usan Linux, .NET 10 y el plan **existente** `asp-stockflow-free` (F1) en `rg-stockflow-demo`.
- SQL: `SuwoGym_Staging` en `stockflow-sql-hga.database.windows.net`, con `Modern_Spanish_CI_AS`, 15 tablas, 74 procedimientos, 19 FK y 1 trigger.
- Catálogos comprobados en Staging: 3 roles, 3 planes y 1 clase; cero usuarios al verificar el esquema inicial.
- Acceso SQL `suwogym_app` probado desde SSMS con EXECUTE; conexión desde App Service aún no probada.
- Variables de entorno registradas en ambas apps, pero valores y conectividad aún pendientes de prueba.
- WEB: `/home/suwo-keys` conservó un archivo de prueba tras el reinicio; archivo temporal eliminado.
- **Pendiente**: identidad federada GitHub → Azure, permisos mínimos, pruebas y publicación manual.

**No confundir el dominio por defecto real con `nombre.azurewebsites.net`: Azure asignó un sufijo regional a estas dos apps.**

## Estado verificado en Azure Portal (7 noviembre 2026)

El plan existente de StockFlow puede alojar también las dos aplicaciones de
SUWÓ GYM para una demostración de bajo tráfico, sin contratar un nuevo plan.

| Campo | Dato verificado |
|---|---|
| Resource group existente | rg-stockflow-demo |
| App Service Plan existente | asp-stockflow-free |
| SKU | F1 Free |
| Sistema operativo | Linux |
| Región | Central US |
| Aplicaciones actuales | 2 (StockFlow WEB y API) |
| Aplicaciones previstas adicionales | 2 (SUWÓ WEB y API) |

Límite documentado: 10 apps por plan F1, NO 10 planes Premium o 10 bases
SQL ilimitadas. StockFlow y SUWÓ consumirán recursos compartidos. F1 tiene
cuotas de CPU, RAM, ancho de banda y almacenamiento, no tiene Always On y
puede suspender o deshabilitar temporalmente las apps al alcanzar cuotas.
La viabilidad y la latencia deben medirse en la práctica. No usar con
datos reales ni como producción comercial.

Referencia Microsoft:
https://learn.microsoft.com/en-us/azure/azure-resource-manager/management/azure-subscription-service-limits
https://learn.microsoft.com/en-us/azure/app-service/overview-hosting-plans

## 1. Crear SOLO las dos Web Apps nuevas en Azure Portal

En https://portal.azure.com → Servicios de aplicaciones → Crear → Aplicación web:

- **Suscripción**: la que contiene StockFlow.
- **Grupo de recursos**: EXISTENTE \`rg-stockflow-demo\` (NO crear otro grupo).
- **Nombre**: escoger nombres únicos que empiecen por
  \`suwogym-stg-\`, por ejemplo \`suwogym-stg-web-hga\` (WEB) y
  \`suwogym-stg-api-hga\` (API); comprobar disponibilidad.
- **Publicar**: Código.
- **Pila en tiempo de ejecución**: .NET 10 (LTS) si aparece.
- **Sistema operativo**: **Linux**, no Windows.
- **Región**: **Central US**, igual que el plan.
- **Plan de App Service**: **Seleccionar EXISTENTE \`asp-stockflow-free\`**.
- **Plan de precios**: confirmar **F1 Gratis** antes de crear.
- **Zona/DNS/Seguridad**: HTTPS Only; utilizar subdominio
  \`azurewebsites.net\` sin comprar un dominio.
- **No habilitar** Application Insights, nuevo App Service Plan,
  SKU B1, IP dedicada ni ningún componente de pago automáticamente.

Si Azure Portal NO permite seleccionar \`asp-stockflow-free\` como plan
existente, o no ofrece .NET 10 para Linux en la suscripción, **detenerse**.
No crear un plan nuevo ni subir de tier sin reevaluar costos.

Estas dos nuevas Web Apps deben permanecer en el mismo plan que StockFlow;
el pipeline verifica esa condición antes de desplegar. El workflow NO
modifica ni reinicia las aplicaciones de StockFlow explícitamente, aunque
sí compartirán cuotas y un agotamiento de recursos puede afectarlas.

## 2. Modificación de staging específica para Linux

La fase 8A exige \`Deployment:DataProtectionKeysPath\` existente y
persistente, fuera de \`wwwroot\`. La antigua guía 8B documentó una
ruta Windows \`D:\home\site\suwo-keys\`: **NO usarla en este plan Linux**.

En la WEB Linux nueva:
- Usar la ruta \`/home/suwo-keys\`.
- Antes de arrancar con \`ASPNETCORE_ENVIRONMENT=Staging\`,
  crear la carpeta con la consola/SSH de la nueva Web App:
  \`mkdir -p /home/suwo-keys\`.
- Verificar que \`/home\` sea persistente en ese App Service.
  En contenedores Linux personalizados, la persistencia requiere
  \`WEBSITES_ENABLE_APP_SERVICE_STORAGE=true\`.
  En la pila .NET integrada, validar comportamiento de persistencia
  y permisos de lectura/escritura de la aplicación.
- Las claves Data Protection escritas en esa carpeta no quedan cifradas
  automáticamente en reposo. Este es un entorno de **demostración con
  datos ficticios**; antes de producción real se requiere cifrado
  apropiado y gestión de claves fuera de la instancia.

### Variables WEB, ejemplo con nombres elegidos

```text
ASPNETCORE_ENVIRONMENT=Staging
AllowedHosts=suwogym-stg-web-hga-emfxdjcfccabdfh2.centralus-01.azurewebsites.net
ConnectionStringsWeb__urlApi=https://suwogym-stg-api-hga-hsdmctgddbbxgpf4.centralus-01.azurewebsites.net
Deployment__DataProtectionKeysPath=/home/suwo-keys
```

### Variables API

```text
ASPNETCORE_ENVIRONMENT=Staging
AllowedHosts=suwogym-stg-api-hga-hsdmctgddbbxgpf4.centralus-01.azurewebsites.net
Jwt__Issuer=SuwoGym.API
Jwt__Audience=SuwoGym.Web
Jwt__SecretKey=[NUEVA clave aleatoria, en settings privados]
ConnectionStringsApi__DefaultConnection=[cadena Azure SQL Staging TLS verificado]
Correos__Correo=[correo remitente de pruebas, privado]
Correos__AppPassword=[credencial SMTP privada]
Recuperacion__UrlBaseWeb=https://suwogym-stg-web-hga-emfxdjcfccabdfh2.centralus-01.azurewebsites.net
```

Los nombres de arriba son ejemplos; si el nombre asignado difiere,
adaptar los valores. No copiar \`secrets.json\` ni credenciales locales.
No compartir capturas que muestren JWT, SMTP o conexión SQL.

## 3. Azure SQL de prueba por separado

El plan App Service gratuito **no incluye una base SQL ilimitada**.
Crear una base **nueva** \`SuwoGym_Staging\` en Azure SQL usando la
modalidad Free si tu suscripción es elegible; confirmar el precio final
y seleccionar auto-pausing al alcanzar el límite gratis para evitar
facturación inesperada.

Usar esquema solamente (DACPAC de la base ficticia \`SuwoGym_Test_7B\`),
verificar compatibilidad Azure SQL Database y ejecutar
\`Database/10_Staging_Catalogos_Minimos.sql\` sólo contra
\`SuwoGym_Staging\`. Nunca importar usuarios, membresías o pagos reales
de la base \`SuwoGym\`.

La cuenta de prueba Azure SQL Free tiene límites mensuales y puede
suspenderse al agotarlos; \`/health/ready\` puede devolver 503
durante la suspensión. La disponibilidad sin interrupciones requiere
un nivel pagado, no contemplado para esta demostración.

## 4. GitHub Actions 8C (solo después de crear recursos)

El workflow \`.github/workflows/azure-staging-manual.yml\` mantiene
las barreras originales: ejecución manual desde main, confirmación
\`DEPLOY_STAGING\`, environment \`staging\`, OIDC, pruebas completas y
health checks posteriores. **No crea ni paga recursos**.

Con la adaptación 8C admite el grupo existente \`rg-stockflow-demo\`,
pero antes de publicar comprueba también:
- Plan exacto: \`asp-stockflow-free\`, Linux, F1 y región Central US.
- Ambas apps se llaman \`suwogym-stg-...\`, no \`stockflow-...\`.
- WEB y API están en el mismo plan permitido.
- En el environment de GitHub, además de las variables de 8B,
  establecer \`AZURE_APP_SERVICE_PLAN=asp-stockflow-free\`.

El workflow NO permite publicar en StockFlow ni crear nuevos servicios.
Entra a GitHub → Settings → Environments → \`staging\` para asignar
los siguientes valores:

Variables: \`AZURE_RESOURCE_GROUP=rg-stockflow-demo\`,
\`AZURE_APP_SERVICE_PLAN=asp-stockflow-free\`,
\`AZURE_WEBAPP_API\`, \`AZURE_WEBAPP_WEB\`.

Secretos OIDC: \`AZURE_CLIENT_ID\`, \`AZURE_TENANT_ID\`,
\`AZURE_SUBSCRIPTION_ID\`. Configurar credenciales federadas y
permisos mínimos **sobre las dos Web Apps de SUWÓ**, no sobre StockFlow
ni sobre toda la suscripción. Si la identidad necesita leer el plan,
asignar solo permiso Reader a dicho recurso.

No ejecutar el workflow hasta tener SQL Staging y los ajustes
ambientales seguros. Primero revisar manualmente los cuatro health
checks, login y módulos de prueba.

### Prueba manual de acceso sin publicar

Después de configurar las cuatro variables de entorno, los secretos OIDC y los tres
roles mínimos de Azure (Website Contributor en API y WEB; Reader en el plan F1):

1. GitHub → **Actions** → **SUWÓ GYM | Azure staging preflight (solo lectura)**.
2. Seleccionar **Run workflow**, rama **main**, escribir `CHECK_STAGING`.
3. Ejecutar y abrir el trabajo **Validar OIDC y recursos existentes**.
4. Confirmar el mensaje `PREFLIGHT_OK` y los dominios de las dos apps.

Esta prueba ejecuta únicamente consultas `az ... show` tras autenticarse por
OIDC. **No despliega**, no cambia Azure y no imprime secretos. Si falla, revisar
el paso de GitHub → Azure indicado en los registros antes de activar el
workflow de publicación `DEPLOY_STAGING`.

### Configuración OIDC necesaria antes del primer Run workflow

El workflow exige `environment: staging`, por lo que la credencial federada de GitHub debe usar **Environment `staging`**, no Branch `main`:

- Issuer: `https://token.actions.githubusercontent.com`
- Subject (GitHub OIDC **inmutable**, repositorio creado el 25-08-2026): `repo:HenryGonzalezAlguera@134661566/SuwoGym@1345610007:environment:staging`
- Audience: `api://AzureADTokenExchange`

1. GitHub → `SuwoGym` → **Settings → Environments**: crear/abrir `staging`, restringir **Deployment branches and tags** a `main`; configurar aprobaciones si están disponibles.
2. Azure Portal → **Microsoft Entra ID → App registrations → New registration**: crear una identidad dedicada a SUWÓ GYM Staging (solo este inquilino); guardar Application (client) ID y Directory (tenant) ID.
3. En la aplicación Entra, abrir **Certificates & secrets → Federated credentials → Add credential → GitHub Actions deploying Azure resources**. Introducir **Organización** `HenryGonzalezAlguera`, **Organización ID** `134661566`, **Repositorio** `SuwoGym`, **Repositorio ID** `1345610007`, tipo de entidad **Environment** y nombre del entorno `staging`. Verificar que Subject coincide exactamente con el valor inmutable anterior. **No crear client secret**. GitHub usa IDs en el `sub` de repositorios creados desde el 15-07-2026 (https://docs.github.com/en/actions/reference/security/oidc).
4. Azure **Access control (IAM)**: asignar a la aplicación Entra el rol **Website Contributor** *únicamente* sobre `suwogym-stg-api-hga` y `suwogym-stg-web-hga`. Dar **Reader** únicamente sobre el recurso del plan `asp-stockflow-free`, necesario para que el workflow pueda validar su SKU/región. **No asignar Contributor sobre `rg-stockflow-demo` o la suscripción**: contiene StockFlow.
5. GitHub → **Settings → Environments → staging**: agregar variables `AZURE_RESOURCE_GROUP=rg-stockflow-demo`, `AZURE_APP_SERVICE_PLAN=asp-stockflow-free`, `AZURE_WEBAPP_API=suwogym-stg-api-hga`, `AZURE_WEBAPP_WEB=suwogym-stg-web-hga`.
6. En el mismo entorno, agregar los secretos `AZURE_CLIENT_ID`, `AZURE_TENANT_ID`, `AZURE_SUBSCRIPTION_ID` con los ID de Entra y la suscripción del proyecto (son identificadores, no contraseñas).
7. Revisar las variables de App Service sin mostrar sus valores. La API necesita `ConnectionStringsApi__DefaultConnection` con `Encrypt=True;TrustServerCertificate=False`, JWT y correo. La WEB necesita `AllowedHosts`, `ConnectionStringsWeb__urlApi` y `Deployment__DataProtectionKeysPath=/home/suwo-keys`.
8. **No ejecutar** Actions → `SUWÓ GYM | Azure staging manual` → Run workflow hasta verificar identidades, roles y configuración; el workflow efectúa despliegues reales y las comprobaciones de salud deben probar SQL/WEB.

Referencia: https://learn.microsoft.com/en-us/azure/app-service/deploy-github-actions?tabs=openid

## 5. Crédito y cautelas

La captura del portal indica US$200 de crédito con vencimiento el
27 de noviembre de 2026. El vencimiento puede causar suspensión
de recursos de una suscripción de prueba: revisar el estado de
suscripción y el comportamiento de Azure antes de depender de una URL
pública duradera.

El objetivo es una demostración técnica con datos inventados.
F1 no garantiza rendimiento, disponibilidad ni capacidad de soportar
muchas reservas simultáneas. No activar escalado a planes de pago sin
comprobar la estimación y consentimiento del propietario.
