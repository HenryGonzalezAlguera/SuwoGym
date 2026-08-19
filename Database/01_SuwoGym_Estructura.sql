/* ============================================================
   SUWO GYM - SCRIPT FINAL PORTABLE (SOLO ESQUEMA)
   Generado a partir de la base funcional SuwoGym.

   IMPORTANTE:
   - Ejecutar en SQL Server 2022 o compatible.
   - Este archivo crea la base si no existe.
   - Ejecutarlo sobre una base SuwoGym vacía.
   - No contiene datos iniciales de roles, clases, planes ni usuarios.
   ============================================================ */

USE [master];
GO

/* Eliminar la base si ya existe */

IF DB_ID(N'SuwoGym') IS NOT NULL
    BEGIN
        DROP DATABASE [SuwoGym];
        CREATE DATABASE [SuwoGym]
    END

ELSE IF DB_ID(N'SuwoGym') IS NULL
    BEGIN
        EXEC(N'CREATE DATABASE [SuwoGym]');
    END
GO


USE [SuwoGym];
GO

ALTER DATABASE [SuwoGym]
SET COMPATIBILITY_LEVEL = 160;
GO

/****** Objeto: Table [dbo].[AsistenciaClase] Fecha de script: 03/08/2026 19:28:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AsistenciaClase](
	[IdAsistencia] [int] IDENTITY(1,1) NOT NULL,
	[IdReserva] [int] NOT NULL,
	[IdEntrenadorRegistro] [int] NOT NULL,
	[EstadoAsistencia] [nvarchar](20) NOT NULL,
	[Observacion] [nvarchar](300) NULL,
	[FechaRegistro] [datetime2](0) NOT NULL,
	[FechaActualizacion] [datetime2](0) NULL,
 CONSTRAINT [PK_AsistenciaClase] PRIMARY KEY CLUSTERED 
(
	[IdAsistencia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_AsistenciaClase_IdReserva] UNIQUE NONCLUSTERED 
(
	[IdReserva] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Clase] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clase](
	[IdClase] [int] NOT NULL,
	[NombreClase] [nvarchar](50) NOT NULL,
	[Descripcion] [nvarchar](200) NULL,
	[Icono] [nvarchar](20) NULL,
	[Color] [nvarchar](10) NULL,
	[Beneficio1] [nvarchar](50) NULL,
	[Beneficio2] [nvarchar](50) NULL,
	[Beneficio3] [nvarchar](50) NULL,
	[Beneficio4] [nvarchar](50) NULL,
 CONSTRAINT [PK_Clase] PRIMARY KEY CLUSTERED 
(
	[IdClase] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[ClaseProgramada] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClaseProgramada](
	[IdClaseProg] [int] IDENTITY(1,1) NOT NULL,
	[IdEntrenador] [int] NOT NULL,
	[IdClase] [int] NOT NULL,
	[FechaHoraInicio] [datetime2](7) NOT NULL,
	[FechaHoraFin] [datetime2](7) NOT NULL,
	[CupoMaximo] [int] NOT NULL,
	[CupoActual] [int] NOT NULL,
	[Estado] [bit] NOT NULL,
 CONSTRAINT [PK_ClaseProgramada] PRIMARY KEY CLUSTERED 
(
	[IdClaseProg] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Ejercicio] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ejercicio](
	[IdEjercicio] [int] IDENTITY(1,1) NOT NULL,
	[IdEntrenador] [int] NOT NULL,
	[NombreEjercicio] [nvarchar](120) NOT NULL,
	[GrupoMuscular] [nvarchar](80) NOT NULL,
	[Descripcion] [nvarchar](300) NULL,
	[FechaCreacion] [datetime2](7) NOT NULL,
	[Estado] [bit] NOT NULL,
 CONSTRAINT [PK_Ejercicio] PRIMARY KEY CLUSTERED 
(
	[IdEjercicio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[EntrenadorCliente] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntrenadorCliente](
	[IdAsignacion] [int] IDENTITY(1,1) NOT NULL,
	[IdEntrenador] [int] NOT NULL,
	[IdCliente] [int] NOT NULL,
	[FechaAsignacion] [datetime2](7) NOT NULL,
	[Estado] [bit] NOT NULL,
 CONSTRAINT [PK_EntrenadorCliente] PRIMARY KEY CLUSTERED 
(
	[IdAsignacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_EntrenadorCliente_Cliente] UNIQUE NONCLUSTERED 
(
	[IdCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[ErrorSistema] Fecha de script: 03/08/2026 19:28:08 ******/
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
/****** Objeto: Table [dbo].[MembresiaUsuario] Fecha de script: 03/08/2026 19:28:08 ******/
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
 CONSTRAINT [PK_MembresiaUsuario] PRIMARY KEY CLUSTERED 
