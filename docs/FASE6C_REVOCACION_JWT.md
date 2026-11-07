# Fase 6C — Revocación de sesiones y JWT

## Objetivo

Los JWT duraban 30 minutos. La API verificaba firma, emisor, audiencia y
vencimiento, pero **no** consultaba el estado actual de usuario, rol o
contraseña después del login. Esta fase añade revocación inmediata mediante
una versión persistente en SQL Server y evita que la WEB conserve pantallas
privadas cuando la API rechaza el token.

## Implementación

- `dbo.Usuario.VersionJwt`: `uniqueidentifier NOT NULL`, valor inicial
  `NEWID()`. No cambia ningún hash de contraseña ni elimina registros.
- `dbo.trg_Usuario_RevocarJwt`: actualiza `VersionJwt` solo cuando
  cambian **ContrasennaHash**, **IdRol** o **Estado**. Se ejecuta en la misma
  transacción del UPDATE. Cubre `spActualizarContrasenna`,
  `spRecuperacionConsumir`, `spEditarEmpleado` y
  `spEditarEstadoCliente` sin reescribir esos procedimientos.
- `dbo.spSeguridadConsultarVersionJwt`: devuelve versión, rol y estado
  de un usuario. No devuelve el hash BCrypt.
- El login emite el JWT con claim firmado `VersionJwt` obtenido de SQL.
- Cada petición autenticada en la API comprueba `IdUsuario`, `IdRol`,
  el rol firmado, estado activo y versión actual por SQL. Si no coincide
  (o SQL está caído) se rechaza el token.
- `GET /api/Usuario/ValidarSesionAPI` devuelve 204 solamente si
  JWT y estado siguen siendo válidos.
- La WEB solicita esa validación antes de cada ruta privada de
  `Admin`, `Entrenador`, `Miembro`, `Usuario` y `Home/Principal`.
  En caso de 401/403 limpia la sesión y redirige a login. Si la API
  falla devuelve 503 y no permite procesar operaciones privadas.
- Se corrigieron 81 redirecciones internas `GET /Home/Logout` obsoletas
  (Logout es POST con CSRF) para limpiar sesión y volver a login.
  Los botones de logout voluntario siguen usando POST con CSRF.

## Migración SQL manual (obligatoria ANTES de arrancar esta rama)

1. Respaldar la base de desarrollo **SuwoGym** en SQL Server.
2. Abrir `Database/08_Revocacion_JWT_Sesiones.sql` desde la rama
   `feature/fase6c-revocacion-jwt-sesiones` en SSMS. Confirmar
   `USE [SuwoGym]` y el servidor correcto.
3. Ejecutar **solo** el script `08`. No volver a ejecutar `01`,
   `07` ni borrar usuarios. El script se puede repetir sin añadir
   nuevamente la columna y recrea trigger/SP si ya existen.
4. Verificar el resultado:
   `ColumnaVersionJwt = OK` y los identificadores
   `TriggerRevocacion` y `ProcedimientoConsulta` distintos de NULL.
5. Reiniciar API/WEB con los perfiles HTTPS. Los JWT anteriores
   **se invalidan** por no incluir `VersionJwt`: todos deben entrar
   nuevamente con su contraseña vigente.
6. Si falta el script `08`, la emisión/validación de tokens se
   rechaza por seguridad. **No fusionar esta rama ni desplegarla antes
   de aplicar la migración.**

## Pruebas end-to-end en base de desarrollo

Probar únicamente con cuentas y datos ficticios, nunca compartiendo
tokens, claves, correos o hashes:

1. Iniciar sesión como Administrador, Entrenador y Miembro. Navegar a
   Dashboard, rutinas, reservas y pagos; no deben aparecer respuestas
   401/503 en el flujo normal.
2. En navegador A iniciar sesión con una cuenta de prueba. En B cambiar
   la contraseña de esa cuenta usando el nuevo enlace de recuperación.
   Al refrescar cualquier ruta privada en A debe volver al login y
   las llamadas a API con el JWT anterior deben responder 401.
3. Con un entrenador de prueba, iniciar sesión y desde Admin cambiar
   temporalmente su estado o rol. Su JWT anterior ya no debe
   autorizar operaciones. Al volver a activar la cuenta, el token
   **no debe recuperarse**; debe iniciar sesión de nuevo.
4. Cambiar una contraseña desde **Configuración** y confirmar que
   cierra la sesión sin generar un GET a `/Home/Logout`.
5. Detener brevemente la API de desarrollo (sin operaciones en marcha);
   ninguna ruta privada debe ofrecer datos como si el token fuera válido.
6. Registrar usuario/empleado nuevo en base de prueba y comprobar su
   primer inicio de sesión: el default `NEWID()` le genera versión.
7. Ejecutar `dotnet test .\SuwoGym.slnx -c Release`,
   y comprobar GitHub Actions, así como las advertencias del compilador.

Las pruebas unitarias incluidas cubren comparación de versiones,
estados/roles y middleware de la WEB con HTTP 204/401/403/503 simulados.
No sustituyen las pruebas SQL transaccionales ni integraciones con SMTP.

## Límites y trabajo pendiente

- Se realiza **una consulta SQL por solicitud JWT** y una petición
  adicional desde cada página MVC privada. Este diseño prioriza
  revocación inmediata y simplicidad; medir rendimiento y ajustar la
  arquitectura antes de escalar.
- El cierre voluntario de sesión elimina la sesión del navegador,
  pero no revoca todos los dispositivos. La revocación global ocurre
  al cambiar credenciales, rol o estado.
- Las API públicas `[AllowAnonymous]` (login/registro/recuperación)
  no requieren un JWT, como antes.
- Mantener claves JWT y contraseñas SMTP en User Secrets, nunca en Git.
  Revocar cualquier clave que haya quedado visible en capturas o commits.
- No afirmar que la aplicación ya está lista para producción sin
  ejecutar el checklist de la fase 7 de integración y carga.
