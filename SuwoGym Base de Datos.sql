
USE [master]
GO

/****** Object:  Database [SuwoGym]    Script Date: 22/7/2026 23:39:58 ******/
-- Borrar y crear base de datos
DROP DATABASE IF EXISTS [SuwoGym];
CREATE DATABASE [SuwoGym]
GO

USE [SuwoGym]
GO


-- TABLAS --------------------------------------------

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
/****** Object:  Table [dbo].[ErrorSistema]    Script Date: 22/7/2026 23:39:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
/****** Object:  Table [dbo].[MembresiaUsuario]    Script Date: 22/7/2026 23:39:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
/****** Object:  Table [dbo].[Pago]    Script Date: 22/7/2026 23:39:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
/****** Object:  Table [dbo].[PlanMembresia]    Script Date: 22/7/2026 23:39:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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


/****** Object:  Table [dbo].[Rol]    Script Date: 22/7/2026 23:39:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
/****** Object:  Table [dbo].[SystemLog]    Script Date: 22/7/2026 23:39:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
/****** Object:  Table [dbo].[Usuario]    Script Date: 22/7/2026 23:39:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** Object:  Index [UQ__Usuario__531402F3A863D846]    Script Date: 22/7/2026 23:39:58 ******/
ALTER TABLE [dbo].[Usuario] ADD UNIQUE NONCLUSTERED 
(
	[CorreoElectronico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Usuario__D6F931E52493A11E]    Script Date: 22/7/2026 23:39:58 ******/
ALTER TABLE [dbo].[Usuario] ADD UNIQUE NONCLUSTERED 
(
	[Identificacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Usuario] ADD  DEFAULT ((0)) FOR [DebeCambiarContrasenna]
GO
ALTER TABLE [dbo].[Usuario] ADD  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Usuario] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[MembresiaUsuario]  WITH CHECK ADD  CONSTRAINT [FK_MembresiaUsuario_Plan] FOREIGN KEY([IdPlan])
REFERENCES [dbo].[PlanMembresia] ([IdPlan])
GO
ALTER TABLE [dbo].[MembresiaUsuario] CHECK CONSTRAINT [FK_MembresiaUsuario_Plan]
GO
ALTER TABLE [dbo].[MembresiaUsuario]  WITH CHECK ADD  CONSTRAINT [FK_MembresiaUsuario_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[MembresiaUsuario] CHECK CONSTRAINT [FK_MembresiaUsuario_Usuario]
GO
ALTER TABLE [dbo].[Pago]  WITH CHECK ADD  CONSTRAINT [FK_Pago_Membresia] FOREIGN KEY([IdMembresia])
REFERENCES [dbo].[MembresiaUsuario] ([IdMembresia])
GO
ALTER TABLE [dbo].[Pago] CHECK CONSTRAINT [FK_Pago_Membresia]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Rol] FOREIGN KEY([IdRol])
REFERENCES [dbo].[Rol] ([IdRol])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Rol]
GO



-- INSERTAR DATOS ------------------------

INSERT [dbo].[Rol] ([IdRol], [NombreRol], [Descripcion]) VALUES (1, N'Administrador', N'Rol de administrador del sistema')
GO
INSERT [dbo].[Rol] ([IdRol], [NombreRol], [Descripcion]) VALUES (2, N'Entrenador', N'Rol de entrenador personal')
GO
INSERT [dbo].[Rol] ([IdRol], [NombreRol], [Descripcion]) VALUES (3, N'Miembro', N'Rol de miembro del gimnasio')
GO

INSERT INTO [dbo].[PlanMembresia] (IdPlan, NombrePlan, Descripcion, DuracionDias, Precio)
VALUES
    (1, N'Mensual', N'El pago se realiza cada mes; 12 pagos anuales', 30, 10500.00),
    (2, N'Trimestral', N'El pago se realiza cada 3 meses; 4 pagos anuales', 90, 30000.00),
    (3, N'Anual', N'El pago se realiza cada año; 1 pago anual', 365, 117000.00);
GO

