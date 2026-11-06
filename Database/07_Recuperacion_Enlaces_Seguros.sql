/*
 SUWÓ GYM — Fase 6B-2 (ejecución MANUAL sobre una copia de desarrollo).
 Script incremental: NO recrea dbo.Usuario, no borra cuentas ni cambia
 contraseñas existentes. Separa la solicitud del consumo atómico.
 Ejecutar con SQL Server Management Studio, conectado a SuwoGym,
 después de realizar una copia de seguridad. Revisar Database/README.md.
*/
USE [SuwoGym];
GO
IF DB_NAME() <> N'SuwoGym' OR OBJECT_ID(N'dbo.Usuario', N'U') IS NULL
    THROW 51001, 'Base de datos o tabla dbo.Usuario incorrecta.', 1;
GO

IF OBJECT_ID(N'dbo.RecuperacionContrasennaToken', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.RecuperacionContrasennaToken
    (
        IdRecuperacion BIGINT IDENTITY(1,1) NOT NULL,
        IdUsuario INT NOT NULL,
        TokenHash VARBINARY(32) NOT NULL,
        CreadoUtc DATETIME2(3) NOT NULL
            CONSTRAINT DF_Recuperacion_CreadoUtc DEFAULT SYSUTCDATETIME(),
        ExpiraUtc DATETIME2(3) NOT NULL,
        UsadoUtc DATETIME2(3) NULL,
        CONSTRAINT PK_RecuperacionContrasennaToken
            PRIMARY KEY CLUSTERED (IdRecuperacion),
        CONSTRAINT FK_RecuperacionContrasenna_Usuario
            FOREIGN KEY (IdUsuario) REFERENCES dbo.Usuario(IdUsuario),
        CONSTRAINT CK_Recuperacion_Expira
            CHECK (ExpiraUtc > CreadoUtc)
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id =
               OBJECT_ID(N'dbo.RecuperacionContrasennaToken')
               AND name = N'UX_Recuperacion_TokenHash')
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UX_Recuperacion_TokenHash
    ON dbo.RecuperacionContrasennaToken(TokenHash);
END;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id =
               OBJECT_ID(N'dbo.RecuperacionContrasennaToken')
               AND name = N'IX_Recuperacion_Usuario')
BEGIN
    CREATE NONCLUSTERED INDEX IX_Recuperacion_Usuario
    ON dbo.RecuperacionContrasennaToken(IdUsuario, UsadoUtc, ExpiraUtc);
END;
GO

-- Bloquea el registro de usuario durante la solicitud para serializar
-- solicitudes simultáneas y anular enlaces previos.
CREATE OR ALTER PROCEDURE dbo.spRecuperacionSolicitar
    @CorreoElectronico NVARCHAR(150),
    @TokenHash VARBINARY(32),
    @ExpiraUtc DATETIME2(3)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @Ahora DATETIME2(3) = SYSUTCDATETIME();
    IF DATALENGTH(@TokenHash) <> 32 OR @ExpiraUtc <= @Ahora
        THROW 51002, 'Token o expiracion invalida.', 1;

    DECLARE @IdUsuario INT, @Nombre NVARCHAR(80), @Correo NVARCHAR(150);

    BEGIN TRY
        BEGIN TRANSACTION;
        SELECT @IdUsuario = IdUsuario, @Nombre = Nombre,
               @Correo = CorreoElectronico
        FROM dbo.Usuario WITH (UPDLOCK, HOLDLOCK)
        WHERE CorreoElectronico = @CorreoElectronico AND Estado = 1;

        IF @IdUsuario IS NOT NULL
        BEGIN
            UPDATE dbo.RecuperacionContrasennaToken
            SET UsadoUtc = @Ahora
            WHERE IdUsuario = @IdUsuario AND UsadoUtc IS NULL;

            INSERT dbo.RecuperacionContrasennaToken
                (IdUsuario, TokenHash, ExpiraUtc)
            VALUES (@IdUsuario, @TokenHash, @ExpiraUtc);
        END;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;

    -- Una cuenta inexistente no produce fila; la API responde igual.
    IF @IdUsuario IS NOT NULL
        SELECT @IdUsuario AS IdUsuario, @Nombre AS Nombre,
               @Correo AS CorreoElectronico;
