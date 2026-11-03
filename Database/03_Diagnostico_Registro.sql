/*
    Diagnóstico de registro SUWÓ GYM. SOLO LECTURA.
    No modifica procedimientos, tablas, usuarios ni contraseñas.
*/
USE [SuwoGym];
GO
SELECT DB_NAME() AS BaseDatosActual, @@SERVERNAME AS ServidorSQL;
SELECT COUNT_BIG(*) AS Usuarios FROM dbo.Usuario;
SELECT IdRol, NombreRol FROM dbo.Rol ORDER BY IdRol;
SELECT IdPlan, NombrePlan, DuracionDias, Precio
FROM dbo.PlanMembresia ORDER BY IdPlan;
SELECT
    p.parameter_id AS Orden,
    p.name AS Parametro,
    TYPE_NAME(p.user_type_id) AS TipoDato,
    p.max_length AS LargoBytes
FROM sys.parameters AS p
WHERE p.object_id = OBJECT_ID(N'dbo.spRegistrarUsuario')
ORDER BY p.parameter_id;
GO
