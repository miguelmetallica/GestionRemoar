

CREATE PROCEDURE [dbo].[ComprobantesTmpInsertarTarjeta]
	@ClienteId nvarchar(150),
	@FormaPagoId nvarchar(150),
	@Cuota int,
	@Importe numeric(18,2),
	@Interes numeric(18,2),
	@Total numeric(18,2),	
	@TarjetaCliente nvarchar(150),
	@TarjetaNumero nvarchar(150),
	@TarjetaVenceMes int,
	@TarjetaVenceAño int,
	@TarjetaCodigoSeguridad int = 0,
	@TarjetaEsDebito bit,	
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
	DECLARE @TarjetaNombre nvarchar(150)
	
	SELECT @FormaPagoCodigo = Codigo,@FormaPago = Descripcion,@FormaPagoTipo = Tipo
	FROM FormasPagos
	WHERE Id = @FormaPagoId

	SELECT @TipoComprobanteId = T.Id			
	FROM SistemaConfiguraciones C
	INNER JOIN ParamTiposComprobantes T ON T.Codigo = C.Valor
	INNER JOIN ComprobantesNumeraciones N ON N.TipoComprobanteId = T.Id
	WHERE C.Configuracion = 'COMPROBANTES_RECIBO_A'
	AND N.Estado = 1
	
	BEGIN TRAN
		INSERT INTO ComprobantesFormasPagosTmp(Id,ClienteId,TipoComprobanteId,
												FormaPagoId,FormaPagoCodigo,
												FormaPagoTipo,FormaPago,
												Importe,Cuota,Interes,Total,
												TarjetaCliente,TarjetaNumero,
												TarjetaVenceAño,TarjetaVenceMes,
												TarjetaCodigoSeguridad,TarjetaEsDebito,
												Observaciones,FechaAlta,UsuarioAlta)
										VALUES(@Id,@ClienteId,@TipoComprobanteId,
												@FormaPagoId,@FormaPagoCodigo,
												@FormaPagoTipo,@FormaPago,
												@Importe,@Cuota,@Interes,@Total,
												@TarjetaCliente,@TarjetaNumero,
												@TarjetaVenceAño,@TarjetaVenceMes,
												@TarjetaCodigoSeguridad,@TarjetaEsDebito,
												@Observaciones,DATEADD(HH,4,GETDATE()),UPPER(@Usuario))
		

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