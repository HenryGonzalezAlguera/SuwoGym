-- =============================================
-- SCRIPT COMPLETO PARA CREAR LA BASE DE DATOS SUWOGYM
-- =============================================
-- Este script crea la base de datos completa desde cero

USE [master]
GO

-- =============================================
-- 1. ELIMINAR BASE DE DATOS SI EXISTE
-- =============================================
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'SuwoGym')
BEGIN
	ALTER DATABASE SuwoGym SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE SuwoGym;
END
GO

-- =============================================
-- 2. CREAR BASE DE DATOS
-- =============================================
CREATE DATABASE [SuwoGym]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SuwoGym', FILENAME = N'C:\Users\Master Piece\SuwoGym.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SuwoGym_log', FILENAME = N'C:\Users\Master Piece\SuwoGym_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO

ALTER DATABASE [SuwoGym] SET COMPATIBILITY_LEVEL = 160
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SuwoGym].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

-- =============================================
-- 3. CONFIGURAR PROPIEDADES DE LA BASE DE DATOS
-- =============================================
ALTER DATABASE [SuwoGym] SET ANSI_NULL_DEFAULT OFF 
ALTER DATABASE [SuwoGym] SET ANSI_NULLS OFF 
ALTER DATABASE [SuwoGym] SET ANSI_PADDING OFF 
ALTER DATABASE [SuwoGym] SET ANSI_WARNINGS OFF 
ALTER DATABASE [SuwoGym] SET ARITHABORT OFF 
ALTER DATABASE [SuwoGym] SET AUTO_CLOSE ON 
ALTER DATABASE [SuwoGym] SET AUTO_SHRINK OFF 
ALTER DATABASE [SuwoGym] SET AUTO_UPDATE_STATISTICS ON 
ALTER DATABASE [SuwoGym] SET CURSOR_CLOSE_ON_COMMIT OFF 
ALTER DATABASE [SuwoGym] SET CURSOR_DEFAULT  GLOBAL 
ALTER DATABASE [SuwoGym] SET CONCAT_NULL_YIELDS_NULL OFF 
ALTER DATABASE [SuwoGym] SET NUMERIC_ROUNDABORT OFF 
ALTER DATABASE [SuwoGym] SET QUOTED_IDENTIFIER OFF 
ALTER DATABASE [SuwoGym] SET RECURSIVE_TRIGGERS OFF 
ALTER DATABASE [SuwoGym] SET  ENABLE_BROKER 
ALTER DATABASE [SuwoGym] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
ALTER DATABASE [SuwoGym] SET DATE_CORRELATION_OPTIMIZATION OFF 
ALTER DATABASE [SuwoGym] SET TRUSTWORTHY OFF 
ALTER DATABASE [SuwoGym] SET ALLOW_SNAPSHOT_ISOLATION OFF 
ALTER DATABASE [SuwoGym] SET PARAMETERIZATION SIMPLE 
ALTER DATABASE [SuwoGym] SET READ_COMMITTED_SNAPSHOT OFF 
ALTER DATABASE [SuwoGym] SET HONOR_BROKER_PRIORITY OFF 
ALTER DATABASE [SuwoGym] SET RECOVERY SIMPLE 
ALTER DATABASE [SuwoGym] SET  MULTI_USER 
ALTER DATABASE [SuwoGym] SET PAGE_VERIFY CHECKSUM  
ALTER DATABASE [SuwoGym] SET DB_CHAINING OFF 
ALTER DATABASE [SuwoGym] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
ALTER DATABASE [SuwoGym] SET TARGET_RECOVERY_TIME = 60 SECONDS 
ALTER DATABASE [SuwoGym] SET DELAYED_DURABILITY = DISABLED 
ALTER DATABASE [SuwoGym] SET ACCELERATED_DATABASE_RECOVERY = OFF  
ALTER DATABASE [SuwoGym] SET QUERY_STORE = ON
ALTER DATABASE [SuwoGym] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO

USE [SuwoGym]
GO