(
	[IdMembresia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Pago] Fecha de script: 03/08/2026 19:28:08 ******/
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
/****** Objeto: Table [dbo].[PlanMembresia] Fecha de script: 03/08/2026 19:28:08 ******/
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
	[EsPopular] [bit] NULL,
	[Color] [nvarchar](10) NULL,
	[Beneficio1] [nvarchar](50) NULL,
	[Beneficio2] [nvarchar](50) NULL,
	[Beneficio3] [nvarchar](50) NULL,
	[Beneficio4] [nvarchar](50) NULL,
 CONSTRAINT [PK_PlanMembresia] PRIMARY KEY CLUSTERED 
(
	[IdPlan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
GO
/****** Objeto: Table [dbo].[ReservaClase] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReservaClase](
	[IdReserva] [int] IDENTITY(1,1) NOT NULL,
	[IdClaseProg] [int] NOT NULL,
	[IdMiembro] [int] NOT NULL,
	[FechaReserva] [datetime2](0) NOT NULL,
	[FechaCancelacion] [datetime2](0) NULL,
	[Estado] [bit] NOT NULL,
 CONSTRAINT [PK_ReservaClase] PRIMARY KEY CLUSTERED 
(
	[IdReserva] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Rol] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rol](
	[IdRol] [int] NOT NULL,
	[NombreRol] [nvarchar](50) NOT NULL,
	[Descripcion] [nvarchar](200) NULL,
 CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[RutinaEjercicio] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RutinaEjercicio](
	[IdRutinaEjercicio] [int] IDENTITY(1,1) NOT NULL,
	[IdRutina] [int] NOT NULL,
	[IdEjercicio] [int] NOT NULL,
	[DiaSemana] [nvarchar](20) NOT NULL,
	[Series] [int] NOT NULL,
	[Repeticiones] [int] NOT NULL,
	[PesoSugerido] [decimal](6, 2) NULL,
	[DescansoSegundos] [int] NULL,
	[Observacion] [nvarchar](250) NULL,
	[Orden] [int] NOT NULL,
	[FechaAsignacion] [datetime2](7) NOT NULL,
	[Estado] [bit] NOT NULL,
 CONSTRAINT [PK_RutinaEjercicio] PRIMARY KEY CLUSTERED 
(
	[IdRutinaEjercicio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Rutinas] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rutinas](
	[IdRutina] [int] IDENTITY(1,1) NOT NULL,
	[NombreRutina] [nvarchar](100) NOT NULL,
	[Nivel] [nvarchar](20) NOT NULL,
	[Detalles] [nvarchar](max) NOT NULL,
	[IdEntrenador] [int] NULL,
	[IdCliente] [int] NULL,
	[FechaCreacion] [datetime2](7) NOT NULL,
	[Estado] [bit] NOT NULL,
 CONSTRAINT [PK_Rutinas] PRIMARY KEY CLUSTERED 
(
	[IdRutina] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[SystemLog] Fecha de script: 03/08/2026 19:28:08 ******/
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
/****** Objeto: Table [dbo].[Usuario] Fecha de script: 03/08/2026 19:28:08 ******/
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
	[FechaNacimiento] [datetime2](7) NULL,
	[Direccion] [nvarchar](200) NULL,
	[ContrasennaHash] [nvarchar](300) NOT NULL,
	[DebeCambiarContrasenna] [bit] NOT NULL,
	[Estado] [bit] NOT NULL,
	[FechaRegistro] [datetime2](7) NOT NULL,
	[FechaUltimoAcceso] [datetime2](7) NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Objeto: Index [IX_AsistenciaClase_Entrenador] Fecha de script: 03/08/2026 19:28:08 ******/
CREATE NONCLUSTERED INDEX [IX_AsistenciaClase_Entrenador] ON [dbo].[AsistenciaClase]
(
	[IdEntrenadorRegistro] ASC,
	[EstadoAsistencia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [IX_ReservaClase_Miembro] Fecha de script: 03/08/2026 19:28:08 ******/
CREATE NONCLUSTERED INDEX [IX_ReservaClase_Miembro] ON [dbo].[ReservaClase]
(
	[IdMiembro] ASC,
	[Estado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Objeto: Index [UX_ReservaClase_ClaseMiembroActiva] Fecha de script: 03/08/2026 19:28:08 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_ReservaClase_ClaseMiembroActiva] ON [dbo].[ReservaClase]
(
	[IdClaseProg] ASC,
	[IdMiembro] ASC
)
WHERE ([Estado]=(1))
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Objeto: Index [UX_Usuario_CorreoElectronico] Fecha de script: 03/08/2026 19:28:08 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_Usuario_CorreoElectronico] ON [dbo].[Usuario]
(
	[CorreoElectronico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Objeto: Index [UX_Usuario_Identificacion] Fecha de script: 03/08/2026 19:28:08 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_Usuario_Identificacion] ON [dbo].[Usuario]
(
	[Identificacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AsistenciaClase] ADD  CONSTRAINT [DF_AsistenciaClase_FechaRegistro]  DEFAULT (sysdatetime()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Ejercicio] ADD  CONSTRAINT [DF_Ejercicio_FechaCreacion]  DEFAULT (getdate()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[Ejercicio] ADD  CONSTRAINT [DF_Ejercicio_Estado]  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[ReservaClase] ADD  CONSTRAINT [DF_ReservaClase_FechaReserva]  DEFAULT (sysdatetime()) FOR [FechaReserva]
GO
ALTER TABLE [dbo].[ReservaClase] ADD  CONSTRAINT [DF_ReservaClase_Estado]  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[RutinaEjercicio] ADD  CONSTRAINT [DF_RutinaEjercicio_Orden]  DEFAULT ((1)) FOR [Orden]
GO
ALTER TABLE [dbo].[RutinaEjercicio] ADD  CONSTRAINT [DF_RutinaEjercicio_FechaAsignacion]  DEFAULT (getdate()) FOR [FechaAsignacion]
GO
ALTER TABLE [dbo].[RutinaEjercicio] ADD  CONSTRAINT [DF_RutinaEjercicio_Estado]  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Rutinas] ADD  CONSTRAINT [DF_Rutinas_FechaCreacion]  DEFAULT (getdate()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[Rutinas] ADD  CONSTRAINT [DF_Rutinas_Estado]  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DF_Usuario_DebeCambiarContrasenna]  DEFAULT ((0)) FOR [DebeCambiarContrasenna]
GO
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DF_Usuario_Estado]  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DF_Usuario_FechaRegistro]  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[AsistenciaClase]  WITH CHECK ADD  CONSTRAINT [FK_AsistenciaClase_Entrenador] FOREIGN KEY([IdEntrenadorRegistro])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[AsistenciaClase] CHECK CONSTRAINT [FK_AsistenciaClase_Entrenador]
GO
ALTER TABLE [dbo].[AsistenciaClase]  WITH CHECK ADD  CONSTRAINT [FK_AsistenciaClase_Reserva] FOREIGN KEY([IdReserva])
REFERENCES [dbo].[ReservaClase] ([IdReserva])
GO
ALTER TABLE [dbo].[AsistenciaClase] CHECK CONSTRAINT [FK_AsistenciaClase_Reserva]
GO
ALTER TABLE [dbo].[ClaseProgramada]  WITH CHECK ADD  CONSTRAINT [FK_ClaseProgramada_Clase] FOREIGN KEY([IdClase])
REFERENCES [dbo].[Clase] ([IdClase])
GO
ALTER TABLE [dbo].[ClaseProgramada] CHECK CONSTRAINT [FK_ClaseProgramada_Clase]
GO
ALTER TABLE [dbo].[ClaseProgramada]  WITH CHECK ADD  CONSTRAINT [FK_ClaseProgramada_Entrenador] FOREIGN KEY([IdEntrenador])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[ClaseProgramada] CHECK CONSTRAINT [FK_ClaseProgramada_Entrenador]
GO
ALTER TABLE [dbo].[Ejercicio]  WITH CHECK ADD  CONSTRAINT [FK_Ejercicio_Entrenador] FOREIGN KEY([IdEntrenador])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[Ejercicio] CHECK CONSTRAINT [FK_Ejercicio_Entrenador]
GO
ALTER TABLE [dbo].[EntrenadorCliente]  WITH CHECK ADD  CONSTRAINT [FK_EntrenadorCliente_Cliente] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[EntrenadorCliente] CHECK CONSTRAINT [FK_EntrenadorCliente_Cliente]
GO
ALTER TABLE [dbo].[EntrenadorCliente]  WITH CHECK ADD  CONSTRAINT [FK_EntrenadorCliente_Entrenador] FOREIGN KEY([IdEntrenador])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[EntrenadorCliente] CHECK CONSTRAINT [FK_EntrenadorCliente_Entrenador]
GO
ALTER TABLE [dbo].[ErrorSistema]  WITH CHECK ADD  CONSTRAINT [FK_ErrorSistema_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[ErrorSistema] CHECK CONSTRAINT [FK_ErrorSistema_Usuario]
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
ALTER TABLE [dbo].[ReservaClase]  WITH CHECK ADD  CONSTRAINT [FK_ReservaClase_ClaseProgramada] FOREIGN KEY([IdClaseProg])
REFERENCES [dbo].[ClaseProgramada] ([IdClaseProg])
GO
ALTER TABLE [dbo].[ReservaClase] CHECK CONSTRAINT [FK_ReservaClase_ClaseProgramada]
GO
ALTER TABLE [dbo].[ReservaClase]  WITH CHECK ADD  CONSTRAINT [FK_ReservaClase_Usuario] FOREIGN KEY([IdMiembro])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[ReservaClase] CHECK CONSTRAINT [FK_ReservaClase_Usuario]
GO
ALTER TABLE [dbo].[RutinaEjercicio]  WITH CHECK ADD  CONSTRAINT [FK_RutinaEjercicio_Ejercicio] FOREIGN KEY([IdEjercicio])
REFERENCES [dbo].[Ejercicio] ([IdEjercicio])
GO
ALTER TABLE [dbo].[RutinaEjercicio] CHECK CONSTRAINT [FK_RutinaEjercicio_Ejercicio]
GO
ALTER TABLE [dbo].[RutinaEjercicio]  WITH CHECK ADD  CONSTRAINT [FK_RutinaEjercicio_Rutina] FOREIGN KEY([IdRutina])
REFERENCES [dbo].[Rutinas] ([IdRutina])
GO
ALTER TABLE [dbo].[RutinaEjercicio] CHECK CONSTRAINT [FK_RutinaEjercicio_Rutina]
GO
ALTER TABLE [dbo].[Rutinas]  WITH CHECK ADD  CONSTRAINT [FK_Rutinas_Cliente] FOREIGN KEY([IdCliente])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[Rutinas] CHECK CONSTRAINT [FK_Rutinas_Cliente]
GO
ALTER TABLE [dbo].[Rutinas]  WITH CHECK ADD  CONSTRAINT [FK_Rutinas_Entrenador] FOREIGN KEY([IdEntrenador])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[Rutinas] CHECK CONSTRAINT [FK_Rutinas_Entrenador]
GO
ALTER TABLE [dbo].[SystemLog]  WITH CHECK ADD  CONSTRAINT [FK_SystemLog_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[SystemLog] CHECK CONSTRAINT [FK_SystemLog_Usuario]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Rol] FOREIGN KEY([IdRol])
REFERENCES [dbo].[Rol] ([IdRol])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Rol]
GO
ALTER TABLE [dbo].[AsistenciaClase]  WITH CHECK ADD  CONSTRAINT [CK_AsistenciaClase_Estado] CHECK  (([EstadoAsistencia]=N'Ausente' OR [EstadoAsistencia]=N'Presente'))
GO
ALTER TABLE [dbo].[AsistenciaClase] CHECK CONSTRAINT [CK_AsistenciaClase_Estado]
GO
ALTER TABLE [dbo].[RutinaEjercicio]  WITH CHECK ADD  CONSTRAINT [CK_RutinaEjercicio_Descanso] CHECK  (([DescansoSegundos] IS NULL OR [DescansoSegundos]>=(0)))
GO
ALTER TABLE [dbo].[RutinaEjercicio] CHECK CONSTRAINT [CK_RutinaEjercicio_Descanso]
GO
ALTER TABLE [dbo].[RutinaEjercicio]  WITH CHECK ADD  CONSTRAINT [CK_RutinaEjercicio_DiaSemana] CHECK  (([DiaSemana]=N'Domingo' OR [DiaSemana]=N'Sábado' OR [DiaSemana]=N'Viernes' OR [DiaSemana]=N'Jueves' OR [DiaSemana]=N'Miércoles' OR [DiaSemana]=N'Martes' OR [DiaSemana]=N'Lunes'))
GO
ALTER TABLE [dbo].[RutinaEjercicio] CHECK CONSTRAINT [CK_RutinaEjercicio_DiaSemana]
GO
ALTER TABLE [dbo].[RutinaEjercicio]  WITH CHECK ADD  CONSTRAINT [CK_RutinaEjercicio_Orden] CHECK  (([Orden]>(0)))
GO
ALTER TABLE [dbo].[RutinaEjercicio] CHECK CONSTRAINT [CK_RutinaEjercicio_Orden]
GO
ALTER TABLE [dbo].[RutinaEjercicio]  WITH CHECK ADD  CONSTRAINT [CK_RutinaEjercicio_Peso] CHECK  (([PesoSugerido] IS NULL OR [PesoSugerido]>=(0)))
GO
ALTER TABLE [dbo].[RutinaEjercicio] CHECK CONSTRAINT [CK_RutinaEjercicio_Peso]
GO
ALTER TABLE [dbo].[RutinaEjercicio]  WITH CHECK ADD  CONSTRAINT [CK_RutinaEjercicio_Repeticiones] CHECK  (([Repeticiones]>(0)))
GO
ALTER TABLE [dbo].[RutinaEjercicio] CHECK CONSTRAINT [CK_RutinaEjercicio_Repeticiones]
GO
ALTER TABLE [dbo].[RutinaEjercicio]  WITH CHECK ADD  CONSTRAINT [CK_RutinaEjercicio_Series] CHECK  (([Series]>(0)))
GO
ALTER TABLE [dbo].[RutinaEjercicio] CHECK CONSTRAINT [CK_RutinaEjercicio_Series]
GO
/****** Objeto: StoredProcedure [dbo].[spActualizarContrasenna] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* ============================================================
   PROCEDIMIENTO: ACTUALIZAR CONTRASEÑA

   No debe usar SET NOCOUNT ON porque el API comprueba
   la cantidad de filas afectadas con Dapper Execute().
   ============================================================ */

CREATE PROCEDURE [dbo].[spActualizarContrasenna]
    @IdUsuario INT,
    @ContrasennaHash NVARCHAR(300),
    @DebeCambiarContrasenna BIT
AS
BEGIN
    UPDATE dbo.Usuario
    SET ContrasennaHash = @ContrasennaHash,
        DebeCambiarContrasenna = @DebeCambiarContrasenna
    WHERE IdUsuario = @IdUsuario;
END

GO
/****** Objeto: StoredProcedure [dbo].[spAgregarEjercicioRutinaEntrenador] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spAgregarEjercicioRutinaEntrenador]
    @IdEntrenador INT,
    @IdRutina INT,
    @IdEjercicio INT,
    @DiaSemana NVARCHAR(20),
    @Series INT,
    @Repeticiones INT,
    @PesoSugerido DECIMAL(6,2),
    @DescansoSegundos INT,
    @Observacion NVARCHAR(250),
    @Orden INT
AS
BEGIN
    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Rutinas
            WHERE IdRutina = @IdRutina
              AND IdEntrenador = @IdEntrenador
        )
        BEGIN
            RAISERROR(
                'La rutina no existe o no pertenece a este entrenador.',
                16,
                1
            );

            RETURN;
        END


        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Ejercicio
            WHERE IdEjercicio = @IdEjercicio
              AND IdEntrenador = @IdEntrenador
              AND Estado = 1
        )
        BEGIN
            RAISERROR(
                'El ejercicio no existe, está inactivo o no pertenece a este entrenador.',
                16,
                1
            );

            RETURN;
        END


        IF @DiaSemana NOT IN
        (
            N'Lunes',
            N'Martes',
            N'Miércoles',
            N'Jueves',
            N'Viernes',
            N'Sábado',
            N'Domingo'
        )
        BEGIN
            RAISERROR(
                'El día de la semana seleccionado no es válido.',
                16,
                1
            );

            RETURN;
        END


        IF @Series <= 0
        BEGIN
            RAISERROR(
                'La cantidad de series debe ser mayor que cero.',
                16,
                1
            );

            RETURN;
        END


        IF @Repeticiones <= 0
        BEGIN
            RAISERROR(
                'La cantidad de repeticiones debe ser mayor que cero.',
                16,
                1
            );

            RETURN;
        END


        IF @PesoSugerido IS NOT NULL
           AND @PesoSugerido < 0
        BEGIN
            RAISERROR(
                'El peso sugerido no puede ser negativo.',
                16,
                1
            );

            RETURN;
        END


        IF @DescansoSegundos IS NOT NULL
           AND @DescansoSegundos < 0
        BEGIN
            RAISERROR(
                'El tiempo de descanso no puede ser negativo.',
                16,
                1
            );

            RETURN;
        END


        IF @Orden <= 0
        BEGIN
            RAISERROR(
                'El orden debe ser mayor que cero.',
                16,
                1
            );

            RETURN;
        END


        IF EXISTS
        (
            SELECT 1
            FROM dbo.RutinaEjercicio
            WHERE IdRutina = @IdRutina
              AND IdEjercicio = @IdEjercicio
              AND DiaSemana = @DiaSemana
              AND Estado = 1
        )
        BEGIN
            RAISERROR(
                'El ejercicio ya está agregado a esta rutina para el día seleccionado.',
                16,
                1
            );

            RETURN;
        END


        INSERT INTO dbo.RutinaEjercicio
        (
            IdRutina,
            IdEjercicio,
            DiaSemana,
            Series,
            Repeticiones,
            PesoSugerido,
            DescansoSegundos,
            Observacion,
            Orden,
            FechaAsignacion,
            Estado
        )
        VALUES
        (
            @IdRutina,
            @IdEjercicio,
            @DiaSemana,
            @Series,
            @Repeticiones,
            @PesoSugerido,
            @DescansoSegundos,
            NULLIF(
                LTRIM(RTRIM(@Observacion)),
                N''
            ),
            @Orden,
            GETDATE(),
            1
        );

    END TRY
    BEGIN CATCH

        DECLARE @ErrorMessage NVARCHAR(4000);

        SET @ErrorMessage = ERROR_MESSAGE();

        RAISERROR(
            @ErrorMessage,
            16,
            1
        );

    END CATCH
END
GO
/****** Objeto: StoredProcedure [dbo].[spAsignarEntrenadorCliente] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spAsignarEntrenadorCliente]
    @IdCliente INT,
    @IdEntrenador INT
AS
BEGIN
    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Usuario
            WHERE IdUsuario = @IdCliente
              AND IdRol = 3
        )
        BEGIN
            RAISERROR(
                'El cliente indicado no existe.',
                16,
                1
            );

            RETURN;
        END


        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Usuario
            WHERE IdUsuario = @IdCliente
              AND IdRol = 3
              AND Estado = 1
        )
        BEGIN
            RAISERROR(
                'No se puede asignar un entrenador a un cliente inactivo.',
                16,
                1
            );

            RETURN;
        END


        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Usuario
            WHERE IdUsuario = @IdEntrenador
              AND IdRol = 2
        )
        BEGIN
            RAISERROR(
                'El entrenador indicado no existe.',
                16,
                1
            );

            RETURN;
        END


        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Usuario
            WHERE IdUsuario = @IdEntrenador
              AND IdRol = 2
              AND Estado = 1
        )
        BEGIN
            RAISERROR(
                'No se puede asignar un entrenador inactivo.',
                16,
                1
            );

            RETURN;
        END


        IF EXISTS
        (
            SELECT 1
            FROM dbo.EntrenadorCliente
            WHERE IdCliente = @IdCliente
        )
        BEGIN

            UPDATE dbo.EntrenadorCliente
            SET IdEntrenador = @IdEntrenador,
                FechaAsignacion = GETDATE(),
                Estado = 1
            WHERE IdCliente = @IdCliente;

        END
        ELSE
        BEGIN

            INSERT INTO dbo.EntrenadorCliente
            (
                IdEntrenador,
                IdCliente,
                FechaAsignacion,
                Estado
            )
            VALUES
            (
                @IdEntrenador,
                @IdCliente,
                GETDATE(),
                1
            );

        END

    END TRY
    BEGIN CATCH

        DECLARE @ErrorMessage NVARCHAR(4000);

        SET @ErrorMessage =
            ERROR_MESSAGE();

        RAISERROR(
            @ErrorMessage,
            16,
            1
        );

    END CATCH
END
GO
/****** Objeto: StoredProcedure [dbo].[spAsignarMembresiaCliente] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spAsignarMembresiaCliente]
    @IdUsuario INT,
    @IdPlan INT
AS
BEGIN
    BEGIN TRY

        /* Validar que el usuario exista y sea miembro */

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Usuario
            WHERE IdUsuario = @IdUsuario
              AND IdRol = 3
        )
        BEGIN
            RAISERROR(
                'El cliente no existe.',
                16,
                1
            );

            RETURN;
        END


        /* Validar que el cliente esté activo */

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Usuario
            WHERE IdUsuario = @IdUsuario
              AND IdRol = 3
              AND Estado = 1
        )
        BEGIN
            RAISERROR(
                'No se puede asignar una membresía a un cliente inactivo.',
                16,
                1
            );

            RETURN;
        END


        /* Validar el plan */

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.PlanMembresia
            WHERE IdPlan = @IdPlan
        )
        BEGIN
            RAISERROR(
                'El plan de membresía seleccionado no existe.',
                16,
                1
            );

            RETURN;
        END


        /* Marcar como vencidas las membresías expiradas */

        UPDATE dbo.MembresiaUsuario
        SET EstadoMembresia = N'Vencida'
        WHERE IdUsuario = @IdUsuario
          AND EstadoMembresia = N'Activa'
          AND FechaVencimiento <= GETDATE();


        /* Evitar dos membresías activas */

        IF EXISTS
        (
            SELECT 1
            FROM dbo.MembresiaUsuario
            WHERE IdUsuario = @IdUsuario
              AND EstadoMembresia = N'Activa'
              AND FechaVencimiento > GETDATE()
        )
        BEGIN
            RAISERROR(
                'El cliente ya tiene una membresía activa.',
                16,
                1
            );

            RETURN;
        END


        DECLARE @DuracionDias INT;

        SELECT
            @DuracionDias = DuracionDias
        FROM dbo.PlanMembresia
        WHERE IdPlan = @IdPlan;


        DECLARE @FechaInicio DATETIME2(7);

        SET @FechaInicio = GETDATE();


        INSERT INTO dbo.MembresiaUsuario
        (
            IdUsuario,
            IdPlan,
            FechaRegistro,
            EstadoMembresia,
            FechaVencimiento
        )
        VALUES
        (
            @IdUsuario,
            @IdPlan,
            @FechaInicio,
            N'Activa',
            DATEADD(
                DAY,
                @DuracionDias,
                @FechaInicio
            )
        );

    END TRY
    BEGIN CATCH

        DECLARE @ErrorMessage NVARCHAR(4000);

        SET @ErrorMessage = ERROR_MESSAGE();

        RAISERROR(
            @ErrorMessage,
            16,
            1
        );

    END CATCH
END
GO
/****** Objeto: StoredProcedure [dbo].[spCambiarEstadoClaseProgramada] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spCambiarEstadoClaseProgramada]
    @IdClaseProg INT,
    @Estado BIT
AS
BEGIN
    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.ClaseProgramada
            WHERE IdClaseProg = @IdClaseProg
        )
        BEGIN
            RAISERROR(
                'La clase programada no existe.',
                16,
                1
            );

            RETURN;
        END


        UPDATE dbo.ClaseProgramada
        SET Estado = @Estado
        WHERE IdClaseProg = @IdClaseProg;

    END TRY
    BEGIN CATCH

        DECLARE @ErrorMessage NVARCHAR(4000);

        SET @ErrorMessage =
            ERROR_MESSAGE();

        RAISERROR(
            @ErrorMessage,
            16,
            1
        );

    END CATCH
END
GO
/****** Objeto: StoredProcedure [dbo].[spCambiarEstadoRutinaEntrenador] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spCambiarEstadoRutinaEntrenador]
    @IdEntrenador INT,
    @IdRutina INT,
    @Estado BIT
AS
BEGIN
    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Rutinas
            WHERE IdRutina = @IdRutina
              AND IdEntrenador = @IdEntrenador
        )
        BEGIN

            RAISERROR(
                'La rutina no existe o no pertenece a este entrenador.',
                16,
                1
            );

            RETURN;

        END


        UPDATE dbo.Rutinas
        SET Estado = @Estado
        WHERE IdRutina = @IdRutina
          AND IdEntrenador = @IdEntrenador;

    END TRY
    BEGIN CATCH

        DECLARE @ErrorMessage NVARCHAR(4000);

        SET @ErrorMessage =
            ERROR_MESSAGE();

        RAISERROR(
            @ErrorMessage,
            16,
            1
        );

    END CATCH
END
GO
/****** Objeto: StoredProcedure [dbo].[spCancelarMembresiaCliente] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spCancelarMembresiaCliente]
    @IdMembresia INT,
    @IdUsuario INT
AS
BEGIN
    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.MembresiaUsuario
            WHERE IdMembresia = @IdMembresia
              AND IdUsuario = @IdUsuario
        )
        BEGIN
            RAISERROR(
                'La membresía indicada no existe para este cliente.',
                16,
                1
            );

            RETURN;
        END


        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.MembresiaUsuario
            WHERE IdMembresia = @IdMembresia
              AND IdUsuario = @IdUsuario
              AND EstadoMembresia = N'Activa'
        )
        BEGIN
            RAISERROR(
                'La membresía ya no se encuentra activa.',
                16,
                1
            );

            RETURN;
        END


        UPDATE dbo.MembresiaUsuario
        SET EstadoMembresia = N'Cancelada'
        WHERE IdMembresia = @IdMembresia
          AND IdUsuario = @IdUsuario
          AND EstadoMembresia = N'Activa';

    END TRY
    BEGIN CATCH

        DECLARE @ErrorMessage NVARCHAR(4000);

        SET @ErrorMessage = ERROR_MESSAGE();

        RAISERROR(
            @ErrorMessage,
            16,
            1
        );

    END CATCH
END
GO
/****** Objeto: StoredProcedure [dbo].[spCancelarReservaClaseMiembro] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spCancelarReservaClaseMiembro]
    @IdMiembro INT,
    @IdReserva INT
AS
BEGIN
    SET XACT_ABORT ON;

    BEGIN TRY

        BEGIN TRANSACTION;


        DECLARE
            @IdClaseProg INT,
            @FechaHoraInicio DATETIME2(7),
            @EstadoReserva BIT;


        /*
         * Consultar la reserva y bloquear
         * temporalmente los registros.
         */

        SELECT
            @IdClaseProg =
                RC.IdClaseProg,

            @FechaHoraInicio =
                CP.FechaHoraInicio,

            @EstadoReserva =
                RC.Estado

        FROM dbo.ReservaClase AS RC
            WITH (UPDLOCK, HOLDLOCK)

        INNER JOIN dbo.ClaseProgramada AS CP
            WITH (UPDLOCK, HOLDLOCK)

            ON CP.IdClaseProg =
               RC.IdClaseProg

        WHERE RC.IdReserva =
              @IdReserva

          AND RC.IdMiembro =
              @IdMiembro;


        /*
         * Validar que la reserva exista
         * y pertenezca al miembro.
         */

        IF @IdClaseProg IS NULL
        BEGIN
            RAISERROR(
                'La reserva no existe o no pertenece a este miembro.',
                16,
                1
            );
        END


        /*
         * Validar que la reserva todavía
         * esté activa.
         */

        IF @EstadoReserva = 0
        BEGIN
            RAISERROR(
                'La reserva ya se encuentra cancelada.',
                16,
                1
            );
        END


        /*
         * Impedir cancelaciones después
         * de iniciada la clase.
         */

        IF @FechaHoraInicio <= GETDATE()
        BEGIN
            RAISERROR(
                'No se puede cancelar una reserva después de iniciada la clase.',
                16,
                1
            );
        END


        /*
         * Cancelar la reserva de forma lógica.
         */

        UPDATE dbo.ReservaClase
        SET
            Estado = 0,

            FechaCancelacion =
                SYSDATETIME()

        WHERE IdReserva =
              @IdReserva

          AND IdMiembro =
              @IdMiembro;


        /*
         * Liberar el cupo de la clase.
         */

        UPDATE dbo.ClaseProgramada
        SET CupoActual =
            CASE
                WHEN ISNULL(CupoActual, 0) > 0
                    THEN CupoActual - 1

                ELSE 0
            END

        WHERE IdClaseProg =
              @IdClaseProg;


        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH

        DECLARE @ErrorMessage NVARCHAR(4000);

        SET @ErrorMessage =
            ERROR_MESSAGE();


        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END


        RAISERROR(
            @ErrorMessage,
            16,
            1
        );

        RETURN;

    END CATCH
END
GO
/****** Objeto: StoredProcedure [dbo].[spConsultarUsuario] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
    FROM dbo.Usuario AS U
    WHERE U.IdUsuario = @IdUsuario;
END

GO
/****** Objeto: StoredProcedure [dbo].[spDashboardAdministrador] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spDashboardAdministrador]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT

        /* Usuarios */

        (
            SELECT COUNT(*)
            FROM dbo.Usuario
            WHERE IdRol = 3
        ) AS TotalClientes,

        (
            SELECT COUNT(*)
            FROM dbo.Usuario
            WHERE IdRol = 3
              AND Estado = 1
        ) AS ClientesActivos,

        (
            SELECT COUNT(*)
            FROM dbo.Usuario
            WHERE IdRol = 2
              AND Estado = 1
        ) AS EntrenadoresActivos,


        /* Membresías */

        (
            SELECT COUNT(*)
            FROM dbo.MembresiaUsuario
            WHERE EstadoMembresia = N'Activa'
              AND FechaVencimiento >=
                  CAST(GETDATE() AS DATE)
        ) AS MembresiasActivas,

        (
            SELECT COUNT(*)
            FROM dbo.MembresiaUsuario
            WHERE EstadoMembresia = N'Vencida'
               OR
               (
                   EstadoMembresia = N'Activa'
                   AND FechaVencimiento <
                       CAST(GETDATE() AS DATE)
               )
        ) AS MembresiasVencidas,

        (
            SELECT COUNT(*)
            FROM dbo.MembresiaUsuario
            WHERE EstadoMembresia = N'Cancelada'
        ) AS MembresiasCanceladas,


        /* Pagos */

        ISNULL(
            (
                SELECT SUM(Total)
                FROM dbo.Pago
            ),
            0
        ) AS IngresosTotales,

        ISNULL(
            (
                SELECT SUM(Total)
                FROM dbo.Pago
                WHERE YEAR(FechaEmision) =
                      YEAR(GETDATE())

                  AND MONTH(FechaEmision) =
                      MONTH(GETDATE())
            ),
            0
        ) AS IngresosMesActual,

        (
            SELECT COUNT(*)
            FROM dbo.Pago
            WHERE YEAR(FechaEmision) =
                  YEAR(GETDATE())

              AND MONTH(FechaEmision) =
                  MONTH(GETDATE())
        ) AS PagosMesActual,


        /* Clases */

        (
            SELECT COUNT(*)
            FROM dbo.ClaseProgramada
            WHERE Estado = 1
              AND FechaHoraInicio >
                  GETDATE()
        ) AS ClasesProgramadas,

        (
            SELECT COUNT(*)
            FROM dbo.ReservaClase
            WHERE Estado = 1
        ) AS ReservasActivas,


        /* Asistencia */

        (
            SELECT COUNT(*)
            FROM dbo.AsistenciaClase
            WHERE EstadoAsistencia =
                  N'Presente'
        ) AS TotalPresentes,

        (
            SELECT COUNT(*)
            FROM dbo.AsistenciaClase
            WHERE EstadoAsistencia =
                  N'Ausente'
        ) AS TotalAusentes;
END
GO
/****** Objeto: StoredProcedure [dbo].[spDashboardPagosRecientes] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spDashboardPagosRecientes]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 5
        P.IdPago,
        P.IdMembresia,

        U.IdUsuario,

        U.Nombre
            + N' '
            + U.Apellido
            AS NombreCliente,

        PM.NombrePlan,

        P.MetodoPago,
        P.FechaEmision,
        P.Total

    FROM dbo.Pago AS P

    INNER JOIN dbo.MembresiaUsuario AS MU
        ON MU.IdMembresia =
           P.IdMembresia

    INNER JOIN dbo.Usuario AS U
        ON U.IdUsuario =
           MU.IdUsuario

    INNER JOIN dbo.PlanMembresia AS PM
        ON PM.IdPlan =
           MU.IdPlan

    ORDER BY
        P.FechaEmision DESC,
        P.IdPago DESC;
END
GO
/****** Objeto: StoredProcedure [dbo].[spDashboardProximasClases] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spDashboardProximasClases]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 5
        CP.IdClaseProg,

        C.NombreClase,

        U.Nombre
            + N' '
            + U.Apellido
            AS NombreEntrenador,

        CP.FechaHoraInicio,
        CP.FechaHoraFin,
        CP.CupoMaximo,
        CP.CupoActual,

        CP.CupoMaximo
            - CP.CupoActual
            AS CuposDisponibles

    FROM dbo.ClaseProgramada AS CP

    INNER JOIN dbo.Clase AS C
        ON C.IdClase =
           CP.IdClase

    INNER JOIN dbo.Usuario AS U
        ON U.IdUsuario =
           CP.IdEntrenador

    WHERE CP.Estado = 1
      AND CP.FechaHoraInicio >
          GETDATE()

    ORDER BY
        CP.FechaHoraInicio ASC;
END
GO
/****** Objeto: StoredProcedure [dbo].[spDetalleClaseEntrenador] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spDetalleClaseEntrenador]
    @IdEntrenador INT,
    @IdClaseProg INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CP.IdClaseProg,
        CP.IdEntrenador,
        CP.IdClase,

        C.NombreClase,
        C.Descripcion AS DescripcionClase,

        U.Nombre
            + N' '
            + U.Apellido
            AS NombreEntrenador,

        U.CorreoElectronico
            AS CorreoEntrenador,

        U.Telefono
            AS TelefonoEntrenador,

        CP.FechaHoraInicio,
        CP.FechaHoraFin,

        CP.CupoMaximo,
        CP.CupoActual,

        CP.CupoMaximo - CP.CupoActual
            AS CuposDisponibles,

        CP.Estado,

        CASE
            WHEN CP.Estado = 0
                THEN N'Inactiva'

            WHEN CP.FechaHoraFin < GETDATE()
                THEN N'Finalizada'

            WHEN GETDATE() BETWEEN
                 CP.FechaHoraInicio
                 AND CP.FechaHoraFin
                THEN N'En curso'

            ELSE N'Programada'
        END AS EstadoHorario

    FROM dbo.ClaseProgramada AS CP

    INNER JOIN dbo.Clase AS C
        ON C.IdClase = CP.IdClase

    INNER JOIN dbo.Usuario AS U
        ON U.IdUsuario = CP.IdEntrenador

    WHERE CP.IdClaseProg = @IdClaseProg
      AND CP.IdEntrenador = @IdEntrenador
      AND U.IdRol = 2;
END
GO
/****** Objeto: StoredProcedure [dbo].[spDetalleClaseProgramada] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spDetalleClaseProgramada]
    @IdClaseProg INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CP.IdClaseProg,
        CP.IdEntrenador,
        CP.IdClase,

        C.NombreClase,
        C.Descripcion AS DescripcionClase,

        U.Nombre
            + N' '
            + U.Apellido
            AS NombreEntrenador,

        U.CorreoElectronico
            AS CorreoEntrenador,

        U.Telefono
            AS TelefonoEntrenador,

        CP.FechaHoraInicio,
        CP.FechaHoraFin,
        CP.CupoMaximo,
        CP.CupoActual,

        CP.CupoMaximo
            - CP.CupoActual
            AS CuposDisponibles,

        CP.Estado

    FROM dbo.ClaseProgramada AS CP

    INNER JOIN dbo.Clase AS C
        ON C.IdClase = CP.IdClase

    INNER JOIN dbo.Usuario AS U
        ON U.IdUsuario = CP.IdEntrenador

    WHERE CP.IdClaseProg = @IdClaseProg
      AND U.IdRol = 2;
END
GO
/****** Objeto: StoredProcedure [dbo].[spDetalleCliente] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spDetalleCliente]
    @IdUser INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.IdUsuario,
        U.Identificacion,
        U.Nombre,
        U.Apellido,
        U.FechaNacimiento AS FechaNac,
        U.Telefono,
        U.CorreoElectronico,
        U.Direccion,
        U.FechaRegistro,
        U.Estado,
        R.IdRol,
        R.NombreRol,

        M.IdMembresia,
        M.IdPlan,
        M.NombrePlan,
        M.Precio,
        M.FechaInicio,
        M.FechaVencimiento,

        CASE
            WHEN M.EstadoMembresia = N'Activa'
                 AND M.FechaVencimiento <= GETDATE()
            THEN N'Vencida'

            ELSE M.EstadoMembresia
        END AS EstadoMembresia

    FROM dbo.Usuario AS U

    INNER JOIN dbo.Rol AS R
        ON R.IdRol = U.IdRol

    OUTER APPLY
    (
        SELECT TOP 1
            MU.IdMembresia,
            MU.IdPlan,
            PM.NombrePlan,
            PM.Precio,
            MU.FechaRegistro AS FechaInicio,
            MU.FechaVencimiento,
            MU.EstadoMembresia

        FROM dbo.MembresiaUsuario AS MU

        INNER JOIN dbo.PlanMembresia AS PM
            ON PM.IdPlan = MU.IdPlan

        WHERE MU.IdUsuario = U.IdUsuario

        ORDER BY MU.IdMembresia DESC
    ) AS M

    WHERE U.IdUsuario = @IdUser
      AND U.IdRol = 3;
END
GO
/****** Objeto: StoredProcedure [dbo].[spDetalleClienteEntrenador] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spDetalleClienteEntrenador]
    @IdEntrenador INT,
    @IdCliente INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        C.IdUsuario,
        C.Identificacion,
        C.Nombre,
        C.Apellido,
        C.FechaNacimiento AS FechaNac,
        C.Telefono,
        C.CorreoElectronico,
        C.Direccion,
        C.Estado,

        EC.FechaAsignacion,

        M.IdMembresia,
        M.IdPlan,
        M.NombrePlan,
        M.EstadoMembresia,
        M.FechaInicio,
        M.FechaVencimiento

    FROM dbo.EntrenadorCliente AS EC

    INNER JOIN dbo.Usuario AS C
        ON C.IdUsuario = EC.IdCliente

    OUTER APPLY
    (
        SELECT TOP 1
            MU.IdMembresia,
            MU.IdPlan,
            PM.NombrePlan,

            CASE
                WHEN MU.EstadoMembresia = N'Activa'
                     AND MU.FechaVencimiento <= GETDATE()
                THEN N'Vencida'

                ELSE MU.EstadoMembresia
            END AS EstadoMembresia,

            MU.FechaRegistro AS FechaInicio,
            MU.FechaVencimiento

        FROM dbo.MembresiaUsuario AS MU

        INNER JOIN dbo.PlanMembresia AS PM
            ON PM.IdPlan = MU.IdPlan

        WHERE MU.IdUsuario = C.IdUsuario

        ORDER BY MU.IdMembresia DESC
    ) AS M

    WHERE EC.IdEntrenador = @IdEntrenador
      AND EC.IdCliente = @IdCliente
      AND EC.Estado = 1
      AND C.IdRol = 3;
END
GO
/****** Objeto: StoredProcedure [dbo].[spDetalleEjercicioRutinaEntrenador] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spDetalleEjercicioRutinaEntrenador]
    @IdEntrenador INT,
    @IdRutinaEjercicio INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        RE.IdRutinaEjercicio,
        RE.IdRutina,
        RE.IdEjercicio,

        E.NombreEjercicio,
        E.GrupoMuscular,
        E.Descripcion,

        RE.DiaSemana,
        RE.Series,
        RE.Repeticiones,
        RE.PesoSugerido,
        RE.DescansoSegundos,
        RE.Observacion,
        RE.Orden,
        RE.FechaAsignacion,
        RE.Estado

    FROM dbo.RutinaEjercicio AS RE

    INNER JOIN dbo.Rutinas AS R
        ON R.IdRutina = RE.IdRutina

    INNER JOIN dbo.Ejercicio AS E
        ON E.IdEjercicio = RE.IdEjercicio

    WHERE RE.IdRutinaEjercicio = @IdRutinaEjercicio
      AND R.IdEntrenador = @IdEntrenador;
END
GO
/****** Objeto: StoredProcedure [dbo].[spDetalleEmpleado] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* ============================================================
   PROCEDIMIENTO: DETALLE DE EMPLEADO
   ============================================================ */

CREATE PROCEDURE [dbo].[spDetalleEmpleado]
    @IdUser INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.IdUsuario,
        U.Nombre,
        U.Apellido,
        U.Identificacion,
        U.FechaNacimiento AS FechaNac,
        U.Telefono,
        U.CorreoElectronico,
        R.IdRol,
        R.NombreRol,
        U.Direccion,
        U.FechaRegistro,
        U.Estado
    FROM dbo.Usuario AS U
    INNER JOIN dbo.Rol AS R
        ON R.IdRol = U.IdRol
    WHERE U.IdUsuario = @IdUser
      AND U.IdRol IN (1, 2);
END

GO
/****** Objeto: StoredProcedure [dbo].[spDetalleMiembro] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spDetalleMiembro]
    @IdMiembro INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.IdUsuario,
        U.Identificacion,
        U.Nombre,
        U.Apellido,
        U.FechaNacimiento AS FechaNac,
        U.Telefono,
        U.CorreoElectronico,
        U.Direccion,
        U.Estado,

        /* Membresía actual */

        M.IdMembresia,
        M.IdPlan,
        M.NombrePlan,
        M.PrecioPlan,
        M.EstadoMembresia,
        M.FechaInicio,
        M.FechaVencimiento,

        /* Entrenador asignado */

        E.IdEntrenador,
        E.NombreEntrenador,
        E.CorreoEntrenador,
        E.TelefonoEntrenador,
        E.FechaAsignacionEntrenador

    FROM dbo.Usuario AS U

    OUTER APPLY
    (
        SELECT TOP 1
            MU.IdMembresia,
            MU.IdPlan,
            PM.NombrePlan,
            PM.Precio AS PrecioPlan,

            CASE
                WHEN MU.EstadoMembresia = N'Activa'
                     AND MU.FechaVencimiento <= GETDATE()
                THEN N'Vencida'

                ELSE MU.EstadoMembresia
            END AS EstadoMembresia,

            MU.FechaRegistro AS FechaInicio,
            MU.FechaVencimiento

        FROM dbo.MembresiaUsuario AS MU

        INNER JOIN dbo.PlanMembresia AS PM
            ON PM.IdPlan = MU.IdPlan

        WHERE MU.IdUsuario = U.IdUsuario

        ORDER BY MU.IdMembresia DESC
    ) AS M

    OUTER APPLY
    (
        SELECT TOP 1
            EC.IdEntrenador,

            EN.Nombre
                + N' '
                + EN.Apellido
                AS NombreEntrenador,

            EN.CorreoElectronico
                AS CorreoEntrenador,

            EN.Telefono
                AS TelefonoEntrenador,

            EC.FechaAsignacion
                AS FechaAsignacionEntrenador

        FROM dbo.EntrenadorCliente AS EC

        INNER JOIN dbo.Usuario AS EN
            ON EN.IdUsuario = EC.IdEntrenador

        WHERE EC.IdCliente = U.IdUsuario
          AND EC.Estado = 1
          AND EN.IdRol = 2
          AND EN.Estado = 1

        ORDER BY EC.FechaAsignacion DESC
    ) AS E

    WHERE U.IdUsuario = @IdMiembro
      AND U.IdRol = 3;
END
GO
/****** Objeto: StoredProcedure [dbo].[spDetallePago] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spDetallePago]
    @IdPago INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        P.IdPago,
        P.IdMembresia,
        MU.IdUsuario,

        U.Identificacion,

        U.Nombre
            + N' '
            + U.Apellido
            AS NombreCliente,

        U.CorreoElectronico,

        PM.IdPlan,
        PM.NombrePlan,
        PM.DuracionDias,

        MU.FechaRegistro
            AS FechaInicioMembresia,

        MU.FechaVencimiento,

        MU.EstadoMembresia,

        P.MetodoPago,
        P.FechaEmision,
        P.NumeroReferencia,
        P.Monto,
        P.Subtotal,
        P.Impuesto,
        P.Total

    FROM dbo.Pago AS P

    INNER JOIN dbo.MembresiaUsuario AS MU
        ON MU.IdMembresia = P.IdMembresia

    INNER JOIN dbo.Usuario AS U
        ON U.IdUsuario = MU.IdUsuario

    INNER JOIN dbo.PlanMembresia AS PM
        ON PM.IdPlan = MU.IdPlan

    WHERE P.IdPago = @IdPago
      AND U.IdRol = 3;
END
GO
/****** Objeto: StoredProcedure [dbo].[spDetallePagoMiembro] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spDetallePagoMiembro]
    @IdMiembro INT,
    @IdPago INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        P.IdPago,
        P.IdMembresia,

        MU.IdUsuario,
        MU.IdPlan,

        U.Identificacion,

        U.Nombre
            + N' '
            + U.Apellido
            AS NombreMiembro,

        U.CorreoElectronico,

        PM.NombrePlan,
        PM.Descripcion AS DescripcionPlan,
        PM.DuracionDias,

        P.MetodoPago,
        P.FechaEmision,
        P.NumeroReferencia,

        P.Monto,
        P.Subtotal,
        P.Impuesto,
        P.Total,

        MU.FechaRegistro AS FechaInicio,
        MU.FechaVencimiento,
        MU.EstadoMembresia

    FROM dbo.Pago AS P

    INNER JOIN dbo.MembresiaUsuario AS MU
        ON MU.IdMembresia = P.IdMembresia

    INNER JOIN dbo.PlanMembresia AS PM
        ON PM.IdPlan = MU.IdPlan

    INNER JOIN dbo.Usuario AS U
        ON U.IdUsuario = MU.IdUsuario

    WHERE P.IdPago = @IdPago
      AND MU.IdUsuario = @IdMiembro
      AND U.IdRol = 3;
END
GO
/****** Objeto: StoredProcedure [dbo].[spDetallePlanMembresia] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* ============================================================
   CONSULTAR DETALLE DE UN PLAN
   ============================================================ */

CREATE   PROCEDURE [dbo].[spDetallePlanMembresia]
    @IdPlan INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdPlan,
        NombrePlan,
        Descripcion,
        DuracionDias,
        Precio
    FROM dbo.PlanMembresia
    WHERE IdPlan = @IdPlan;
END
GO
/****** Objeto: StoredProcedure [dbo].[spDetalleRutinaEntrenador] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spDetalleRutinaEntrenador]
    @IdEntrenador INT,
    @IdRutina INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        R.IdRutina,
        R.IdEntrenador,
        R.IdCliente,
        R.NombreRutina,
        R.Nivel,
        R.Detalles,
        R.FechaCreacion,
        R.Estado,

        C.Identificacion,

        C.Nombre
            + N' '
            + C.Apellido
            AS NombreCliente,

        C.CorreoElectronico

    FROM dbo.Rutinas AS R

    INNER JOIN dbo.Usuario AS C
        ON C.IdUsuario = R.IdCliente

    WHERE R.IdRutina = @IdRutina
      AND R.IdEntrenador = @IdEntrenador
      AND C.IdRol = 3;
END
GO
/****** Objeto: StoredProcedure [dbo].[spDetalleRutinaMiembro] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spDetalleRutinaMiembro]
    @IdMiembro INT,
    @IdRutina INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        R.IdRutina,
        R.IdEntrenador,
        R.IdCliente,
        R.NombreRutina,
        R.Nivel,
        R.Detalles,
        R.FechaCreacion,
        R.Estado,

        EN.Nombre
            + N' '
            + EN.Apellido
            AS NombreEntrenador,

        EN.CorreoElectronico
            AS CorreoEntrenador,

        EN.Telefono
            AS TelefonoEntrenador,

        (
            SELECT COUNT(1)

            FROM dbo.RutinaEjercicio AS RE

            WHERE RE.IdRutina = R.IdRutina
              AND RE.Estado = 1
        ) AS CantidadEjercicios

    FROM dbo.Rutinas AS R

    INNER JOIN dbo.Usuario AS EN
        ON EN.IdUsuario = R.IdEntrenador

    WHERE R.IdRutina = @IdRutina
      AND R.IdCliente = @IdMiembro
      AND R.Estado = 1
      AND EN.IdRol = 2;
END
GO
/****** Objeto: StoredProcedure [dbo].[spEditarClaseProgramada] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spEditarClaseProgramada]
    @IdClaseProg INT,
    @IdEntrenador INT,
    @IdClase INT,
    @FechaHoraInicio DATETIME2(7),
    @FechaHoraFin DATETIME2(7),
    @CupoMaximo INT
AS
BEGIN
    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.ClaseProgramada
            WHERE IdClaseProg = @IdClaseProg
        )
        BEGIN
            RAISERROR(
                'La clase programada no existe.',
                16,
                1
            );

            RETURN;
        END


        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Usuario
            WHERE IdUsuario = @IdEntrenador
              AND IdRol = 2
              AND Estado = 1
        )
        BEGIN
            RAISERROR(
                'El entrenador no existe o se encuentra inactivo.',
                16,
                1
            );

            RETURN;
        END


        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Clase
            WHERE IdClase = @IdClase
        )
        BEGIN
            RAISERROR(
                'La clase seleccionada no existe.',
                16,
                1
            );

            RETURN;
        END


        IF @FechaHoraFin <= @FechaHoraInicio
        BEGIN
            RAISERROR(
                'La hora de finalización debe ser posterior a la hora de inicio.',
                16,
                1
            );

            RETURN;
        END


        IF @CupoMaximo <= 0
        BEGIN
            RAISERROR(
                'El cupo máximo debe ser mayor que cero.',
                16,
                1
            );

            RETURN;
        END


        IF EXISTS
        (
            SELECT 1
            FROM dbo.ClaseProgramada
            WHERE IdClaseProg = @IdClaseProg
              AND CupoActual > @CupoMaximo
        )
        BEGIN
            RAISERROR(
                'El cupo máximo no puede ser menor que la cantidad de participantes actuales.',
                16,
                1
            );

            RETURN;
        END


        IF EXISTS
        (
            SELECT 1
            FROM dbo.ClaseProgramada
            WHERE IdEntrenador = @IdEntrenador
              AND IdClaseProg != @IdClaseProg
              AND Estado = 1
              AND @FechaHoraInicio < FechaHoraFin
              AND @FechaHoraFin > FechaHoraInicio
        )
        BEGIN
            RAISERROR(
                'El entrenador ya tiene otra clase programada dentro de ese horario.',
                16,
                1
            );

            RETURN;
        END


        UPDATE dbo.ClaseProgramada
        SET
            IdEntrenador =
                @IdEntrenador,

            IdClase =
                @IdClase,

            FechaHoraInicio =
                @FechaHoraInicio,

            FechaHoraFin =
                @FechaHoraFin,

            CupoMaximo =
                @CupoMaximo

        WHERE IdClaseProg =
              @IdClaseProg;

    END TRY
    BEGIN CATCH

        DECLARE @ErrorMessage NVARCHAR(4000);

        SET @ErrorMessage =
            ERROR_MESSAGE();

        RAISERROR(
            @ErrorMessage,
            16,
            1
        );

    END CATCH
END
GO
/****** Objeto: StoredProcedure [dbo].[spEditarCliente] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* ============================================================
   PROCEDIMIENTO: EDITAR CLIENTE
   ============================================================ */

CREATE PROCEDURE [dbo].[spEditarCliente]
    @IdUser INT,
    @Nombre NVARCHAR(80),
    @Apellido NVARCHAR(80),
    @FechaNacimiento DATETIME2(7),
    @Telefono NVARCHAR(30),
    @CorreoElectronico NVARCHAR(150),
    @Direccion NVARCHAR(200)
AS
BEGIN
    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Usuario
            WHERE IdUsuario = @IdUser
              AND IdRol = 3
        )
        BEGIN
            RAISERROR(
                'El cliente no existe.',
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
              AND IdUsuario != @IdUser
        )
        BEGIN
            RAISERROR(
                'El correo electrónico ya está registrado por otro usuario.',
                16,
                1
            );

            RETURN;
        END

        UPDATE dbo.Usuario
        SET Nombre = @Nombre,
            Apellido = @Apellido,
            FechaNacimiento = @FechaNacimiento,
            Telefono = @Telefono,
            CorreoElectronico = @CorreoElectronico,
            Direccion = @Direccion
        WHERE IdUsuario = @IdUser
          AND IdRol = 3;

    END TRY
    BEGIN CATCH

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
/****** Objeto: StoredProcedure [dbo].[spEditarEjercicioRutinaEntrenador] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spEditarEjercicioRutinaEntrenador]
    @IdEntrenador INT,
    @IdRutinaEjercicio INT,
    @DiaSemana NVARCHAR(20),
    @Series INT,
    @Repeticiones INT,
    @PesoSugerido DECIMAL(6,2),
    @DescansoSegundos INT,
    @Observacion NVARCHAR(250),
    @Orden INT
AS
BEGIN
    BEGIN TRY

        /* =====================================================
           VALIDAR QUE EL EJERCICIO ASIGNADO PERTENEZCA
           AL ENTRENADOR
           ===================================================== */

        IF NOT EXISTS
        (
            SELECT 1

            FROM dbo.RutinaEjercicio AS RE

            INNER JOIN dbo.Rutinas AS R
                ON R.IdRutina = RE.IdRutina

            WHERE RE.IdRutinaEjercicio =
                    @IdRutinaEjercicio

              AND R.IdEntrenador =
                    @IdEntrenador

              AND RE.Estado = 1
        )
        BEGIN

            RAISERROR(
                'El ejercicio asignado no existe, está retirado o no pertenece a este entrenador.',
                16,
                1
            );

            RETURN;

        END


        /* =====================================================
           VALIDAR DÍA DE LA SEMANA
           ===================================================== */

        IF @DiaSemana NOT IN
        (
            N'Lunes',
            N'Martes',
            N'Miércoles',
            N'Jueves',
            N'Viernes',
            N'Sábado',
            N'Domingo'
        )
        BEGIN

            RAISERROR(
                'El día de la semana seleccionado no es válido.',
                16,
                1
            );

            RETURN;

        END


        /* =====================================================
           VALIDAR SERIES
           ===================================================== */

        IF @Series <= 0
        BEGIN

            RAISERROR(
                'La cantidad de series debe ser mayor que cero.',
                16,
                1
            );

            RETURN;

        END


        /* =====================================================
           VALIDAR REPETICIONES
           ===================================================== */

        IF @Repeticiones <= 0
        BEGIN

            RAISERROR(
                'La cantidad de repeticiones debe ser mayor que cero.',
                16,
                1
            );

            RETURN;

        END


        /* =====================================================
           VALIDAR PESO
           ===================================================== */

        IF @PesoSugerido IS NOT NULL
           AND @PesoSugerido < 0
        BEGIN

            RAISERROR(
                'El peso sugerido no puede ser negativo.',
                16,
                1
            );

            RETURN;

        END


        /* =====================================================
           VALIDAR DESCANSO
           ===================================================== */

        IF @DescansoSegundos IS NOT NULL
           AND @DescansoSegundos < 0
        BEGIN

            RAISERROR(
                'El tiempo de descanso no puede ser negativo.',
                16,
                1
            );

            RETURN;

        END


        /* =====================================================
           VALIDAR ORDEN
           ===================================================== */

        IF @Orden <= 0
        BEGIN

            RAISERROR(
                'El orden debe ser mayor que cero.',
                16,
                1
            );

            RETURN;

        END


        /* =====================================================
           EVITAR EL MISMO EJERCICIO DOS VECES EL MISMO DÍA
           ===================================================== */

        IF EXISTS
        (
            SELECT 1

            FROM dbo.RutinaEjercicio AS Actual

            INNER JOIN dbo.Rutinas AS R
                ON R.IdRutina = Actual.IdRutina

            INNER JOIN dbo.RutinaEjercicio AS Otro
                ON Otro.IdRutina = Actual.IdRutina

            WHERE Actual.IdRutinaEjercicio =
                    @IdRutinaEjercicio

              AND R.IdEntrenador =
                    @IdEntrenador

              AND Otro.IdEjercicio =
                    Actual.IdEjercicio

              AND Otro.DiaSemana =
                    @DiaSemana

              AND Otro.IdRutinaEjercicio !=
                    @IdRutinaEjercicio

              AND Otro.Estado = 1
        )
        BEGIN

            RAISERROR(
                'El ejercicio ya está agregado a esta rutina para el día seleccionado.',
                16,
                1
            );

            RETURN;

        END


        /* =====================================================
           ACTUALIZAR CONFIGURACIÓN
           ===================================================== */

        UPDATE RE

        SET
            RE.DiaSemana =
                @DiaSemana,

            RE.Series =
                @Series,

            RE.Repeticiones =
                @Repeticiones,

            RE.PesoSugerido =
                @PesoSugerido,

            RE.DescansoSegundos =
                @DescansoSegundos,

            RE.Observacion =
                NULLIF(
                    LTRIM(RTRIM(@Observacion)),
                    N''
                ),

            RE.Orden =
                @Orden

        FROM dbo.RutinaEjercicio AS RE

        INNER JOIN dbo.Rutinas AS R
            ON R.IdRutina = RE.IdRutina

        WHERE RE.IdRutinaEjercicio =
                @IdRutinaEjercicio

          AND R.IdEntrenador =
                @IdEntrenador

          AND RE.Estado = 1;

    END TRY
    BEGIN CATCH

        DECLARE @ErrorMessage NVARCHAR(4000);

        SET @ErrorMessage =
            ERROR_MESSAGE();

        RAISERROR(
            @ErrorMessage,
            16,
            1
        );

    END CATCH
END
GO
/****** Objeto: StoredProcedure [dbo].[spEditarEmpleado] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* ============================================================
   PROCEDIMIENTO: EDITAR EMPLEADO
   ============================================================ */

CREATE PROCEDURE [dbo].[spEditarEmpleado]
    @IdUser INT,
    @Estado BIT,
    @Rol INT,
    @IdUsuarioActual INT
AS
BEGIN
    BEGIN TRY

        IF @IdUser = @IdUsuarioActual
        BEGIN
            RAISERROR(
                'No puede modificar su propio rol o estado.',
                16,
                1
            );

            RETURN;
        END

        IF @Rol NOT IN (1, 2)
        BEGIN
            RAISERROR(
                'El rol seleccionado no es válido para un empleado.',
                16,
                1
            );

            RETURN;
        END

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Rol
            WHERE IdRol = @Rol
              AND IdRol IN (1, 2)
        )
        BEGIN
            RAISERROR(
                'El rol seleccionado no existe.',
                16,
                1
            );

            RETURN;
        END

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Usuario
            WHERE IdUsuario = @IdUser
              AND IdRol IN (1, 2)
        )
        BEGIN
            RAISERROR(
                'El empleado no existe o el usuario seleccionado es un cliente.',
                16,
                1
            );

            RETURN;
        END

        UPDATE dbo.Usuario
        SET Estado = @Estado,
            IdRol = @Rol
        WHERE IdUsuario = @IdUser
          AND IdRol IN (1, 2);

    END TRY
    BEGIN CATCH

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
/****** Objeto: StoredProcedure [dbo].[spEditarEstadoCliente] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* ============================================================
   PROCEDIMIENTO: EDITAR ESTADO DEL CLIENTE
   ============================================================ */

