

CREATE PROCEDURE [Comprobantes_Cabecera_Get]
	@ComprobanteId int = NULL	
AS
BEGIN
	SELECT ComprobanteId,
		ISNULL(ComprobanteCodigo ,'')ComprobanteCodigo,
		ISNULL(TipoComprobanteId ,0)TipoComprobanteId,
		ISNULL(TipoComprobante,'')TipoComprobante,
		ISNULL(TipoComprobanteCodigo,'')TipoComprobanteCodigo,
		ISNULL(Letra,'')Letra,
		ISNULL(PtoVenta,0)PtoVenta,
		ISNULL(Numero,0)Numero,
		ISNULL(FechaComprobante,getdate())FechaComprobante,
		ISNULL(ConceptoIncluidoId,0)ConceptoIncluidoId,
		ISNULL(ConceptoIncluido,'')ConceptoIncluido,
		ISNULL(PeriodoFacturadoDesde,getdate())PeriodoFacturadoDesde,
		ISNULL(PeriodoFacturadoHasta,getdate())PeriodoFacturadoHasta,
		ISNULL(FechaVencimiento,getdate())FechaVencimiento,
		ISNULL(TipoResponsableId,0)TipoResponsableId,
		ISNULL(TipoResponsable,'')TipoResponsable,
		ISNULL(ClienteId,0)ClienteId,
		ISNULL(ClienteCodigo,'')ClienteCodigo,
		ISNULL(TipoDocumentoId,0)TipoDocumentoId,
		ISNULL(TipoDocumento,'')TipoDocumento,
		ISNULL(NroDocumento,'')NroDocumento,
		ISNULL(Apellido,'') + ISNULL(' ' + Nombre,'') Apellido,
		ISNULL(Nombre,'')Nombre,
		ISNULL(ProvinciaId,0)ProvinciaId,
		ISNULL(Provincia,'')Provincia,
		ISNULL(LocalidadId,0)LocalidadId,
		ISNULL(Localidad,'')Localidad,
		ISNULL(Calle,'')Calle,
		ISNULL(Nro,'')Nro,
		ISNULL(OtrasReferencias,'')OtrasReferencias,
		ISNULL(Email,'')Email,
		ISNULL(Telefono,'')Telefono,
		ISNULL(SubTotal,0)SubTotal,
		ISNULL(ImporteTributos,0)ImporteTributos,
		ISNULL((SELECT ROUND(SUM(Pa.Total),2)FROM ComprobantesFormasPagos Pa WHERE Pa.ComprobanteId = P.ComprobanteId),0) Total,
		ISNULL(Anulado,0)Anulado,
		ISNULL(FechaAnulacion,getdate())FechaAnulacion,
		ISNULL(LetraAnulado,'')LetraAnulado,
		ISNULL(PtoVtaAnulado,0)PtoVtaAnulado,
		ISNULL(NumeroAnulado,0)NumeroAnulado,
		Estado,
		FechaAlta,
		UsuarioAlta
	FROM Comprobantes P
	WHERE P.ComprobanteId = @ComprobanteId
END