/*
  SUWÓ GYM | Fase 3: primer Administrador LOCAL DE DESARROLLO.
  EJECUCIÓN MANUAL Y EXPLÍCITA EN SSMS. NO es parte del inicio de la aplicación.

  Requisitos:
    1) Usar una base SuwoGym LOCAL con datos ficticios, nunca producción.
    2) Crear ANTES una segunda cuenta ficticia desde /Home/Registro.
    3) Copiar este script a NUEVA CONSULTA en SSMS (no guardar un correo personal
       ni credenciales en el repositorio).
    4) Sustituir @CorreoDePrueba por el correo de ESA cuenta ficticia.
    5) La cuenta existente de Miembro no debe utilizarse para este procedimiento.

  Este script NO genera contraseñas, NO cambia hashes ni altera membresías.
  Se detiene si ya existe un Administrador activo.
*/
USE [SuwoGym];
GO
SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @CorreoDePrueba NVARCHAR(150) =
    N'REEMPLAZAR_POR_CORREO_DE_CUENTA_DE_PRUEBA';

-- Requerimos una decisión explícita antes de cambiar privilegios.
IF @CorreoDePrueba = N'REEMPLAZAR_POR_CORREO_DE_CUENTA_DE_PRUEBA'
   OR NULLIF(LTRIM(RTRIM(@CorreoDePrueba)), N'') IS NULL
BEGIN
    THROW 51001,
        'Antes de ejecutar, escribe el correo de la segunda cuenta ficticia.',
        1;
END;

IF DB_NAME() <> N'SuwoGym'
BEGIN
    THROW 51002, 'Base de datos inesperada. Se requiere SuwoGym.', 1;
END;

BEGIN TRY
    BEGIN TRANSACTION;

    -- Primera habilitación únicamente. Evita conceder privilegios adicionales
    -- cuando el sistema ya dispone de un administrador activo.
    IF EXISTS (
        SELECT 1
        FROM dbo.Usuario WITH (UPDLOCK, HOLDLOCK)
        WHERE IdRol = 1 AND Estado = 1
    )
    BEGIN
        THROW 51003,
            'Ya existe un Administrador activo. No se realizará ningún cambio.',
            1;
    END;

    DECLARE @IdUsuario INT;
    DECLARE @RolActual INT;
    DECLARE @CuentaActiva BIT;

    SELECT
        @IdUsuario = U.IdUsuario,
        @RolActual = U.IdRol,
        @CuentaActiva = U.Estado
    FROM dbo.Usuario AS U WITH (UPDLOCK, HOLDLOCK)
    WHERE U.CorreoElectronico = @CorreoDePrueba;

    IF @IdUsuario IS NULL
    BEGIN
        THROW 51004,
            'No existe esa cuenta. Regístrala primero desde la WEB.',
            1;
    END;

    IF @RolActual <> 3 OR @CuentaActiva <> 1
    BEGIN
        THROW 51005,
            'La cuenta debe ser un Miembro activo (rol 3) antes de habilitar Admin.',
            1;
    END;

    IF NOT EXISTS (SELECT 1 FROM dbo.Rol WHERE IdRol = 1)
    BEGIN
        THROW 51006,
            'No existe el rol Administrador (IdRol 1). Revisa los catálogos.',
            1;
    END;

    UPDATE dbo.Usuario
    SET IdRol = 1
    WHERE IdUsuario = @IdUsuario
      AND IdRol = 3
      AND Estado = 1;

    IF @@ROWCOUNT <> 1
    BEGIN
        THROW 51007,
            'No se actualizó exactamente una cuenta. Operación cancelada.',
            1;
    END;

    COMMIT TRANSACTION;

    -- Verificación, sin contraseñas ni hashes.
    SELECT
        U.IdUsuario, U.Nombre, U.Apellido,
        U.CorreoElectronico, U.IdRol, R.NombreRol, U.Estado
    FROM dbo.Usuario AS U
    INNER JOIN dbo.Rol AS R ON R.IdRol = U.IdRol
    WHERE U.IdUsuario = @IdUsuario;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO
