USE [SuwoGym]
GO

SET NOCOUNT ON;
GO

/* ============================================================
   SUWÓ GYM - ADMINISTRADOR INICIAL (PLANTILLA PÚBLICA)

   IMPORTANTE:
   1. Este archivo sí puede subirse a GitHub.
   2. Antes de ejecutarlo, reemplace los valores marcados.
   3. La contraseña debe almacenarse como hash BCrypt, nunca en texto plano.
   ============================================================ */

DECLARE @Nombre NVARCHAR(80) = N'Administrador';
DECLARE @Apellido NVARCHAR(80) = N'Inicial';
DECLARE @CorreoElectronico NVARCHAR(150) = N'CAMBIAR_CORREO';
DECLARE @Identificacion NVARCHAR(50) = N'CAMBIAR_IDENTIFICACION';
DECLARE @ContrasennaHash NVARCHAR(300) = N'CAMBIAR_HASH_BCRYPT';

IF NOT EXISTS
(
    SELECT 1
    FROM dbo.Rol
    WHERE IdRol = 1
      AND NombreRol = N'Administrador'
)
BEGIN
    RAISERROR(
        'Primero debe ejecutar 02_SuwoGym_DatosIniciales.sql.',
        16,
        1
    );
    RETURN;
END
GO

DECLARE @Nombre NVARCHAR(80) = N'Administrador';
DECLARE @Apellido NVARCHAR(80) = N'Inicial';
DECLARE @CorreoElectronico NVARCHAR(150) = N'CAMBIAR_CORREO';
DECLARE @Identificacion NVARCHAR(50) = N'CAMBIAR_IDENTIFICACION';
DECLARE @ContrasennaHash NVARCHAR(300) = N'CAMBIAR_HASH_BCRYPT';

IF @CorreoElectronico = N'CAMBIAR_CORREO'
   OR @Identificacion = N'CAMBIAR_IDENTIFICACION'
   OR @ContrasennaHash = N'CAMBIAR_HASH_BCRYPT'
BEGIN
    RAISERROR(
        'Debe reemplazar el correo, la identificación y el hash BCrypt antes de ejecutar este archivo.',
        16,
        1
    );
    RETURN;
END

IF EXISTS
(
    SELECT 1
    FROM dbo.Usuario
    WHERE CorreoElectronico = @CorreoElectronico
)
BEGIN
    RAISERROR(
        'Ya existe un usuario con ese correo electrónico.',
        16,
        1
    );
    RETURN;
END

IF EXISTS
(
    SELECT 1
    FROM dbo.Usuario
    WHERE Identificacion = @Identificacion
)
BEGIN
    RAISERROR(
        'Ya existe un usuario con esa identificación.',
        16,
        1
    );
    RETURN;
END

INSERT INTO dbo.Usuario
(
    IdRol,
    Nombre,
    Apellido,
    CorreoElectronico,
    Identificacion,
    Telefono,
    FechaNacimiento,
    Direccion,
    ContrasennaHash,
    DebeCambiarContrasenna,
    Estado,
    FechaRegistro,
    FechaUltimoAcceso
)
VALUES
(
    1,
    @Nombre,
    @Apellido,
    @CorreoElectronico,
    @Identificacion,
    NULL,
    NULL,
    NULL,
    @ContrasennaHash,
    0,
    1,
    SYSDATETIME(),
    NULL
);

PRINT N'Administrador inicial creado correctamente.';
GO
