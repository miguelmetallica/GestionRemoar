﻿CREATE PROCEDURE [eliasm_db].[ComprobantesImputacionGet]
	@ComprobanteId nvarchar(150)
AS
BEGIN
	declare @existe int = 0;

	IF(SELECT COUNT(*) FROM ComprobantesDetalleImputacion where RemitoId = @ComprobanteId) <> 0
	BEGIN
		SET @ComprobanteId = (SELECT ComprobanteId FROM ComprobantesDetalleImputacion where RemitoId = @ComprobanteId group by ComprobanteId)
	END

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
		ISNULL(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * P.TotalSinImpuesto,0)TotalSinImpuesto,
		ISNULL(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * P.TotalSinDescuento,0)TotalSinDescuento,
		ISNULL(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * P.TotalSinImpuestoSinDescuento,0)TotalSinImpuestoSinDescuento,
		ISNULL(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * P.DescuentoPorcentaje,0)DescuentoPorcentaje,
		ISNULL(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * P.DescuentoTotal,0)DescuentoTotal,
		ISNULL(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * P.DescuentoSinImpuesto,0)DescuentoSinImpuesto,
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
		eliasm_db.Saldo(P.PresupuestoId) Saldo,
		eliasm_db.SaldoPorcentaje(P.PresupuestoId,P.Id) Saldo_Porcentaje,
		PR.Codigo CodigoPresupuesto
		FROM Comprobantes P	
		LEFT JOIN Presupuestos PR ON PR.Id = P.PresupuestoId
		WHERE P.Codigo IS NOT NULL	
		AND P.Estado = 1
		AND P.Id IN (
					SELECT DISTINCT RemitoId 
					FROM(
						SELECT RemitoId
						FROM ComprobantesDetalleImputacion
						where comprobanteid = @ComprobanteId
						UNION ALL
						SELECT RemitoId
						FROM ComprobantesDetalleImputacionHist
						where comprobanteid = @ComprobanteId
						UNION ALL
						SELECT DevolucionId
						FROM ComprobantesDetalleImputacion
						where comprobanteid = @ComprobanteId
						UNION ALL
						SELECT DevolucionId
						FROM ComprobantesDetalleImputacionHist
						where comprobanteid = @ComprobanteId
					)Q
		)	
	Order by FechaComprobante DESC
END
;