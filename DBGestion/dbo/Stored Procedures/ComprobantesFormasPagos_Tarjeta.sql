
CREATE PROCEDURE [ComprobantesFormasPagos_Tarjeta]
	@ComprobanteFormaPagoId int,
	@ComprobanteId int,
	@Importe numeric(18,2),
	@Cuota int,
	@ImporteInteres numeric(18,2),
	@Total numeric(18,2),
	@TarjetaId int,
	@TarjetaCliente varchar(300),
	@TarjetaNumero varchar(16),
	@TarjetaVenceMes int,
	@TarjetaVenceAño int,
	@TarjetaEsDebito bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	DECLARE @FormaPagoId int = 2
	DECLARE @FormaPago varchar(150) 

	IF @TarjetaEsDebito = 1
	BEGIN
		SET @FormaPago = 'TARJETA DE DEBITO'
		SET @FormaPagoId = 3
	END
	ELSE
	BEGIN
		SET @FormaPago = 'TARJETA DE CREDITO'
		SET @FormaPagoId = 2
	END

	DECLARE @TarjetaNombre varchar(150)
	SELECT @TarjetaNombre = TarjetaNombre FROM ParamTarjetasCreditosDebitos WHERE TarjetaId = @TarjetaId
	
	BEGIN TRAN
		IF @ComprobanteFormaPagoId = 0
		BEGIN
			EXEC @ComprobanteFormaPagoId = NextNumber 'ComprobantesFormasPagos'

			INSERT INTO ComprobantesFormasPagos(ComprobanteFormaPagoId,ComprobanteId,FormaPagoId,FormaPago,Importe,Cuota,ImporteInteres,Total,
												TarjetaId,TarjetaNombre,TarjetaCliente,TarjetaNumero,TarjetaVenceMes,TarjetaVenceAño,TarjetaEsDebito,
												FechaAlta,UsuarioAlta)
									VALUES(@ComprobanteFormaPagoId,@ComprobanteId,@FormaPagoId,@FormaPago,@Importe,@Cuota,@ImporteInteres,@Total,
												@TarjetaId,UPPER(@TarjetaNombre),UPPER(@TarjetaCliente),@TarjetaNumero,@TarjetaVenceMes,@TarjetaVenceAño,@TarjetaEsDebito,
												GETDATE(),UPPER(@Usuario))
		END
		
		IF @ComprobanteFormaPagoId > 0
		BEGIN
			UPDATE ComprobantesFormasPagos
			SET Importe = @Importe,
				Total = @Total,
				ImporteInteres = @ImporteInteres,
				Cuota = @Cuota,
				TarjetaId = @TarjetaId,
				TarjetaNombre = UPPER(@TarjetaNombre),
				TarjetaCliente = UPPER(@TarjetaCliente),
				TarjetaNumero = @TarjetaNumero,
				TarjetaVenceMes = @TarjetaVenceMes,
				TarjetaVenceAño = @TarjetaVenceAño,
				TarjetaEsDebito = @TarjetaEsDebito
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