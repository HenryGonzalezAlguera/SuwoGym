-- =============================================
-- Stored Procedures para SuwoGym
-- =============================================

-- ========== REGISTRAR USUARIO ==========
CREATE OR ALTER PROCEDURE spRegistrarUsuario
	@Nombre NVARCHAR(80),
	@Apellido NVARCHAR(80),
	@CorreoElectronico NVARCHAR(150),
	@ContrasennaHash NVARCHAR(300)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		-- Verificar si el correo ya existe
		IF EXISTS (SELECT 1 FROM Usuarios WHERE CorreoElectronico = @CorreoElectronico)
		BEGIN
			RAISERROR('El correo electrónico ya está registrado.', 16, 1);
			RETURN;
		END

		-- Insertar nuevo usuario con rol Miembro (IdRol = 3)
		INSERT INTO Usuarios (IdRol, Nombre, Apellido, CorreoElectronico, ContrasennaHash, Estado, DebeCambiarContrasenna, FechaRegistro)
		VALUES (3, @Nombre, @Apellido, @CorreoElectronico, @ContrasennaHash, 1, 0, GETDATE());

		-- Obtener el ID del usuario creado
		DECLARE @IdUsuario INT = SCOPE_IDENTITY();

		-- Crear registro de cliente
		INSERT INTO Clientes (IdUsuario, Identificacion, Estado, FechaRegistro)
		VALUES (@IdUsuario, '', 1, GETDATE());

		SELECT @IdUsuario AS Resultado;
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
		RAISERROR (@ErrorMessage, 16, 1);
	END CATCH
END
GO

-- ========== INICIAR SESIÓN ==========
CREATE OR ALTER PROCEDURE spIniciarSesionUsuario
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
	FROM Usuarios u
	INNER JOIN Roles r ON u.IdRol = r.IdRol
	WHERE u.CorreoElectronico = @CorreoElectronico
	  AND u.ContrasennaHash = @ContrasennaHash
	  AND u.Estado = 1;
END
GO

-- ========== VALIDAR CORREO ==========
CREATE OR ALTER PROCEDURE spValidarCorreo
	@CorreoElectronico NVARCHAR(150)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		u.IdUsuario,
		u.Nombre,
		u.Apellido,
		u.CorreoElectronico
	FROM Usuarios u
	WHERE u.CorreoElectronico = @CorreoElectronico
	  AND u.Estado = 1;
END
GO

-- ========== ACTUALIZAR CONTRASEÑA ==========
CREATE OR ALTER PROCEDURE spActualizarContrasenna
	@IdUsuario INT,
	@ContrasennaHash NVARCHAR(300)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE Usuarios
	SET ContrasennaHash = @ContrasennaHash,
		DebeCambiarContrasenna = 0
	WHERE IdUsuario = @IdUsuario;

	SELECT @@ROWCOUNT AS Resultado;
END
GO
