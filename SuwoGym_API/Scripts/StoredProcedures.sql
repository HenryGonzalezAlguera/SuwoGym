-- =============================================
-- Stored Procedures para SuwoGym
-- =============================================

-- ========== REGISTRAR USUARIO ==========
CREATE OR ALTER PROCEDURE spRegistrarUsuario
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
	FROM Usuario u
	INNER JOIN Rol r ON u.IdRol = r.IdRol
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
	FROM Usuario u
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

	UPDATE Usuario
	SET ContrasennaHash = @ContrasennaHash,
		DebeCambiarContrasenna = 0
	WHERE IdUsuario = @IdUsuario;

	SELECT @@ROWCOUNT AS Resultado;
END
GO
