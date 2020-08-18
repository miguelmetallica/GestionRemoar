

CREATE PROCEDURE [FormasPagos_Get]
	@FormaPagoId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @FormaPagoId IS NOT NULL
  BEGIN
	SELECT P.FormaPagoId,P.FormaPagoCodigo,P.FormaPago,P.Tipo,P.Estado,P.FechaDesde,P.FechaHasta,P.FechaAlta,P.UsuarioAlta
	FROM FormasPagos P
	WHERE P.FormaPagoId = @FormaPagoId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	  SELECT P.FormaPagoId,P.FormaPagoCodigo,P.FormaPago,P.Tipo,P.Estado,P.FechaDesde,P.FechaHasta,P.FechaAlta,P.UsuarioAlta
	  FROM FormasPagos P
	  WHERE 
		(
			CONVERT(Varchar(10),P.FormaPagoId) LIKE @BusquedaLike + '%'
		OR
			P.FormaPago LIKE @BusquedaLike + '%'
		OR
			P.FormaPagoCodigo LIKE @BusquedaLike + '%'
		)
	  ORDER BY p.FormaPago
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	  SELECT P.FormaPagoId,P.FormaPagoCodigo,P.FormaPago,P.Tipo,P.Estado,P.FechaDesde,P.FechaHasta,P.FechaAlta,P.UsuarioAlta
	  FROM FormasPagos P
	  WHERE 
		(
			CONVERT(Varchar(10),P.FormaPagoId) LIKE @BusquedaLikeActivo + '%'
		OR
			P.FormaPago LIKE @BusquedaLikeActivo + '%'
		OR
			P.FormaPagoCodigo LIKE @BusquedaLikeActivo + '%'
		)
	  ORDER BY p.FormaPago
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	  SELECT P.FormaPagoId,P.FormaPagoCodigo,P.FormaPago,P.Tipo,P.Estado,P.FechaDesde,P.FechaHasta,P.FechaAlta,P.UsuarioAlta
	  FROM FormasPagos P
	  WHERE P.FormaPago = @Busqueda	  
  END
  
END