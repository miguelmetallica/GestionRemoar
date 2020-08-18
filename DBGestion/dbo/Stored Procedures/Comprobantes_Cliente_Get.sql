

CREATE PROCEDURE [Comprobantes_Cliente_Get]
	@ComprobanteId int = NULL	
AS
BEGIN
	SELECT isnull(ClienteId,0)ClienteId,
		isnull(ClienteCodigo,'')ClienteCodigo,
		isnull(TipoDocumentoId,0)TipoDocumentoId,
		isnull(TipoDocumento,'')TipoDocumento,
		isnull(NroDocumento,'')NroDocumento,
		isnull(Apellido,'')Apellido,
		isnull(Nombre,'')Nombre,
		isnull(ProvinciaId,0)ProvinciaId,
		isnull(Provincia,'')Provincia,
		isnull(LocalidadId,0)LocalidadId,
		isnull(Localidad,'')Localidad,
		isnull(Calle,'')Calle,
		isnull(Nro,'')Nro,
		isnull(OtrasReferencias,'')OtrasReferencias,
		isnull(Email,'')Email,
		isnull(Telefono,'')Telefono
	FROM Comprobantes P
	WHERE P.ComprobanteId = @ComprobanteId
END