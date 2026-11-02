# Base de datos SUWÓ Gym (SQL Server)

## Archivos disponibles

- `01_SuwoGym_Estructura.sql`: estructura, tablas, índices y procedimientos almacenados.
- `02_SuwoGym_DatosIniciales.sql`: datos iniciales de roles, clases y planes de membresía.

**Importante:** una versión anterior de esta guía mencionaba `03_SuwoGym_AdministradorInicial_Plantilla.sql`; ese archivo **no está incluido** en el repositorio. No se deben inventar tablas, procedimientos ni contraseñas para suplirlo. La preparación documentada y validada del primer administrador forma parte de una fase posterior.

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
