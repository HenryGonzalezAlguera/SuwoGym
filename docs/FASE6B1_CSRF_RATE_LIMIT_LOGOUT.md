# Fase 6B-1 — CSRF, límites de solicitudes y cierre de sesión

## Objetivo

Primera parte de la fase 6B. Endurecer operaciones HTTP de la WEB y limitar
el abuso de endpoints públicos sin modificar SQL Server ni contratos de negocio.

**Se preserva** .NET 10, MVC + API + Dapper, JWT, los roles numéricos
Administrador (1), Entrenador (2), Miembro (3), sesiones, perfiles HTTPS,
asignación de entrenadores, reservas, clases, pagos y rutinas.

## Cambios aplicados

### 1. Protección antifalsificación de MVC

Los controladores `AdminController`, `EntrenadorController`,
`MiembroController` y `UsuarioController` ahora incorporan
`[AutoValidateAntiforgeryToken]`. Esta protección valida tokens en
métodos que cambian estado, principalmente POST, y no exige token para GET.

Se inspeccionaron las vistas Razor existentes de esas áreas: los formularios
POST utilizan `asp-action`/`asp-controller` y `method="post"`.
`_ViewImports.cshtml` incorpora `Microsoft.AspNetCore.Mvc.TagHelpers`,
por lo que FormTagHelper genera automáticamente el token para estos formularios.
No se modifican los campos o rutas de las operaciones. El formulario de
acceso temporal en `Empleados_Editar` fue ajustado para evitar un segundo
token antifalsificación explícito.

Los formularios públicos de inicio de sesión, registro y recuperación ya
tenían protección desde la fase 6A.

### 2. Protección de acceso y recuperación en la API

`SuwoGym_API/Program.cs` configura dos políticas por IP mediante el middleware
nativo de ASP.NET Core (sin nuevas bibliotecas):

- `auth-login`: máximo **12 solicitudes por minuto**, sin cola.
- `auth-recovery`: máximo **4 solicitudes por 15 minutos**, sin cola.

Un exceso devuelve **HTTP 429 Too Many Requests** sin ejecutar el endpoint.
`SuwoGym_API/Controllers/HomeController.cs` aplica las políticas a
`IniciarSesionAPI` y `RecuperarAccesoAPI`.

**Limitación:** el conteo es en memoria del proceso; se reinicia al
reiniciar la API, no se comparte entre instancias y una IP compartida
puede alcanzar el límite. Si se publica detrás de un reverse proxy,
configurar explícitamente `ForwardedHeaders` y lista de proxies confiables;
**no confiar en X-Forwarded-For suministrado directamente por el cliente**.
Para producción deberá añadirse limitación distribuida, monitoreo y
protección contra abuso por cuentas.

### 3. Cierre de sesión POST

`HomeController.Logout` deja de admitir GET y requiere
`[HttpPost]` y `[ValidateAntiForgeryToken]`.
`_Layout.cshtml` usa un formulario POST con el botón «Cerrar sesión»
y conserva el estilo mediante `suwo-polish.css`.
No se cambian las otras opciones del menú ni el resto del layout.

**Límite de seguridad:** cerrar sesión vacía la sesión en la WEB, pero
**no revoca criptográficamente** un JWT previamente emitido. La revocación
de tokens/rotación de sesiones queda pendiente de una fase posterior.

### 4. Mensajes seguros

El login ya no devuelve al navegador detalles internos de excepción SQL.
La WEB muestra errores genéricos cuando la API falla y un mensaje específico
cuando el límite de peticiones devuelve 429.

### 5. Recuperación: prevención de bloqueo sin SMTP

Antes de consultar SQL Server o actualizar una contraseña, el endpoint de
recuperación comprueba que `Correos:Correo` y `Correos:AppPassword` estén
configurados. Si faltan, devuelve **HTTP 503**, y la WEB muestra un mensaje
sin revelar detalles internos. Se añadió una prueba automatizada que
comprueba que esta ruta se detiene antes de acceder a SQL.

