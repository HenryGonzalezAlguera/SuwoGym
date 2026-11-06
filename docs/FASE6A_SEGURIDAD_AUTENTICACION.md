# Fase 6A — Autenticación y sesiones seguras

## Objetivo y límites

Endurecer los JWT, la sesión HTTP de la WEB y los formularios de acceso sin cambiar controladores de negocio, modelos de datos, SQL, endpoints, credenciales ni navegación por roles.

**Ámbito:** `SuwoGym_API/Program.cs`, `SuwoGym_API/Services/UtilesService.cs`, `SuwoGym_API/appsettings.json`, `SuwoGym_Web/Program.cs`, `SuwoGym_Web/Controllers/HomeController.cs` y dos formularios Razor. Se agregan pruebas unitarias.

### Cambios de API

- El JWT emitido contiene los campos estándar `iss` y `aud`.
- El validador JWT exige `ValidateIssuer = true` y `ValidateAudience = true`, además de la firma y expiración que ya se verificaban.
- Los identificadores públicos `Jwt:Issuer = SuwoGym.API` y `Jwt:Audience = SuwoGym.Web` se versionan en `appsettings.json` porque **no son secretos**.
- La API continúa exigiendo una `Jwt:SecretKey` fuerte desde User Secrets o variable de entorno. No se incluyen claves privadas.

### Cambios de WEB

- Las cookies de sesión y antifalsificación se envían solo por HTTPS (`CookieSecurePolicy.Always`), con `HttpOnly` y `SameSite=Lax`.
- `POST /Home/Index` y `POST /Home/RecuperarAcceso` ahora llevan `[ValidateAntiForgeryToken]` y generan un único campo antifalsificación usando el FormTagHelper con `asp-antiforgery="true"`.
- Los scripts jQuery actuales envían formularios HTML normales, conservando los campos y las rutas existentes.

## Requisitos para probarlo en Visual Studio

1. Cambiar a la rama `feature/fase6a-autenticacion-segura` mediante Git → Administrar ramas. Si aún no aparece, Git → Recuperar y expandir `remotes/origin/feature`.
2. **No ejecutar ningún script SQL** ni borrar la base. Detener API y WEB, compilar y reiniciar ambos proyectos con sus perfiles **HTTPS** (API habitualmente en 7129 y WEB en 7035). Estas cookies no se enviarán por HTTP.
3. Los JWT emitidos **antes** de la actualización no incluyen `iss` ni `aud` y serán rechazados. Cerrar sesión e iniciar sesión otra vez para obtener un token nuevo. Si queda una sesión antigua sin token válido, cerrar navegador/cookies del sitio local, no la base SQL.
4. Verificar inicio de sesión y redirección al rol **Administrador**, **Entrenador** y **Miembro**. Confirmar que Dashboard, rutinas y reservas abren normalmente.
5. Probar contraseña incorrecta y el formulario de recuperación **solo con un usuario de prueba y SMTP de desarrollo configurado**. El sistema de recuperación previo genera una contraseña temporal y todavía requiere rediseño; no realizar esta prueba con cuentas importantes.
6. Desde el navegador, abrir Herramientas de desarrollador → Aplicación → Cookies para `https://localhost:7035`; comprobar las marcas `Secure`, `HttpOnly` y `SameSite=Lax` en cookies de sesión/antifalsificación.
7. En `Inicio de sesión`, usar el botón del formulario, no una petición HTTP fabricada. Verificar que `__RequestVerificationToken` existe como campo oculto y que una petición sin él recibe respuesta 400. Repetir en Recuperar acceso sin provocar un cambio de contraseña real.
8. Ejecutar `dotnet test .\SuwoGym.slnx -c Release` y verificar GitHub Actions; comprueba que no haya `401` ni `403` inesperados.

## Pruebas automatizadas

`SuwoGym.Tests/SeguridadAutenticacionFase6ATests.cs` verifica:

- Token JWT válido: emisor, audiencia, ID de usuario, rol y firma.
- Rechazo de emisor distinto, audiencia distinta y clave de firma distinta.
- Los dos POST de autenticación de la WEB exigen `[ValidateAntiForgeryToken]`.

Estas pruebas **no son pruebas end-to-end**: no utilizan SQL Server real ni SMTP, no prueban un navegador ni ejercitan todas las operaciones de la aplicación.

## Seguridad todavía pendiente — Fase 6B en adelante

- Revisar antifalsificación **en los demás POST MVC** y los endpoints que usen AJAX. No aplicar un filtro global sin auditar los formularios primero.
- Sustituir recuperación de acceso mediante contraseña temporal por un enlace de un solo uso que expire; evitar modificar contraseñas si falla SMTP; ofrecer una respuesta genérica para evitar enumeración de cuentas.
- Proteger el inicio de sesión y recuperación contra abuso (límites de intentos) y revisar el manejo de errores SQL.
- Revisar logout GET, invalidación de tokens después de cambios de contraseña/rol y revocación de sesiones.
- Verificar que las contraseñas de aplicación y claves JWT expuestas en commits históricos se **revocaron y rotaron**; una actualización de código no revoca secretos previos.

No considerar este proyecto listo para producción sin completar la fase 6B, integración y evaluación de seguridad.

## Sin afectación a la base

No se modifican stored procedures, tablas, datos, ni configuración de conexión a SQL Server. Los únicos efectos esperables al iniciar la versión nueva son necesidad de volver a iniciar sesión y cookies marcadas como Secure.