CREATE PROCEDURE [dbo].[spEditarEstadoCliente]
    @IdUser INT,
    @Estado BIT
AS
BEGIN
    UPDATE dbo.Usuario
    SET Estado = @Estado
    WHERE IdUsuario = @IdUser
      AND IdRol = 3;
END

GO
/****** Objeto: StoredProcedure [dbo].[spEditarPlanMembresia] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* ============================================================
   MODIFICAR PLAN DE MEMBRESÍA
   ============================================================ */

CREATE   PROCEDURE [dbo].[spEditarPlanMembresia]
    @IdPlan INT,
    @NombrePlan NVARCHAR(100),
    @Descripcion NVARCHAR(300),
    @DuracionDias INT,
    @Precio DECIMAL(10,2)
AS
BEGIN
    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.PlanMembresia
            WHERE IdPlan = @IdPlan
        )
        BEGIN
            RAISERROR(
                'El plan de membresía no existe.',
                16,
                1
            );

            RETURN;
        END

        IF LTRIM(RTRIM(@NombrePlan)) = ''
        BEGIN
            RAISERROR(
                'El nombre del plan es obligatorio.',
                16,
                1
            );

            RETURN;
        END

        IF @DuracionDias <= 0
        BEGIN
            RAISERROR(
                'La duración del plan debe ser mayor que cero.',
                16,
                1
            );

            RETURN;
        END

        IF @Precio <= 0
        BEGIN
            RAISERROR(
                'El precio del plan debe ser mayor que cero.',
                16,
                1
            );

            RETURN;
        END

        IF EXISTS
        (
            SELECT 1
            FROM dbo.PlanMembresia
            WHERE NombrePlan = @NombrePlan
              AND IdPlan != @IdPlan
        )
        BEGIN
            RAISERROR(
                'Ya existe otro plan con el mismo nombre.',
                16,
                1
            );

            RETURN;
        END

        UPDATE dbo.PlanMembresia
        SET NombrePlan = LTRIM(RTRIM(@NombrePlan)),
            Descripcion = @Descripcion,
            DuracionDias = @DuracionDias,
            Precio = @Precio
        WHERE IdPlan = @IdPlan;

    END TRY
    BEGIN CATCH

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
/****** Objeto: StoredProcedure [dbo].[spEditarRutinaEntrenador] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spEditarRutinaEntrenador]
    @IdEntrenador INT,
    @IdRutina INT,
    @NombreRutina NVARCHAR(100),
    @Nivel NVARCHAR(20),
    @Detalles NVARCHAR(MAX)
AS
BEGIN
    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Rutinas
            WHERE IdRutina = @IdRutina
              AND IdEntrenador = @IdEntrenador
        )
        BEGIN

            RAISERROR(
                'La rutina no existe o no pertenece a este entrenador.',
                16,
                1
            );

            RETURN;

        END


        IF LTRIM(RTRIM(@NombreRutina)) = N''
        BEGIN

            RAISERROR(
                'El nombre de la rutina es obligatorio.',
                16,
                1
            );

            RETURN;

        END


        IF @Nivel NOT IN
        (
            N'Principiante',
            N'Intermedio',
            N'Avanzado'
        )
        BEGIN

            RAISERROR(
                'El nivel seleccionado no es válido.',
                16,
                1
            );

            RETURN;

        END


        IF @Detalles IS NULL
           OR LTRIM(RTRIM(@Detalles)) = N''
        BEGIN

            RAISERROR(
                'Los detalles de la rutina son obligatorios.',
                16,
                1
            );

            RETURN;

        END


        IF EXISTS
        (
            SELECT 1
            FROM dbo.Rutinas AS R1

            INNER JOIN dbo.Rutinas AS R2
                ON R2.IdRutina = @IdRutina

            WHERE R1.IdEntrenador =
                  @IdEntrenador

              AND R1.IdCliente =
                  R2.IdCliente

              AND R1.NombreRutina =
                  LTRIM(RTRIM(@NombreRutina))

              AND R1.IdRutina !=
                  @IdRutina

              AND R1.Estado = 1
        )
        BEGIN

            RAISERROR(
                'El cliente ya tiene otra rutina activa con ese nombre.',
                16,
                1
            );

            RETURN;

        END


        UPDATE dbo.Rutinas
        SET NombreRutina =
                LTRIM(RTRIM(@NombreRutina)),

            Nivel =
                @Nivel,

            Detalles =
                LTRIM(RTRIM(@Detalles))

        WHERE IdRutina =
              @IdRutina

          AND IdEntrenador =
              @IdEntrenador;

    END TRY
    BEGIN CATCH

        DECLARE @ErrorMessage NVARCHAR(4000);

        SET @ErrorMessage =
            ERROR_MESSAGE();

        RAISERROR(
            @ErrorMessage,
            16,
            1
        );

    END CATCH
END
GO
/****** Objeto: StoredProcedure [dbo].[spIniciarSesionUsuario] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spIniciarSesionUsuario]
    @CorreoElectronico NVARCHAR(150)
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
    FROM dbo.Usuario AS U
    INNER JOIN dbo.Rol AS R
        ON U.IdRol = R.IdRol
    WHERE U.CorreoElectronico = @CorreoElectronico
      AND U.Estado = 1;
END
GO
/****** Objeto: StoredProcedure [dbo].[spListarAsistenciaClaseEntrenador] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spListarAsistenciaClaseEntrenador]
    @IdEntrenador INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CP.IdClaseProg,
        CP.IdClase,
        CP.IdEntrenador,

        C.NombreClase,
        C.Descripcion AS DescripcionClase,

        CP.FechaHoraInicio,
        CP.FechaHoraFin,

        CP.CupoMaximo,
        CP.CupoActual,

        COUNT(
            CASE
                WHEN RC.Estado = 1
                    THEN 1
            END
        ) AS TotalParticipantes,

        COUNT(
            CASE
                WHEN RC.Estado = 1
                 AND AC.EstadoAsistencia =
                     N'Presente'
                    THEN 1
            END
        ) AS TotalPresentes,

        COUNT(
            CASE
                WHEN RC.Estado = 1
                 AND AC.EstadoAsistencia =
                     N'Ausente'
                    THEN 1
            END
        ) AS TotalAusentes,

        COUNT(
            CASE
                WHEN RC.Estado = 1
                 AND AC.IdAsistencia IS NULL
                    THEN 1
            END
        ) AS TotalPendientes,

        CP.Estado,

        CASE
            WHEN CP.Estado = 0
                THEN N'Inactiva'

            WHEN CP.FechaHoraFin <
                 GETDATE()
                THEN N'Finalizada'

            WHEN GETDATE() BETWEEN
                 CP.FechaHoraInicio
                 AND CP.FechaHoraFin
                THEN N'En curso'

            ELSE N'Programada'
        END AS EstadoHorario

    FROM dbo.ClaseProgramada AS CP

    INNER JOIN dbo.Clase AS C
        ON C.IdClase =
           CP.IdClase

    LEFT JOIN dbo.ReservaClase AS RC
        ON RC.IdClaseProg =
           CP.IdClaseProg

    LEFT JOIN dbo.AsistenciaClase AS AC
        ON AC.IdReserva =
           RC.IdReserva

    WHERE CP.IdEntrenador =
          @IdEntrenador

    GROUP BY
        CP.IdClaseProg,
        CP.IdClase,
        CP.IdEntrenador,
        C.NombreClase,
        C.Descripcion,
        CP.FechaHoraInicio,
        CP.FechaHoraFin,
        CP.CupoMaximo,
        CP.CupoActual,
        CP.Estado

    ORDER BY
        CASE
            WHEN CP.FechaHoraFin >=
                 GETDATE()
                THEN 0
            ELSE 1
        END,

        CP.FechaHoraInicio ASC;
END
GO
/****** Objeto: StoredProcedure [dbo].[spListarClases] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spListarClases]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        C.IdClase,
        C.NombreClase,
        C.Descripcion

    FROM dbo.Clase AS C

    ORDER BY C.NombreClase ASC;
END
GO
/****** Objeto: StoredProcedure [dbo].[spListarClasesDisponiblesMiembro] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spListarClasesDisponiblesMiembro]
    @IdMiembro INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CP.IdClaseProg,
        CP.IdClase,
        CP.IdEntrenador,

        C.NombreClase,
        C.Descripcion
            AS DescripcionClase,

        U.Nombre
            + N' '
            + U.Apellido
            AS NombreEntrenador,

        CP.FechaHoraInicio,
        CP.FechaHoraFin,

        DATEDIFF
        (
            MINUTE,
            CP.FechaHoraInicio,
            CP.FechaHoraFin
        ) AS DuracionMinutos,

        CP.CupoMaximo,
        CP.CupoActual,

        CP.CupoMaximo
            - CP.CupoActual
            AS CuposDisponibles,

        CP.Estado

    FROM dbo.ClaseProgramada AS CP

    INNER JOIN dbo.Clase AS C
        ON C.IdClase =
           CP.IdClase

    INNER JOIN dbo.Usuario AS U
        ON U.IdUsuario =
           CP.IdEntrenador

    WHERE CP.Estado = 1

      AND CP.FechaHoraInicio >
          GETDATE()

      AND CP.CupoActual <
          CP.CupoMaximo

      AND U.IdRol = 2

      AND U.Estado = 1

      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.ReservaClase AS RC

          WHERE RC.IdClaseProg =
                CP.IdClaseProg

            AND RC.IdMiembro =
                @IdMiembro

            AND RC.Estado = 1
      )

    ORDER BY
        CP.FechaHoraInicio ASC,
        C.NombreClase ASC;
END
GO
/****** Objeto: StoredProcedure [dbo].[spListarClasesEntrenador] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spListarClasesEntrenador]
    @IdEntrenador INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CP.IdClaseProg,
        CP.IdEntrenador,
        CP.IdClase,

        C.NombreClase,
        C.Descripcion AS DescripcionClase,

        CP.FechaHoraInicio,
        CP.FechaHoraFin,

        CP.CupoMaximo,
        CP.CupoActual,

        CP.CupoMaximo - CP.CupoActual
            AS CuposDisponibles,

        CP.Estado,

        CASE
            WHEN CP.Estado = 0
                THEN N'Inactiva'

            WHEN CP.FechaHoraFin < GETDATE()
                THEN N'Finalizada'

            WHEN GETDATE() BETWEEN
                 CP.FechaHoraInicio
                 AND CP.FechaHoraFin
                THEN N'En curso'

            ELSE N'Programada'
        END AS EstadoHorario

    FROM dbo.ClaseProgramada AS CP

    INNER JOIN dbo.Clase AS C
        ON C.IdClase = CP.IdClase

    INNER JOIN dbo.Usuario AS U
        ON U.IdUsuario = CP.IdEntrenador

    WHERE CP.IdEntrenador = @IdEntrenador
      AND U.IdRol = 2

    ORDER BY
        CASE
            WHEN CP.FechaHoraFin >= GETDATE()
                THEN 0
            ELSE 1
        END,

        CP.FechaHoraInicio ASC;
END
GO
/****** Objeto: StoredProcedure [dbo].[spListarClasesProgramadas] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spListarClasesProgramadas]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CP.IdClaseProg,
        CP.IdEntrenador,
        CP.IdClase,

        C.NombreClase,
        C.Descripcion AS DescripcionClase,

        U.Nombre
            + N' '
            + U.Apellido
            AS NombreEntrenador,

        U.CorreoElectronico
            AS CorreoEntrenador,

        CP.FechaHoraInicio,
        CP.FechaHoraFin,
        CP.CupoMaximo,
        CP.CupoActual,

        CP.CupoMaximo
            - CP.CupoActual
            AS CuposDisponibles,

        CP.Estado

    FROM dbo.ClaseProgramada AS CP

    INNER JOIN dbo.Clase AS C
        ON C.IdClase = CP.IdClase

    INNER JOIN dbo.Usuario AS U
        ON U.IdUsuario = CP.IdEntrenador

    WHERE U.IdRol = 2

    ORDER BY
        CP.FechaHoraInicio DESC,
        CP.IdClaseProg DESC;
END
GO
/****** Objeto: StoredProcedure [dbo].[spListarClientes] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* ============================================================
   PROCEDIMIENTO: LISTAR CLIENTES
   ============================================================ */

