/*
    Catálogos locales de SUWÓ GYM. INSERTS SEGUROS E IDEMPOTENTES.
    NO inserta usuarios de prueba ni contraseñas compartidas.
    Ejecutar SOLO sobre una base de desarrollo ya creada con
    Database/01_SuwoGym_Estructura.sql después de revisar los datos.
*/
USE [SuwoGym];
GO
SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;

    IF NOT EXISTS (SELECT 1 FROM dbo.Rol WHERE IdRol = 1)
        INSERT dbo.Rol (IdRol, NombreRol, Descripcion)
        VALUES (1, N'Administrador', N'Rol de administrador del sistema');
    IF NOT EXISTS (SELECT 1 FROM dbo.Rol WHERE IdRol = 2)
        INSERT dbo.Rol (IdRol, NombreRol, Descripcion)
        VALUES (2, N'Entrenador', N'Rol de entrenador personal');
    IF NOT EXISTS (SELECT 1 FROM dbo.Rol WHERE IdRol = 3)
        INSERT dbo.Rol (IdRol, NombreRol, Descripcion)
        VALUES (3, N'Miembro', N'Rol de miembro del gimnasio');

    IF NOT EXISTS (SELECT 1 FROM dbo.PlanMembresia WHERE IdPlan = 1)
        INSERT dbo.PlanMembresia
            (IdPlan, NombrePlan, Descripcion, DuracionDias, Precio, EsPopular,
             Color, Beneficio1, Beneficio2, Beneficio3, Beneficio4)
        VALUES
            (1, N'Mensual', N'Perfecto para comenzar tu transformación',
             30, 10600.00, 0, N'#293039',
             N'Acceso ilimitado al gimnasio', N'Evaluación física inicial',
             N'Plan nutricional básico', N'Plan de ejercicios básico');

    IF NOT EXISTS (SELECT 1 FROM dbo.PlanMembresia WHERE IdPlan = 2)
        INSERT dbo.PlanMembresia
            (IdPlan, NombrePlan, Descripcion, DuracionDias, Precio, EsPopular,
             Color, Beneficio1, Beneficio2, Beneficio3, Beneficio4)
        VALUES
            (2, N'Trimestral', N'¡La favorita de nuestros miembros!',
             90, 30000.00, 1, N'#ff5500',
             N'Acceso ilimitado al gimnasio', N'Evaluación física trimestral',
             N'Plan nutricional premium', N'Camiseta SUWO GYM de regalo');

    IF NOT EXISTS (SELECT 1 FROM dbo.PlanMembresia WHERE IdPlan = 3)
        INSERT dbo.PlanMembresia
            (IdPlan, NombrePlan, Descripcion, DuracionDias, Precio, EsPopular,
             Color, Beneficio1, Beneficio2, Beneficio3, Beneficio4)
        VALUES
            (3, N'Anual', N'El compromiso total con tu salud',
             365, 117000.00, 0, N'#293039',
             N'Acceso ilimitado al gimnasio', N'Evaluación física mensual',
             N'Plan nutricional premium', N'Kit de entrenamiento SUWO GYM gratis');

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO
SELECT IdRol, NombreRol FROM dbo.Rol ORDER BY IdRol;
SELECT IdPlan, NombrePlan, DuracionDias, Precio
FROM dbo.PlanMembresia ORDER BY IdPlan;
SELECT COUNT_BIG(*) AS UsuariosSinModificar FROM dbo.Usuario;
GO
