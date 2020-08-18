
CREATE PROCEDURE [ParamTiposComprobantes_AddEdit]
	@TipoComprobanteId int,
	@TipoComprobanteCodigo varchar(5),
	@TipoComprobante varchar(150),	
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @TipoComprobanteId = 0
		BEGIN
			EXEC @TipoComprobanteId = NextNumber 'ParamTiposComprobantes'
			INSERT INTO ParamTiposComprobantes(TipoComprobanteId,TipoComprobanteCodigo,TipoComprobante,Estado,FechaAlta,UsuarioAlta)
			VALUES(@TipoComprobanteId,UPPER(@TipoComprobanteCodigo),UPPER(@TipoComprobante),@Estado,GETDATE(),UPPER(@Usuario))
		END
		
		IF @TipoComprobanteId > 0
		BEGIN
			UPDATE ParamTiposComprobantes
			SET TipoComprobanteCodigo = UPPER(@TipoComprobanteCodigo),
				TipoComprobante = UPPER(@TipoComprobante),
				Estado = @Estado
			WHERE TipoComprobanteId = @TipoComprobanteId
		END

	COMMIT;

	SELECT @TipoComprobanteId Id

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