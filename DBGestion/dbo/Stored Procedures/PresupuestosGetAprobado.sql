CREATE PROCEDURE [dbo].[PresupuestosGetAprobado]
	@Id varchar(150) = ''
AS
BEGIN
	SELECT 
	Id,
	DetalleId,
	Codigo,
	Fecha,
	FechaVencimiento,
	DescuentoPorcentaje,
	ClienteId,
	ClienteCodigo,
	RazonSocial,
	NroDocumento,
	CuilCuit,
	TipoResponsableId,
	TipoResponsable,
	Estado,
	ProductoId,
	ProductoCodigo,
	Producto,
	Cantidad,
	Precio,
	PrecioSinImpuesto,
	SubTotal,
	SubTotalSinImpuesto,
	CantidadTotal,
	PrecioTotal,
	PrecioTotalSinImpuesto,
	ISNULL((PrecioTotal),0) - (ISNULL((PrecioTotal),0) * (ISNULL(DescuentoPorcentaje,0) / 100 ))PrecioTotalDescuento,
	ISNULL((PrecioTotalSinImpuesto),0) - (ISNULL((PrecioTotalSinImpuesto),0) * (ISNULL(DescuentoPorcentaje,0) / 100 ))PrecioTotalSinImpuestoDescuento,
	UsuarioAlta
	FROM (
		SELECT P.Id,
				ISNULL(D.Id,'') DetalleId,
				P.Codigo,
				P.Fecha,
				P.FechaVencimiento,
				ISNULL(P.DescuentoPorcentaje,0)DescuentoPorcentaje,
				P.ClienteId,
				C.Codigo ClienteCodigo,
				C.RazonSocial,
				C.NroDocumento,
				ISNULL(C.CuilCuit,'')CuilCuit,
				--ISNULL(P.TipoResponsableId,'')TipoResponsableId,
				--ISNULL(TR.Descripcion,'')TipoResponsable,
				P.TipoResponsableId TipoResponsableId,
				TR.Descripcion TipoResponsable,

				E.Descripcion Estado,
				ISNULL(D.ProductoId,'')ProductoId,
				ISNULL(PR.Codigo,'')ProductoCodigo,
				ISNULL(PR.Producto,'')Producto,
				ISNULL(D.Cantidad,0)Cantidad,
				ISNULL(D.Precio,0)Precio,
				ISNULL(D.PrecioSinImpuesto,0)PrecioSinImpuesto,
				ISNULL(D.Cantidad,0) * ISNULL(D.Precio,0)SubTotal,
				ISNULL(D.Cantidad,0) * ISNULL(D.PrecioSinImpuesto,0)SubTotalSinImpuesto,
				ISNULL((SELECT SUM(Cantidad) FROM PresupuestosDetalle PD WHERE PD.PresupuestoId = P.Id),0) CantidadTotal,
				ISNULL((SELECT SUM(Cantidad * Precio) FROM PresupuestosDetalle PD WHERE PD.PresupuestoId = P.Id),0) PrecioTotal,
				ISNULL((SELECT SUM(Cantidad * PrecioSinImpuesto) FROM PresupuestosDetalle PD WHERE PD.PresupuestoId = P.Id),0) PrecioTotalSinImpuesto,
				P.UsuarioAlta
	FROM Presupuestos P
	INNER JOIN Clientes C ON C.Id = P.ClienteId
	INNER JOIN ParamPresupuestosEstados E ON E.Id = P.EstadoId
	LEFT JOIN PresupuestosDetalle D ON D.PresupuestoId = P.Id
	LEFT JOIN Productos PR ON PR.Id = D.ProductoId
	LEFT JOIN ParamTiposResponsables TR ON TR.Id = P.TipoResponsableId
	WHERE P.Id = @Id
	AND P.Estado = 1
	AND P.EstadoId = (
						SELECT E.Id
						FROM SistemaConfiguraciones C
						INNER JOIN ParamPresupuestosEstados E ON 
						E.Codigo = C.Valor
						WHERE C.Configuracion = 'PRESUPUESTO_APROBADO_CLIENTE'
						AND C.Estado = 1
					)
	)P
	
END