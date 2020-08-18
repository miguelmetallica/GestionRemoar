
create PROCEDURE [ParamTiposComprobantes_Get]
	@TipoComprobanteId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @TipoComprobanteId IS NOT NULL
  BEGIN
	SELECT P.TipoComprobanteId,
		P.TipoComprobanteCodigo,
		P.TipoComprobante,
		P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM ParamTiposComprobantes P
	WHERE P.TipoComprobanteId = @TipoComprobanteId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	SELECT P.TipoComprobanteId,
		P.TipoComprobanteCodigo,
		P.TipoComprobante,
		P.Estado,P.FechaAlta,P.UsuarioAlta	
	FROM ParamTiposComprobantes P
	WHERE 
		(
			CONVERT(Varchar(10),P.TipoComprobanteId) LIKE @BusquedaLike + '%'
		OR
			P.TipoComprobante LIKE @BusquedaLike + '%'
		OR
			P.TipoComprobanteCodigo LIKE @BusquedaLike + '%'
		)
	  ORDER BY p.TipoComprobante
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	SELECT P.TipoComprobanteId,
		P.TipoComprobanteCodigo,
		P.TipoComprobante,
		P.Estado,P.FechaAlta,P.UsuarioAlta	
	FROM ParamTiposComprobantes P
	WHERE 
		(
			CONVERT(Varchar(10),P.TipoComprobanteId) LIKE @BusquedaLikeActivo + '%'
		OR
			P.TipoComprobante LIKE @BusquedaLikeActivo + '%'
		OR
			P.TipoComprobanteCodigo LIKE @BusquedaLikeActivo + '%'
		)
	  ORDER BY p.TipoComprobante
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	SELECT P.TipoComprobanteId,
		P.TipoComprobanteCodigo,
		P.TipoComprobante,
		P.Estado,P.FechaAlta,P.UsuarioAlta	
	FROM ParamTiposComprobantes P
	WHERE P.TipoComprobante = @Busqueda	  
  END
  
END