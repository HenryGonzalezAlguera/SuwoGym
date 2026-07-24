/* ============================================================
   BASE DE DATOS: SUWOGYM
   ============================================================ */

USE [master]
GO

/* Eliminar la base si ya existe */

IF DB_ID(N'SuwoGym') IS NOT NULL
BEGIN
    ALTER DATABASE [SuwoGym]
    SET SINGLE_USER
    WITH ROLLBACK IMMEDIATE;

    DROP DATABASE [SuwoGym];
END
GO

CREATE DATABASE [SuwoGym]
GO

USE [SuwoGym]
GO


/* ============================================================
   TABLA: Clase
   ============================================================ */

CREATE TABLE [dbo].[Clase]
(
    [IdClase] INT NOT NULL,
    [NombreClase] NVARCHAR(50) NOT NULL,
    [Descripcion] NVARCHAR(200) NULL,

    PRIMARY KEY CLUSTERED
    (
        [IdClase] ASC
    )
);
GO


/* ============================================================
   TABLA: ErrorSistema
   ============================================================ */

CREATE TABLE [dbo].[ErrorSistema]
(
    [IdError] INT IDENTITY(1,1) NOT NULL,
    [IdUsuario] INT NULL,
    [Modulo] NVARCHAR(100) NOT NULL,
    [Mensaje] NVARCHAR(MAX) NOT NULL,
    [FechaHora] DATETIME NOT NULL,

    CONSTRAINT [PK_ErrorSistema]
    PRIMARY KEY CLUSTERED
    (
        [IdError] ASC
    )
);
GO


/* ============================================================
   TABLA: PlanMembresia
   ============================================================ */

CREATE TABLE [dbo].[PlanMembresia]
(
    [IdPlan] INT NOT NULL,
    [NombrePlan] NVARCHAR(100) NOT NULL,
    [Descripcion] NVARCHAR(300) NULL,
    [DuracionDias] INT NOT NULL,
    [Precio] DECIMAL(10,2) NOT NULL,

    PRIMARY KEY CLUSTERED
    (
        [IdPlan] ASC
    )
);
GO


/* ============================================================
   TABLA: Rol
   ============================================================ */

CREATE TABLE [dbo].[Rol]
(
    [IdRol] INT NOT NULL,
    [NombreRol] NVARCHAR(50) NOT NULL,
    [Descripcion] NVARCHAR(200) NULL,

    PRIMARY KEY CLUSTERED
    (
        [IdRol] ASC
    )
);
GO


/* ============================================================
   TABLA: SystemLog
   ============================================================ */

CREATE TABLE [dbo].[SystemLog]
(
    [IdLog] INT IDENTITY(1,1) NOT NULL,
    [IdUsuario] INT NULL,
    [Accion] NVARCHAR(100) NOT NULL,
    [FechaHora] DATETIME NOT NULL,

    CONSTRAINT [PK_SystemLog]
    PRIMARY KEY CLUSTERED
    (
        [IdLog] ASC
    )
);
GO


/* ============================================================
   TABLA: Usuario
   ============================================================ */

CREATE TABLE [dbo].[Usuario]
(
    [IdUsuario] INT IDENTITY(1,1) NOT NULL,
    [IdRol] INT NOT NULL,
    [Nombre] NVARCHAR(80) NOT NULL,
    [Apellido] NVARCHAR(80) NOT NULL,
    [CorreoElectronico] NVARCHAR(150) NOT NULL,
    [Identificacion] NVARCHAR(50) NOT NULL,
    [Telefono] NVARCHAR(30) NULL,
    [FechaNacimiento] NVARCHAR(30) NULL,
    [Direccion] NVARCHAR(200) NULL,
    [ContrasennaHash] NVARCHAR(300) NOT NULL,
    [DebeCambiarContrasenna] BIT NOT NULL,
    [Estado] BIT NOT NULL,
    [FechaRegistro] DATETIME2(7) NOT NULL,
    [FechaUltimoAcceso] DATETIME2(7) NULL,

    PRIMARY KEY CLUSTERED
    (
        [IdUsuario] ASC
    )
);
GO


/* ============================================================
   TABLA: Rutinas
   ============================================================ */

