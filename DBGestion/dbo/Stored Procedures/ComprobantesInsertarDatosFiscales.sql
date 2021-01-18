CREATE PROCEDURE [dbo].[ComprobantesInsertarDatosFiscales]
	@Id nvarchar(150),
	@TipoComprobanteFiscal nvarchar(2),
	@LetraFiscal char(1),
	@PtoVentaFiscal int,
	@NumeroFiscal numeric(12, 0),
	@Usuario nvarchar(256) = NULL
AS
BEGIN TRY
	SET NOCOUNT ON;
	
	BEGIN TRAN
		UPDATE Comprobantes
		SET TipoComprobanteFiscal = @TipoComprobanteFiscal,
			LetraFiscal = @LetraFiscal,
			PtoVentaFiscal = @PtoVentaFiscal,
			NumeroFiscal = @NumeroFiscal
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