
CREATE PROCEDURE [eliasm_db].[PresupuestoInsertarDolar]
	@PresupuestoId nvarchar(150),
	@FormaPagoId nvarchar(150),
	@Importe numeric(18,2),
	@DolarImporte numeric(18,2),
	@DolarCotizacion numeric(18,2),
	@TotalImporte numeric(18,2),
	@DescuentoImporte numeric(18,2),
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
	DECLARE @PorcentTMP numeric(18,12) = 0

	SELECT @FormaPagoCodigo = Codigo,
	@FormaPago = Descripcion,@FormaPagoTipo = Tipo
	FROM FormasPagos
	WHERE Id = @FormaPagoId

	SET @Total = @Importe
	
	IF @DescuentoImporte > 0
	BEGIN
		SET @PorcentTMP = @Importe * 100 / @DescuentoImporte
		SET @ImporteDescuento = (@TotalImporte - @DescuentoImporte) * (@PorcentTMP / 100)

		SET @Importe = @Importe + @ImporteDescuento
	END

	BEGIN TRAN
		INSERT INTO PresupuestosFormasPagos(Id,PresupuestoId,
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
												DolarImporte,
												DolarCotizacion,
												FechaAlta,UsuarioAlta)
										VALUES(@Id,@PresupuestoId,
												@FormaPagoId,@FormaPagoCodigo,
												@FormaPagoTipo,@FormaPago,
												@Importe,
												@Cuota,
												@Interes,
												@ImporteDescuento,
												@Total,
												upper(@Observaciones),
												CONVERT(NUMERIC(18,2), @DolarImporte),
												CONVERT(NUMERIC(18,2), @DolarCotizacion),
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