USE [SuwoGym]
GO

SET NOCOUNT ON;
GO

/* =========================================================
   DATOS INICIALES: ROLES
   ========================================================= */

IF NOT EXISTS (SELECT 1 FROM dbo.Rol WHERE IdRol = 1)
BEGIN
    INSERT INTO dbo.Rol
    (
        IdRol,
        NombreRol,
        Descripcion
    )
    VALUES
    (
        1,
        N'Administrador',
        N'Rol de administrador del sistema'
    );
END
GO
IF NOT EXISTS (SELECT 1 FROM dbo.Rol WHERE IdRol = 2)
BEGIN
    INSERT INTO dbo.Rol
    (
        IdRol,
        NombreRol,
        Descripcion
    )
    VALUES
    (
        2,
        N'Entrenador',
        N'Rol de entrenador personal'
    );
END
GO
IF NOT EXISTS (SELECT 1 FROM dbo.Rol WHERE IdRol = 3)
BEGIN
    INSERT INTO dbo.Rol
    (
        IdRol,
        NombreRol,
        Descripcion
    )
    VALUES
    (
        3,
        N'Miembro',
        N'Rol de miembro del gimnasio'
    );
END
GO

/* =========================================================
   DATOS INICIALES: CLASES
   ========================================================= */

IF NOT EXISTS (SELECT 1 FROM dbo.Clase WHERE IdClase = 1)
BEGIN
    INSERT INTO dbo.Clase
    (
        IdClase,
        NombreClase,
        Descripcion,
        Icono,
        Color,
        Beneficio1,
        Beneficio2,
        Beneficio3,
        Beneficio4

    )
    VALUES
    (
        1,
        N'Funcionales',
        N'Entrenamiento dinámico que mejora fuerza, resistencia, movilidad y coordinación mediante movimientos aplicables a la vida diaria',
        N'fa-solid fa-dumbbell', 
        N'#ff5500', 
        N'Mejora la movilidad articular', 
        N'Aumenta la fuerza funcional',
        N'Previene lesiones', 
        N'Entrenamiento adaptado a todos los niveles'
    );
END
GO
IF NOT EXISTS (SELECT 1 FROM dbo.Clase WHERE IdClase = 2)
BEGIN
    INSERT INTO dbo.Clase
    (
        IdClase,
        NombreClase,
        Descripcion,
        Icono,
        Color,
        Beneficio1,
        Beneficio2,
        Beneficio3,
        Beneficio4
    )
    VALUES
    (
        2,
        N'CrossFit',
        N'Programa de alta intensidad que combina fuerza, resistencia y acondicionamiento físico con ejercicios variados y desafiantes',
        N'fa-solid fa-bolt', 
        N'#007bff', 
        N'Quema grasa rápidamente', 
        N'Desarrolla resistencia extrema', 
        N'Fortalece todo el cuerpo', 
        N'Comunidad motivadora'
    );
END
GO
IF NOT EXISTS (SELECT 1 FROM dbo.Clase WHERE IdClase = 3)
BEGIN
    INSERT INTO dbo.Clase
    (
        IdClase,
        NombreClase,
        Descripcion,
        Icono,
        Color,
        Beneficio1,
        Beneficio2,
        Beneficio3,
        Beneficio4
    )
    VALUES
    (
        3,
        N'Baile',
        N'Actividad física divertida que mejora la coordinación, resistencia y expresión corporal mediante movimientos al ritmo de la música',
        N'fa-solid fa-music', 
        N'#ff6b6b', 
        N'Cardiovascular divertido', 
        N'Mejora la coordinación', 
        N'Reduce el estrés', 
        N'Ritmos variados: Salsa, Zumba, Urbano'
    );
END
GO

/* =========================================================
   DATOS INICIALES: PLANES DE MEMBRESÍA
   ========================================================= */

IF NOT EXISTS (SELECT 1 FROM dbo.PlanMembresia WHERE IdPlan = 1)
BEGIN
    INSERT INTO dbo.PlanMembresia
    (
        IdPlan,
        NombrePlan,
        Descripcion,
        DuracionDias,
        Precio,
        EsPopular,
        Color,
        Beneficio1,
        Beneficio2,
        Beneficio3,
        Beneficio4
    )
    VALUES
    (
        1,
        N'Mensual',
        N'Perfecto para comenzar tu transformación',
        30,
        10600.00,
        0, 
        N'#293039', 
        N'Acceso ilimitado al gimnasio', 
        N'Evaluación física inicial', 
        N'Plan nutricional básico', 
        N'Plan de ejercicios básico'
    );
