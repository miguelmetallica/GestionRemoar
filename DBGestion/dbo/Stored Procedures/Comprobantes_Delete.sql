
create PROCEDURE [Comprobantes_Delete]
	@ComprobanteId int,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	
	BEGIN TRAN
		UPDATE Comprobantes set Estado = 0
		WHERE ComprobanteId = @ComprobanteId
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