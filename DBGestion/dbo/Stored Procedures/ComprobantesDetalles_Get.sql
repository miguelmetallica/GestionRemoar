

create PROCEDURE [ComprobantesDetalles_Get]
	@ComprobanteId int = NULL	
AS
BEGIN
	SELECT ComprobanteDetalleId,
		ComprobanteId,
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
		ISNULL(SubTotal,0) SubTotal,
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
		FechaAlta,
		UsuarioAlta
	FROM ComprobantesDetalles P
	WHERE P.ComprobanteId = @ComprobanteId
END