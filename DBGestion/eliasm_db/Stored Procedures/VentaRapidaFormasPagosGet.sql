CREATE PROCEDURE [eliasm_db].[VentaRapidaFormasPagosGet] 
	@VentaRapidaId nvarchar(150)

AS
BEGIN
	SELECT Id,
			VentaRapidaId,
			FormaPagoId,
			FormaPagoCodigo,
			FormaPagoTipo,
			FormaPago,
			Importe,
			Cuota,
			Interes,
			isnull(Descuento,0)Descuento,
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
			DolarCotizacion,
			DolarImporte,
			FechaAlta,
			UsuarioAlta
	FROM VentasRapidasFormasPagos P
	WHERE P.VentaRapidaId = @VentaRapidaId	
	ORDER BY FormaPagoCodigo
END;