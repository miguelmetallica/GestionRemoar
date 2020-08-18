
CREATE PROCEDURE [Comprobantes_CabeceraConfirmaRecibo]
	@ComprobanteId int
AS
BEGIN TRY
	BEGIN TRAN
		IF @ComprobanteId > 0
		BEGIN
			UPDATE Comprobantes
			SET Cobrado = 1
			WHERE ComprobanteId = @ComprobanteId
		END

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