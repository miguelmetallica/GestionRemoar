

CREATE PROCEDURE [eliasm_db].[VentaRapidaInsertarCheque]
	@VentaRapidaId nvarchar(150),
	@FormaPagoId nvarchar(150),
	@ChequeBancoId nvarchar(150),
	@Importe numeric(18,2),
	@ChequeNumero nvarchar(50) = NULL,
	@ChequeFechaEmision datetime = NULL,
	@ChequeFechaVencimiento datetime = NULL,
	@ChequeCuit nvarchar(11) = NULL,
	@ChequeNombre nvarchar(300) = NULL,
	@ChequeCuenta nvarchar(50) = NULL,
	@TotalImporte numeric(18,2),
	@DescuentoImporte numeric(18,2),
	@RecargoImporte numeric(18,2),
	@Observaciones nvarchar(150) = NULL,
	@Usuario nvarchar(256) = NULL

AS
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @Id nvarchar(150) = NEWId()
	DECLARE @TipoComprobanteId nvarchar(150)
	DECLARE @FormaPagoCodigo nvarchar(150)
	DECLARE @FormaPago nvarchar(150)
	DECLARE @FormaPagoTipo char(1)
	DECLARE @ChequeBanco nvarchar(150)
	DECLARE @Cuota int = 1
	DECLARE @Interes numeric(18,2) = 0
	DECLARE @Total numeric(18,2) = 0
	DECLARE @ImporteDescuento numeric(18,2) = 0

	DECLARE @PorcentTMP numeric(18,12)

	SELECT @FormaPagoCodigo = Codigo,
		@FormaPago = Descripcion,
		@FormaPagoTipo = Tipo
	FROM FormasPagos
	WHERE Id = @FormaPagoId

	SELECT @ChequeNombre = Descripcion	
	FROM ParamEntidades
	WHERE Id = @ChequeBancoId


	SET @Total = @Importe
	IF @DescuentoImporte > 0
	BEGIN
		SET @PorcentTMP = @Importe * 100 / @DescuentoImporte
		SET @ImporteDescuento = (@TotalImporte - @DescuentoImporte) * (@PorcentTMP / 100)
		SET @Importe = @Importe + @ImporteDescuento
	END

	IF @RecargoImporte > 0
	BEGIN
		SET @PorcentTMP = @Importe * 100 / @RecargoImporte
		SET @Interes = (@RecargoImporte - @TotalImporte) * (@PorcentTMP / 100)
		SET @Importe = @Importe - @Interes
	END

	BEGIN TRAN
		INSERT INTO VentasRapidasFormasPagos(Id,VentaRapidaId,
												FormaPagoId,
												FormaPagoCodigo,
												FormaPagoTipo,
												FormaPago,
												Importe,
												Cuota,
												Interes,
												Descuento,
												Total,
												ChequeBancoId,
												ChequeBanco,
												ChequeNumero,
												ChequeFechaEmision,
												ChequeFechaVencimiento,
												ChequeCuit,
												ChequeNombre,
												ChequeCuenta,
												Observaciones,
												FechaAlta,UsuarioAlta)
										VALUES(@Id,
												@VentaRapidaId,
												@FormaPagoId,
												@FormaPagoCodigo,
												@FormaPagoTipo,
												@FormaPago,
												@Importe,
												@Cuota,
												@Interes,
												@ImporteDescuento,
												@Total,
												@ChequeBancoId,
												upper(@ChequeBanco),
												upper(@ChequeNumero),
												@ChequeFechaEmision,
												@ChequeFechaVencimiento,
												upper(@ChequeCuit),
												upper(@ChequeNombre),
												upper(@ChequeCuenta),
												upper(@Observaciones),
												DATEADD(HH,4,GETDATE()),UPPER(@Usuario))
		

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