CREATE PROCEDURE [dbo].[spListarClientes]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.IdUsuario,
        U.Nombre,
        U.Apellido,
        U.CorreoElectronico,
        U.Estado
    FROM dbo.Usuario AS U
    WHERE U.IdRol = 3
    ORDER BY U.Apellido ASC,
             U.Nombre ASC;
END

GO
/****** Objeto: StoredProcedure [dbo].[spListarClientesEntrenador] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spListarClientesEntrenador]
    @IdEntrenador INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        C.IdUsuario,
        C.Identificacion,
        C.Nombre,
        C.Apellido,
        C.CorreoElectronico,
        C.Telefono,
        C.Estado,

        EC.FechaAsignacion,

        M.IdMembresia,
        M.NombrePlan,
        M.EstadoMembresia,
        M.FechaVencimiento

    FROM dbo.EntrenadorCliente AS EC

    INNER JOIN dbo.Usuario AS C
        ON C.IdUsuario = EC.IdCliente

    OUTER APPLY
    (
        SELECT TOP 1
            MU.IdMembresia,
            PM.NombrePlan,

            CASE
                WHEN MU.EstadoMembresia = N'Activa'
                     AND MU.FechaVencimiento <= GETDATE()
                THEN N'Vencida'

                ELSE MU.EstadoMembresia
            END AS EstadoMembresia,

            MU.FechaVencimiento

        FROM dbo.MembresiaUsuario AS MU

        INNER JOIN dbo.PlanMembresia AS PM
            ON PM.IdPlan = MU.IdPlan

        WHERE MU.IdUsuario = C.IdUsuario

        ORDER BY MU.IdMembresia DESC
    ) AS M

    WHERE EC.IdEntrenador = @IdEntrenador
      AND EC.Estado = 1
      AND C.IdRol = 3

    ORDER BY
        C.Nombre,
        C.Apellido;
END
GO
/****** Objeto: StoredProcedure [dbo].[spListarEjerciciosEntrenador] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spListarEjerciciosEntrenador]
    @IdEntrenador INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        E.IdEjercicio,
        E.IdEntrenador,
        E.NombreEjercicio,
        E.GrupoMuscular,
        E.Descripcion,
        E.FechaCreacion,
        E.Estado

    FROM dbo.Ejercicio AS E

    WHERE E.IdEntrenador = @IdEntrenador

    ORDER BY
        E.Estado DESC,
        E.GrupoMuscular ASC,
        E.NombreEjercicio ASC;
END
GO
/****** Objeto: StoredProcedure [dbo].[spListarEjerciciosRutinaEntrenador] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spListarEjerciciosRutinaEntrenador]
    @IdEntrenador INT,
    @IdRutina INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        RE.IdRutinaEjercicio,
        RE.IdRutina,
        RE.IdEjercicio,

        E.NombreEjercicio,
        E.GrupoMuscular,
        E.Descripcion,

        RE.DiaSemana,
        RE.Series,
        RE.Repeticiones,
        RE.PesoSugerido,
        RE.DescansoSegundos,
        RE.Observacion,
        RE.Orden,
        RE.FechaAsignacion,
        RE.Estado

    FROM dbo.RutinaEjercicio AS RE

    INNER JOIN dbo.Rutinas AS R
        ON R.IdRutina = RE.IdRutina

    INNER JOIN dbo.Ejercicio AS E
        ON E.IdEjercicio = RE.IdEjercicio

    WHERE R.IdEntrenador = @IdEntrenador
      AND R.IdRutina = @IdRutina
      AND RE.Estado = 1

    ORDER BY

        CASE RE.DiaSemana

            WHEN N'Lunes' THEN 1
            WHEN N'Martes' THEN 2
            WHEN N'Miércoles' THEN 3
            WHEN N'Jueves' THEN 4
            WHEN N'Viernes' THEN 5
            WHEN N'Sábado' THEN 6
            WHEN N'Domingo' THEN 7

            ELSE 8

        END,

        RE.Orden ASC,
        RE.IdRutinaEjercicio ASC;
END
GO
/****** Objeto: StoredProcedure [dbo].[spListarEjerciciosRutinaMiembro] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spListarEjerciciosRutinaMiembro]
    @IdMiembro INT,
    @IdRutina INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        RE.IdRutinaEjercicio,
        RE.IdRutina,
        RE.IdEjercicio,

        E.NombreEjercicio,
        E.GrupoMuscular,
        E.Descripcion,

        RE.DiaSemana,
        RE.Series,
        RE.Repeticiones,
        RE.PesoSugerido,
        RE.DescansoSegundos,
        RE.Observacion,
        RE.Orden,
        RE.FechaAsignacion,
        RE.Estado

    FROM dbo.RutinaEjercicio AS RE

    INNER JOIN dbo.Rutinas AS R
        ON R.IdRutina = RE.IdRutina

    INNER JOIN dbo.Ejercicio AS E
        ON E.IdEjercicio = RE.IdEjercicio

    WHERE R.IdRutina = @IdRutina
      AND R.IdCliente = @IdMiembro
      AND R.Estado = 1
      AND RE.Estado = 1

    ORDER BY
        CASE RE.DiaSemana

            WHEN N'Lunes' THEN 1
            WHEN N'Martes' THEN 2
            WHEN N'Miércoles' THEN 3
            WHEN N'Jueves' THEN 4
            WHEN N'Viernes' THEN 5
            WHEN N'Sábado' THEN 6
            WHEN N'Domingo' THEN 7

            ELSE 8

        END,

        RE.Orden ASC,
        RE.IdRutinaEjercicio ASC;
END
GO
/****** Objeto: StoredProcedure [dbo].[spListarEmpleados] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* ============================================================
   PROCEDIMIENTO: LISTAR EMPLEADOS
   ============================================================ */

