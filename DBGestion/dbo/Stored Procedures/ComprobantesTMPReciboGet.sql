CREATE PROCEDURE [dbo].[ComprobantesTMPReciboGet]
	@ComprobanteId nvarchar(150)

AS
BEGIN
	SELECT Id,
			ComprobanteId,
			TipoComprobanteId,
			FormaPagoId,
			FormaPagoCodigo,
			FormaPagoTipo,
			FormaPago,
			Importe,
			Cuota,
			Interes,
			Total,
			TarjetaId,
			TarjetaNombre,
			TarjetaCliente,
			TarjetaNumero,
			TarjetaVenceMes,
			TarjetaVenceAño,
			TarjetaCodigoSeguridad,
			TarjetaEsDebito,
			ChequeBancoId,
			ChequeBanco,
			ChequeNumero,
			ChequeFechaEmision,
			ChequeFechaVencimiento,
			ChequeCuit,
			ChequeNombre,
			ChequeCuenta,
			Otros,
			Observaciones,
			CodigoAutorizacion,
			FechaAlta,
			UsuarioAlta
	FROM ComprobantesFormasPagosTmp P
	WHERE P.ComprobanteId = @ComprobanteId
	AND P.TipoComprobanteId IN (
								SELECT T.Id 
								FROM SistemaConfiguraciones C
								INNER JOIN ParamTiposComprobantes T ON T.Codigo = C.Valor
								INNER JOIN ComprobantesNumeraciones N ON N.TipoComprobanteId = T.Id
								WHERE C.Configuracion = 'COMPROBANTES_RECIBO'
								AND N.Estado = 1
								)
END;