

CREATE PROCEDURE [dbo].[ComprobantesTmpInsertarEfectivo]
	@ComprobanteId nvarchar(150),
	--@TipoComprobanteId nvarchar(150),
	@FormaPagoId nvarchar(150),
	@Importe numeric(18,2),
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
	DECLARE @Total numeric(18,2) = @Importe
	
	SELECT @FormaPagoCodigo = Codigo,@FormaPago = Descripcion,@FormaPagoTipo = Tipo
	FROM FormasPagos
	WHERE Id = @FormaPagoId

	SELECT @TipoComprobanteId = T.Id			
	FROM SistemaConfiguraciones C
	INNER JOIN ParamTiposComprobantes T ON T.Codigo = C.Valor
	INNER JOIN ComprobantesNumeraciones N ON N.TipoComprobanteId = T.Id
	WHERE C.Configuracion = 'COMPROBANTES_RECIBO'
	AND N.Estado = 1
	
	BEGIN TRAN
		INSERT INTO ComprobantesFormasPagosTmp(Id,ComprobanteId,TipoComprobanteId,
												FormaPagoId,FormaPagoCodigo,
												FormaPagoTipo,FormaPago,
												Importe,Cuota,Interes,Total,Observaciones,
												FechaAlta,UsuarioAlta)
										VALUES(@Id,@ComprobanteId,@TipoComprobanteId,
												@FormaPagoId,@FormaPagoCodigo,
												@FormaPagoTipo,@FormaPago,
												@Importe,
												@Cuota,
												@Interes,
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