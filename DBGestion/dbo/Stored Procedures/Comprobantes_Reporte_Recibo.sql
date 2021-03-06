﻿create PROCEDURE [Comprobantes_Reporte_Recibo]
	@ComprobanteId int = NULL	
AS
BEGIN
	SELECT P.ComprobanteId,
		P.ComprobanteCodigo,
		P.TipoComprobanteId,
		P.TipoComprobante,
		P.TipoComprobanteCodigo,
		ISNULL(Letra,'') Letra,
		ISNULL(PtoVenta,0) PtoVenta,
		ISNULL(Numero,0) Numero,
		P.FechaComprobante,
		P.ConceptoIncluidoId,
		P.ConceptoIncluido,
		P.PeriodoFacturadoDesde,
		P.PeriodoFacturadoHasta,
		P.FechaVencimiento,
		P.TipoResponsableId,
		P.TipoResponsable,
		P.Estado,
		ComprobanteDetalleId,
		ProductoId,
		ISNULL(ProductoCodigo,'') ProductoCodigo,
		ISNULL(ProductoNombre,'') ProductoNombre,
		ISNULL(Cantidad,0) Cantidad,
		ISNULL(UnidadMedidaId,0) UnidadMedidaId,
		ISNULL(UnidadMedidaCodigo,'') UnidadMedidaCodigo,
		ISNULL(UnidadMedida,'') UnidadMedida,
		ISNULL(CuentaVentaId,0) CuentaVentaId,
		ISNULL(CuentaVentaCodigo,'') CuentaVentaCodigo,
		ISNULL(CuentaVenta,'') CuentaVenta,
		ISNULL(PrecioUnitarioSinIva,0) PrecioUnitarioSinIva,
		ISNULL(PrecioUnitario,0) PrecioUnitario,
		ISNULL(SubTotalSinIva,0) SubTotalSinIva,
		ISNULL(d.SubTotal,0) mSubTotalProducto,
		ISNULL(PorcentajeBonificacion,0) PorcentajeBonificacion,
		ISNULL(ImporteBonificacion,0) ImporteBonificacion,
		ISNULL(AlicuotaIvaId,0) AlicuotaIvaId,
		ISNULL(AlicuotaIva,'') AlicuotaIva,
		ISNULL(AlicuotaProcentaje,0) AlicuotaProcentaje,
		ISNULL(ImporteNetoNoGravado,0) ImporteNetoNoGravado,
		ISNULL(ImporteExento,0) ImporteExento,
		ISNULL(ImporteNetoGravado,0) ImporteNetoGravado,
		ISNULL(Iva27,0) Iva27,
		ISNULL(Iva21,0) Iva21,
		ISNULL(Iva105,0) Iva105,
		ISNULL(Iva5,0) Iva5,
		ISNULL(Iva25,0) Iva25,
		ISNULL(Iva0,0) Iva0,

		ComprobanteFormaPagoId,
		ISNULL(FormaPagoId,0)FormaPagoId,
		ISNULL(FormaPago,'')FormaPago,
		ISNULL(Importe,0)Importe,
		ISNULL(ImporteInteres,0)ImporteInteres,
		ISNULL(Cuota,0)Cuota,
		ISNULL(F.Total,0)TotalFormaPago,
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

		P.FechaAlta,
		P.UsuarioAlta
	FROM Comprobantes P
	INNER JOIN ComprobantesDetalles D ON D.ComprobanteId = P.ComprobanteId
	INNER JOIN ComprobantesFormasPagos F ON F.ComprobanteId = P.ComprobanteId
	WHERE P.ComprobanteId = @ComprobanteId
END