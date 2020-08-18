

CREATE PROCEDURE [ParamUnidadesMedidas_AddEdit]
	@UnidadMedidaId int,
	@UnidadMedidaCodigo varchar(5),
	@UnidadMedida varchar(150),	
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @UnidadMedidaId = 0
		BEGIN
			EXEC @UnidadMedidaId = NextNumber 'ParamUnidadesMedidas'
			INSERT INTO ParamUnidadesMedidas(UnidadMedidaId,UnidadMedidaCodigo,UnidadMedida,Estado,FechaAlta,UsuarioAlta)
			VALUES(@UnidadMedidaId,UPPER(@UnidadMedidaCodigo),UPPER(@UnidadMedida),@Estado,GETDATE(),UPPER(@Usuario))
		END
		
		IF @UnidadMedidaId > 0
		BEGIN
			UPDATE ParamUnidadesMedidas
			SET UnidadMedidaCodigo = UPPER(@UnidadMedidaCodigo),
				UnidadMedida = UPPER(@UnidadMedida),
				Estado = @Estado
			WHERE UnidadMedidaId = @UnidadMedidaId
		END

	COMMIT;

	SELECT @UnidadMedidaId Id

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