CREATE PROCEDURE [eliasm_db].[ComprobanteFormaPagoGet]
	@Id nvarchar(150) = ''
AS
BEGIN
	SELECT P.Id,
		P.ComprobanteId,
		P.FormaPagoId,
		P.FormaPagoCodigo,
		P.FormaPagoTipo,
		P.FormaPago,
		P.Importe,
		P.Cuota,
		P.Interes,
		P.Total,
		P.TarjetaId,
		P.TarjetaNombre,
		P.TarjetaCliente,
		P.TarjetaNumero,
		P.TarjetaVenceMes,
		P.TarjetaVenceAño,
		P.TarjetaCodigoSeguridad,
		P.TarjetaEsDebito,
		P.ChequeBancoId,
		P.ChequeBanco,
		P.ChequeNumero,
		P.ChequeFechaEmision,
		P.ChequeFechaVencimiento,
		P.ChequeCuit,
		P.ChequeNombre,
		P.ChequeCuenta,
		P.Otros,
		P.CodigoAutorizacion,
		P.Observaciones,
		P.FechaAlta,
		P.UsuarioAlta
	FROM Comprobantes C
	INNER JOIN ComprobantesFormasPagos P ON C.Id = P.ComprobanteId
	WHERE C.Id = @Id
	
END
;