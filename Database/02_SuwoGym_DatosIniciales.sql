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
        Descripcion
    )
    VALUES
    (
        1,
        N'Funcionales',
        N'Entrenamiento dinámico que mejora fuerza, resistencia, movilidad y coordinación mediante movimientos aplicables a la vida diaria'
    );
END
GO
IF NOT EXISTS (SELECT 1 FROM dbo.Clase WHERE IdClase = 2)
BEGIN
    INSERT INTO dbo.Clase
    (
        IdClase,
        NombreClase,
        Descripcion
    )
    VALUES
    (
        2,
        N'CrossFit',
        N'Programa de alta intensidad que combina fuerza, resistencia y acondicionamiento físico con ejercicios variados y desafiantes'
    );
END
GO
IF NOT EXISTS (SELECT 1 FROM dbo.Clase WHERE IdClase = 3)
BEGIN
    INSERT INTO dbo.Clase
    (
        IdClase,
        NombreClase,
        Descripcion
    )
    VALUES
    (
        3,
        N'Baile',
        N'Actividad física divertida que mejora la coordinación, resistencia y expresión corporal mediante movimientos al ritmo de la música'
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
        Precio
    )
    VALUES
    (
        1,
        N'Mensual',
        N'El pago se realiza cada mes; 12 pagos anuales',
        30,
        10600.00
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
        Precio
    )
    VALUES
    (
        2,
        N'Trimestral',
        N'El pago se realiza cada 3 meses; 4 pagos anuales',
        90,
        30000.00
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
        Precio
    )
    VALUES
    (
        3,
        N'Anual',
        N'El pago se realiza cada año; 1 pago anual',
        365,
        117000.00
    );
END
GO

PRINT N'Datos iniciales insertados correctamente.';
GO
