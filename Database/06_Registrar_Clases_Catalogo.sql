/*
 SUWÓ GYM — Fase 3: catálogo de clases para Administrador
 Aplicar manualmente en SSMS, SOLO sobre la instancia SuwoGym que usa la API.
 No elimina tablas, no inserta datos ficticios y no modifica usuarios.
 Puede ejecutarse varias veces (CREATE OR ALTER PROCEDURE).
 Antes de ejecutarlo: confirma @@SERVERNAME, DB_NAME() y haz respaldo si corresponde.
*/
USE [SuwoGym];
GO

CREATE OR ALTER PROCEDURE dbo.spRegistrarClase
    @NombreClase NVARCHAR(50),
    @Descripcion NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @NombreClase = LTRIM(RTRIM(@NombreClase));
    SET @Descripcion = NULLIF(LTRIM(RTRIM(@Descripcion)), N'');

    IF @NombreClase IS NULL OR @NombreClase = N''
        THROW 51010, 'El nombre de la clase es obligatorio.', 1;

    IF LEN(@NombreClase) > 50
        THROW 51011, 'El nombre supera los 50 caracteres.', 1;

    IF @Descripcion IS NOT NULL AND LEN(@Descripcion) > 200
        THROW 51012, 'La descripcion supera los 200 caracteres.', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        /* IdClase no es IDENTITY en el esquema actual.
           El bloqueo exclusivo protege la asignación de MAX(IdClase)+1
           ante altas simultáneas desde la aplicación. */
        DECLARE @IdClase INT;

        SELECT @IdClase = ISNULL(MAX(IdClase), 0) + 1
        FROM dbo.Clase WITH (TABLOCKX, HOLDLOCK);

        IF EXISTS (
            SELECT 1
            FROM dbo.Clase
            WHERE UPPER(LTRIM(RTRIM(NombreClase))) = UPPER(@NombreClase)
        )
        BEGIN
            THROW 51013, 'Ya existe una clase con ese nombre.', 1;
        END;

        INSERT INTO dbo.Clase
            (IdClase, NombreClase, Descripcion)
        VALUES
            (@IdClase, @NombreClase, @Descripcion);

        COMMIT TRANSACTION;

        SELECT @IdClase AS IdClase;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

/* Verificación sin cambios de datos */
SELECT
    OBJECT_ID(N'dbo.spRegistrarClase', N'P') AS ProcedimientoRegistrado,
    DB_NAME() AS BaseDatos,
    @@SERVERNAME AS Servidor;
GO
