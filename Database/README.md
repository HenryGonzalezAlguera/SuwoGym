# Base de datos SUWÓ Gym (SQL Server)

## Archivos disponibles

- `01_SuwoGym_Estructura.sql`: estructura, tablas, índices y procedimientos almacenados.
- `02_SuwoGym_DatosIniciales.sql`: datos iniciales de roles, clases y planes de membresía.

**Importante:** el antiguo archivo `03_SuwoGym_AdministradorInicial_Plantilla.sql` no existe. El primer Administrador **de una base de desarrollo local** se habilita ahora mediante el script manual controlado [`05_Habilitar_Primer_Admin_Desarrollo.sql`](05_Habilitar_Primer_Admin_Desarrollo.sql), después de registrar una segunda cuenta ficticia desde la WEB. No es un mecanismo de producción.

## Recuperación segura del registro

Para diagnosticar una instalación donde no aparecen usuarios o falla el
registro, usa [`03_Diagnostico_Registro.sql`](03_Diagnostico_Registro.sql)
(solo lectura). Si faltan los catálogos de roles/planes, el script
[`04_Catalogos_Registro_Sin_Usuarios.sql`](04_Catalogos_Registro_Sin_Usuarios.sql)
inserta únicamente los registros ausentes, sin usuarios ni contraseñas.

La guía detallada está en
[`docs/REGISTRO_Y_CATALOGOS_LOCALES.md`](../docs/REGISTRO_Y_CATALOGOS_LOCALES.md).

**Advertencia:** `02_SuwoGym_DatosIniciales.sql` incluye usuarios cuyos hashes
figuran en el repositorio público. No deben considerarse credenciales seguras
ni usarse para crear acceso de demostración. Los scripts históricos dentro de
`SuwoGym_API/Scripts/` no son equivalentes al esquema actual; uno contiene
una instrucción destructiva `DROP DATABASE`.

## Primera cuenta Administrador (solo desarrollo)

Para probar la administración sin publicar contraseñas, sigue la
[guía de Fase 3](../docs/FASE3_ADMIN_Y_DATOS_DE_PRUEBA.md). El script
[`05_Habilitar_Primer_Admin_Desarrollo.sql`](05_Habilitar_Primer_Admin_Desarrollo.sql)
requiere una cuenta ficticia previamente registrada y valida que no exista
otro administrador activo. Se ejecuta manualmente y no cambia contraseñas.
**No ejecutes este script contra bases reales o de producción.**

## Catálogo de clases (Fase 3B)

El script [`06_Registrar_Clases_Catalogo.sql`](06_Registrar_Clases_Catalogo.sql)
crea o actualiza únicamente el procedimiento `dbo.spRegistrarClase` requerido
por **Admin → Clases → Nueva clase**. Se debe ejecutar **manualmente** en SSMS
sobre la instancia local SuwoGym; no recrea ni inserta usuarios, clases,
horarios o membresías. Consulta la
[guía](../docs/FASE3B_CATALOGO_CLASES.md). El campo `IdClase` existente no
es `IDENTITY`; el procedimiento asigna el siguiente identificador de forma
transaccional con bloqueo exclusivo.

## Recuperación con enlaces seguros (fase 6B-2)

El script incremental [`07_Recuperacion_Enlaces_Seguros.sql`](07_Recuperacion_Enlaces_Seguros.sql)
crea una tabla de tokens SHA-256 y cuatro procedimientos para emisión,
validación, consumo atómico y revocación de enlaces de 15 minutos.

**Ejecución manual en SSMS**, solo después de respaldar la BD local
`SuwoGym`. No ejecuta ni modifica `01`, `spActualizarContrasenna`,
`Usuario` o contraseñas actuales. Es necesario para que funcione la
nueva API de recuperación; compilar el proyecto no lo ejecuta.

Ver [procedimiento de instalación y pruebas](../docs/FASE6B2_RECUPERACION_ENLACES.md).

## Revocación de JWT — Fase 6C

La migración incremental manual [`08_Revocacion_JWT_Sesiones.sql`](08_Revocacion_JWT_Sesiones.sql)
añade `Usuario.VersionJwt`, un trigger de revocación ante cambios
de contraseña, rol o estado, y el procedimiento de consulta
`spSeguridadConsultarVersionJwt`.

**Ejecutar en SSMS sobre una base de desarrollo respaldada, antes
de iniciar la API de fase 6C.** No modifica contraseñas existentes ni
requiere volver a ejecutar scripts anteriores.
Guía y checklist: [fase 6C](../docs/FASE6C_REVOCACION_JWT.md).

## Preparación segura

1. Usa una instancia SQL Server de prueba, nunca una base de datos productiva con información real.
2. Respaldar previamente cualquier base existente. Revisa `01` antes de ejecutarlo: **puede crear o modificar objetos y datos**.
3. En una instalación nueva, ejecutar `01_SuwoGym_Estructura.sql` desde SQL Server Management Studio.
4. Ejecutar `02_SuwoGym_DatosIniciales.sql`.
5. Configurar `ConnectionStringsApi:DefaultConnection` según tu servidor, usando [la guía de secretos](../docs/SEGURIDAD_Y_CONFIGURACION.md).
6. Para probar inicio de sesión, todavía se requiere validar un procedimiento de creación de cuenta administradora y comprobar que los scripts SQL corresponden exactamente a los procedimientos consumidos por la API.

## No publicar

No incluyas respaldos `.bak`, archivos `.mdf`, `.ldf`, datos reales de socios, tokens, contraseñas, claves JWT o credenciales SMTP. Evita subir cualquier script local con usuarios o contraseñas.

El repositorio es **fuente del proyecto y esquema de demostración**, no un paquete de instalación de producción.