END;
GO

-- Comprobación sin revelar ID ni email.
CREATE OR ALTER PROCEDURE dbo.spRecuperacionValidar
    @TokenHash VARBINARY(32)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CAST(CASE WHEN EXISTS
    (
        SELECT 1
        FROM dbo.RecuperacionContrasennaToken AS T
        INNER JOIN dbo.Usuario AS U ON U.IdUsuario = T.IdUsuario
        WHERE T.TokenHash = @TokenHash
          AND T.UsadoUtc IS NULL
          AND T.ExpiraUtc > SYSUTCDATETIME()
          AND U.Estado = 1
    ) THEN 1 ELSE 0 END AS BIT) AS EsValido;
END;
GO

-- Consumir token, actualizar BCrypt y revocar enlaces hermanos en UNA
-- transacción. Si no existe/expiró/ya se usó, NO toca dbo.Usuario.
CREATE OR ALTER PROCEDURE dbo.spRecuperacionConsumir
    @TokenHash VARBINARY(32),
    @ContrasennaHash NVARCHAR(300)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    IF DATALENGTH(@TokenHash) <> 32 OR
       NULLIF(LTRIM(RTRIM(@ContrasennaHash)), N'') IS NULL
        THROW 51003, 'Datos de restablecimiento invalidos.', 1;

    DECLARE @Ahora DATETIME2(3) = SYSUTCDATETIME();
    DECLARE @UsuarioConsumido TABLE (IdUsuario INT NOT NULL);
    DECLARE @IdUsuario INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE T
           SET UsadoUtc = @Ahora
           OUTPUT inserted.IdUsuario INTO @UsuarioConsumido(IdUsuario)
        FROM dbo.RecuperacionContrasennaToken AS T
        INNER JOIN dbo.Usuario AS U ON U.IdUsuario = T.IdUsuario
        WHERE T.TokenHash = @TokenHash
          AND T.UsadoUtc IS NULL
          AND T.ExpiraUtc > @Ahora
          AND U.Estado = 1;

        SELECT TOP (1) @IdUsuario = IdUsuario FROM @UsuarioConsumido;

        IF @IdUsuario IS NULL
        BEGIN
            COMMIT TRANSACTION;
            SELECT CAST(0 AS BIT) AS Actualizado;
            RETURN;
        END;

        UPDATE dbo.Usuario
        SET ContrasennaHash = @ContrasennaHash,
            DebeCambiarContrasenna = 0
        WHERE IdUsuario = @IdUsuario AND Estado = 1;

        IF @@ROWCOUNT <> 1
            THROW 51004, 'No se pudo actualizar la contrasena.', 1;

        UPDATE dbo.RecuperacionContrasennaToken
        SET UsadoUtc = @Ahora
        WHERE IdUsuario = @IdUsuario AND UsadoUtc IS NULL;

        COMMIT TRANSACTION;
        SELECT CAST(1 AS BIT) AS Actualizado;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- Revocación puntual si el correo no llega a enviarse.
CREATE OR ALTER PROCEDURE dbo.spRecuperacionRevocar
    @TokenHash VARBINARY(32)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.RecuperacionContrasennaToken
    SET UsadoUtc = SYSUTCDATETIME()
    WHERE TokenHash = @TokenHash AND UsadoUtc IS NULL;
END;
GO

-- Comprobación sin datos sensibles:
SELECT OBJECT_ID(N'dbo.RecuperacionContrasennaToken', N'U') AS Tabla,
       OBJECT_ID(N'dbo.spRecuperacionSolicitar', N'P') AS Solicitar,
       OBJECT_ID(N'dbo.spRecuperacionValidar', N'P') AS Validar,
       OBJECT_ID(N'dbo.spRecuperacionConsumir', N'P') AS Consumir,
       OBJECT_ID(N'dbo.spRecuperacionRevocar', N'P') AS Revocar;
GO
