

CREATE PROCEDURE [eliasm_db].[ComprobantesTmpInsertarEfectivo]
	@ComprobanteId nvarchar(150),
	@FormaPagoId nvarchar(150),
	@Importe numeric(18,2),
	@TotalImporte numeric(18,2),
	@DescuentoImporte numeric(18,2),
	@DescuentoPorcentaje numeric(18,2),
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
	DECLARE @Cuota int = 1
	DECLARE @Interes numeric(18,2) = 0
	DECLARE @Total numeric(18,2) = 0
	DECLARE @ImporteDescuento numeric(18,2) = 0
	DECLARE @PorcentCancelacion numeric(18,12)
	
	SELECT @FormaPagoCodigo = Codigo,@FormaPago = Descripcion,@FormaPagoTipo = Tipo
	FROM FormasPagos
	WHERE Id = @FormaPagoId

	SELECT @TipoComprobanteId = T.Id			
	FROM SistemaConfiguraciones C
	INNER JOIN ParamTiposComprobantes T ON T.Codigo = C.Valor
	INNER JOIN ComprobantesNumeraciones N ON N.TipoComprobanteId = T.Id
	WHERE C.Configuracion = 'COMPROBANTES_RECIBO'
	AND N.Estado = 1

	DECLARE @TotalFatura NUMERIC(18,2)
	DECLARE @TotalFaturaDescuento NUMERIC(18,2)
	DECLARE @InteresFormaPago NUMERIC(18,2)
	DECLARE @DescImporte NUMERIC(18,2)
	DECLARE @ImportePago NUMERIC(18,2)  = @Importe
	DECLARE @ImportePagoLista NUMERIC(18,2)

	SELECT @TotalFatura = Total
	FROM Comprobantes P
	WHERE P.Id = @ComprobanteId

	SELECT TOP 1 @InteresFormaPago = abs(P.Interes)
	FROM FormasPagosCuotas P
	WHERE P.FormaPagoId = @FormaPagoId
	AND P.Cuota = 1
	AND CONVERT(DATE,GETDATE()) BETWEEN P.FechaDesde AND P.FechaHasta
	AND P.Estado = 1	

	SET @DescImporte = @TotalFatura * (ABS(@InteresFormaPago) / 100)
	SET @TotalFaturaDescuento = @TotalFatura - @DescImporte
	SET @ImportePagoLista = @TotalFatura / @TotalFaturaDescuento * @ImportePago 

	SET @Total = @ImportePago
	SET @ImporteDescuento = @ImportePagoLista - @ImportePago
	SET @Importe = @ImportePagoLista
	
	BEGIN TRAN
		INSERT INTO ComprobantesFormasPagosTmp(Id,ComprobanteId,TipoComprobanteId,
												FormaPagoId,
												FormaPagoCodigo,
												FormaPagoTipo,
												FormaPago,
												Importe,
												Cuota,
												Interes,
												Descuento,
												Total,
												Observaciones,
												FechaAlta,UsuarioAlta)
										VALUES(@Id,@ComprobanteId,@TipoComprobanteId,
												@FormaPagoId,
												@FormaPagoCodigo,
												@FormaPagoTipo,
												@FormaPago,
												@Importe,
												@Cuota,
												@Interes,
												@ImporteDescuento,
												@Total,
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