INSERT [dbo].[Clase] ([IdClase], [NombreClase], [Descripcion]) 
VALUES (1, 'Funcionales', 'Entrenamiento dinámico que mejora fuerza, resistencia, movilidad y coordinación mediante movimientos aplicables a la vida diaria')
GO
INSERT [dbo].[Clase] ([IdClase], [NombreClase], [Descripcion]) 
VALUES (2, 'CrossFit', 'Programa de alta intensidad que combina fuerza, resistencia y acondicionamiento físico con ejercicios variados y desafiantes')
GO
INSERT [dbo].[Clase] ([IdClase], [NombreClase], [Descripcion]) 
VALUES (3, 'Baile', 'Actividad física divertida que mejora la coordinación, resistencia y expresión corporal mediante movimientos al ritmo de la música')
GO


SET IDENTITY_INSERT [dbo].[Usuario] ON 
GO
-- Usuarios de acceso general
INSERT [dbo].[Usuario] ([IdUsuario], [IdRol], [Nombre], [Apellido], [CorreoElectronico], [Identificacion], [Telefono], [FechaNacimiento], [Direccion], [ContrasennaHash], [DebeCambiarContrasenna], [Estado], [FechaRegistro], [FechaUltimoAcceso]) 
VALUES (1, 1, N'Admin', N'Sistema', N'admin@mail.com', N'001', NULL, NULL, NULL, N'123456789', 0, 1, CAST(N'2026-07-22T23:56:59.9233333' AS DateTime2), NULL)
GO
INSERT [dbo].[Usuario] ([IdUsuario], [IdRol], [Nombre], [Apellido], [CorreoElectronico], [Identificacion], [Telefono], [FechaNacimiento], [Direccion], [ContrasennaHash], [DebeCambiarContrasenna], [Estado], [FechaRegistro], [FechaUltimoAcceso]) 
VALUES (2, 2, N'Trainer', N'Sistema', N'entrenador@mail.com', N'002', NULL, NULL, NULL, N'123456789', 0, 1, CAST(N'2026-07-22T23:56:59.9233333' AS DateTime2), NULL)
GO
INSERT [dbo].[Usuario] ([IdUsuario], [IdRol], [Nombre], [Apellido], [CorreoElectronico], [Identificacion], [Telefono], [FechaNacimiento], [Direccion], [ContrasennaHash], [DebeCambiarContrasenna], [Estado], [FechaRegistro], [FechaUltimoAcceso]) 
VALUES (3, 3, N'Client', N'Sistema', N'cliente@mail.com', N'003', NULL, NULL, NULL, N'123456789', 0, 1, CAST(N'2026-07-22T23:56:59.9233333' AS DateTime2), NULL)
GO
-- Usuarios personales del equipo
INSERT [dbo].[Usuario] ([IdUsuario], [IdRol], [Nombre], [Apellido], [CorreoElectronico], [Identificacion], [Telefono], [FechaNacimiento], [Direccion], [ContrasennaHash], [DebeCambiarContrasenna], [Estado], [FechaRegistro], [FechaUltimoAcceso]) 
VALUES (4, 3, N'SSSSS', N'qqqqq', N'rpennant80803@ufide.ac.cr', N'1868080333qq', NULL, NULL, NULL, N'123456789', 0, 1, CAST(N'2026-07-22T23:36:01.3233333' AS DateTime2), NULL)
GO
INSERT [dbo].[Usuario] ([IdUsuario], [IdRol], [Nombre], [Apellido], [CorreoElectronico], [Identificacion], [Telefono], [FechaNacimiento], [Direccion], [ContrasennaHash], [DebeCambiarContrasenna], [Estado], [FechaRegistro], [FechaUltimoAcceso]) 
VALUES (5, 3, N'Indira', N'Rodríguez', N'indyrodg@gmail.com', N'304930425', NULL, NULL, NULL, N'123456789', 0, 1, CAST(N'2026-07-22T23:56:59.9233333' AS DateTime2), NULL)
GO
SET IDENTITY_INSERT [dbo].[Usuario] OFF
GO






/****** Object:  StoredProcedure [dbo].[spActualizarContrasenna]    Script Date: 22/7/2026 23:39:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[spIniciarSesionUsuario]    Script Date: 22/7/2026 23:39:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[spRegistrarUsuario]    Script Date: 22/7/2026 23:39:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[spValidarCorreo]    Script Date: 22/7/2026 23:39:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
USE [master]
GO
ALTER DATABASE [SuwoGym] SET  READ_WRITE 
GO
