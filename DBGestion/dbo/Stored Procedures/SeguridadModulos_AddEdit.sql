CREATE PROCEDURE [SeguridadModulos_AddEdit]
	@ModuloId Int, 
	@Modulo varchar(150),
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @ModuloId = 0
		BEGIN
			EXEC @ModuloId = NextNumber 'SeguridadModulos'
			INSERT INTO SeguridadModulos(ModuloId,Modulo,Estado,FechaAlta,UsuarioAlta)
			VALUES(@ModuloId,upper(@Modulo),@Estado,getdate(),upper(@Usuario))
		END
		
		IF @ModuloId > 0
		BEGIN
			UPDATE SeguridadModulos
			SET Modulo = upper(@Modulo),Estado = @Estado
			WHERE ModuloId = @ModuloId
		END

	COMMIT;

	SELECT @ModuloId Id

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