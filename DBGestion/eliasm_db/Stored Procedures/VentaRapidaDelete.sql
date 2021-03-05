
CREATE PROCEDURE [eliasm_db].[VentaRapidaDelete]
	@Id nvarchar(150)
AS
BEGIN TRY
	SET NOCOUNT ON;
	
	BEGIN TRAN
			DELETE VentasRapidasFormasPagos
			WHERE VentaRapidaId = @Id

			DELETE VentasRapidasDetalle
			WHERE VentaRapidaId = @Id
			
			DELETE VentasRapidas
			WHERE Id = @Id

	COMMIT;

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