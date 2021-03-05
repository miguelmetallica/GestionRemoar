CREATE PROCEDURE [eliasm_db].[ComprobantesPresupuestoGet]
	@Id nvarchar(150) = ''
AS
BEGIN
	DECLARE @InteresFormaPago NUMERIC(18,2) = 0
	SELECT TOP 1 @InteresFormaPago = abs(P.Interes)
	FROM FormasPagosCuotas P
	WHERE P.FormaPagoId = '6DB16344-8DB9-4F81-903D-50913E53FE86'
	AND P.Cuota = 1
	AND CONVERT(DATE,GETDATE()) BETWEEN P.FechaDesde AND P.FechaHasta
	AND P.Estado = 1

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
		ISNULL(P.Total,0) Total,
		ISNULL(P.TotalSinImpuesto,0)TotalSinImpuesto,
		ISNULL(P.TotalSinDescuento,0)TotalSinDescuento,
		ISNULL(P.TotalSinImpuestoSinDescuento,0)TotalSinImpuestoSinDescuento,
		ISNULL(P.DescuentoPorcentaje,0)DescuentoPorcentaje,
		ISNULL(P.DescuentoTotal,0)DescuentoTotal,
		ISNULL(P.DescuentoSinImpuesto,0)DescuentoSinImpuesto,
		ISNULL(P.ImporteTributos,0)ImporteTributos,
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
		convert(numeric(18,2),eliasm_db.Saldo(P.PresupuestoId) - eliasm_db.Saldo(P.PresupuestoId) * (@InteresFormaPago/ 100)) SaldoContado,
		@InteresFormaPago InteresFormaPago,
		eliasm_db.SaldoPorcentaje(P.PresupuestoId,P.Id) Saldo_Porcentaje,
	PR.Codigo CodigoPresupuesto
	FROM Comprobantes P
	INNER JOIN Presupuestos PR ON PR.Id = P.PresupuestoId
	WHERE P.PresupuestoId = @Id
	AND P.Codigo IS NOT NULL	
	AND P.Estado = 1
	Order by P.FechaComprobante

END
;