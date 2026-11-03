# Fase 3 — Primera cuenta Administrador local y pruebas de gestión

## Diagnóstico técnico verificado

El procedimiento `dbo.spRegistrarUsuario` siempre crea `IdRol = 3`
(**Miembro**). Esto es correcto para registro público. El procedimiento
`dbo.spIniciarSesionUsuario` devuelve el rol actual de un usuario activo,
y `Home/IniciarSesionAPI` construye el token JWT con ese rol después de
comprobar BCrypt.

La WEB protege `AdminController` mediante `[SessionAuthorize(1)]`; la API
aplica `[Authorize(Roles = "1")]`. Para abrir las pantallas de gestión se
necesita iniciar sesión **de nuevo** con un usuario cuyo `IdRol = 1`
y obtener un token nuevo. Cambiar el rol en SQL no altera el token ya emitido.

No existe un flujo público para registrar la primera cuenta Administrador.
Esto es deliberado desde el punto de vista de seguridad: una cuenta pública
no debe poder elegirse privilegios de Administrador.

## Procedimiento controlado — solo base de desarrollo

1. Asegúrate de que tu cuenta actual de **Miembro** funciona y mantenla así.
   La usaremos para probar reservas y pagos después.
2. Cierra sesión de SUWÓ GYM o abre una ventana privada de Edge.
3. Visita `https://localhost:7035/Home/Registro`. Registra una **segunda
   cuenta ficticia de prueba**, con un correo distinto, identificación
   ficticia distinta y contraseña segura elegida localmente. Elige cualquiera
   de los planes del catálogo. Esta segunda cuenta nacerá como Miembro.
   **No publiques su contraseña ni datos personales.**
4. Abre SSMS, conéctate a la instancia de desarrollo que utiliza
   `SuwoGym_API` y verifica que la base se llama `SuwoGym`.
5. Abre el archivo
   `Database/05_Habilitar_Primer_Admin_Desarrollo.sql`.
   **Copia su contenido a una nueva consulta de SSMS** (no modifiques ni
   guardes el archivo del repositorio con datos locales).
6. En esa consulta, sustituye exclusivamente el valor literal de
   `@CorreoDePrueba` por el correo de la segunda cuenta ficticia y ejecuta F5.
   La transacción comprobará que no exista otro administrador activo, que
   la cuenta exista y sea Miembro activo, y que el rol Administrador exista.
   Cambiará exclusivamente `dbo.Usuario.IdRol` en esa cuenta.
7. Confirma que el resultado termina con `IdRol = 1`,
   `NombreRol = Administrador` y `Estado = 1`.
8. Cierra sesión completamente en la WEB. Si hay una sesión anterior,
   inicia en una ventana privada, para obligar a generar un JWT nuevo.
9. Ingresa con la **segunda cuenta ficticia** desde la WEB. La pantalla
   `/Home/Principal` debe mostrar las opciones de Administrador. Abre
   `/Admin/Dashboard`.

**Importante:** el registro público crea una membresía asociada a esta
segunda cuenta antes de elevarla a Administrador. Esa membresía de
demostración permanece para evitar eliminar datos; puede aparecer en
estadísticas. Este procedimiento es **únicamente un bootstrap de desarrollo**
y no constituye un mecanismo de aprovisionamiento de empleados en producción.
Para un despliegue real se requiere un flujo interno de aprovisionamiento
de identidades con control de privilegios y auditoría.

### Verificar que no se modificó la cuenta Miembro original

En SSMS ejecuta una consulta de solo lectura:

```sql
USE SuwoGym;
SELECT IdUsuario, Nombre, CorreoElectronico, IdRol, Estado
FROM dbo.Usuario
ORDER BY IdUsuario;
```

Debería existir tu cuenta Miembro original con `IdRol = 3` y la segunda
cuenta ficticia con `IdRol = 1`.

## Comprobaciones en el panel

| Área WEB | Ruta MVC | Resultado inicial esperado |
| --- | --- | --- |
| Principal | `/Home/Principal` | Muestra navegación de Administrador |
| Dashboard | `/Admin/Dashboard` | KPIs (muchos en cero si la base está vacía) |
| Empleados | `/Admin/Empleados_Lista` | Listado y botón de agregar empleado |
| Clientes | `/Admin/Clientes_Lista` | Miembro original visible |
| Membresías | `/Admin/Planes_Lista` | Planes Mensual, Trimestral y Anual |
| Horarios | `/Admin/Horarios_Lista` | Lista posiblemente vacía, opción agregar |

El primer recorrido de pruebas no requiere crear horarios ni alterar datos
de la cuenta Miembro. Después usaremos Administrador para configurar
empleados/entrenadores y clases ficticias, y volveremos a probar las vistas
de Miembro con información.

## Advertencias y pendientes

- No se instaló un endpoint de bootstrap ni un usuario Administrador
  predeterminado. El SQL es manual, de una sola operación y está protegido
  con comprobaciones y rollback.
- **No** ejecutes los scripts históricos que recrean/destruyen la base.
- El correo de ejemplo debe sustituirse solo en la consulta local de SSMS.
- El sistema usa JWT de 30 minutos. Cerrar sesión y entrar de nuevo
  es obligatorio tras cambiar roles en SQL.
- Rotar la clave JWT de desarrollo previamente expuesta en la conversación
  y cualquier contraseña SMTP antigua antes de publicar el sistema.
- Sigue pendiente una validación de extremo a extremo con SQL Server real,
  revisión de CSRF en acciones de Administrador y pruebas visuales de
  pantallas administrativas.
- No reutilices una cuenta de producción para estas pruebas.
