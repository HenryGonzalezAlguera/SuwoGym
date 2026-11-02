# Autenticación de SUWÓ GYM — Estado actual del código

> **Documento actualizado:** esta guía reemplaza las referencias históricas a SHA-256, Entity Framework Core, migraciones automáticas y rutas `/api/auth/*`, que no corresponden con los archivos de código actuales.

## Arquitectura implementada

- **`SuwoGym_API`**: ASP.NET Core 10, controladores REST, Dapper, SQL Server, JWT Bearer y BCrypt para verificar contraseñas.
- **`SuwoGym_Web`**: ASP.NET Core MVC, Razor, HttpClient, almacenamiento de token JWT en sesión y filtro `SessionAuthorizeAttribute`.
- **Roles de la API**: `Administrador` (ID 1), `Entrenador` (ID 2), `Miembro` (ID 3). La API declara restricciones `[Authorize(Roles = "1")]`, `"2"` o `"3"` en sus respectivos controladores.
- **SQL**: consultas y procedimientos almacenados mediante Dapper; **no** se encontraron DbContext ni migraciones EF en los proyectos actuales.

## Rutas identificadas en el código

| Operación | Método y ruta de la API |
| --- | --- |
| Iniciar sesión | `POST /api/Home/IniciarSesionAPI` |
| Crear miembro | `POST /api/Home/RegistroAPI` |
| Consultar planes | `GET /api/Home/MembresiasAPI` |
| Recuperar acceso | `POST /api/Home/RecuperarAccesoAPI` |
| Consultar perfil autenticado | `GET /api/Usuario/ConsultarUsuarioAPI` |
| Cambiar contraseña | `PUT /api/Usuario/CambiarContrasennaAPI` |

La web utiliza rutas MVC de `Home`, `Admin`, `Entrenador`, `Miembro` y `Usuario`. Los métodos de la API pueden requerir JWT según sus atributos.

## Seguridad y verificación

Las contraseñas de cuentas se contrastan con BCrypt en el login, y la web calcula hashes BCrypt para los flujos de registro y cambio de contraseña. La API firma tokens JWT mediante un secreto que **debe venir del entorno local**, nunca de Git.

**Esta implementación todavía necesita revisión adicional** de autorización en las rutas web, protección CSRF, recuperación de acceso, manejo de excepciones y pruebas de los tres perfiles. No es apropiado afirmar que está lista para producción.

Para instalar y configurar consulta:
- [README principal](README.md).
- [Seguridad y User Secrets](docs/SEGURIDAD_Y_CONFIGURACION.md).
- [Instrucciones de base de datos](Database/README.md).

El código fuente refleja los módulos de una aplicación funcional en desarrollo; la validación end-to-end depende de la instancia SQL Server y de cuentas de prueba.
