
CREATE PROCEDURE [ComprobantesFormasPagos_Delete]
	@ComprobanteFormaPagoId int,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	DECLARE @ComprobanteId int
	SELECT @ComprobanteId = ComprobanteId 
	FROM ComprobantesFormasPagos
	WHERE ComprobanteFormaPagoId = @ComprobanteFormaPagoId
	
	BEGIN TRAN
		DELETE FROM ComprobantesFormasPagos
		WHERE ComprobanteFormaPagoId = @ComprobanteFormaPagoId
	COMMIT;

	SELECT @ComprobanteId Id

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