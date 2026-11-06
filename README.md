# SUWÓ GYM | Gym Management System

![.NET 10](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-ASP.NET%20Core-239120?logo=sharp&logoColor=white)
![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-CC2927)
[![Build](https://github.com/HenryGonzalezAlguera/SuwoGym/actions/workflows/dotnet-ci.yml/badge.svg?branch=main)](https://github.com/HenryGonzalezAlguera/SuwoGym/actions/workflows/dotnet-ci.yml)

**SUWÓ GYM** es un proyecto académico de gestión de gimnasios desarrollado con una aplicación **ASP.NET Core MVC**, una **API REST** y **SQL Server**. Incluye módulos diferenciados por tipo de usuario para administración, entrenamiento y membresías.

> **English summary:** A .NET 10 gym-management portfolio project featuring separate ASP.NET Core MVC and REST API applications, Dapper/SQL Server persistence, JWT authentication, and role-specific modules for administrators, trainers, and members. This repository is a **source-code portfolio project**, not a publicly deployed service.

## Componentes del sistema

| Área | Funcionalidad identificada en el código |
| --- | --- |
| Administrador | Panel de indicadores, empleados, clientes, planes, membresías, programación de clases y pagos |
| Entrenador | Clientes asignados, rutinas, ejercicios, horarios y registro de asistencia |
| Miembro | Perfil, rutinas, historial de asistencia, pagos, clases y reservas |
| Autenticación | Login, registro y recuperación de acceso; hash BCrypt y tokens JWT |
| Infraestructura | API + MVC, servicios HTTP, Dapper, procedimientos almacenados y SQL Server |

Las funcionalidades se identificaron en controladores y vistas. **No se afirma que todos los recorridos estén probados extremo a extremo**; la validación de bases de datos, roles y operaciones reales forma parte del trabajo siguiente.

## Arquitectura

```mermaid
flowchart LR
    U["Navegador"] --> W["SuwoGym_Web · ASP.NET Core MVC"]
    W --> S["Sesión de la aplicación web"]
    S --> H["HttpClient · Bearer JWT"]
    H --> A["SuwoGym_API · ASP.NET Core"]
    A --> P["Dapper + Procedimientos almacenados"]
    P --> D[("SQL Server · SuwoGym")]
```

- **`SuwoGym_Web/`**: interfaz Razor/MVC, vistas por rol y consumo HTTP de la API.
- **`SuwoGym_API/`**: controladores REST, autenticación JWT y acceso a SQL Server.
- **`Database/`**: esquema y datos iniciales en SQL. No contiene una base de datos real ni credenciales.
- **`IEEE/`**: recursos académicos de modelado.

La API declara autorización por roles numéricos: `1` Administrador, `2` Entrenador, `3` Miembro. La web aplica `SessionAuthorize` con rol explícito en cada controlador (`[SessionAuthorize(1)]`, `(2)` o `(3)`), además de requerir sesión completa para `/Home/Principal`. La API sigue siendo la autoridad final sobre la firma y vencimiento del JWT. **Quedan pendientes** CSRF, recuperación de acceso, comprobaciones E2E y revocación de tokens.

## Tecnologías

| Capa | Herramientas |
| --- | --- |
| Lenguaje / framework | C#, .NET 10, ASP.NET Core |
| Interfaz | MVC / Razor, HTML, CSS, JavaScript y Bootstrap |
| Datos | SQL Server, Dapper, procedimientos almacenados |
| Identidad | JWT Bearer, BCrypt y sesiones web |
| Correo | MailKit / SMTP, configurable fuera del repositorio |
| CI | GitHub Actions: restauración y compilación de ambos proyectos |

## Diseño de interfaz (Fase Visual 1)

SUWÓ GYM utiliza una capa de diseño propia sobre el tema existente: `suwo-polish.css`
y `suwo-polish.js`. Mejoran de forma compartida el sidebar, topbar, tarjetas,
botones, tablas, formularios y navegación activa de las vistas de Administrador,
Entrenador y Miembro, conservando el logo y los estilos originales del equipo.
También se pulieron el panel principal y los indicadores de asistencia.

Consulta [la guía de la Fase Visual 1](docs/FASE_VISUAL_1.md) para verificar la
presentación en Visual Studio, incluidos móvil y escritorio. La compilación de
GitHub Actions no sustituye una inspección visual con datos reales de prueba.

### Fase Visual 2 — Vistas de Miembro

Las nueve vistas de Miembro ahora comparten estilos de presentación mediante
`wwwroot/css/suwo-member.css`: encabezados legibles, tarjetas de detalle,
tablas, indicadores, estado vacío y contadores de listas. La capa se aplica
solo a las páginas del Miembro y mantiene los formularios de reserva y las
rutas MVC existentes. Consulta la [guía de verificación manual](docs/FASE_VISUAL_2_MIEMBRO.md).

### Fase 4 — Diseño de Entrenador

Se aplicó una capa visual específica para el rol Entrenador sobre las **16
vistas Razor existentes**: clientes, ejercicios, rutinas, cronograma y
asistencias. La hoja `suwo-trainer.css` mejora encabezados, tablas, fichas,
formularios, indicadores y estados vacíos, y conserva sin modificaciones los
flujos funcionales.

Consulta la [guía de la Fase 4](docs/FASE4_DISENO_ENTRENADOR.md) y su
checklist de revisión manual desde Visual Studio.

### Fase 5 — Diseño del Administrador

Las **22 vistas Razor de Administrador** tienen una capa de presentación propia
`wwwroot/css/suwo-admin.css` con encabezados, indicadores de Dashboard, tarjetas,
tablas, formularios y estados vacíos. Se activa únicamente para la sesión con
rol Administrador, sin alterar controladores, API, formularios o procedimientos SQL.

Consulta [la guía de la Fase 5](docs/FASE5_DISENO_ADMINISTRADOR.md) para el
checklist de pruebas funcionales, visuales y responsive. La validación manual
de esta fase queda pendiente hasta su comprobación en Visual Studio.

### Fase 6A — Seguridad de autenticación

La API ahora emite y valida JWT con **emisor y audiencia explícitos** además de
firma y expiración. La WEB establece cookies de sesión y antifalsificación
seguras para HTTPS; los formularios de inicio de sesión y recuperación exigen
tokens antifalsificación. Los tokens anteriores a esta actualización dejan de
ser válidos y se debe volver a iniciar sesión.

Consulta la [guía de la fase 6A](docs/FASE6A_SEGURIDAD_AUTENTICACION.md)
para pruebas con los tres roles, límites de seguridad y trabajo pendiente
(fase 6B: recuperación de acceso, auditoría de CSRF y revocación). No se cambia SQL.

### Fase 6B-1 — Protección CSRF, límites de acceso y logout

Los controladores MVC de Administrador, Entrenador, Miembro y Usuario
exigen tokens antifalsificación en POST; el cierre de sesión usa
POST protegido en vez de GET. La API limita los intentos de inicio de
sesión (12/min) y recuperación (4/15 min) por IP, devolviendo HTTP 429
cuando se supera el máximo. **No se requiere SQL adicional**.

Consulta la [guía de fase 6B-1](docs/FASE6B1_CSRF_RATE_LIMIT_LOGOUT.md).
La recuperación de contraseñas por enlace seguro y la revocación de JWT
quedan pendientes de la fase siguiente. **No utilizar recuperación actual
con cuentas reales hasta rediseñar el envío y consumo de tokens.**

### Fase 6B-2 — Recuperación por enlace seguro

La recuperación pública deja de generar contraseñas temporales. Se usan
tokens aleatorios de 256 bits, cuyo hash SHA-256 se almacena en SQL Server,
con **15 minutos** de vigencia y **un solo uso**. La contraseña original
se mantiene hasta completar la transacción de restablecimiento.

La nueva vista `Home/RestablecerContrasenna` requiere una contraseña
nueva y confirmación. El email incluye un enlace HTTPS configurado y
la API no revela si una cuenta está registrada.

**Requiere ejecutar manualmente** `Database/07_Recuperacion_Enlaces_Seguros.sql`
en una base de desarrollo previamente respaldada. La rama no aplica
migraciones automáticamente y no modifica usuarios preexistentes.

Consulta [la guía de implementación y pruebas de fase 6B-2](docs/FASE6B2_RECUPERACION_ENLACES.md).
**No utilizar recuperación con cuentas reales** hasta validar SQL, SMTP,
expiración, consumo único y revocación de tokens JWT.

## Registro y primera cuenta local

Si la tabla de usuarios está vacía o no puedes crear una cuenta, consulta la
[guía de diagnóstico y reparación del registro](docs/REGISTRO_Y_CATALOGOS_LOCALES.md).
Incluye SQL de **solo lectura**, un script idempotente para completar roles y
planes **sin usuarios predefinidos**, verificación de API antigua ocupando el
puerto 7129 y pruebas del formulario de registro.

## Acceso inicial de Administrador (entorno de desarrollo)

El registro público asigna rol Miembro por seguridad. Para probar el primer
Administrador sobre una base de pruebas, consulta la
[guía Fase 3](docs/FASE3_ADMIN_Y_DATOS_DE_PRUEBA.md). Se utiliza una cuenta
ficticia independiente y un script SQL manual con validación de rol y rollback.
No existen credenciales administrativas predeterminadas ni endpoint público
para conceder privilegios.

## Gestión de clases por Administrador (Fase 3B)

Para programar horarios, primero se deben registrar actividades en el catálogo.
La sección **Admin → Clases** permite listar y crear tipos de clase sin alterar
los módulos de reservas. **Instala primero en tu instancia SQL Server**
`Database/06_Registrar_Clases_Catalogo.sql` mediante SSMS, después de
revisar el script. Sigue la
[guía de Fase 3B](docs/FASE3B_CATALOGO_CLASES.md).

## Acceso de Entrenador en desarrollo (Fase 3D)

Las cuentas de empleados reciben una clave temporal automática por correo
si SMTP está configurado. Para una base de desarrollo sin SMTP, un Administrador
puede abrir **Empleados → Editar** y generar una **nueva contraseña temporal**
para un Entrenador activo. La pantalla muestra la clave una única vez, sin
guardarla en el repositorio; al iniciar sesión, el Entrenador debe cambiarla.
La opción y el endpoint solo están habilitados en `Development`.
Se utiliza `spActualizarContrasenna` existente; **no se instala SQL adicional**.
Consulta la [guía de la Fase 3D](docs/FASE3D_ACCESO_ENTRENADOR.md).

## Ejecución local

**Requisitos:** SDK de .NET 10, SQL Server y una instancia local de base de datos configurada. Para usar Visual Studio, abre `SuwoGym.slnx`.

1. Clonar el repositorio:

```powershell
git clone https://github.com/HenryGonzalezAlguera/SuwoGym.git
cd SuwoGym
```

2. Revisar [Database/README.md](Database/README.md). Los archivos públicos disponibles son `01_SuwoGym_Estructura.sql` y `02_SuwoGym_DatosIniciales.sql`. El primer Administrador local se habilita mediante el script manual `Database/05_Habilitar_Primer_Admin_Desarrollo.sql`. El catálogo de clases requiere `Database/06_Registrar_Clases_Catalogo.sql`. No ejecutes scripts destructivos sobre la base ya utilizada.

3. **Configurar secretos locales antes de iniciar la API**. No hay clave JWT ni contraseña SMTP predeterminadas. Sigue los pasos en [SEGURIDAD_Y_CONFIGURACION.md](docs/SEGURIDAD_Y_CONFIGURACION.md) para revocar credenciales anteriores y crear un nuevo secreto de firma con `dotnet user-secrets`.

4. Restaurar y compilar:

```powershell
dotnet restore .\SuwoGym.slnx
dotnet build .\SuwoGym.slnx -c Release --no-restore
```

5. Ejecutar API y WEB en terminales separadas, con el ambiente `Development` y la configuración local correcta:

```powershell
dotnet run --project .\SuwoGym_API\SuwoGym_API.csproj --launch-profile https
```

```powershell
dotnet run --project .\SuwoGym_Web\SuwoGym_Web.csproj --launch-profile https
```

La configuración versionada de la WEB apunta a `https://localhost:7129` y su perfil HTTPS normalmente se abre en `https://localhost:7035`. Confirma puertos en `launchSettings.json` y confía en el certificado de desarrollo solo en tu equipo (`dotnet dev-certs https --trust`) si es necesario.

**Importante:** compilar correctamente **no garantiza** que los procedimientos SQL, los correos o los flujos de roles funcionen con una instalación nueva. Para la validación completa falta preparar una cuenta de demostración y una base local coherente.

## Seguridad de acceso MVC y pruebas

La [fase 2](docs/FASE2_SESIONES_Y_ROLES.md) documenta las validaciones de sesión y la matriz de autorización por rol. Puedes ejecutar las pruebas sin SQL Server con `dotnet test SuwoGym.Tests/SuwoGym.Tests.csproj`.

## Evidencia y límites actuales

| Verificación | Situación |
| --- | --- |
| Código fuente API y MVC | Disponible |
| Scripts SQL de estructura y datos iniciales | Disponibles |
| Compilación automatizada | Definida en [CI](.github/workflows/dotnet-ci.yml) |
| Pruebas automatizadas de sesión y permisos MVC | Disponibles en [SuwoGym.Tests](SuwoGym.Tests/) |
| Pruebas funcionales con SQL Server | **Validaciones manuales en entorno local**: altas de clases/horarios, reservas, asistencia, acceso por roles y ejercicios; el usuario también indicó pruebas de rutinas y pagos. No equivalen a pruebas E2E automatizadas. |
| Capturas reales del sistema | **Pendientes** |
| Despliegue público | **No realizado** |

## Siguientes mejoras

Se priorizan pruebas de integración SQL, revisión de formularios CSRF, endurecimiento de recuperación de acceso, validación SQL desde cero, capturas reales y documentación de demo. Evitamos publicar datos personales, credenciales, contraseñas o tokens.

## Autoría

Proyecto de portafolio de [HenryGonzalezAlguera](https://github.com/HenryGonzalezAlguera), desarrollado en .NET y SQL Server con fines académicos y de demostración.
