CREATE PROCEDURE [eliasm_db].[PresupuestosGetImprimir]
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
	UsuarioAlta,
	SucursalNombre,
	SucursalCalle,
	SucursalCalleNro,
	SucursalLocalidad,
	SucursalCodigoPostal,
	SucursalTelefono,
	VersionImpresion
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
				ISNULL(D.ProductoCodigo,'')ProductoCodigo,
				ISNULL(D.ProductoNombre,'')Producto,
				ISNULL(D.Cantidad,0)Cantidad,
				ISNULL(D.Precio,0)Precio,
				ISNULL(D.PrecioSinImpuesto,0)PrecioSinImpuesto,
				ISNULL(D.Cantidad,0) * ISNULL(D.Precio,0)SubTotal,
				ISNULL(D.Cantidad,0) * ISNULL(D.PrecioSinImpuesto,0)SubTotalSinImpuesto,
				ISNULL((SELECT SUM(Cantidad) FROM PresupuestosDetalle PD WHERE PD.PresupuestoId = P.Id),0) CantidadTotal,
				ISNULL((SELECT SUM(Cantidad * Precio) FROM PresupuestosDetalle PD WHERE PD.PresupuestoId = P.Id),0) PrecioTotal,
				ISNULL((SELECT SUM(Cantidad * PrecioSinImpuesto) FROM PresupuestosDetalle PD WHERE PD.PresupuestoId = P.Id),0) PrecioTotalSinImpuesto,
				P.UsuarioAlta,
				S.Nombre SucursalNombre,
				S.Calle SucursalCalle,
				S.CalleNro SucursalCalleNro,
				S.Localidad SucursalLocalidad,
				S.CodigoPostal SucursalCodigoPostal,
				S.Telefono SucursalTelefono,
				(
					SELECT RIGHT('0000'+ CONVERT(VARCHAR(4),DATEPART(YEAR,MAX(Fecha))),4) + RIGHT('00'+ CONVERT(VARCHAR(2),DATEPART(MONTH,MAX(Fecha))),2) + RIGHT('00'+ CONVERT(VARCHAR(2),DATEPART(DAY,MAX(Fecha))),2) + RIGHT('00'+ CONVERT(VARCHAR(2),DATEPART(HOUR,MAX(Fecha))),2) + RIGHT('00'+ CONVERT(VARCHAR(2),DATEPART(MINUTE,MAX(Fecha))),2) + RIGHT('00'+ CONVERT(VARCHAR(2),DATEPART(SECOND,MAX(Fecha))),2)
					FROM (
						SELECT MAX(FechaAlta)Fecha FROM Presupuestos WHERE Id = @Id
						UNION ALL
						SELECT MAX(FechaAlta) FROM PresupuestosDetalle WHERE PresupuestoId = @Id
						UNION ALL
						SELECT MAX(FechaEdit)Fecha FROM Presupuestos WHERE Id = @Id
						UNION ALL
						SELECT MAX(FechaEdit) FROM PresupuestosDetalle WHERE PresupuestoId = @Id
					)P
				)VersionImpresion
		FROM Presupuestos P
		INNER JOIN Clientes C ON C.Id = P.ClienteId
		INNER JOIN ParamPresupuestosEstados E ON E.Id = P.EstadoId
		LEFT JOIN PresupuestosDetalle D ON D.PresupuestoId = P.Id
		--LEFT JOIN Productos PR ON PR.Id = D.ProductoId
		LEFT JOIN ParamTiposResponsables TR ON TR.Id = P.TipoResponsableId
		LEFT JOIN AspNetUsers U ON U.UserName = P.UsuarioAlta
		LEFT JOIN Sucursales S ON S.Id = U.SucursalId
		WHERE P.Id = @Id
		AND P.Estado = 1	
	)P
	
END