/*
 SUWÓ GYM — Fase 6C: revocación inmediata de JWT.
 MIGRACIÓN INCREMENTAL MANUAL. Respaldar SuwoGym antes de ejecutarla.
 NO ejecutar el script 01 ni borrar usuarios.
 Las versiones de JWT nuevas se almacenan en dbo.Usuario; no afecta
 el contenido de los hashes BCrypt ni las contraseñas existentes.
*/
USE [SuwoGym];
GO

IF DB_NAME() <> N'SuwoGym' OR OBJECT_ID(N'dbo.Usuario', N'U') IS NULL
    THROW 51020, 'Base de datos o tabla dbo.Usuario incorrecta.', 1;
GO

IF COL_LENGTH(N'dbo.Usuario', N'VersionJwt') IS NULL
BEGIN
    ALTER TABLE dbo.Usuario
    ADD VersionJwt UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT DF_Usuario_VersionJwt DEFAULT NEWID() WITH VALUES;
END;
GO

-- El trigger revoca tokens existentes al cambiar credenciales, estado o rol.
-- No modifica otros procedimientos almacenados (recuperación, empleado, etc.).
-- La versión solo rota cuando el valor realmente cambió.
CREATE OR ALTER TRIGGER dbo.trg_Usuario_RevocarJwt
ON dbo.Usuario
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT (UPDATE(ContrasennaHash) OR UPDATE(IdRol) OR UPDATE(Estado))
        RETURN;

    UPDATE U
       SET VersionJwt = NEWID()
    FROM dbo.Usuario AS U
    INNER JOIN inserted AS I ON I.IdUsuario = U.IdUsuario
    INNER JOIN deleted AS D ON D.IdUsuario = I.IdUsuario
    WHERE I.ContrasennaHash <> D.ContrasennaHash
       OR I.IdRol <> D.IdRol
       OR I.Estado <> D.Estado;
END;
GO

-- Única fuente de verdad para inicio de sesión y cada autenticación JWT.
-- No se devuelve el hash de contraseña.
CREATE OR ALTER PROCEDURE dbo.spSeguridadConsultarVersionJwt
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT VersionJwt, IdRol, Estado
    FROM dbo.Usuario
    WHERE IdUsuario = @IdUsuario;
END;
GO

-- Verificación sin exponer datos de usuario ni cambiar su estado.
SELECT CASE
           WHEN COL_LENGTH(N'dbo.Usuario', N'VersionJwt') IS NOT NULL
           THEN N'OK' ELSE N'FALTA' END AS ColumnaVersionJwt,
       OBJECT_ID(N'dbo.trg_Usuario_RevocarJwt', N'TR') AS TriggerRevocacion,
       OBJECT_ID(N'dbo.spSeguridadConsultarVersionJwt', N'P') AS ProcedimientoConsulta;
GO
