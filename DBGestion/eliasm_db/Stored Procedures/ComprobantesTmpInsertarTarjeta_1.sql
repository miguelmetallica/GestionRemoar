

CREATE PROCEDURE [eliasm_db].[ComprobantesTmpInsertarTarjeta]
	@ComprobanteId nvarchar(150),
	@FormaPagoId nvarchar(150),
	@TarjetaId nvarchar(150) = null,
	@Cuota int,
	@Importe numeric(18,2),
	@Interes numeric(18,2),
	@Total numeric(18,2),	
	@TarjetaCliente nvarchar(150) = null,
	@TarjetaNumero nvarchar(150) = null,
	@TarjetaVenceMes int = 0,
	@TarjetaVenceAño int = 0,
	@TarjetaCodigoSeguridad int = 0,
	@TarjetaEsDebito bit,	
	@Observaciones nvarchar(150) = NULL,
	@CodigoAutorizacion nvarchar(50) = NULL,
	@Usuario nvarchar(256) = NULL
AS
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @Id nvarchar(150) = NEWId()
	DECLARE @TipoComprobanteId nvarchar(150)
	DECLARE @FormaPagoCodigo nvarchar(150)
	DECLARE @FormaPago nvarchar(150)
	DECLARE @FormaPagoTipo char(1)
	DECLARE @TarjetaNombre nvarchar(150)
	DECLARE @Porcentaje numeric(18,4)
	DECLARE @ImporteTMP numeric(18,2) = @Importe;
	
	SET @Importe = @Total
	SET @Interes = ABS(@Importe * @Cuota  - @ImporteTMP) / @Cuota;
	SET @Total = @ImporteTMP
	
	SELECT @FormaPagoCodigo = Codigo,@FormaPago = Descripcion,@FormaPagoTipo = Tipo
	FROM FormasPagos
	WHERE Id = @FormaPagoId

	SELECT @TipoComprobanteId = T.Id			
	FROM SistemaConfiguraciones C
	INNER JOIN ParamTiposComprobantes T ON T.Codigo = C.Valor
	INNER JOIN ComprobantesNumeraciones N ON N.TipoComprobanteId = T.Id
	WHERE C.Configuracion = 'COMPROBANTES_RECIBO'
	AND N.Estado = 1
	
	SELECT @TarjetaNombre = Descripcion
	FROM ParamEntidades
	WHERE Id = @TarjetaId


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
												TarjetaId,
												TarjetaNombre,
												TarjetaCliente,
												TarjetaNumero,
												TarjetaVenceAño,
												TarjetaVenceMes,
												TarjetaCodigoSeguridad,
												TarjetaEsDebito,
												Observaciones,
												CodigoAutorizacion,
												FechaAlta,UsuarioAlta)
										VALUES(@Id,@ComprobanteId,@TipoComprobanteId,
												@FormaPagoId,
												@FormaPagoCodigo,
												@FormaPagoTipo,
												upper(@FormaPago),
												@Importe,
												@Cuota,
												@Interes,
												0,
												@Total,
												@TarjetaId,
												@TarjetaNombre,
												upper(@TarjetaCliente),
												upper(@TarjetaNumero),
												@TarjetaVenceAño,
												@TarjetaVenceMes,
												upper(@TarjetaCodigoSeguridad),
												@TarjetaEsDebito,
												upper(@Observaciones),
												upper(@CodigoAutorizacion),
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