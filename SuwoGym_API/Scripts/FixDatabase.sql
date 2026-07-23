-- =============================================
-- SCRIPT DE CORRECCIÓN - SuwoGym Database
-- =============================================
-- Este script corrige los errores en la base de datos existente

-- Usar la base de datos SuwoGym
USE [SuwoGym]
GO

-- =============================================
-- 1. CREAR TABLA CLIENTES (FALTANTE)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Clientes]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Clientes](
		[IdCliente] [int] IDENTITY(1,1) NOT NULL,
		[IdUsuario] [int] NOT NULL,
		[Identificacion] [nvarchar](50) NOT NULL,
		[Estado] [bit] NOT NULL DEFAULT 1,
		[FechaRegistro] [datetime2](7) NOT NULL DEFAULT GETDATE(),
		CONSTRAINT [PK_Clientes] PRIMARY KEY CLUSTERED 
		(
			[IdCliente] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

-- Agregar Foreign Key a Usuario si no existe
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Cliente_Usuario')
BEGIN
	ALTER TABLE [dbo].[Clientes]  WITH CHECK ADD  CONSTRAINT [FK_Cliente_Usuario] FOREIGN KEY([IdUsuario])
	REFERENCES [dbo].[Usuario] ([IdUsuario])
	ALTER TABLE [dbo].[Clientes] CHECK CONSTRAINT [FK_Cliente_Usuario]
END
GO

-- =============================================
-- 2. INSERTAR ROLES BÁSICOS
-- =============================================
IF NOT EXISTS (SELECT 1 FROM [dbo].[Rol] WHERE IdRol = 1)
BEGIN
	INSERT INTO [dbo].[Rol] (IdRol, NombreRol, Descripcion) 
	VALUES (1, 'Administrador', 'Rol de administrador del sistema')
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Rol] WHERE IdRol = 2)
BEGIN
	INSERT INTO [dbo].[Rol] (IdRol, NombreRol, Descripcion) 
	VALUES (2, 'Entrenador', 'Rol de entrenador personal')
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Rol] WHERE IdRol = 3)
BEGIN
	INSERT INTO [dbo].[Rol] (IdRol, NombreRol, Descripcion) 
	VALUES (3, 'Miembro', 'Rol de miembro del gimnasio')
END
GO

-- =============================================
-- 3. RECREAR PROCEDIMIENTOS ALMACENADOS CON CORRECCIONES
-- =============================================

-- ========== REGISTRAR USUARIO ==========
DROP PROCEDURE IF EXISTS [dbo].[spRegistrarUsuario]
GO

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
		IF EXISTS (SELECT 1 FROM Clientes WHERE Identificacion = @Identificacion)
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

		-- Crear registro de cliente
		INSERT INTO Clientes (IdUsuario, Identificacion, Estado, FechaRegistro)
		VALUES (@IdUsuario, @Identificacion, 1, GETDATE());

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

-- ========== INICIAR SESIÓN ==========
DROP PROCEDURE IF EXISTS [dbo].[spIniciarSesionUsuario]
GO

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

-- ========== VALIDAR CORREO ==========
DROP PROCEDURE IF EXISTS [dbo].[spValidarCorreo]
GO

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

-- ========== ACTUALIZAR CONTRASEÑA ==========
DROP PROCEDURE IF EXISTS [dbo].[spActualizarContrasenna]
GO

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
-- CONFIRMACIÓN
-- =============================================
PRINT N'✓ Script de corrección ejecutado exitosamente'
PRINT N'✓ Tabla Clientes creada'
PRINT N'✓ Roles básicos insertados'
PRINT N'✓ Procedimientos almacenados corregidos'