CREATE TABLE [dbo].[Rutinas]
(
    [IdRutina] INT IDENTITY(1,1) NOT NULL,
    [NombreRutina] NVARCHAR(100) NOT NULL,
    [Nivel] NVARCHAR(10) NOT NULL,
    [Detalles] NVARCHAR(MAX) NOT NULL,

    CONSTRAINT [PK_Rutinas]
    PRIMARY KEY CLUSTERED
    (
        [IdRutina] ASC
    )
);
GO


/* ============================================================
   TABLA: ClaseProgramada
   ============================================================ */

CREATE TABLE [dbo].[ClaseProgramada]
(
    [IdClaseProg] INT IDENTITY(1,1) NOT NULL,
    [IdEntrenador] INT NOT NULL,
    [IdClase] INT NOT NULL,
    [FechaHoraInicio] DATETIME2(7) NOT NULL,
    [FechaHoraFin] DATETIME2(7) NOT NULL,
    [CupoMaximo] INT NOT NULL,
    [CupoActual] INT NOT NULL,
    [Estado] BIT NOT NULL,

    CONSTRAINT [PK_ClaseProgramada]
    PRIMARY KEY CLUSTERED
    (
        [IdClaseProg] ASC
    )
);
GO


/* ============================================================
   TABLA: MembresiaUsuario
   ============================================================ */

CREATE TABLE [dbo].[MembresiaUsuario]
(
    [IdMembresia] INT IDENTITY(1,1) NOT NULL,
    [IdUsuario] INT NOT NULL,
    [IdPlan] INT NOT NULL,
    [FechaRegistro] DATETIME2(7) NOT NULL,
    [EstadoMembresia] NVARCHAR(30) NOT NULL,
    [FechaVencimiento] DATETIME2(7) NOT NULL,

    PRIMARY KEY CLUSTERED
    (
        [IdMembresia] ASC
    )
);
GO


/* ============================================================
   TABLA: Pago
   ============================================================ */

CREATE TABLE [dbo].[Pago]
(
    [IdPago] INT IDENTITY(1,1) NOT NULL,
    [IdMembresia] INT NOT NULL,
    [MetodoPago] NVARCHAR(50) NOT NULL,
    [FechaEmision] DATETIME2(7) NOT NULL,
    [NumeroReferencia] NVARCHAR(100) NOT NULL,
    [Monto] DECIMAL(10,2) NOT NULL,
    [Subtotal] DECIMAL(10,2) NOT NULL,
    [Impuesto] DECIMAL(10,2) NOT NULL,
    [Total] DECIMAL(10,2) NOT NULL,

    CONSTRAINT [PK_Pago]
    PRIMARY KEY CLUSTERED
    (
        [IdPago] ASC
    )
);
GO


/* ============================================================
   ÍNDICES ÚNICOS
   ============================================================ */

ALTER TABLE [dbo].[Usuario]
ADD UNIQUE NONCLUSTERED
(
    [CorreoElectronico] ASC
);
GO

ALTER TABLE [dbo].[Usuario]
ADD UNIQUE NONCLUSTERED
(
    [Identificacion] ASC
);
GO


/* ============================================================
   VALORES POR DEFECTO
   ============================================================ */

ALTER TABLE [dbo].[Usuario]
ADD DEFAULT ((0))
FOR [DebeCambiarContrasenna];
GO

ALTER TABLE [dbo].[Usuario]
ADD DEFAULT ((1))
FOR [Estado];
GO

ALTER TABLE [dbo].[Usuario]
ADD DEFAULT (GETDATE())
FOR [FechaRegistro];
GO


/* ============================================================
   LLAVES FORÁNEAS
   ============================================================ */

ALTER TABLE [dbo].[Usuario]
WITH CHECK ADD CONSTRAINT [FK_Usuario_Rol]
FOREIGN KEY
(
    [IdRol]
)
REFERENCES [dbo].[Rol]
(
    [IdRol]
);
GO

ALTER TABLE [dbo].[Usuario]
CHECK CONSTRAINT [FK_Usuario_Rol];
GO


ALTER TABLE [dbo].[MembresiaUsuario]
WITH CHECK ADD CONSTRAINT [FK_MembresiaUsuario_Usuario]
FOREIGN KEY
(
    [IdUsuario]
)
REFERENCES [dbo].[Usuario]
(
    [IdUsuario]
);
GO

ALTER TABLE [dbo].[MembresiaUsuario]
CHECK CONSTRAINT [FK_MembresiaUsuario_Usuario];
GO


