
CREATE PROCEDURE [Clientes_Get]
	@ClienteId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @ClienteId IS NOT NULL
  BEGIN
	SELECT C.ClienteId,C.CodigoCliente,ISNULL(C.Foto,'')Foto,C.Apellido,C.Nombre,
			C.TipoDocumentoId,D.TipoDocumento,C.NroDocumento,
			C.FechaNacimiento,C.ProvinciaId,ISNULL(P.Provincia,'')Provincia,
			C.LocalidadId,ISNULL(L.Localidad,'')Localidad,C.Calle,C.CalleNro,
			C.OtrasReferencias,C.Telefono,C.Celular,
			C.Email,C.Facebook,C.Instagram,C.Twitter,
			C.Estado,C.FechaAlta,C.UsuarioAlta,CASE WHEN C.ESTADO = 1 THEN 'ACTIVO' ELSE 'INACTIVO' END ObservacionesEstado
	FROM Clientes C
	INNER JOIN ParamTiposDocumentos D ON D.TipoDocumentoId = C.TipoDocumentoId
	LEFT JOIN ParamProvincias P ON P.ProvinciaId = C.ProvinciaId
	LEFT JOIN ParamLocalidades L ON L.LocalidadId = C.LocalidadId
	WHERE C.ClienteId = @ClienteId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	SELECT C.ClienteId,C.CodigoCliente,ISNULL(C.Foto,'')Foto,C.Apellido,C.Nombre,
			C.TipoDocumentoId,D.TipoDocumento,C.NroDocumento,
			C.FechaNacimiento,C.ProvinciaId,ISNULL(P.Provincia,'')Provincia,
			C.LocalidadId,L.Localidad,C.Calle,C.CalleNro,
			C.OtrasReferencias,C.Telefono,C.Celular,
			C.Email,C.Facebook,C.Instagram,C.Twitter,
			C.Estado,C.FechaAlta,C.UsuarioAlta,CASE WHEN C.ESTADO = 1 THEN 'ACTIVO' ELSE 'INACTIVO' END ObservacionesEstado
	FROM Clientes C
	INNER JOIN ParamTiposDocumentos D ON D.TipoDocumentoId = C.TipoDocumentoId
	LEFT JOIN ParamProvincias P ON P.ProvinciaId = C.ProvinciaId
	LEFT JOIN ParamLocalidades L ON L.LocalidadId = C.LocalidadId
    WHERE 
		(
			C.CodigoCliente LIKE @BusquedaLike + '%'
		OR
			C.Apellido LIKE @BusquedaLike + '%'
		OR
			C.Nombre LIKE @BusquedaLike + '%'
		OR
			C.NroDocumento LIKE @BusquedaLike + '%'
		)
	  ORDER BY C.CodigoCliente
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	SELECT C.ClienteId,C.CodigoCliente,ISNULL(C.Foto,'')Foto,C.Apellido,C.Nombre,
			C.TipoDocumentoId,D.TipoDocumento,C.NroDocumento,
			C.FechaNacimiento,C.ProvinciaId,ISNULL(P.Provincia,'')Provincia,
			C.LocalidadId,ISNULL(L.Localidad,'')Localidad,C.Calle,C.CalleNro,
			C.OtrasReferencias,C.Telefono,C.Celular,
			C.Email,C.Facebook,C.Instagram,C.Twitter,
			C.Estado,C.FechaAlta,C.UsuarioAlta,CASE WHEN C.ESTADO = 1 THEN 'ACTIVO' ELSE 'INACTIVO' END ObservacionesEstado
	FROM Clientes C
	INNER JOIN ParamTiposDocumentos D ON D.TipoDocumentoId = C.TipoDocumentoId
	LEFT JOIN ParamProvincias P ON P.ProvinciaId = C.ProvinciaId
	LEFT JOIN ParamLocalidades L ON L.LocalidadId = C.LocalidadId
    WHERE 
		(
			C.CodigoCliente LIKE @BusquedaLikeActivo + '%'
		OR
			C.Apellido LIKE @BusquedaLikeActivo + '%'
		OR
			C.Nombre LIKE @BusquedaLikeActivo + '%'
		OR
			C.NroDocumento LIKE @BusquedaLikeActivo + '%'
		)	  
	  ORDER BY C.CodigoCliente
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	SELECT C.ClienteId,C.CodigoCliente,ISNULL(C.Foto,'')Foto,C.Apellido,C.Nombre,
		C.TipoDocumentoId,D.TipoDocumento,C.NroDocumento,
		C.FechaNacimiento,C.ProvinciaId,ISNULL(P.Provincia,'')Provincia,
		C.LocalidadId,ISNULL(L.Localidad,'')Localidad,C.Calle,C.CalleNro,
		C.OtrasReferencias,C.Telefono,C.Celular,
		C.Email,C.Facebook,C.Instagram,C.Twitter,
		C.Estado,C.FechaAlta,C.UsuarioAlta,CASE WHEN C.ESTADO = 1 THEN 'ACTIVO' ELSE 'INACTIVO' END ObservacionesEstado
	FROM Clientes C
	INNER JOIN ParamTiposDocumentos D ON D.TipoDocumentoId = C.TipoDocumentoId
	LEFT JOIN ParamProvincias P ON P.ProvinciaId = C.ProvinciaId
	LEFT JOIN ParamLocalidades L ON L.LocalidadId = C.LocalidadId
    WHERE C.CodigoCliente = @Busqueda	  
  END
  
END