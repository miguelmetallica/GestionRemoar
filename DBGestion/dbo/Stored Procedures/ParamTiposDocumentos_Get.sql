
CREATE PROCEDURE [ParamTiposDocumentos_Get]
	@TipoDocumentoId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @TipoDocumentoId IS NOT NULL
  BEGIN
	SELECT P.TipoDocumentoId,P.TipoDocumentoCodigo,P.TipoDocumento,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM ParamTiposDocumentos P
	WHERE P.TipoDocumentoId = @TipoDocumentoId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	  SELECT P.TipoDocumentoId,P.TipoDocumentoCodigo,P.TipoDocumento,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamTiposDocumentos P
	  WHERE 
		(
			CONVERT(Varchar(10),P.TipoDocumentoId) LIKE @BusquedaLike + '%'
		OR
			P.TipoDocumento LIKE @BusquedaLike + '%'
		OR
			P.TipoDocumentoCodigo LIKE @BusquedaLike + '%'
		)
	  ORDER BY p.TipoDocumento
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	  SELECT P.TipoDocumentoId,P.TipoDocumentoCodigo,P.TipoDocumento,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamTiposDocumentos P
	  WHERE 
		(
			CONVERT(Varchar(10),P.TipoDocumentoId) LIKE @BusquedaLikeActivo + '%'
		OR
			P.TipoDocumento LIKE @BusquedaLikeActivo + '%'
		OR
			P.TipoDocumentoCodigo LIKE @BusquedaLikeActivo + '%'
		)
	  ORDER BY p.TipoDocumento
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	  SELECT P.TipoDocumentoId,P.TipoDocumentoCodigo,P.TipoDocumento,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamTiposDocumentos P
	  WHERE P.TipoDocumento = @Busqueda	  
  END
  
END