CREATE PROCEDURE [eliasm_db].[ComprobantesPrespuestosEntregaGet]
AS
BEGIN
	SELECT 
		P.Id,
		P.Codigo,
		P.TipoComprobanteId,
		P.TipoComprobante,
		P.TipoComprobanteCodigo,
		ISNULL(P.TipoComprobanteEsDebe,0)TipoComprobanteEsDebe,
		P.PresupuestoId,
		P.Letra,
		P.PtoVenta,
		P.Numero,
		P.FechaComprobante,
		P.ConceptoIncluidoId,
		P.ConceptoIncluidoCodigo,
		P.ConceptoIncluido,
		P.PeriodoFacturadoDesde,
		P.PeriodoFacturadoHasta,
		P.FechaVencimiento,
		P.TipoResponsableId,
		P.TipoResponsableCodigo,
		P.TipoResponsable,
		P.ClienteId,
		P.ClienteCodigo,
		P.TipoDocumentoId,
		P.TipoDocumentoCodigo,
		P.TipoDocumento,
		P.NroDocumento,
		P.CuilCuit,
		P.RazonSocial,
		P.ProvinciaId,
		P.ProvinciaCodigo,
		P.Provincia,
		P.Localidad,
		P.CodigoPostal,
		P.Calle,
		P.CalleNro,
		P.PisoDpto,
		P.OtrasReferencias,
		P.Email,
		P.Telefono,
		P.Celular,
		ISNULL(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * P.Total,0) Total,
		ISNULL(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * P.TotalLista,0)TotalLista,
		ISNULL(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * P.TotalSinImpuesto,0)TotalSinImpuesto,
		ISNULL(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * P.TotalListaSinImpuesto,0)TotalListaSinImpuesto,
		ISNULL(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * P.TotalSinDescuento,0)TotalSinDescuento,
		ISNULL(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * P.TotalListaSinDescuento,0)TotalSinDescuento,
		ISNULL(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * P.TotalSinImpuestoSinDescuento,0)TotalSinImpuestoSinDescuento,
		ISNULL(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * P.TotalListaSinImpuestoSinDescuento,0)TotalListaSinImpuestoSinDescuento,
		ISNULL(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * P.DescuentoPorcentaje,0)DescuentoPorcentaje,
		ISNULL(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * P.DescuentoTotal,0)DescuentoTotal,
		ISNULL(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * P.DescuentoListaTotal,0)DescuentoListaTotal,
		ISNULL(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * P.DescuentoSinImpuesto,0)DescuentoSinImpuesto,
		ISNULL(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * P.DescuentoListaSinImpuesto,0)DescuentoListaSinImpuesto,
		ISNULL(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * P.ImporteTributos,0)ImporteTributos,
		P.Observaciones,
		P.Confirmado,
		P.Cobrado,
		P.ComprobanteAnulaId,
		P.Anulado,
		P.FechaAnulacion,
		P.TipoComprobanteAnulaId,
		P.TipoComprobanteAnulaCodigo,
		P.TipoComprobanteAnula,
		P.CodigoAnula,
		P.LetraAnula,
		P.PtoVtaAnula,
		P.NumeroAnula,
		P.UsuarioAnula,
		P.TipoComprobanteFiscal,
		P.LetraFiscal,
		P.PtoVentaFiscal,
		P.NumeroFiscal,
		P.Estado,
		P.FechaAlta,
		P.UsuarioAlta,
		(
			SELECT SUM(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * Total)Importe	
			FROM Comprobantes p1
			WHERE P1.PresupuestoId = P.PresupuestoId
		)Saldo,
		CASE WHEN P.Total <> 0 THEN CONVERT(NUMERIC(18,2),(
			SELECT SUM(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * Total)Importe	
			FROM Comprobantes p1
			WHERE P1.PresupuestoId = P.PresupuestoId
		) * 100 / P.Total) 
		ELSE 0 END	Saldo_Porcentaje,
		PR.Codigo CodigoPresupuesto
	FROM Comprobantes P		
	INNER JOIN Presupuestos PR ON PR.Id = P.PresupuestoId
	WHERE P.Codigo IS NOT NULL
	AND P.PresupuestoId IS NOT NULL
	AND P.Estado = 1
	AND EXISTS(
			SELECT 1
			FROM ComprobantesDetalleImputacion P1
			WHERE P1.ComprobanteId = P.Id
			AND P1.Estado = 1 AND (P1.Entrega = 1 OR P1.AutorizaEntrega = 1)
			)
	Order By FechaComprobante desc
END
;