CREATE PROCEDURE [dbo].[spListarEmpleados]
    @IdUser INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.IdUsuario,
        U.Identificacion,
        U.Nombre,
        U.Apellido,
        R.NombreRol,
        U.CorreoElectronico,
        U.Estado
    FROM dbo.Usuario AS U
    INNER JOIN dbo.Rol AS R
        ON R.IdRol = U.IdRol
    WHERE U.IdRol IN (1, 2)
      AND U.IdUsuario != @IdUser
    ORDER BY U.Apellido ASC,
             U.Nombre ASC;
END

GO
/****** Objeto: StoredProcedure [dbo].[spListarEntrenadoresActivos] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spListarEntrenadoresActivos]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.IdUsuario AS IdEntrenador,
        U.Identificacion,
        U.Nombre,
        U.Apellido,

        U.Nombre
            + N' '
            + U.Apellido
            AS NombreCompleto,

        U.CorreoElectronico
    FROM dbo.Usuario AS U
    WHERE U.IdRol = 2
      AND U.Estado = 1
    ORDER BY
        U.Nombre,
        U.Apellido;
END
GO
/****** Objeto: StoredProcedure [dbo].[spListarHistorialAsistenciaMiembro] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spListarHistorialAsistenciaMiembro]
    @IdMiembro INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        RC.IdReserva,
        RC.IdClaseProg,
        RC.IdMiembro,

        C.IdClase,
        C.NombreClase,
        C.Descripcion AS DescripcionClase,

        CP.IdEntrenador,

        UE.Nombre
            + N' '
            + UE.Apellido
            AS NombreEntrenador,

        UE.CorreoElectronico
            AS CorreoEntrenador,

        CP.FechaHoraInicio,
        CP.FechaHoraFin,

        DATEDIFF
        (
            MINUTE,
            CP.FechaHoraInicio,
            CP.FechaHoraFin
        ) AS DuracionMinutos,

        RC.FechaReserva,
        RC.FechaCancelacion,
        RC.Estado AS EstadoReserva,

        AC.IdAsistencia,

        CASE
            WHEN RC.Estado = 0
                THEN N'Reserva cancelada'

            WHEN AC.IdAsistencia IS NULL
                THEN N'Pendiente'

            ELSE AC.EstadoAsistencia
        END AS EstadoAsistencia,

        AC.Observacion,
        AC.FechaRegistro,
        AC.FechaActualizacion,

        CASE
            WHEN CP.Estado = 0
                THEN N'Clase inactiva'

            WHEN CP.FechaHoraFin < GETDATE()
                THEN N'Finalizada'

            WHEN GETDATE() BETWEEN
                 CP.FechaHoraInicio
                 AND CP.FechaHoraFin
                THEN N'En curso'

            ELSE N'Programada'
        END AS EstadoHorario

    FROM dbo.ReservaClase AS RC

    INNER JOIN dbo.ClaseProgramada AS CP
        ON CP.IdClaseProg =
           RC.IdClaseProg

    INNER JOIN dbo.Clase AS C
        ON C.IdClase =
           CP.IdClase

    INNER JOIN dbo.Usuario AS UE
        ON UE.IdUsuario =
           CP.IdEntrenador

    LEFT JOIN dbo.AsistenciaClase AS AC
        ON AC.IdReserva =
           RC.IdReserva

    WHERE RC.IdMiembro =
          @IdMiembro

    ORDER BY
        CP.FechaHoraInicio DESC,
        RC.IdReserva DESC;
END
GO
/****** Objeto: StoredProcedure [dbo].[spListarPagosCliente] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spListarPagosCliente]
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        P.IdPago,
        P.IdMembresia,
        MU.IdUsuario,

        U.Nombre
            + N' '
            + U.Apellido
            AS NombreCliente,

        PM.NombrePlan,
        P.MetodoPago,
        P.FechaEmision,
        P.NumeroReferencia,
        P.Monto,
        P.Subtotal,
        P.Impuesto,
        P.Total

    FROM dbo.Pago AS P

    INNER JOIN dbo.MembresiaUsuario AS MU
        ON MU.IdMembresia = P.IdMembresia

    INNER JOIN dbo.Usuario AS U
        ON U.IdUsuario = MU.IdUsuario

    INNER JOIN dbo.PlanMembresia AS PM
        ON PM.IdPlan = MU.IdPlan

    WHERE MU.IdUsuario = @IdUsuario
      AND U.IdRol = 3

    ORDER BY P.FechaEmision DESC,
             P.IdPago DESC;
END
GO
/****** Objeto: StoredProcedure [dbo].[spListarPagosMiembro] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spListarPagosMiembro]
    @IdMiembro INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        P.IdPago,
        P.IdMembresia,

        MU.IdUsuario,
        MU.IdPlan,

        PM.NombrePlan,

        P.MetodoPago,
        P.FechaEmision,
        P.NumeroReferencia,

        P.Monto,
        P.Subtotal,
        P.Impuesto,
        P.Total,

        MU.FechaRegistro AS FechaInicio,
        MU.FechaVencimiento,
        MU.EstadoMembresia

    FROM dbo.Pago AS P

    INNER JOIN dbo.MembresiaUsuario AS MU
        ON MU.IdMembresia = P.IdMembresia

    INNER JOIN dbo.PlanMembresia AS PM
        ON PM.IdPlan = MU.IdPlan

    INNER JOIN dbo.Usuario AS U
        ON U.IdUsuario = MU.IdUsuario

    WHERE MU.IdUsuario = @IdMiembro
      AND U.IdRol = 3

    ORDER BY
        P.FechaEmision DESC,
        P.IdPago DESC;
END
GO
/****** Objeto: StoredProcedure [dbo].[spListarParticipantesClaseEntrenador] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spListarParticipantesClaseEntrenador]
    @IdEntrenador INT,
    @IdClaseProg INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        RC.IdReserva,
        RC.IdClaseProg,
        RC.IdMiembro,

        C.NombreClase,
        C.Descripcion AS DescripcionClase,

        CP.FechaHoraInicio,
        CP.FechaHoraFin,

        U.Identificacion,

        U.Nombre
            + N' '
            + U.Apellido
            AS NombreMiembro,

        U.CorreoElectronico,
        U.Telefono,

        RC.FechaReserva,
        RC.Estado AS EstadoReserva,

        ISNULL(
            AC.EstadoAsistencia,
            N'Pendiente'
        ) AS EstadoAsistencia,

        AC.Observacion,
        AC.FechaRegistro,
        AC.FechaActualizacion

    FROM dbo.ReservaClase AS RC

    INNER JOIN dbo.ClaseProgramada AS CP
        ON CP.IdClaseProg =
           RC.IdClaseProg

    INNER JOIN dbo.Clase AS C
        ON C.IdClase =
           CP.IdClase

    INNER JOIN dbo.Usuario AS U
        ON U.IdUsuario =
           RC.IdMiembro

    LEFT JOIN dbo.AsistenciaClase AS AC
        ON AC.IdReserva =
           RC.IdReserva

    WHERE RC.IdClaseProg =
          @IdClaseProg

      AND CP.IdEntrenador =
          @IdEntrenador

      AND RC.Estado = 1

      AND U.IdRol = 3

    ORDER BY
        U.Nombre ASC,
        U.Apellido ASC;
END
GO
/****** Objeto: StoredProcedure [dbo].[spListarPlanesMembresia] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* ============================================================
   LISTAR PLANES DE MEMBRESÍA
   ============================================================ */

