/*
 SUWÓ GYM — Fase 8B / Azure SQL STAGING
 ÚNICA finalidad: inicializar catálogos de roles, SIN crear usuarios.
 NO COPIAR DATOS desde SuwoGym ni ejecutar 02_SuwoGym_DatosIniciales.sql.
 PROTECCIÓN: lote único sin GO ni USE. La conexión debe apuntar
 explícitamente a la base SuwoGym_Staging.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

IF DB_NAME() <> N'SuwoGym_Staging'
    THROW 53001, 'BLOQUEADO: ejecutar exclusivamente sobre SuwoGym_Staging.', 1;

IF OBJECT_ID(N'dbo.Rol', N'U') IS NULL OR
   OBJECT_ID(N'dbo.Usuario', N'U') IS NULL OR
   OBJECT_ID(N'dbo.spRecuperacionConsumir', N'P') IS NULL OR
   OBJECT_ID(N'dbo.spSeguridadConsultarVersionJwt', N'P') IS NULL OR
   OBJECT_ID(N'dbo.trg_Usuario_RevocarJwt', N'TR') IS NULL
    THROW 53002, 'BLOQUEADO: falta esquema completo o migraciones 07/08.', 1;

IF EXISTS (SELECT 1 FROM dbo.Usuario) OR
   EXISTS (SELECT 1 FROM dbo.Pago) OR
   EXISTS (SELECT 1 FROM dbo.ReservaClase)
    THROW 53003, 'BLOQUEADO: staging debe estar sin usuarios ni transacciones.', 1;

BEGIN TRY
    BEGIN TRANSACTION;

    IF NOT EXISTS (SELECT 1 FROM dbo.Rol WHERE IdRol = 1)
        INSERT dbo.Rol (IdRol, NombreRol, Descripcion)
        VALUES (1, N'Administrador', N'Rol de demostración');

    IF NOT EXISTS (SELECT 1 FROM dbo.Rol WHERE IdRol = 2)
        INSERT dbo.Rol (IdRol, NombreRol, Descripcion)
        VALUES (2, N'Entrenador', N'Rol de demostración');

    IF NOT EXISTS (SELECT 1 FROM dbo.Rol WHERE IdRol = 3)
        INSERT dbo.Rol (IdRol, NombreRol, Descripcion)
        VALUES (3, N'Miembro', N'Rol de demostración');

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;

SELECT DB_NAME() AS BaseVerificada,
       (SELECT COUNT(*) FROM dbo.Usuario) AS Usuarios,
       (SELECT COUNT(*) FROM dbo.Pago) AS Pagos,
       (SELECT COUNT(*) FROM dbo.Rol WHERE IdRol IN (1, 2, 3)) AS RolesBase;
