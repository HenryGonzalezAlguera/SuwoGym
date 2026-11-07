# Fase 8C — Publicación de demostración en Azure Linux F1 compartido

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

\`\`\`text
ASPNETCORE_ENVIRONMENT=Staging
AllowedHosts=suwogym-stg-web-hga.azurewebsites.net
ConnectionStringsWeb__urlApi=https://suwogym-stg-api-hga.azurewebsites.net
Deployment__DataProtectionKeysPath=/home/suwo-keys
\`\`\`

### Variables API

\`\`\`text
ASPNETCORE_ENVIRONMENT=Staging
AllowedHosts=suwogym-stg-api-hga.azurewebsites.net
Jwt__Issuer=SuwoGym.API
Jwt__Audience=SuwoGym.Web
Jwt__SecretKey=[NUEVA clave aleatoria, en settings privados]
ConnectionStringsApi__DefaultConnection=[cadena Azure SQL Staging TLS verificado]
Correos__Correo=[cuenta SMTP ficticia]
Correos__AppPassword=[credencial SMTP privada]
Recuperacion__UrlBaseWeb=https://suwogym-stg-web-hga.azurewebsites.net
\`\`\`

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
