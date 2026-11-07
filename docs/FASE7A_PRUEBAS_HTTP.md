# Fase 7A — Pruebas HTTP de autenticación, roles y revocación

## Objetivo

El proyecto ya cuenta con pruebas unitarias de modelos, filtros y middlewares.
La fase 7A agrega una capa intermedia: **peticiones HTTP reales** de la API
mediante `Microsoft.AspNetCore.Mvc.Testing` / `TestServer`, con base de
datos **simulada** únicamente dentro del proceso de pruebas.

No se alteran los endpoints, modelos de negocio, procesos de cobro,
procedimientos SQL, credenciales ni vistas de usuarios reales.

## Qué incluye

`SuwoGym.Tests/IntegracionHttpSeguridadTests.cs` inicializa la API con
una clave JWT **ficticia** y un servicio de versión JWT simulado.
Se hacen solicitudes HTTP para comprobar:

| Escenario | Respuesta esperada |
| --- | --- |
| GET `/api/Usuario/ValidarSesionAPI` sin JWT | HTTP 401 |
| JWT válido para Administrador, Entrenador o Miembro | HTTP 204 |
| JWT de otro rol en rutas exclusivas | HTTP 403 |
| Cambio de versión JWT en SQL simulado | HTTP 401 |
| Cuenta desactivada | HTTP 401 |
| Cambio de rol | HTTP 401 |
| Fallo del servicio de versiones (SQL simulado) | HTTP 401 |
| JWT con firma distinta | HTTP 401 |

`SuwoGym.Tests/ContratosAutorizacionApiTests.cs` confirma por reflexión
que los controladores `Admin`, `Entrenador` y `Miembro` conservan
sus respectivas políticas `[Authorize(Roles = "1"|"2"|"3")]`;
`Usuario` conserva `[Authorize]`; y los métodos públicos de
login/recuperación mantienen límites de solicitudes.

La API se inicia de verdad en memoria y atraviesa el middleware de
autenticación/autorización. Las consultas a SQL se reemplazan por el
servicio `ISeguridadJwtVersionService` simulado **solo en las pruebas**.
No hay dependencia de Azure, SQL Server local ni SMTP en GitHub Actions.

## Integración continua

En `.github/workflows/dotnet-ci.yml`:

1. Revisar que `appsettings.json` no incluya secretos.
2. Restaurar y compilar solución .NET 10 en Release.
3. Ejecutar toda la suite, incluidos los HTTP tests.
4. Guardar un reporte **TRX** como artefacto `suwo-tests-trx`
   (14 días), accesible desde la ejecución de GitHub Actions.

Una prueba fallida bloquea el paso de tests y la verificación del PR.
No confundas el resultado CI con una certificación de seguridad.

## Cómo probar en Visual Studio

1. Detener proyectos si están ejecutándose.
2. Git → Administrar ramas → Recuperar y cambiar a
   `feature/fase7a-pruebas-http-roles-revocacion`.
3. **Compilar → Recompilar solución**.
4. Menú **Prueba → Explorador de pruebas**, ejecutar toda la suite.
5. También puedes abrir terminal en la carpeta con
   `SuwoGym.slnx` y ejecutar:

   ```powershell
   dotnet restore .\SuwoGym.slnx
   dotnet build .\SuwoGym.slnx -c Release
   dotnet test .\SuwoGym.slnx -c Release
   ```

6. Verificar que las nuevas pruebas `IntegracionHttpSeguridadTests`
   y `ContratosAutorizacionApiTests` están aprobadas.
7. Abrir el PR, verificar CI verde y el artefacto TRX.
8. Como prueba rápida de regresión visual, abrir Dashboard,
   una rutina, una reserva y el perfil con sus roles correctos.

No ejecutar scripts SQL para la fase 7A: la BD se mantiene como
quedó tras la fase 6C.

## Fase 7B — End-to-end con SQL real (pendiente)

Preparar una base **aislada**, restaurada desde backup **sin datos reales**,
con los scripts `07` y `08` aplicados. Probar antes de publicar:

- Cambio de contraseña con el usuario abierto en dos navegadores:
  token anterior rechazado inmediatamente.
- Desactivación/reactivación y cambio de rol desde Admin:
  tokens anteriores no vuelven a ser válidos.
- Vencimiento de enlaces de recuperación, doble uso e intentos
  simultáneos de consumo del mismo enlace.
- Transacciones y restricciones al crear/cancelar reservas.
- Generación de pagos, impuestos, conciliación e integridad de relaciones.
- Manejo de errores HTTP con datos de entrada inválidos.
- Verificación de secretos, TLS, cabeceras y configuración de despliegue.

No se ha ejecutado SQL real, SMTP ni una carga de rendimiento en CI.
Esos son controles de aceptación pendientes, **no** resultados de las
pruebas en memoria.

## Riesgos conocidos por revisar

- La versión actual valida contra SQL por cada JWT y, desde la WEB,
  realiza una petición adicional por página privada. Medir latencia y
  throughput antes de escalar.
- Hay advertencias de nulabilidad en algunas vistas Razor; no se
  fuerza `TreatWarningsAsErrors` todavía para no romper el proyecto
  sin reparar primero esas advertencias.
- La sesión se borra del navegador con logout, pero un JWT sigue
  vigente hasta su expiración si no se rota la versión o se implementa
  revocación individual de dispositivo.
- Asegurarse de haber rotado claves que aparecieron en capturas o
  commits históricos antes de una publicación real.