CREATE   PROCEDURE [dbo].[spListarPlanesMembresia]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdPlan,
        NombrePlan,
        Descripcion,
        DuracionDias,
        Precio
    FROM dbo.PlanMembresia
    ORDER BY IdPlan ASC;
END
GO
/****** Objeto: StoredProcedure [dbo].[spListarReservasMiembro] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spListarReservasMiembro]
    @IdMiembro INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        RC.IdReserva,
        RC.IdClaseProg,
        RC.IdMiembro,

        C.IdClase,
        C.NombreClase,

        C.Descripcion
            AS DescripcionClase,

        U.Nombre
            + N' '
            + U.Apellido
            AS NombreEntrenador,

        CP.FechaHoraInicio,
        CP.FechaHoraFin,

        CP.CupoMaximo,
        CP.CupoActual,

        CP.CupoMaximo
            - CP.CupoActual
            AS CuposDisponibles,

        RC.FechaReserva,
        RC.FechaCancelacion,
        RC.Estado,

        CASE
            WHEN RC.Estado = 0
                THEN N'Cancelada'

            WHEN CP.Estado = 0
                THEN N'Clase inactiva'

            WHEN CP.FechaHoraFin <
                 GETDATE()
                THEN N'Finalizada'

            WHEN GETDATE() BETWEEN
                 CP.FechaHoraInicio
                 AND CP.FechaHoraFin
                THEN N'En curso'

            ELSE N'Reservada'
        END AS EstadoReserva,

        CONVERT
        (
            BIT,

            CASE
                WHEN RC.Estado = 1
                 AND CP.FechaHoraInicio >
                     GETDATE()
                    THEN 1

                ELSE 0
            END
        ) AS PuedeCancelar

    FROM dbo.ReservaClase AS RC

    INNER JOIN dbo.ClaseProgramada AS CP
        ON CP.IdClaseProg =
           RC.IdClaseProg

    INNER JOIN dbo.Clase AS C
        ON C.IdClase =
           CP.IdClase

    INNER JOIN dbo.Usuario AS U
        ON U.IdUsuario =
           CP.IdEntrenador

    WHERE RC.IdMiembro =
          @IdMiembro

    ORDER BY
        CASE
            WHEN RC.Estado = 1
             AND CP.FechaHoraFin >=
                 GETDATE()
                THEN 0

            ELSE 1
        END,

        CP.FechaHoraInicio ASC,
        RC.IdReserva DESC;
END
GO
/****** Objeto: StoredProcedure [dbo].[spListarRoles] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* ============================================================
   PROCEDIMIENTO: LISTAR ROLES PARA EMPLEADOS
   ============================================================ */