**Esta protección es parcial:** con SMTP configurado, un fallo posterior
al escribir en SQL podría igualmente dejar la contraseña temporal sin
entregar. La recuperación por enlace de un solo uso sigue siendo necesaria
antes de usar esta funcionalidad con cuentas reales.

### 6. Evitar exposición de hashes de contraseña

El modelo `DatoUsuarioResponseModel` conserva `ContrasennaHash` para las
consultas internas de Dapper y la verificación BCrypt, pero ahora usa
`[JsonIgnore]`: las respuestas JSON de ASP.NET Core no incluyen esa
propiedad. El inicio de sesión sigue devolviendo ID, rol, token y datos
que la WEB necesita para crear la sesión. Una prueba xUnit verifica
que el hash permanece disponible internamente y no se serializa.

## Guía de pruebas manuales en Visual Studio

1. Trabajar en una rama local `feature/fase6b1-csrf-rate-limit-logout`
   (Git → Administrar ramas → origin/feature → crear/cambiar rama).
2. Detener API y WEB y ejecutar **Compilar → Recompilar solución**.
3. Ejecutar ambos proyectos con HTTPS; usar la instancia SQL Server de
   desarrollo ya configurada, sin ejecutar scripts nuevos.
4. Autenticarse como **Administrador**. Probar alta/edición de empleado,
   asignación de entrenador y membresía, edición de clases/horarios y pagos
   (con datos ficticios, evitando cambios irreversibles).
5. Autenticarse como **Entrenador**. Probar edición de rutinas y registro
   de asistencia en una clase de pruebas que ya haya comenzado.
6. Autenticarse como **Miembro**. Probar reservar una clase y cancelar la
   reserva de prueba. No debe aparecer un **400** inesperado.
7. Probar el botón «Cerrar sesión» desde el menú de usuario; debe llevar al
   login. Solicitar manualmente `GET /Home/Logout` no debe cerrar sesión.
8. En navegador (F12 → Red), revisar que los POST MVC incorporen el campo
   oculto `__RequestVerificationToken`. Un POST sin token debe dar
   **HTTP 400**; no utilizar acciones destructivas para comprobarlo.
9. Para el límite de intentos, usar una cuenta ficticia: tras **12**
   peticiones directas al endpoint de login dentro de un minuto, la
   siguiente debe devolver **429**. No probar recuperación real sin SMTP
   seguro, y no forzar contraseñas de cuentas existentes.
10. Revisar consola y `dotnet test .\SuwoGym.slnx -c Release`.
    En GitHub Actions, verificar que la compilación y tests estén verdes.

Los límites son compartidos por IP: si se usa el mismo equipo para probar
los tres roles, distribuir el inicio de sesiones para evitar bloqueos
temporales por haber superado 12 intentos/minuto.

## Validación automatizada

`SuwoGym.Tests/SeguridadFase6B1Tests.cs` comprueba por reflexión:

- Protección antifalsificación de los cuatro controladores por rol.
- Cierre de sesión exclusivamente POST con antifalsificación.
- Políticas de limitación activadas en login y recuperación de API.

No reemplaza una prueba integral en navegador con SQL Server ni una
prueba funcional de los límites del middleware.

## Pendientes de la fase 6B-2

La recuperación actual **genera y guarda una contraseña temporal antes de
enviarla por correo**. Ahora se bloquea antes de consultar SQL cuando
SMTP no está configurado, pero si el envío SMTP falla después de actualizar
la contraseña, el usuario todavía podría perder acceso.
No usar este flujo con cuentas reales.

Para solucionarlo hace falta implementar un enlace de un solo uso
con expiración, tokens almacenados como hash, consumo atómico
y envío de correo sin alterar la contraseña vigente. Requiere diseñar
una migración SQL segura y validar procedimientos existentes contra la
BD local antes de ejecutarla.

Pendientes adicionales: invalidación de JWT después de cambios de contraseña
o rol, auditoría completa de permisos y revocación de secretos históricos.
**No desplegar públicamente hasta resolver estos puntos**.
