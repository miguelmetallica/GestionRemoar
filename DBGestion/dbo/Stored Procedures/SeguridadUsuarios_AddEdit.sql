CREATE PROCEDURE [SeguridadUsuarios_AddEdit]
	@UsuarioId Int, 
	@Usuario nvarchar(256),
	@Password nvarchar(100),
	@Apellido varchar(150),
	@Nombre varchar(150),
	@Telefono varchar(50),
	@Celular varchar(50),
	@Puesto varchar(250),
	@SucursalId int,
	@Estado bit,
	@_Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @UsuarioId = 0
		BEGIN
			EXEC @UsuarioId = NextNumber 'SeguridadUsuarios'
			INSERT INTO SeguridadUsuarios(UsuarioId,Usuario,"Password",Apellido,Nombre,Telefono,Celular,Puesto,SucursalId,Estado,FechaAlta,UsuarioAlta)
			VALUES(@UsuarioId,upper(@Usuario),dbo.encriptar(@Password),upper(@Apellido),upper(@Nombre),upper(@Telefono),upper(@Celular),upper(@Puesto), @SucursalId,@Estado,getdate(),upper(@Usuario))
		END
		
		IF @UsuarioId > 0
		BEGIN
			UPDATE SeguridadUsuarios
			SET Usuario = upper(@Usuario),
				Apellido = upper(@Apellido),
				Nombre = upper(@Nombre),
				Telefono = upper(@Telefono),
				Celular = upper(@Celular),
				Puesto = upper(@Puesto), 
				SucursalId = @SucursalId
			WHERE UsuarioId = @UsuarioId
		END

	COMMIT;

	SELECT @UsuarioId Id

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 
		ROLLBACK;

	DECLARE @ErrorMessage NVARCHAR(4000);  
	DECLARE @ErrorSeverity INT;  
	DECLARE @ErrorState INT;  

	SELECT @ErrorMessage = ERROR_MESSAGE(),@ErrorSeverity = ERROR_SEVERITY(),@ErrorState = ERROR_STATE();  

	RAISERROR (@ErrorMessage, -- Message text.  
				@ErrorSeverity, -- Severity.  
				@ErrorState -- State.  
			);  
END CATCH