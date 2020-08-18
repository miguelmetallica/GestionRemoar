
CREATE PROCEDURE [ComprobantesFormasPagos_Cheque]
	@ComprobanteFormaPagoId int,
	@ComprobanteId int,
	@Importe numeric(18,2),
	@ChequeBancoId int,
	@ChequeNumero varchar(50),
	@ChequeFechaEmision datetime,
	@ChequeFechaVencimiento datetime,
	@ChequeCuit varchar(11),
	@ChequeNombre varchar(300),
	@ChequeCuenta varchar(50),
	@Usuario nvarchar(256)
AS
BEGIN TRY
	DECLARE @FormaPagoId int = 4
	DECLARE @FormaPago varchar(150)= 'CHEQUE'
	DECLARE @Cuota int = 1
	DECLARE @Total numeric(18,2) = @Importe
	DECLARE @ImporteInteres numeric(18,2) = @Importe
	
	DECLARE @ChequeBanco varchar(150)
	SELECT @ChequeBanco = Banco FROM ParamBancos WHERE BancoId = @ChequeBancoId

	BEGIN TRAN
		IF @ComprobanteFormaPagoId = 0
		BEGIN
			EXEC @ComprobanteFormaPagoId = NextNumber 'ComprobantesFormasPagos'
			INSERT INTO ComprobantesFormasPagos(ComprobanteFormaPagoId,ComprobanteId,FormaPagoId,FormaPago,Importe,Cuota,ImporteInteres,Total,
												ChequeBancoId,ChequeBanco,ChequeNumero,ChequeFechaEmision,ChequeFechaVencimiento,ChequeCuit,ChequeNombre,ChequeCuenta,
												FechaAlta,UsuarioAlta)
									VALUES(@ComprobanteFormaPagoId,@ComprobanteId,@FormaPagoId,@FormaPago,@Importe,@Cuota,@ImporteInteres,@Total,
												@ChequeBancoId,@ChequeBanco,@ChequeNumero,@ChequeFechaEmision,@ChequeFechaVencimiento,@ChequeCuit,@ChequeNombre,
												@ChequeCuenta,GETDATE(),UPPER(@Usuario))
		END
		
		IF @ComprobanteFormaPagoId > 0
		BEGIN
			UPDATE ComprobantesFormasPagos
			SET Importe = @Importe,
				Total = @Total,
				ImporteInteres = @ImporteInteres,
				ChequeBancoId = @ChequeBancoId,
				ChequeBanco = @ChequeBanco,
				ChequeNumero = @ChequeNumero,
				ChequeFechaEmision = @ChequeFechaEmision,
				ChequeFechaVencimiento = @ChequeFechaVencimiento,
				ChequeCuit = @ChequeCuit,
				ChequeNombre = @ChequeNombre,
				ChequeCuenta = @ChequeCuenta
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