# Fase 8A — Preparación de preproducción (sin publicar)

## Alcance verificado en el repositorio

Arquitectura: `.NET 10`, API ASP.NET Core con JWT/Dapper/SQL Server y WEB
ASP.NET Core MVC que consume la API mediante HTTPS.
La WEB mantiene sesión ASP.NET Core en el proceso y la API valida contra
SQL Server la versión y el rol vigentes de cada JWT. SMTP usa Gmail con
MailKit/STARTTLS. Se preservan las funcionalidades y las migraciones 07/08.

**Esta fase no desplegó el sitio, no abrió puertos ni modificó SQL Server.
No convierte automáticamente SUWÓ GYM en una aplicación lista para producción.**

## Cambios en esta fase

- `/health/live` en API y WEB: HTTP 200 si el proceso HTTP responde.
  **No consulta SQL** y nunca muestra credenciales.
- `/health/ready` en API: comprueba conexión SQL Server y objetos
  imprescindibles de las migraciones 07 y 08. Devuelve 200 o 503,
  sin detallar errores SQL ni cadenas de conexión.
- `/health/ready` en WEB: comprueba que la API responda correctamente
  a su readiness en un máximo de 5 segundos.
- En Staging/Production, la API exige configuración SMTP, JWT, URL HTTPS
  de recuperación, `AllowedHosts` explícitos y conexión SQL cifrada
  sin confiar ciegamente en el certificado del servidor.
- La WEB exige `AllowedHosts` explícitos, URL de API HTTPS no local
  y una carpeta de claves Data Protection persistente, externa a `wwwroot`.
- Desarrollo local (`ASPNETCORE_ENVIRONMENT=Development`) mantiene
  `localhost` y User Secrets, sin cambiar tu experiencia en Visual Studio.
- GitHub Actions mantiene las 85 pruebas anteriores, agrega pruebas de
  validación y liveness y crea binarios Release API/WEB descargables.
  Los artefactos NO realizan deployment y se guardan 7 días.

## Arquitectura recomendada para la primera prueba remota

```text
Navegador  -- HTTPS -->  SUWÓ WEB (1 instancia)
                               |
                               | HTTPS, red privada / protegida
                               v
                            SUWÓ API (1 instancia)
                               |
                               | TLS, cuenta con mínimos permisos
                               v
                       SQL Server de STAGING
```

- Usar una **base de preproducción diferente** de `SuwoGym` y
  `SuwoGym_Test_7B` (p.ej., `SuwoGym_Staging`), sin datos reales.
- API preferiblemente no expuesta directamente al Internet público:
  la WEB consume los endpoints de forma interna/privada cuando la
  plataforma de hospedaje lo permita.
- No usar contraseñas SQL de administrador/sa para la app.
- Mantener puerto HTTPS y certificado válido (no
  `TrustServerCertificate=True` en publicación).
- **Solo una instancia de WEB durante esta etapa.** Las sesiones
  todavía son en memoria; un balanceador con varias instancias requerirá
  Redis/SQL para sesión distribuida y coordinación de claves Data Protection.
- Las claves Data Protection se guardan en un volumen persistente,
  con permisos restringidos, **nunca bajo `wwwroot`** y protegidas
  en reposo por el proveedor/infraestructura.
- HTTPS detrás de un proxy requiere configuración explícita de
  `ForwardedHeaders` con redes/proxies de confianza. No confiar en
  encabezados `X-Forwarded-For` suministrados arbitrariamente por clientes.
  Hasta configurar esa capa no se debe publicar la API al público:
  el limitador por IP podría identificar al proxy y no al cliente.

## Variables de entorno requeridas (ejemplo de nombres, SIN valores)

En .NET, `__` representa `:` en la jerarquía de configuración.

### API

| Variable | Finalidad |
|---|---|
| `ASPNETCORE_ENVIRONMENT=Staging` | Activa validaciones |
| `Jwt__SecretKey` | Clave aleatoria exclusiva del entorno (>=32 bytes) |
| `Jwt__Issuer`, `Jwt__Audience` | Emisor y audiencia |
| `ConnectionStringsApi__DefaultConnection` | SQL Staging con Encrypt=True, TrustServerCertificate=False |
| `Correos__Correo`, `Correos__AppPassword` | Credenciales SMTP en un gestor de secretos |
| `Recuperacion__UrlBaseWeb` | Origen HTTPS público de WEB |
| `AllowedHosts` | Dominio o dominios reales de API |

