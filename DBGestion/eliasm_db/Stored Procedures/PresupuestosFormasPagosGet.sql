create PROCEDURE [eliasm_db].[PresupuestosFormasPagosGet] 
	@PresupuestoId nvarchar(150)

AS
BEGIN
	SELECT Id,
			PresupuestoId,
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
	FROM PresupuestosFormasPagos P
	WHERE P.PresupuestoId = @PresupuestoId	
	ORDER BY FormaPagoCodigo
END;