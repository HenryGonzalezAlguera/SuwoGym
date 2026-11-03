# Fase 2 — Sesiones y permisos por rol (MVC + API)

## Diagnóstico verificado

Antes de esta fase, la WEB ya verificaba el `IdRol` en los métodos `OnActionExecuting` de cada controlador protegido. La API también restringía sus controladores con `[Authorize(Roles = "1")]`, `"2"` y `"3"`. Sin embargo:

- El filtro `SessionAuthorize` solo comprobaba que `Autenticado` tuviera algún valor; no exigía un ID de usuario, rol válido o token presentes.
- `GET /Home/Principal` no tenía un filtro propio, por lo que podía renderizarse sin sesión.
- Las comprobaciones de rol WEB estaban duplicadas en los tres controladores, dificultando las pruebas aisladas.
- No existían pruebas automatizadas para el filtro.

## Cambios realizados

1. El filtro `SessionAuthorizeAttribute` exige:
   - `Autenticado == "1"`
   - `IdUsuario > 0`
   - `IdRol` entre `1` y `3`
   - `Token` presente y no vacío.
2. El mismo filtro acepta el rol requerido: `[SessionAuthorize(1)]`, `[SessionAuthorize(2)]` o `[SessionAuthorize(3)]`.
3. `AdminController`, `EntrenadorController` y `MiembroController` aplican el atributo anterior en lugar de repetir la comparación en `OnActionExecuting`. `UsuarioController` conserva `[SessionAuthorize]` para cualquiera de los tres perfiles.
4. `HomeController.Principal` está protegido con `[SessionAuthorize]`.
5. `SuwoGym.Tests/` contiene pruebas unitarias de acceso, redirecciones, sesiones incompletas y permisos; `dotnet test` se ejecuta en GitHub Actions.

## Matriz de autorización esperada

| Ruta MVC | Sin sesión | Administrador (1) | Entrenador (2) | Miembro (3) |
| --- | --- | --- | --- | --- |
| `Home/Principal` | Redirigir a login | Permitir | Permitir | Permitir |
| `Admin/*` | Redirigir a login | Permitir | Redirigir al panel | Redirigir al panel |
| `Entrenador/*` | Redirigir a login | Redirigir al panel | Permitir | Redirigir al panel |
| `Miembro/*` | Redirigir a login | Redirigir al panel | Redirigir al panel | Permitir |
| `Usuario/Configuracion` | Redirigir a login | Permitir | Permitir | Permitir |

## Ejecutar pruebas

```powershell
dotnet restore .\SuwoGym.slnx
dotnet build .\SuwoGym.slnx -c Release --no-restore
dotnet test .\SuwoGym.Tests\SuwoGym.Tests.csproj -c Release --no-restore
```

Las pruebas del filtro utilizan sesiones en memoria: **no** requieren SQL Server, credenciales ni iniciar la API.

## Límites de seguridad

- **El filtro MVC no valida criptográficamente el JWT.** La API sigue siendo responsable de validar firma, fecha de expiración y roles en cada endpoint protegido.
- El estado de sesión WEB debe considerarse una ayuda de control de acceso, no un reemplazo de las comprobaciones de autorización de la API.
- No se han incorporado todavía pruebas de extremo a extremo de tres roles ni pruebas sobre sesiones expiradas contra la API.
- Pendientes: revisar los formularios y su protección antifalsificación (CSRF), respuesta ante credenciales revocadas, logout, recuperación por correo y rate limiting.
- Cualquier cambio en roles y autorizaciones debe validarse con cuentas ficticias y datos de prueba antes de publicar la aplicación.

## Prueba funcional manual recomendada

Con API y WEB iniciadas, intentar abrir `/Home/Principal` desde una ventana privada sin sesión. Debe redirigir a `/Home/Index`. Después iniciar sesión con un usuario de prueba de cada rol y tratar de acceder a una ruta de los otros dos roles, confirmando la redirección al panel principal. **No usar cuentas ni datos reales.**