ALTER TABLE [dbo].[MembresiaUsuario]
WITH CHECK ADD CONSTRAINT [FK_MembresiaUsuario_Plan]
FOREIGN KEY
(
    [IdPlan]
)
REFERENCES [dbo].[PlanMembresia]
(
    [IdPlan]
);
GO

ALTER TABLE [dbo].[MembresiaUsuario]
CHECK CONSTRAINT [FK_MembresiaUsuario_Plan];
GO


ALTER TABLE [dbo].[Pago]
WITH CHECK ADD CONSTRAINT [FK_Pago_Membresia]
FOREIGN KEY
(
    [IdMembresia]
)
REFERENCES [dbo].[MembresiaUsuario]
(
    [IdMembresia]
);
GO

ALTER TABLE [dbo].[Pago]
CHECK CONSTRAINT [FK_Pago_Membresia];
GO


/* ============================================================
   DATOS INICIALES: ROLES
   ============================================================ */

INSERT INTO [dbo].[Rol]
(
    [IdRol],
    [NombreRol],
    [Descripcion]
)
VALUES
(
    1,
    N'Administrador',
    N'Rol de administrador del sistema'
);
GO

INSERT INTO [dbo].[Rol]
(
    [IdRol],
    [NombreRol],
    [Descripcion]
)
VALUES
(
    2,
    N'Entrenador',
    N'Rol de entrenador personal'
);
GO

INSERT INTO [dbo].[Rol]
(
    [IdRol],
    [NombreRol],
    [Descripcion]
)
VALUES
(
    3,
    N'Miembro',
    N'Rol de miembro del gimnasio'
);
GO


/* ============================================================
   DATOS INICIALES: PLANES DE MEMBRESÍA
   ============================================================ */

INSERT INTO [dbo].[PlanMembresia]
(
    [IdPlan],
    [NombrePlan],
    [Descripcion],
    [DuracionDias],
    [Precio]
)
VALUES
(
    1,
    N'Mensual',
    N'El pago se realiza cada mes; 12 pagos anuales',
    30,
    10500.00
);
GO

INSERT INTO [dbo].[PlanMembresia]
(
    [IdPlan],
    [NombrePlan],
    [Descripcion],
    [DuracionDias],
    [Precio]
)
VALUES
(
    2,
    N'Trimestral',
    N'El pago se realiza cada 3 meses; 4 pagos anuales',
    90,
    30000.00
);
GO

INSERT INTO [dbo].[PlanMembresia]
(
    [IdPlan],
    [NombrePlan],
    [Descripcion],
    [DuracionDias],
    [Precio]
)
VALUES
(
    3,
    N'Anual',
    N'El pago se realiza cada año; 1 pago anual',
    365,
    117000.00
);
GO


/* ============================================================
   DATOS INICIALES: CLASES
   ============================================================ */

INSERT INTO [dbo].[Clase]
(
    [IdClase],
    [NombreClase],
    [Descripcion]
)
VALUES
(
    1,
    N'Funcionales',
    N'Entrenamiento dinámico que mejora fuerza, resistencia, movilidad y coordinación mediante movimientos aplicables a la vida diaria'
);
GO

INSERT INTO [dbo].[Clase]
(
    [IdClase],
    [NombreClase],
    [Descripcion]
)
VALUES
(
    2,
    N'CrossFit',
    N'Programa de alta intensidad que combina fuerza, resistencia y acondicionamiento físico con ejercicios variados y desafiantes'
);
GO

INSERT INTO [dbo].[Clase]
(
    [IdClase],
    [NombreClase],
    [Descripcion]
)
VALUES
(
    3,
    N'Baile',
    N'Actividad física divertida que mejora la coordinación, resistencia y expresión corporal mediante movimientos al ritmo de la música'
);
GO


/* ============================================================
   PROCEDIMIENTO: REGISTRAR USUARIO
   ============================================================ */

