/*
 SUWÓ GYM — Fase 7B: MARCA EXPLÍCITA de BD ficticia.
 ATENCIÓN: ejecutar exclusivamente sobre una base nueva, de esquema solamente,
 llamada SuwoGym_Test_7B. No usar ni restaurar datos reales.
 NO ejecutar contra dbo.Usuario de SuwoGym.
*/
USE [SuwoGym_Test_7B];
GO
IF DB_NAME() <> N'SuwoGym_Test_7B'
    THROW 52001, 'Solo puede ejecutarse en SuwoGym_Test_7B.', 1;
GO

IF OBJECT_ID(N'dbo.Usuario', N'U') IS NULL OR
   OBJECT_ID(N'dbo.Rol', N'U') IS NULL OR
   OBJECT_ID(N'dbo.spRecuperacionConsumir', N'P') IS NULL OR
   OBJECT_ID(N'dbo.spReservarClaseMiembro', N'P') IS NULL OR
   OBJECT_ID(N'dbo.trg_Usuario_RevocarJwt', N'TR') IS NULL
    THROW 52002, 'Falta esquema o migraciones 07/08 de SUWÓ GYM.', 1;
GO

-- Proteger contra un clon que contenga usuarios o correos reales.
-- Solo se toleran residuos ficticios identificables de ejecuciones anteriores.
IF EXISTS
(
    SELECT 1 FROM dbo.Usuario
    WHERE CorreoElectronico NOT LIKE N't7b-%@example.invalid'
)
    THROW 52003, 'Se detectaron usuarios ajenos a pruebas. Usar esquema SIN datos.', 1;
GO

-- Roles necesarios para las FK, sin generar cuentas ni contraseñas.
IF NOT EXISTS (SELECT 1 FROM dbo.Rol WHERE IdRol = 1)
    INSERT dbo.Rol(IdRol, NombreRol, Descripcion)
    VALUES (1, N'Administrador', N'Rol de pruebas de SUWO GYM');
IF NOT EXISTS (SELECT 1 FROM dbo.Rol WHERE IdRol = 2)
    INSERT dbo.Rol(IdRol, NombreRol, Descripcion)
    VALUES (2, N'Entrenador', N'Rol de pruebas de SUWO GYM');
IF NOT EXISTS (SELECT 1 FROM dbo.Rol WHERE IdRol = 3)
    INSERT dbo.Rol(IdRol, NombreRol, Descripcion)
    VALUES (3, N'Miembro', N'Rol de pruebas de SUWO GYM');
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.extended_properties
    WHERE class = 0 AND name = N'SuwoGym7BTestOnly'
)
BEGIN
    EXEC sys.sp_addextendedproperty
        @name = N'SuwoGym7BTestOnly',
        @value = N'YES';
END;
GO

SELECT DB_NAME() AS BaseDePruebas,
       CONVERT(NVARCHAR(30), value) AS MarcaAislamiento
FROM sys.extended_properties
WHERE class = 0 AND name = N'SuwoGym7BTestOnly';
GO
