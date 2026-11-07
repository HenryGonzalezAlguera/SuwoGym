/*
 SUWÓ GYM — Fase 7B. Ejecutar solo después de crear la BD
 SuwoGym_Test_7B de ESQUEMA, SIN DATOS.
 SEGURIDAD: este archivo no usa USE ni GO. La validación y el marcado
 se procesan en UN SOLO LOTE. Un THROW impide ejecutar las instrucciones
 restantes, incluso si SSMS estaba conectado a la base equivocada.
 Antes de ejecutarlo, seleccione SuwoGym_Test_7B en la lista de
 bases de datos de la ventana de consulta de SSMS.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

IF DB_NAME() <> N'SuwoGym_Test_7B'
    THROW 52001,
        'BLOQUEADO: seleccione primero la base SuwoGym_Test_7B en SSMS.',
        1;

IF OBJECT_ID(N'dbo.Usuario', N'U') IS NULL OR
   OBJECT_ID(N'dbo.Rol', N'U') IS NULL OR
   OBJECT_ID(N'dbo.Pago', N'U') IS NULL OR
   OBJECT_ID(N'dbo.ReservaClase', N'U') IS NULL OR
   OBJECT_ID(N'dbo.spRecuperacionConsumir', N'P') IS NULL OR
   OBJECT_ID(N'dbo.spReservarClaseMiembro', N'P') IS NULL OR
   OBJECT_ID(N'dbo.trg_Usuario_RevocarJwt', N'TR') IS NULL OR
   COL_LENGTH(N'dbo.Usuario', N'VersionJwt') IS NULL
    THROW 52002,
        'BLOQUEADO: falta el esquema completo o las migraciones 07/08.',
        1;

-- Una base de pruebas NUEVA no contiene usuarios ni transacciones.
-- No autorizar una copia de una base real ni residuos de ejecuciones.
IF EXISTS (SELECT 1 FROM dbo.Usuario) OR
   EXISTS (SELECT 1 FROM dbo.Pago) OR
   EXISTS (SELECT 1 FROM dbo.ReservaClase) OR
   EXISTS (SELECT 1 FROM dbo.MembresiaUsuario)
    THROW 52003,
        'BLOQUEADO: la base debe contener solo esquema, sin usuarios ni operaciones.',
        1;

BEGIN TRY
    BEGIN TRANSACTION;

    -- Únicamente datos de catálogo imprescindibles para las FK de tests.
    IF NOT EXISTS (SELECT 1 FROM dbo.Rol WHERE IdRol = 1)
        INSERT dbo.Rol(IdRol, NombreRol, Descripcion)
        VALUES (1, N'Administrador', N'Rol ficticio de pruebas');

    IF NOT EXISTS (SELECT 1 FROM dbo.Rol WHERE IdRol = 2)
        INSERT dbo.Rol(IdRol, NombreRol, Descripcion)
        VALUES (2, N'Entrenador', N'Rol ficticio de pruebas');

    IF NOT EXISTS (SELECT 1 FROM dbo.Rol WHERE IdRol = 3)
        INSERT dbo.Rol(IdRol, NombreRol, Descripcion)
        VALUES (3, N'Miembro', N'Rol ficticio de pruebas');

    IF NOT EXISTS
    (
        SELECT 1 FROM sys.extended_properties
        WHERE class = 0 AND name = N'SuwoGym7BTestOnly'
    )
    BEGIN
        EXEC sys.sp_addextendedproperty
            @name = N'SuwoGym7BTestOnly',
            @value = N'YES';
    END;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;

SELECT DB_NAME() AS BaseDePruebas,
       CONVERT(NVARCHAR(30), value) AS MarcaAislamiento
FROM sys.extended_properties
WHERE class = 0 AND name = N'SuwoGym7BTestOnly';
