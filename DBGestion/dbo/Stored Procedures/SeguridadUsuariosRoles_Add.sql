CREATE PROCEDURE [SeguridadUsuariosRoles_Add]	
	@UsuarioId Int,
	@RolId Int,	 
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @UsuarioRolId Int = 0
	BEGIN TRAN
		IF @UsuarioRolId = 0
		BEGIN
			EXEC @UsuarioRolId = NextNumber 'SeguridadUsuariosRoles'
			INSERT INTO SeguridadUsuariosRoles(UsuarioRolId,UsuarioId,RolId,FechaAlta,UsuarioAlta)
			VALUES(@UsuarioRolId,@UsuarioId,@RolId,getdate(),upper(@Usuario))
		END

	COMMIT;

	SELECT @UsuarioRolId Id

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