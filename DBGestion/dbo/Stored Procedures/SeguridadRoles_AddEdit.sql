
CREATE PROCEDURE [SeguridadRoles_AddEdit]
	@RolId Int, 
	@Rol varchar(150),
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @RolId = 0
		BEGIN
			EXEC @RolId = NextNumber 'SeguridadRoles'
			INSERT INTO SeguridadRoles(RolId,Rol,Estado,FechaAlta,UsuarioAlta)
			VALUES(@RolId,upper(@Rol),@Estado,getdate(),upper(@Usuario))
		END
		
		IF @RolId > 0
		BEGIN
			UPDATE SeguridadRoles
			SET Rol = upper(@Rol),Estado = @Estado
			WHERE RolId = @RolId
		END

	COMMIT;

	SELECT @RolId Id

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