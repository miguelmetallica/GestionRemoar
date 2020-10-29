CREATE PROCEDURE [dbo].[ClientesGetUno]
	 @Id varchar(150) = NULL
AS
BEGIN
	SELECT C.Id,
		C.Codigo,
		C.Apellido,
		C.Nombre,
		C.RazonSocial,
		C.TipoDocumentoId,
		T.Descripcion TipoDocumento,
		C.NroDocumento,
		C.CuilCuit,
		C.esPersonaJuridica,
		C.FechaNacimiento,
		C.ProvinciaId,
		P.Descripcion Provincia,
		C.Localidad,
		C.CodigoPostal,
		C.Calle,
		C.CalleNro,
		C.PisoDpto,
		C.OtrasReferencias,
		C.Telefono,
		C.Celular,
		C.Email,
		C.Estado,
		C.FechaAlta,
		C.UsuarioAlta
	FROM Clientes C
	LEFT JOIN ParamTiposDocumentos T ON T.Id = C.TipoDocumentoId
	LEFT JOIN ParamProvincias P ON P.Id = C.ProvinciaId
	WHERE C.Id = @Id
END