CREATE PROCEDURE [dbo].[spListarRoles]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdRol,
        NombreRol
    FROM dbo.Rol
    WHERE IdRol IN (1, 2)
    ORDER BY IdRol ASC;
END

GO
/****** Objeto: StoredProcedure [dbo].[spListarRutinasEntrenador] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spListarRutinasEntrenador]
    @IdEntrenador INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        R.IdRutina,
        R.IdEntrenador,
        R.IdCliente,
        R.NombreRutina,
        R.Nivel,
        R.Detalles,
        R.FechaCreacion,
        R.Estado,

        C.Identificacion,

        C.Nombre
            + N' '
            + C.Apellido
            AS NombreCliente,

        C.CorreoElectronico

    FROM dbo.Rutinas AS R

    INNER JOIN dbo.Usuario AS C
        ON C.IdUsuario = R.IdCliente

    WHERE R.IdEntrenador = @IdEntrenador
      AND C.IdRol = 3

    ORDER BY
        R.Estado DESC,
        R.FechaCreacion DESC,
        R.IdRutina DESC;
END
GO
/****** Objeto: StoredProcedure [dbo].[spListarRutinasMiembro] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spListarRutinasMiembro]
    @IdMiembro INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        R.IdRutina,
        R.IdEntrenador,
        R.IdCliente,
        R.NombreRutina,
        R.Nivel,
        R.Detalles,
        R.FechaCreacion,
        R.Estado,

        EN.Nombre
            + N' '
            + EN.Apellido
            AS NombreEntrenador,

        EN.CorreoElectronico
            AS CorreoEntrenador,

        (
            SELECT COUNT(1)

            FROM dbo.RutinaEjercicio AS RE

            WHERE RE.IdRutina = R.IdRutina
              AND RE.Estado = 1
        ) AS CantidadEjercicios

    FROM dbo.Rutinas AS R

    INNER JOIN dbo.Usuario AS EN
        ON EN.IdUsuario = R.IdEntrenador

    WHERE R.IdCliente = @IdMiembro
      AND R.Estado = 1
      AND EN.IdRol = 2

    ORDER BY
        R.FechaCreacion DESC,
        R.IdRutina DESC;
END
GO


/* ============================================================
   HOMEPAGE: OBTENER EL DETALLE DE LAS CLASES PARA (METODOLOGÍAS)
   ============================================================ */


CREATE PROCEDURE [dbo].[spObtenerClasesDetalle]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        NombreClase,
        Descripcion,
        Icono,
        Color,
        Beneficio1,
        Beneficio2,
        Beneficio3,
        Beneficio4
    FROM dbo.Clase 
END
GO

/* ============================================================
   HOMEPAGE: OBTENER EL DETALLE DE LOS PLANES DE MEMBRESIA
   ============================================================ */

CREATE PROCEDURE [dbo].[spObtenerPlanMembresiaDetalle]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        NombrePlan AS 'Tipo',
        Precio,
        Descripcion,
        DuracionDias,
        EsPopular,
        Color,
        Beneficio1,
        Beneficio2,
        Beneficio3,
        Beneficio4
    FROM dbo.PlanMembresia
END
GO


/* ============================================================
   HOMEPAGE: OBTENER lISTA DE CLASES PARA LOS PRÓX. 7 DÍAS
   ============================================================ */

CREATE PROCEDURE [dbo].[spProximasClases]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        C.NombreClase,
        
        U.Nombre
            + N' '
            + U.Apellido
            AS NombreEntrenador,
        
        CP.FechaHoraInicio,
        CP.FechaHoraFin,

        CP.CupoMaximo
            - CP.CupoActual
            AS CuposDisponibles,

        C.Color AS 'ColorTag'

    FROM dbo.ClaseProgramada AS CP

    INNER JOIN dbo.Clase AS C
        ON C.IdClase = CP.IdClase

    INNER JOIN dbo.Usuario AS U
        ON U.IdUsuario = CP.IdEntrenador
    WHERE
        CP.Estado = 1
        AND CP.FechaHoraInicio > GETDATE()
        AND CP.FechaHoraInicio < DATEADD(DAY, 7, GETDATE())
    ORDER BY
        CP.FechaHoraInicio DESC,
        CP.IdClaseProg DESC
END
GO



/****** Objeto: StoredProcedure [dbo].[spQuitarEjercicioRutinaEntrenador] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spQuitarEjercicioRutinaEntrenador]
    @IdEntrenador INT,
    @IdRutinaEjercicio INT
AS
BEGIN
    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1

            FROM dbo.RutinaEjercicio AS RE

            INNER JOIN dbo.Rutinas AS R
                ON R.IdRutina = RE.IdRutina

            WHERE RE.IdRutinaEjercicio =
                    @IdRutinaEjercicio

              AND R.IdEntrenador =
                    @IdEntrenador

              AND RE.Estado = 1
        )
        BEGIN
            RAISERROR(
                'El ejercicio asignado no existe o no pertenece a este entrenador.',
                16,
                1
            );

            RETURN;
        END


        UPDATE RE

        SET RE.Estado = 0

        FROM dbo.RutinaEjercicio AS RE

        INNER JOIN dbo.Rutinas AS R
            ON R.IdRutina = RE.IdRutina

        WHERE RE.IdRutinaEjercicio =
                @IdRutinaEjercicio

          AND R.IdEntrenador =
                @IdEntrenador;

    END TRY
    BEGIN CATCH

        DECLARE @ErrorMessage NVARCHAR(4000);

        SET @ErrorMessage = ERROR_MESSAGE();

        RAISERROR(
            @ErrorMessage,
            16,
            1
        );

    END CATCH
END
GO
/****** Objeto: StoredProcedure [dbo].[spRegistrarAsistenciaClaseEntrenador] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spRegistrarAsistenciaClaseEntrenador]
    @IdEntrenador INT,
    @IdReserva INT,
    @EstadoAsistencia NVARCHAR(20),
    @Observacion NVARCHAR(300) = NULL
AS
BEGIN
    SET XACT_ABORT ON;

    BEGIN TRY

        BEGIN TRANSACTION;


        IF @EstadoAsistencia NOT IN
        (
            N'Presente',
            N'Ausente'
        )
        BEGIN
            RAISERROR(
                'El estado de asistencia debe ser Presente o Ausente.',
                16,
                1
            );
        END


        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Usuario
            WHERE IdUsuario =
                  @IdEntrenador
              AND IdRol = 2
              AND Estado = 1
        )
        BEGIN
            RAISERROR(
                'El entrenador no existe o se encuentra inactivo.',
                16,
                1
            );
        END


        DECLARE
            @IdClaseProg INT,
            @FechaHoraInicio DATETIME2(7),
            @EstadoReserva BIT;


        SELECT
            @IdClaseProg =
                RC.IdClaseProg,

            @FechaHoraInicio =
                CP.FechaHoraInicio,

            @EstadoReserva =
                RC.Estado

        FROM dbo.ReservaClase AS RC

        INNER JOIN dbo.ClaseProgramada AS CP
            ON CP.IdClaseProg =
               RC.IdClaseProg

        WHERE RC.IdReserva =
              @IdReserva

          AND CP.IdEntrenador =
              @IdEntrenador;


        IF @IdClaseProg IS NULL
        BEGIN
            RAISERROR(
                'La reserva no existe o la clase no pertenece a este entrenador.',
                16,
                1
            );
        END


        IF @EstadoReserva = 0
        BEGIN
            RAISERROR(
                'No se puede registrar asistencia para una reserva cancelada.',
                16,
                1
            );
        END


        IF GETDATE() < @FechaHoraInicio
        BEGIN
            RAISERROR(
                'No se puede registrar asistencia antes de iniciar la clase.',
                16,
                1
            );
        END


        IF EXISTS
        (
            SELECT 1
            FROM dbo.AsistenciaClase
            WHERE IdReserva =
                  @IdReserva
        )
        BEGIN

            UPDATE dbo.AsistenciaClase
            SET
                EstadoAsistencia =
                    @EstadoAsistencia,

                Observacion =
                    NULLIF(
                        LTRIM(RTRIM(@Observacion)),
                        N''
                    ),

                IdEntrenadorRegistro =
                    @IdEntrenador,

                FechaActualizacion =
                    SYSDATETIME()

            WHERE IdReserva =
                  @IdReserva;

        END
        ELSE
        BEGIN

            INSERT INTO dbo.AsistenciaClase
            (
                IdReserva,
                IdEntrenadorRegistro,
                EstadoAsistencia,
                Observacion,
                FechaRegistro,
                FechaActualizacion
            )
            VALUES
            (
                @IdReserva,
                @IdEntrenador,
                @EstadoAsistencia,

                NULLIF(
                    LTRIM(RTRIM(@Observacion)),
                    N''
                ),

                SYSDATETIME(),
                NULL
            );

        END


        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH

        DECLARE @ErrorMessage NVARCHAR(4000);

        SET @ErrorMessage =
            ERROR_MESSAGE();


        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END


        RAISERROR(
            @ErrorMessage,
            16,
            1
        );

        RETURN;

    END CATCH
END
GO
/****** Objeto: StoredProcedure [dbo].[spRegistrarClaseProgramada] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spRegistrarClaseProgramada]
    @IdEntrenador INT,
    @IdClase INT,
    @FechaHoraInicio DATETIME2(7),
    @FechaHoraFin DATETIME2(7),
    @CupoMaximo INT
AS
BEGIN
    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Usuario
            WHERE IdUsuario = @IdEntrenador
              AND IdRol = 2
              AND Estado = 1
        )
        BEGIN
            RAISERROR(
                'El entrenador no existe o se encuentra inactivo.',
                16,
                1
            );

            RETURN;
        END


        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Clase
            WHERE IdClase = @IdClase
        )
        BEGIN
            RAISERROR(
                'La clase seleccionada no existe.',
                16,
                1
            );

            RETURN;
        END


        IF @FechaHoraInicio IS NULL
           OR @FechaHoraFin IS NULL
        BEGIN
            RAISERROR(
                'La fecha y el horario de la clase son obligatorios.',
                16,
                1
            );

            RETURN;
        END


        IF @FechaHoraInicio <= GETDATE()
        BEGIN
            RAISERROR(
                'La fecha de inicio debe ser posterior a la fecha actual.',
                16,
                1
            );

            RETURN;
        END


        IF @FechaHoraFin <= @FechaHoraInicio
        BEGIN
            RAISERROR(
                'La hora de finalización debe ser posterior a la hora de inicio.',
                16,
                1
            );

            RETURN;
        END


        IF @CupoMaximo <= 0
        BEGIN
            RAISERROR(
                'El cupo máximo debe ser mayor que cero.',
                16,
                1
            );

            RETURN;
        END


        IF EXISTS
        (
            SELECT 1
            FROM dbo.ClaseProgramada
            WHERE IdEntrenador = @IdEntrenador
              AND Estado = 1
              AND @FechaHoraInicio < FechaHoraFin
              AND @FechaHoraFin > FechaHoraInicio
        )
        BEGIN
            RAISERROR(
                'El entrenador ya tiene otra clase programada dentro de ese horario.',
                16,
                1
            );

            RETURN;
        END


        INSERT INTO dbo.ClaseProgramada
        (
            IdEntrenador,
            IdClase,
            FechaHoraInicio,
            FechaHoraFin,
            CupoMaximo,
            CupoActual,
            Estado
        )
        VALUES
        (
            @IdEntrenador,
            @IdClase,
            @FechaHoraInicio,
            @FechaHoraFin,
            @CupoMaximo,
            0,
            1
        );

    END TRY
    BEGIN CATCH

        DECLARE @ErrorMessage NVARCHAR(4000);

        SET @ErrorMessage =
            ERROR_MESSAGE();

        RAISERROR(
            @ErrorMessage,
            16,
            1
        );

    END CATCH
END
GO
/****** Objeto: StoredProcedure [dbo].[spRegistrarEjercicioEntrenador] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spRegistrarEjercicioEntrenador]
    @IdEntrenador INT,
    @NombreEjercicio NVARCHAR(120),
    @GrupoMuscular NVARCHAR(80),
    @Descripcion NVARCHAR(300)
AS
BEGIN
    BEGIN TRY

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Usuario
            WHERE IdUsuario = @IdEntrenador
              AND IdRol = 2
              AND Estado = 1
        )
        BEGIN
            RAISERROR(
                'El entrenador no existe o se encuentra inactivo.',
                16,
                1
            );

            RETURN;
        END


        IF @NombreEjercicio IS NULL
           OR LTRIM(RTRIM(@NombreEjercicio)) = N''
        BEGIN
            RAISERROR(
                'El nombre del ejercicio es obligatorio.',
                16,
                1
            );

            RETURN;
        END


        IF @GrupoMuscular IS NULL
           OR LTRIM(RTRIM(@GrupoMuscular)) = N''
        BEGIN
            RAISERROR(
                'El grupo muscular es obligatorio.',
                16,
                1
            );

            RETURN;
        END


        IF EXISTS
        (
            SELECT 1
            FROM dbo.Ejercicio
            WHERE IdEntrenador = @IdEntrenador
              AND NombreEjercicio =
                    LTRIM(RTRIM(@NombreEjercicio))
              AND Estado = 1
        )
        BEGIN
            RAISERROR(
                'Ya existe un ejercicio activo con ese nombre.',
                16,
                1
            );

            RETURN;
        END


        INSERT INTO dbo.Ejercicio
        (
            IdEntrenador,
            NombreEjercicio,
            GrupoMuscular,
            Descripcion,
            FechaCreacion,
            Estado
        )
        VALUES
        (
            @IdEntrenador,
            LTRIM(RTRIM(@NombreEjercicio)),
            LTRIM(RTRIM(@GrupoMuscular)),
            NULLIF(
                LTRIM(RTRIM(@Descripcion)),
                N''
            ),
            GETDATE(),
            1
        );

    END TRY
    BEGIN CATCH

        DECLARE @ErrorMessage NVARCHAR(4000);

        SET @ErrorMessage = ERROR_MESSAGE();

        RAISERROR(
            @ErrorMessage,
            16,
            1
        );

    END CATCH
END
GO
/****** Objeto: StoredProcedure [dbo].[spRegistrarEmpleado] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* ============================================================
   PROCEDIMIENTO: REGISTRAR EMPLEADO
   ============================================================ */