END
GO
IF NOT EXISTS (SELECT 1 FROM dbo.PlanMembresia WHERE IdPlan = 2)
BEGIN
    INSERT INTO dbo.PlanMembresia
    (
        IdPlan,
        NombrePlan,
        Descripcion,
        DuracionDias,
        Precio,
        EsPopular,
        Color,
        Beneficio1,
        Beneficio2,
        Beneficio3,
        Beneficio4
    )
    VALUES
    (
        2,
        N'Trimestral',
        N'¡La favorita de nuestros miembros!',
        90,
        30000.00,
        1, 
        N'#ff5500', 
        N'Acceso ilimitado al gimnasio', 
        N'Evaluación física trimestral', 
        N'Plan nutricional premium', 
        N'Camiseta SUWO GYM de regalo'
    );
END
GO
IF NOT EXISTS (SELECT 1 FROM dbo.PlanMembresia WHERE IdPlan = 3)
BEGIN
    INSERT INTO dbo.PlanMembresia
    (
        IdPlan,
        NombrePlan,
        Descripcion,
        DuracionDias,
        Precio,
        EsPopular,
        Color,
        Beneficio1,
        Beneficio2,
        Beneficio3,
        Beneficio4
    )
    VALUES
    (
        3,
        N'Anual',
        N'El compromiso total con tu salud',
        365,
        117000.00,
        0, 
        N'#293039', 
        N'Acceso ilimitado al gimnasio', 
        N'Evaluación física mensual', 
        N'Plan nutricional premium', 
        N'Kit de entrenamiento SUWO GYM gratis'
    );
END
GO

PRINT N'Datos iniciales insertados correctamente.';
GO

/* =========================================================
   DATOS INICIALES: USUARIOS BÁSICOS
   ========================================================= */

SET IDENTITY_INSERT [dbo].[Usuario] ON 
GO
INSERT [dbo].[Usuario] ([IdUsuario], [IdRol], [Nombre], [Apellido], [CorreoElectronico], [Identificacion], [Telefono], [FechaNacimiento], [Direccion], [ContrasennaHash], [DebeCambiarContrasenna], [Estado], [FechaRegistro], [FechaUltimoAcceso]) VALUES (1, 1, N'Admin', N'Sistema', N'admin@mail.com', N'001', NULL, NULL, NULL, N'$2a$11$uTiuit1mfQYVLYo/fqCfu.eSYMw3n20FjTwibDDfFObTdCG0Sw8Qe', 0, 1, CAST(N'2026-07-24T06:48:40.4833333' AS DateTime2), NULL)
GO
INSERT [dbo].[Usuario] ([IdUsuario], [IdRol], [Nombre], [Apellido], [CorreoElectronico], [Identificacion], [Telefono], [FechaNacimiento], [Direccion], [ContrasennaHash], [DebeCambiarContrasenna], [Estado], [FechaRegistro], [FechaUltimoAcceso]) VALUES (2, 2, N'Trainer', N'Sistema', N'entrenador@mail.com', N'002', NULL, NULL, NULL, N'$2a$11$NJx8LnBKcDb.Ind5jij0DO2.VVkBUEag3LR5LHwnDnlVw/kPuySZe', 0, 1, CAST(N'2026-07-24T06:51:31.4600000' AS DateTime2), NULL)
GO
INSERT [dbo].[Usuario] ([IdUsuario], [IdRol], [Nombre], [Apellido], [CorreoElectronico], [Identificacion], [Telefono], [FechaNacimiento], [Direccion], [ContrasennaHash], [DebeCambiarContrasenna], [Estado], [FechaRegistro], [FechaUltimoAcceso]) VALUES (3, 3, N'Cliente', N'Sistema', N'cliente@mail.com', N'003', NULL, NULL, NULL, N'$2a$11$p/aBlaYzvA/WRt3TBH9Ue.ijUwgQ4rHYxbsWEe6qPa1ucJloX8KQ.', 0, 1, CAST(N'2026-07-24T06:51:57.8466667' AS DateTime2), NULL)
GO
SET IDENTITY_INSERT [dbo].[Usuario] OFF
GO

/* =========================================================
   DATOS INICIALES: MEMBRESÍA DE USUARIO
   ========================================================= */

SET IDENTITY_INSERT [dbo].[MembresiaUsuario] ON 
GO
INSERT [dbo].[MembresiaUsuario] ([IdMembresia], [IdUsuario], [IdPlan], [FechaRegistro], [EstadoMembresia], [FechaVencimiento]) VALUES (1, 3, 2, CAST(N'2026-08-04T20:54:47.5100000' AS DateTime2), N'Activa', CAST(N'2026-11-02T20:54:47.5100000' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[MembresiaUsuario] OFF
GO