create PROCEDURE [dbo].[ComprobantesEntregaEliminarTMP]	
	@Id nvarchar(150),
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @CANTIDAD INT = 0;
	
	BEGIN
		BEGIN TRAN	
			DELETE ComprobantesDetalleTMP
			WHERE Id = @Id

		COMMIT;
	END;

	SELECT 1 Id

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