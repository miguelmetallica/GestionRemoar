
CREATE PROCEDURE [ComprobantesFormasPagos_Get]
	@ComprobanteId int = NULL	
AS
BEGIN
	SELECT ComprobanteFormaPagoId,
		ComprobanteId,
		ISNULL(FormaPagoId,0)FormaPagoId,
		ISNULL(FormaPago,'')FormaPago,
		ISNULL(Importe,0)Importe,
		ISNULL(ImporteInteres,0)ImporteInteres,
		ISNULL(Cuota,0)Cuota,
		ISNULL(Total,0)Total,
		ISNULL(TarjetaId,0)TarjetaId,
		ISNULL(TarjetaNombre,'')TarjetaNombre,
		ISNULL(TarjetaCliente,'')TarjetaCliente,
		ISNULL(TarjetaNumero,'')TarjetaNumero,
		ISNULL(TarjetaVenceMes,0)TarjetaVenceMes,
		ISNULL(TarjetaVenceAño,0)TarjetaVenceAño,
		ISNULL(TarjetaCodigoSeguridad,0)TarjetaCodigoSeguridad,
		ISNULL(TarjetaEsDebito,0)TarjetaEsDebito,
		ISNULL(ChequeBancoId,0)ChequeBancoId,
		ISNULL(ChequeBanco,'')ChequeBanco,
		ISNULL(ChequeNumero,'')ChequeNumero,
		ISNULL(ChequeFechaEmision,GETDATE())ChequeFechaEmision,
		ISNULL(ChequeFechaVencimiento,GETDATE())ChequeFechaVencimiento,
		ISNULL(ChequeCuit,'')ChequeCuit,
		ISNULL(ChequeNombre,'')ChequeNombre,
		ISNULL(ChequeCuenta,'')ChequeCuenta,
		FechaAlta,
		UsuarioAlta
	FROM ComprobantesFormasPagos P
	WHERE P.ComprobanteId = @ComprobanteId
END