-- =============================================
-- 4. CREAR TABLA ROL
-- =============================================
CREATE TABLE [dbo].[Rol](
	[IdRol] [int] NOT NULL,
	[NombreRol] [nvarchar](50) NOT NULL,
	[Descripcion] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- =============================================
-- 5. CREAR TABLA USUARIO
-- =============================================
CREATE TABLE [dbo].[Usuario](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[IdRol] [int] NOT NULL,
	[Nombre] [nvarchar](80) NOT NULL,
	[Apellido] [nvarchar](80) NOT NULL,
	[CorreoElectronico] [nvarchar](150) NOT NULL,
	[Identificacion] [nvarchar](50) NOT NULL,
	[Telefono] [nvarchar](30) NULL,
	[FechaNacimiento] [nvarchar](30) NULL,
	[Direccion] [nvarchar](200) NULL,
	[ContrasennaHash] [nvarchar](300) NOT NULL,
	[DebeCambiarContrasenna] [bit] NOT NULL,
	[Estado] [bit] NOT NULL,
	[FechaRegistro] [datetime2](7) NOT NULL,
	[FechaUltimoAcceso] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[CorreoElectronico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Identificacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Usuario] ADD  DEFAULT ((0)) FOR [DebeCambiarContrasenna]
GO
ALTER TABLE [dbo].[Usuario] ADD  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Usuario] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO

-- =============================================
-- 6. CREAR TABLA CLASE
-- =============================================
CREATE TABLE [dbo].[Clase](
	[IdClase] [int] NOT NULL,
	[NombreClase] [nvarchar](50) NOT NULL,
	[Descripcion] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdClase] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- =============================================
-- 7. CREAR TABLA PLAN MEMBRESIA
-- =============================================
CREATE TABLE [dbo].[PlanMembresia](
	[IdPlan] [int] NOT NULL,
	[NombrePlan] [nvarchar](100) NOT NULL,
	[Descripcion] [nvarchar](300) NULL,
	[DuracionDias] [int] NOT NULL,
	[Precio] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdPlan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- =============================================
-- 8. CREAR TABLA MEMBRESIA USUARIO
-- =============================================
CREATE TABLE [dbo].[MembresiaUsuario](
	[IdMembresia] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[IdPlan] [int] NOT NULL,
	[FechaRegistro] [datetime2](7) NOT NULL,
	[EstadoMembresia] [nvarchar](30) NOT NULL,
	[FechaVencimiento] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdMembresia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- =============================================
-- 9. CREAR TABLA PAGO
-- =============================================
CREATE TABLE [dbo].[Pago](
	[IdPago] [int] IDENTITY(1,1) NOT NULL,
	[IdMembresia] [int] NOT NULL,
	[MetodoPago] [nvarchar](50) NOT NULL,
	[FechaEmision] [datetime2](7) NOT NULL,
	[NumeroReferencia] [nvarchar](100) NOT NULL,
	[Monto] [decimal](10, 2) NOT NULL,
	[Subtotal] [decimal](10, 2) NOT NULL,
	[Impuesto] [decimal](10, 2) NOT NULL,
	[Total] [decimal](10, 2) NOT NULL,
 CONSTRAINT [PK_Pago] PRIMARY KEY CLUSTERED 
(
	[IdPago] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- =============================================
-- 10. CREAR TABLA ERROR SISTEMA
-- =============================================
CREATE TABLE [dbo].[ErrorSistema](
	[IdError] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NULL,
	[Modulo] [nvarchar](100) NOT NULL,
	[Mensaje] [nvarchar](max) NOT NULL,
	[FechaHora] [datetime] NOT NULL,
 CONSTRAINT [PK_ErrorSistema] PRIMARY KEY CLUSTERED 
(
	[IdError] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

-- =============================================
-- 11. CREAR TABLA SYSTEM LOG
-- =============================================
CREATE TABLE [dbo].[SystemLog](
	[IdLog] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NULL,
	[Accion] [nvarchar](100) NOT NULL,
	[FechaHora] [datetime] NOT NULL,
 CONSTRAINT [PK_SystemLog] PRIMARY KEY CLUSTERED 
(
	[IdLog] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- =============================================
-- 12. CREAR FOREIGN KEYS
-- =============================================

-- FK: Usuario -> Rol
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Rol] FOREIGN KEY([IdRol])
REFERENCES [dbo].[Rol] ([IdRol])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Rol]
GO

-- FK: MembresiaUsuario -> Usuario
ALTER TABLE [dbo].[MembresiaUsuario]  WITH CHECK ADD  CONSTRAINT [FK_MembresiaUsuario_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[MembresiaUsuario] CHECK CONSTRAINT [FK_MembresiaUsuario_Usuario]
GO

-- FK: MembresiaUsuario -> Plan
ALTER TABLE [dbo].[MembresiaUsuario]  WITH CHECK ADD  CONSTRAINT [FK_MembresiaUsuario_Plan] FOREIGN KEY([IdPlan])
REFERENCES [dbo].[PlanMembresia] ([IdPlan])
GO
ALTER TABLE [dbo].[MembresiaUsuario] CHECK CONSTRAINT [FK_MembresiaUsuario_Plan]
GO

-- FK: Pago -> MembresiaUsuario
ALTER TABLE [dbo].[Pago]  WITH CHECK ADD  CONSTRAINT [FK_Pago_Membresia] FOREIGN KEY([IdMembresia])
REFERENCES [dbo].[MembresiaUsuario] ([IdMembresia])
GO
ALTER TABLE [dbo].[Pago] CHECK CONSTRAINT [FK_Pago_Membresia]
GO

-- =============================================
-- 13. INSERTAR DATOS BASE (ROLES)
-- =============================================
INSERT INTO [dbo].[Rol] (IdRol, NombreRol, Descripcion) 
VALUES 
	(1, 'Administrador', 'Rol de administrador del sistema'),
	(2, 'Entrenador', 'Rol de entrenador personal'),
	(3, 'Miembro', 'Rol de miembro del gimnasio')
GO

-- =============================================
-- 14. CREAR PROCEDIMIENTO ALMACENADO: REGISTRAR USUARIO
-- =============================================
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
		-- Verificar si el correo ya existe
		IF EXISTS (SELECT 1 FROM Usuario WHERE CorreoElectronico = @CorreoElectronico)
		BEGIN
			RAISERROR('El correo electrónico ya está registrado.', 16, 1);
			RETURN;
		END

		-- Verificar si la identificación ya existe
		IF EXISTS (SELECT 1 FROM Usuario WHERE Identificacion = @Identificacion)
		BEGIN
			RAISERROR('La identificación ya está registrada en el sistema.', 16, 1);
			RETURN;
		END

		-- Iniciar transacción explícita
		BEGIN TRANSACTION

		-- Insertar nuevo usuario con rol Miembro (IdRol = 3)
		INSERT INTO Usuario (IdRol, Nombre, Apellido, CorreoElectronico, ContrasennaHash, Identificacion, Estado, DebeCambiarContrasenna, FechaRegistro)
		VALUES (3, @Nombre, @Apellido, @CorreoElectronico, @ContrasennaHash, @Identificacion, 1, 0, GETDATE());

		-- Obtener el ID del usuario creado
		DECLARE @IdUsuario INT = SCOPE_IDENTITY();

		-- Confirmar la transacción
		COMMIT TRANSACTION

		-- Retornar los datos del usuario creado
		SELECT 
			u.IdUsuario,
			u.Nombre,
			u.Apellido,
			u.CorreoElectronico,
			r.NombreRol
		FROM Usuario u
		INNER JOIN Rol r ON u.IdRol = r.IdRol
		WHERE u.IdUsuario = @IdUsuario;
	END TRY
	BEGIN CATCH
		-- Revertir la transacción si ocurre un error
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION

		-- Capturar y lanzar el mensaje de error
		DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
		RAISERROR (@ErrorMessage, 16, 1);
	END CATCH
END
GO

-- =============================================
-- 15. CREAR PROCEDIMIENTO ALMACENADO: INICIAR SESIÓN
-- =============================================
CREATE PROCEDURE [dbo].[spIniciarSesionUsuario]
	@CorreoElectronico NVARCHAR(150),
	@ContrasennaHash NVARCHAR(300)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		u.IdUsuario,
		u.Nombre,
		u.Apellido,
		u.CorreoElectronico,
		r.NombreRol
	FROM Usuario u
	INNER JOIN Rol r ON u.IdRol = r.IdRol
	WHERE u.CorreoElectronico = @CorreoElectronico
	  AND u.ContrasennaHash = @ContrasennaHash
	  AND u.Estado = 1;
END
GO

-- =============================================
-- 16. CREAR PROCEDIMIENTO ALMACENADO: VALIDAR CORREO
-- =============================================
CREATE PROCEDURE [dbo].[spValidarCorreo]
	@CorreoElectronico NVARCHAR(150)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		u.IdUsuario,
		u.Nombre,
		u.Apellido,
		u.CorreoElectronico
	FROM Usuario u
	WHERE u.CorreoElectronico = @CorreoElectronico
	  AND u.Estado = 1;
END
GO

-- =============================================
-- 17. CREAR PROCEDIMIENTO ALMACENADO: ACTUALIZAR CONTRASEÑA
-- =============================================
CREATE PROCEDURE [dbo].[spActualizarContrasenna]
	@IdUsuario INT,
	@ContrasennaHash NVARCHAR(300)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE Usuario
	SET ContrasennaHash = @ContrasennaHash,
		DebeCambiarContrasenna = 0
	WHERE IdUsuario = @IdUsuario;

	SELECT @@ROWCOUNT AS Resultado;
END
GO

-- =============================================
-- FIN DEL SCRIPT
-- =============================================
PRINT N'✓ Base de datos SuwoGym creada exitosamente'
PRINT N'✓ Todas las tablas creadas'
PRINT N'✓ Todos los procedimientos almacenados creados'
PRINT N'✓ Datos base insertados (Roles)'
GO