### WEB

| Variable | Finalidad |
|---|---|
| `ASPNETCORE_ENVIRONMENT=Staging` | Activa validaciones |
| `ConnectionStringsWeb__urlApi` | URL HTTPS alcanzable de API |
| `AllowedHosts` | Host público de WEB |
| `Deployment__DataProtectionKeysPath` | Carpeta existente y persistente fuera de wwwroot |

No subir valores a GitHub, `appsettings.json`, historial de terminal
ni capturas. Los valores locales `https://localhost:7035` y
`https://localhost:7129` NO funcionan como URLs remotas de staging.

Las variables de entorno y la configuración del proveedor deben
configurarse **por separado para API y WEB**.

### Clave JWT

Utilizar una clave aleatoria nueva para Staging, distinta de Desarrollo
y Producción. Si una clave apareció en capturas o commits,
**rotarla antes de publicarla**. Cambiar la clave invalida los JWT
firmados anteriormente.

## Cómo descargar y revisar los binarios

1. En el PR de la rama, abrir **Checks → SUWO Gym .NET build**.
2. Confirmar todos los pasos en verde.
3. En **Artifacts**, descargar `suwo-staging-binaries`.
4. El archivo contiene directorios `publish/api` y `publish/web`.
5. No publicar ni compartir esos paquetes como servidor funcionando:
   contienen únicamente binarios y `appsettings` públicos de desarrollo
   sin secretos; el entorno remoto debe proporcionar las variables de
   Staging descritas antes.

## Checklist de preproducción

- [ ] Proveedor, dominio y método de hospedaje confirmados.
- [ ] Dos apps separadas (WEB/API) con certificados HTTPS válidos.
- [ ] SQL Staging aislado y sin datos personales, con migraciones 07/08.
- [ ] Usuario SQL de permisos mínimos; TLS validado, firewall privado.
- [ ] Variables/secretos cargados en almacén del proveedor.
- [ ] Volumen persistente con permisos de lectura/escritura exclusivos
      para claves Data Protection; copia de seguridad de esas claves.
- [ ] URL de API accesible desde WEB, no desde rutas no autorizadas.
- [ ] `/health/live` devuelve 200 en ambas aplicaciones.
- [ ] `/health/ready` devuelve 200 en ambas; si SQL se desconecta,
      la API devuelve 503.
- [ ] Se puede iniciar sesión como tres roles con cuentas ficticias.
- [ ] Reservas, asistencias, pagos de prueba y recuperación SMTP,
      con enlace de 15 min y revocación de JWT, funcionan.
- [ ] Configurar logs sin contraseñas, correo, tokens ni cadenas SQL.
- [ ] Prueba de respaldo/restauración en Staging y procedimiento de rollback.
- [ ] Revisar configuración de proxies confiables y límites por IP.
- [ ] Revisar trazas, métricas de errores y tiempos de respuesta.
- [ ] Firma de aceptación antes de habilitar acceso de terceros.

## Rollback y riesgos pendientes

- Registrar qué artefacto y commit se desplegaron; conservar la versión
  anterior para volver a ella si hay fallos.
- Tomar backup SQL previo al despliegue y comprobar que se puede restaurar
  en un entorno aislado. La aplicación no ejecuta migraciones automáticamente.
- La API realiza consulta SQL por cada JWT; medir consumo y latencia antes
  de aumentar el número de usuarios.
- La sesión de WEB no es distribuida: no escalar a varias instancias sin
  modificar almacenamiento y protección de claves.
- Correo de recuperación SMTP no garantiza entrega; monitorear errores
  sin exponer si una cuenta existe.
- Revisar políticas de protección de datos y retención antes de usar
  información de clientes reales.

## Siguiente etapa: 8B

Elegir proveedor, crear recursos de staging, DNS, TLS, SQL y almacén
de secretos, establecer proxies confiables y probar la URL remota.
**No crear credenciales ni contratar infraestructura hasta que se
defina plataforma, presupuesto y modelo de hosting.**
