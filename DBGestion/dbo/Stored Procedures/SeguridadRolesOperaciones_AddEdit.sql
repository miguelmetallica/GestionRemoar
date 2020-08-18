CREATE PROCEDURE [SeguridadRolesOperaciones_AddEdit]
	@RolOperacionId Int,
	@RolId Int,
	@OperacionId Int, 
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @RolOperacionId = 0
		BEGIN
			EXEC @RolOperacionId = NextNumber 'SeguridadRolesOperaciones'
			INSERT INTO SeguridadRolesOperaciones(RolOperacionId,RolId,OperacionId,FechaAlta,UsuarioAlta)
			VALUES(@RolOperacionId,@RolId,@OperacionId,getdate(),upper(@Usuario))
		END

	COMMIT;

	SELECT @RolOperacionId Id

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