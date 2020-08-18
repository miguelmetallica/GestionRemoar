

CREATE PROCEDURE [ParamImpuestos_Delete]
	@ImpuestoId [int],
	@Usuario [nvarchar](256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		UPDATE ParamImpuestos
		SET Estado = (SELECT CASE WHEN Estado = 1 THEN 0 ELSE 1 END ParamImpuestos WHERE ImpuestoId = @ImpuestoId)
		WHERE ImpuestoId = @ImpuestoId
  
	COMMIT;
  
	SELECT @ImpuestoId Id
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