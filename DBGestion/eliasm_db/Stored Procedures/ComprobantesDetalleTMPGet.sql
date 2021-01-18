create PROCEDURE [eliasm_db].[ComprobantesDetalleTMPGet]
	@ComprobanteId nvarchar(150)
AS
BEGIN
	SELECT Id,ComprobanteId,ProductoId,ProductoCodigo,ProductoNombre,Cantidad,
		UnidadMedidaId,UnidadMedidaCodigo,UnidadMedida,
		CuentaVentaId,CuentaVentaCodigo,CuentaVenta,
		PrecioUnitarioSinIva,PrecioUnitario,SubTotalSinIva,SubTotal,
		PorcentajeBonificacion,ImporteBonificacion,
		AlicuotaIvaId,AlicuotaIva,AlicuotaIvaCodigo,AlicuotaProcentaje,
		ImporteNetoNoGravado,ImporteExento,ImporteNetoGravado,
		Iva27,Iva21,Iva105,Iva5,Iva25,Iva0,
		FechaAlta,UsuarioAlta
	FROM ComprobantesDetalleTMP P
	WHERE P.ComprobanteId = @ComprobanteId
	ORDER BY P.ProductoCodigo

END