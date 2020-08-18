
CREATE PROCEDURE [ComprobantesFormasPagos_Efectivo]
	@ComprobanteFormaPagoId int,
	@ComprobanteId int,
	@Importe numeric(18,2),
	@Usuario nvarchar(256)
AS
BEGIN TRY
	DECLARE @FormaPagoId int = 1
	DECLARE @FormaPago varchar(150)= 'EFECTIVO'
	DECLARE @Cuota int = 1
	DECLARE @Total numeric(18,2) = @Importe
	DECLARE @ImporteInteres numeric(18,2) = @Importe
	BEGIN TRAN
		IF @ComprobanteFormaPagoId = 0
		BEGIN
			EXEC @ComprobanteFormaPagoId = NextNumber 'ComprobantesFormasPagos'

			INSERT INTO ComprobantesFormasPagos(ComprobanteFormaPagoId,ComprobanteId,FormaPagoId,FormaPago,Importe,Cuota,ImporteInteres,Total,FechaAlta,UsuarioAlta)
									VALUES(@ComprobanteFormaPagoId,@ComprobanteId,@FormaPagoId,@FormaPago,@Importe,@Cuota,@ImporteInteres,@Total,GETDATE(),UPPER(@Usuario))
		END
		
		IF @ComprobanteFormaPagoId > 0
		BEGIN
			UPDATE ComprobantesFormasPagos
			SET Importe = @Importe,
				Total = @Total,
				ImporteInteres = @ImporteInteres
			WHERE ComprobanteFormaPagoId = @ComprobanteFormaPagoId
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