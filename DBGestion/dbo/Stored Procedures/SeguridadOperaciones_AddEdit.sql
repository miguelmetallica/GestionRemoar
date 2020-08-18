CREATE PROCEDURE [SeguridadOperaciones_AddEdit]
	@OperacionId Int, 
	@Operacion varchar(150),
	@ModuloId Int, 
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @OperacionId = 0
		BEGIN
			EXEC @OperacionId = NextNumber 'SeguridadOperaciones'
			INSERT INTO SeguridadOperaciones(OperacionId,Operacion,ModuloId,Estado,FechaAlta,UsuarioAlta)
			VALUES(@OperacionId,upper(@Operacion),@ModuloId,@Estado,getdate(),upper(@Usuario))
		END
		
		IF @OperacionId > 0
		BEGIN
			UPDATE SeguridadOperaciones
			SET Operacion = upper(@Operacion),
				ModuloId = @ModuloId,
				Estado = @Estado
			WHERE OperacionId = @OperacionId
		END

	COMMIT;

	SELECT @OperacionId Id

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