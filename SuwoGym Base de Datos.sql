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
   DATOS INICIALES: USUARIOS
   ============================================================ */

SET IDENTITY_INSERT [dbo].[Usuario] ON 
GO
INSERT [dbo].[Usuario] ([IdUsuario], [IdRol], [Nombre], [Apellido], [CorreoElectronico], [Identificacion], [Telefono], [FechaNacimiento], [Direccion], [ContrasennaHash], [DebeCambiarContrasenna], [Estado], [FechaRegistro], [FechaUltimoAcceso]) 
VALUES (1, 1, N'Admin', N'Sistema', N'admin@mail.com', N'001', NULL, NULL, NULL, N'$2a$11$uTiuit1mfQYVLYo/fqCfu.eSYMw3n20FjTwibDDfFObTdCG0Sw8Qe', 0, 1, CAST(N'2026-07-24T06:48:40.4833333' AS DateTime2), NULL)
GO
INSERT [dbo].[Usuario] ([IdUsuario], [IdRol], [Nombre], [Apellido], [CorreoElectronico], [Identificacion], [Telefono], [FechaNacimiento], [Direccion], [ContrasennaHash], [DebeCambiarContrasenna], [Estado], [FechaRegistro], [FechaUltimoAcceso]) 
VALUES (2, 2, N'Trainer', N'Sistema', N'entrenador@mail.com', N'002', NULL, NULL, NULL, N'$2a$11$NJx8LnBKcDb.Ind5jij0DO2.VVkBUEag3LR5LHwnDnlVw/kPuySZe', 0, 1, CAST(N'2026-07-24T06:51:31.4600000' AS DateTime2), NULL)
GO
INSERT [dbo].[Usuario] ([IdUsuario], [IdRol], [Nombre], [Apellido], [CorreoElectronico], [Identificacion], [Telefono], [FechaNacimiento], [Direccion], [ContrasennaHash], [DebeCambiarContrasenna], [Estado], [FechaRegistro], [FechaUltimoAcceso]) 
VALUES (3, 3, N'Cliente', N'Sistema', N'cliente@mail.com', N'003', NULL, NULL, NULL, N'$2a$11$p/aBlaYzvA/WRt3TBH9Ue.ijUwgQ4rHYxbsWEe6qPa1ucJloX8KQ.', 0, 1, CAST(N'2026-07-24T06:51:57.8466667' AS DateTime2), NULL)
GO
INSERT [dbo].[Usuario] ([IdUsuario], [IdRol], [Nombre], [Apellido], [CorreoElectronico], [Identificacion], [Telefono], [FechaNacimiento], [Direccion], [ContrasennaHash], [DebeCambiarContrasenna], [Estado], [FechaRegistro], [FechaUltimoAcceso]) 
VALUES (4, 1, N'Henry', N'González Alguera', N'KkaiserKing@gmail.com', N'98765431', N'456788765432', N'Oct 10 2002 12:00AM', N'De la subestación JASEC 50N, 50O', N'$2a$11$s9XLDctwe23lO/0hr0tT3uLRQgnvjaL10J9ZQX.lWpbocuKZgrQiW', 0, 1, CAST(N'2026-07-27T20:55:13.6866667' AS DateTime2), NULL)
GO
SET IDENTITY_INSERT [dbo].[Usuario] OFF
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
   PROCEDIMIENTO: LISTAR EMPLEADOS
   ============================================================ */
   
CREATE PROCEDURE [dbo].[spListarEmpleados] 
AS
BEGIN
    SELECT  u.Identificacion, u.Nombre, u.Apellido, r.NombreRol, u.CorreoElectronico, u.Estado
    FROM Usuario AS u
    INNER JOIN Rol as r
        ON r.IdRol = u.IdRol
    WHERE u.IdRol != 3
    ORDER BY u.Apellido ASC
END
GO


/* ============================================================
   PROCEDIMIENTO: LISTAR CLIENTES
   ============================================================ 
   
   NOTA:
   Este procedimiento requiere que en el registro de usuarios se
   proceda con un módulo de definir el plan de membresía

   ============================================================ */

CREATE PROCEDURE [dbo].[spListarClientes] 
AS
BEGIN
    SELECT  u.Nombre, u.Apellido, u.CorreoElectronico, u.Estado
        -- , p.NombrePlan, m.EstadoMembresia, m.FechaVencimiento
    FROM Usuario AS u
    /*INNER JOIN MembresiaUsuario as m
        ON m.IdUsuario = u.IdUsuario
    INNER JOIN PlanMembresia as p
        ON p.IdPlan = m.IdPlan */
    WHERE u.IdRol = 3
    ORDER BY u.Apellido ASC
END
GO


/* ============================================================
   PROCEDIMIENTO: LISTAR PLANES DE MEMBRESÍA
   ============================================================ */

CREATE PROCEDURE [dbo].[spListarPlanesMembresia] 
AS
BEGIN
    SELECT  NombrePlan, Descripcion, DuracionDias, Precio
    FROM PlanMembresia
END
GO



/* ============================================================
   PROCEDIMIENTO: REGISTRAR EMPLEADO
   ============================================================ */

CREATE PROCEDURE [dbo].[spRegistrarEmpleado]
    @Nombre NVARCHAR(80),
    @Apellido NVARCHAR(80),
    @Identificacion NVARCHAR(50),
    @FechaNacimiento NVARCHAR(30),
    @Telefono NVARCHAR(30),
    @CorreoElectronico NVARCHAR(150),
    @Rol INT,
    @Direccion NVARCHAR(200),
    @ContrasennaHash NVARCHAR(300),
    @FechaRegistro DATETIME
AS

BEGIN
    
    IF EXISTS ( SELECT 1 FROM dbo.Usuario
                WHERE CorreoElectronico = @CorreoElectronico)
        BEGIN
            RAISERROR( 'El correo electrónico ya está registrado.', 16,1);
            RETURN;
        END

    IF EXISTS ( SELECT 1 FROM dbo.Usuario
                WHERE Identificacion = @Identificacion)
        BEGIN
            RAISERROR('La identificación ya está registrada en el sistema.',16,1);
            RETURN;
        END



    INSERT INTO dbo.Usuario (
        IdRol, Nombre, Apellido, CorreoElectronico, Identificacion, Telefono,
        FechaNacimiento, Direccion, ContrasennaHash, DebeCambiarContrasenna, Estado, FechaRegistro
    )
    VALUES
    (
        @Rol,@Nombre,@Apellido,@CorreoElectronico,@Identificacion,@Telefono,
        @FechaNacimiento, @Direccion, @ContrasennaHash, 1,1, @FechaRegistro
    );


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
