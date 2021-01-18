CREATE PROCEDURE [eliasm_db].[ClientesGetUno]
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
		ISNULL(C.TipoResponsableId,R1.Id)TipoResponsableId,
		ISNULL(R.Descripcion,R1.Descripcion) TipoResponsable,
		ISNULL(C.CategoriaId,CA1.Id)CategoriaId,
		ISNULL(CA.Descripcion,CA1.Descripcion) Categoria,
		C.Estado,
		C.FechaAlta,
		C.UsuarioAlta
	FROM Clientes C
	LEFT JOIN ParamTiposDocumentos T ON T.Id = C.TipoDocumentoId
	LEFT JOIN ParamProvincias P ON P.Id = C.ProvinciaId
	LEFT JOIN ParamTiposResponsables R ON R.Id = C.TipoResponsableId
	LEFT JOIN ParamTiposResponsables R1 ON R1.Defecto = 1
	LEFT JOIN ParamClientesCategorias CA ON CA.Id = C.CategoriaId
	LEFT JOIN ParamClientesCategorias CA1 ON CA1.Defecto = 1
	WHERE C.Id = @Id
END