CREATE PROCEDURE [dbo].[spRegistrarUsuario]
    @Nombre NVARCHAR(80),
    @Apellido NVARCHAR(80),
    @CorreoElectronico NVARCHAR(150),
    @ContrasennaHash NVARCHAR(300),
    @Identificacion NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF EXISTS
        (
            SELECT 1
            FROM dbo.Usuario
            WHERE CorreoElectronico = @CorreoElectronico
        )
        BEGIN
            RAISERROR(
                'El correo electrónico ya está registrado.',
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
                'La identificación ya está registrada en el sistema.',
                16,
                1
            );

            RETURN;
        END

        BEGIN TRANSACTION;

        INSERT INTO dbo.Usuario
        (
            IdRol,
            Nombre,
            Apellido,
            CorreoElectronico,
            ContrasennaHash,
            Identificacion,
            Estado,
            DebeCambiarContrasenna,
            FechaRegistro
        )
        VALUES
        (
            3,
            @Nombre,
            @Apellido,
            @CorreoElectronico,
            @ContrasennaHash,
            @Identificacion,
            1,
            0,
            GETDATE()
        );

        DECLARE @IdUsuario INT =
            SCOPE_IDENTITY();

        COMMIT TRANSACTION;

        SELECT
            U.IdUsuario,
            U.Nombre,
            U.Apellido,
            U.CorreoElectronico,
            U.IdRol,
            R.NombreRol
        FROM dbo.Usuario U
        INNER JOIN dbo.Rol R
            ON U.IdRol = R.IdRol
        WHERE U.IdUsuario = @IdUsuario;

    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) =
            ERROR_MESSAGE();

        RAISERROR(
            @ErrorMessage,
            16,
            1
        );

    END CATCH
END
GO


/* ============================================================
   PROCEDIMIENTO: INICIAR SESIÓN
   ============================================================

   El procedimiento no compara la contraseña.

   SQL devuelve el hash almacenado y la API realiza:

   BCrypt.Net.BCrypt.Verify(
       contraseñaIngresada,
       hashAlmacenado
   );
   ============================================================ */

CREATE PROCEDURE [dbo].[spIniciarSesionUsuario]
    @CorreoElectronico NVARCHAR(150),
    @ContrasennaHash NVARCHAR(300)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.IdUsuario,
        U.Nombre,
        U.Apellido,
        U.CorreoElectronico,
        U.ContrasennaHash,
        U.Estado,
        U.DebeCambiarContrasenna,
        U.IdRol,
        R.NombreRol
    FROM dbo.Usuario U
    INNER JOIN dbo.Rol R
        ON U.IdRol = R.IdRol
    WHERE U.CorreoElectronico = @CorreoElectronico
      AND U.Estado = 1;

END
GO


/* ============================================================
   PROCEDIMIENTO: VALIDAR CORREO
   ============================================================ */

CREATE PROCEDURE [dbo].[spValidarCorreo]
    @CorreoElectronico NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.IdUsuario,
        U.Nombre,
        U.Apellido,
        U.CorreoElectronico,
        U.Estado,
        U.DebeCambiarContrasenna,
        U.IdRol
    FROM dbo.Usuario U
    WHERE U.CorreoElectronico = @CorreoElectronico
      AND U.Estado = 1;

END
GO


/* ============================================================
   PROCEDIMIENTO: ACTUALIZAR CONTRASEÑA
   ============================================================

   En recuperación:
       @DebeCambiarContrasenna = 1

   Después de cambiar la contraseña:
       @DebeCambiarContrasenna = 0
   ============================================================ */

CREATE PROCEDURE [dbo].[spActualizarContrasenna]
    @IdUsuario INT,
    @ContrasennaHash NVARCHAR(300),
    @DebeCambiarContrasenna BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Usuario
    SET ContrasennaHash = @ContrasennaHash,
        DebeCambiarContrasenna =
            @DebeCambiarContrasenna
    WHERE IdUsuario = @IdUsuario;

END
GO


/* ============================================================
   PROCEDIMIENTO: CONSULTAR USUARIO
   ============================================================ */

CREATE PROCEDURE [dbo].[spConsultarUsuario]
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.IdUsuario,
        U.Identificacion,
        U.Nombre,
        U.Apellido,
        U.CorreoElectronico,
        U.Estado,
        U.DebeCambiarContrasenna,
        U.IdRol
    FROM dbo.Usuario U
    WHERE U.IdUsuario = @IdUsuario;

END
GO


/* ============================================================
   FINALIZAR CONFIGURACIÓN
   ============================================================ */

USE [master]
GO

ALTER DATABASE [SuwoGym]
SET READ_WRITE;
GO