CREATE PROCEDURE [dbo].[spRegistrarEmpleado]
    @Nombre NVARCHAR(80),
    @Apellido NVARCHAR(80),
    @Identificacion NVARCHAR(50),
    @FechaNacimiento DATETIME2(7),
    @Telefono NVARCHAR(30),
    @CorreoElectronico NVARCHAR(150),
    @Rol INT,
    @Direccion NVARCHAR(200),
    @ContrasennaHash NVARCHAR(300),
    @FechaRegistro DATETIME2(7)
AS
BEGIN
    BEGIN TRY

        IF @Rol NOT IN (1, 2)
        BEGIN
            RAISERROR(
                'El rol seleccionado no es válido para un empleado.',
                16,
                1
            );

            RETURN;
        END

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Rol
            WHERE IdRol = @Rol
              AND IdRol IN (1, 2)
        )
        BEGIN
            RAISERROR(
                'El rol seleccionado no existe.',
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
            Identificacion,
            Telefono,
            FechaNacimiento,
            Direccion,
            ContrasennaHash,
            DebeCambiarContrasenna,
            Estado,
            FechaRegistro
        )
        VALUES
        (
            @Rol,
            @Nombre,
            @Apellido,
            @CorreoElectronico,
            @Identificacion,
            @Telefono,
            @FechaNacimiento,
            @Direccion,
            @ContrasennaHash,
            1,
            1,
            @FechaRegistro
        );

        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END

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
/****** Objeto: StoredProcedure [dbo].[spRegistrarPagoMembresia] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spRegistrarPagoMembresia]
    @IdMembresia INT,
    @MetodoPago NVARCHAR(50),
    @NumeroReferencia NVARCHAR(100)
AS
BEGIN
    BEGIN TRY

        /* Validar la membresía */

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.MembresiaUsuario AS MU
            INNER JOIN dbo.Usuario AS U
                ON U.IdUsuario = MU.IdUsuario
            WHERE MU.IdMembresia = @IdMembresia
              AND U.IdRol = 3
        )
        BEGIN
            RAISERROR(
                'La membresía indicada no existe.',
                16,
                1
            );

            RETURN;
        END


        /* Solo se pagan membresías activas */

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.MembresiaUsuario
            WHERE IdMembresia = @IdMembresia
              AND EstadoMembresia = N'Activa'
        )
        BEGIN
            RAISERROR(
                'Solo se puede registrar el pago de una membresía activa.',
                16,
                1
            );

            RETURN;
        END


        /* Evitar registrar dos pagos para la misma membresía */

        IF EXISTS
        (
            SELECT 1
            FROM dbo.Pago
            WHERE IdMembresia = @IdMembresia
        )
        BEGIN
            RAISERROR(
                'Esta membresía ya tiene un pago registrado.',
                16,
                1
            );

            RETURN;
        END


        /* Validar método de pago */

        IF @MetodoPago NOT IN
        (
            N'Efectivo',
            N'Tarjeta',
            N'SINPE Móvil',
            N'Transferencia'
        )
        BEGIN
            RAISERROR(
                'El método de pago seleccionado no es válido.',
                16,
                1
            );

            RETURN;
        END


        /* Generar referencia para pagos en efectivo */

        IF @MetodoPago = N'Efectivo'
           AND
           (
               @NumeroReferencia IS NULL
               OR LTRIM(RTRIM(@NumeroReferencia)) = N''
           )
        BEGIN
            SET @NumeroReferencia =
                N'EFECTIVO-'
                + CAST(@IdMembresia AS NVARCHAR(20))
                + N'-'
                + REPLACE(
                    REPLACE(
                        REPLACE(
                            CONVERT(
                                NVARCHAR(19),
                                GETDATE(),
                                120
                            ),
                            N'-',
                            N''
                        ),
                        N':',
                        N''
                    ),
                    N' ',
                    N''
                );
        END


        /* La referencia es obligatoria para otros métodos */

        IF @MetodoPago != N'Efectivo'
           AND
           (
               @NumeroReferencia IS NULL
               OR LTRIM(RTRIM(@NumeroReferencia)) = N''
           )
        BEGIN
            RAISERROR(
                'El número de referencia es obligatorio.',
                16,
                1
            );

            RETURN;
        END


        DECLARE @Subtotal DECIMAL(10,2);
        DECLARE @Impuesto DECIMAL(10,2);
        DECLARE @Total DECIMAL(10,2);


        /* Obtener el precio directamente del plan */

        SELECT
            @Subtotal = PM.Precio
        FROM dbo.MembresiaUsuario AS MU

        INNER JOIN dbo.PlanMembresia AS PM
            ON PM.IdPlan = MU.IdPlan

        WHERE MU.IdMembresia = @IdMembresia;


        IF @Subtotal IS NULL
        BEGIN
            RAISERROR(
                'No se pudo determinar el precio de la membresía.',
                16,
                1
            );

            RETURN;
        END


        SET @Impuesto =
            ROUND(
                @Subtotal * 0.13,
                2
            );

        SET @Total =
            @Subtotal + @Impuesto;


        BEGIN TRANSACTION;


        INSERT INTO dbo.Pago
        (
            IdMembresia,
            MetodoPago,
            FechaEmision,
            NumeroReferencia,
            Monto,
            Subtotal,
            Impuesto,
            Total
        )
        VALUES
        (
            @IdMembresia,
            @MetodoPago,
            GETDATE(),
            LTRIM(RTRIM(@NumeroReferencia)),
            @Total,
            @Subtotal,
            @Impuesto,
            @Total
        );


        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END

        DECLARE @ErrorMessage NVARCHAR(4000);

        SET @ErrorMessage =
            ERROR_MESSAGE();

        RAISERROR(
            @ErrorMessage,
            16,
            1
        );

    END CATCH
END
GO
/****** Objeto: StoredProcedure [dbo].[spRegistrarRutinaEntrenador] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spRegistrarRutinaEntrenador]
    @IdEntrenador INT,
    @IdCliente INT,
    @NombreRutina NVARCHAR(100),
    @Nivel NVARCHAR(20),
    @Detalles NVARCHAR(MAX)
AS
BEGIN
    BEGIN TRY

        /* Validar entrenador */

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Usuario
            WHERE IdUsuario = @IdEntrenador
              AND IdRol = 2
              AND Estado = 1
        )
        BEGIN

            RAISERROR(
                'El entrenador no existe o se encuentra inactivo.',
                16,
                1
            );

            RETURN;

        END


        /* Validar cliente */

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Usuario
            WHERE IdUsuario = @IdCliente
              AND IdRol = 3
              AND Estado = 1
        )
        BEGIN

            RAISERROR(
                'El cliente no existe o se encuentra inactivo.',
                16,
                1
            );

            RETURN;

        END


        /* Validar asignación */

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.EntrenadorCliente
            WHERE IdEntrenador = @IdEntrenador
              AND IdCliente = @IdCliente
              AND Estado = 1
        )
        BEGIN

            RAISERROR(
                'El cliente no está asignado a este entrenador.',
                16,
                1
            );

            RETURN;

        END


        /* Validar nombre */

        IF LTRIM(RTRIM(@NombreRutina)) = N''
        BEGIN

            RAISERROR(
                'El nombre de la rutina es obligatorio.',
                16,
                1
            );

            RETURN;

        END


        /* Validar nivel */

        IF @Nivel NOT IN
        (
            N'Principiante',
            N'Intermedio',
            N'Avanzado'
        )
        BEGIN

            RAISERROR(
                'El nivel seleccionado no es válido.',
                16,
                1
            );

            RETURN;

        END


        /* Validar detalles */

        IF @Detalles IS NULL
           OR LTRIM(RTRIM(@Detalles)) = N''
        BEGIN

            RAISERROR(
                'Los detalles de la rutina son obligatorios.',
                16,
                1
            );

            RETURN;

        END


        /* Evitar nombre duplicado para el mismo cliente */

        IF EXISTS
        (
            SELECT 1
            FROM dbo.Rutinas
            WHERE IdEntrenador = @IdEntrenador
              AND IdCliente = @IdCliente
              AND NombreRutina =
                  LTRIM(RTRIM(@NombreRutina))
              AND Estado = 1
        )
        BEGIN

            RAISERROR(
                'El cliente ya tiene una rutina activa con ese nombre.',
                16,
                1
            );

            RETURN;

        END


        INSERT INTO dbo.Rutinas
        (
            IdEntrenador,
            IdCliente,
            NombreRutina,
            Nivel,
            Detalles,
            FechaCreacion,
            Estado
        )
        VALUES
        (
            @IdEntrenador,
            @IdCliente,
            LTRIM(RTRIM(@NombreRutina)),
            @Nivel,
            LTRIM(RTRIM(@Detalles)),
            GETDATE(),
            1
        );

    END TRY
    BEGIN CATCH

        DECLARE @ErrorMessage NVARCHAR(4000);

        SET @ErrorMessage =
            ERROR_MESSAGE();

        RAISERROR(
            @ErrorMessage,
            16,
            1
        );

    END CATCH
END
GO
/****** Objeto: StoredProcedure [dbo].[spRegistrarUsuario] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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

        DECLARE @IdUsuario INT = SCOPE_IDENTITY();

        COMMIT TRANSACTION;

        SELECT
            U.IdUsuario,
            U.Nombre,
            U.Apellido,
            U.CorreoElectronico,
            U.IdRol,
            R.NombreRol
        FROM dbo.Usuario AS U
        INNER JOIN dbo.Rol AS R
            ON U.IdRol = R.IdRol
        WHERE U.IdUsuario = @IdUsuario;

    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END

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
/****** Objeto: StoredProcedure [dbo].[spReservarClaseMiembro] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[spReservarClaseMiembro]
    @IdMiembro INT,
    @IdClaseProg INT
AS
BEGIN
    SET XACT_ABORT ON;

    BEGIN TRY

        BEGIN TRANSACTION;


        /*
         * Validar que el miembro exista,
         * esté activo y tenga rol de miembro.
         */

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Usuario
            WHERE IdUsuario = @IdMiembro
              AND IdRol = 3
              AND Estado = 1
        )
        BEGIN
            RAISERROR(
                'El miembro no existe o se encuentra inactivo.',
                16,
                1
            );
        END


        /*
         * Validar que el miembro tenga
         * una membresía activa y vigente.
         */

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.MembresiaUsuario
            WHERE IdUsuario = @IdMiembro
              AND EstadoMembresia = N'Activa'
              AND FechaVencimiento >=
                  CAST(GETDATE() AS DATE)
        )
        BEGIN
            RAISERROR(
                'Debe tener una membresía activa para reservar una clase.',
                16,
                1
            );
        END


        DECLARE
            @FechaHoraInicio DATETIME2(7),
            @FechaHoraFin DATETIME2(7),
            @CupoMaximo INT,
            @CupoActual INT,
            @EstadoClase BIT;


        /*
         * Consultar y bloquear temporalmente
         * la clase programada.
         */

        SELECT
            @FechaHoraInicio =
                FechaHoraInicio,

            @FechaHoraFin =
                FechaHoraFin,

            @CupoMaximo =
                CupoMaximo,

            @CupoActual =
                ISNULL(CupoActual, 0),

            @EstadoClase =
                Estado

        FROM dbo.ClaseProgramada
            WITH (UPDLOCK, HOLDLOCK)

        WHERE IdClaseProg =
              @IdClaseProg;


        /*
         * Validar que la clase exista.
         */

        IF @FechaHoraInicio IS NULL
        BEGIN
            RAISERROR(
                'La clase programada no existe.',
                16,
                1
            );
        END


        /*
         * Validar que la clase esté activa.
         */

        IF @EstadoClase = 0
        BEGIN
            RAISERROR(
                'La clase seleccionada se encuentra inactiva.',
                16,
                1
            );
        END


        /*
         * Validar que la clase todavía
         * no haya comenzado.
         */

        IF @FechaHoraInicio <= GETDATE()
        BEGIN
            RAISERROR(
                'No se puede reservar una clase que ya comenzó o finalizó.',
                16,
                1
            );
        END


        /*
         * Validar disponibilidad de cupos.
         */

        IF @CupoActual >= @CupoMaximo
        BEGIN
            RAISERROR(
                'La clase seleccionada ya no tiene cupos disponibles.',
                16,
                1
            );
        END


        /*
         * Evitar reservas duplicadas.
         */

        IF EXISTS
        (
            SELECT 1
            FROM dbo.ReservaClase
                WITH (UPDLOCK, HOLDLOCK)

            WHERE IdClaseProg = @IdClaseProg
              AND IdMiembro = @IdMiembro
              AND Estado = 1
        )
        BEGIN
            RAISERROR(
                'Ya tiene una reserva activa para esta clase.',
                16,
                1
            );
        END


        /*
         * Evitar que el miembro reserve dos
         * clases con horarios cruzados.
         */

        IF EXISTS
        (
            SELECT 1
            FROM dbo.ReservaClase AS RC

            INNER JOIN dbo.ClaseProgramada AS CP
                ON CP.IdClaseProg =
                   RC.IdClaseProg

            WHERE RC.IdMiembro =
                  @IdMiembro

              AND RC.Estado = 1

              AND CP.Estado = 1

              AND @FechaHoraInicio <
                  CP.FechaHoraFin

              AND @FechaHoraFin >
                  CP.FechaHoraInicio
        )
        BEGIN
            RAISERROR(
                'Ya tiene otra clase reservada dentro de ese horario.',
                16,
                1
            );
        END


        /*
         * Registrar la reserva.
         */

        INSERT INTO dbo.ReservaClase
        (
            IdClaseProg,
            IdMiembro,
            FechaReserva,
            FechaCancelacion,
            Estado
        )
        VALUES
        (
            @IdClaseProg,
            @IdMiembro,
            SYSDATETIME(),
            NULL,
            1
        );


        /*
         * Aumentar el cupo actual de la clase.
         */

        UPDATE dbo.ClaseProgramada
        SET CupoActual =
            ISNULL(CupoActual, 0) + 1
        WHERE IdClaseProg =
              @IdClaseProg;


        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH

        DECLARE @ErrorMessage NVARCHAR(4000);

        SET @ErrorMessage =
            ERROR_MESSAGE();


        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END


        RAISERROR(
            @ErrorMessage,
            16,
            1
        );

        RETURN;

    END CATCH
END
GO
/****** Objeto: StoredProcedure [dbo].[spValidarCorreo] Fecha de script: 03/08/2026 19:28:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
    FROM dbo.Usuario AS U
    WHERE U.CorreoElectronico = @CorreoElectronico
      AND U.Estado = 1;
END

GO
