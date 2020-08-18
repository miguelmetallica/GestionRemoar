

CREATE PROCEDURE [FormasPagosCuotas_Get]
	@FormaPagoId int = NULL,
	@FormaPagoCuotaId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @FormaPagoCuotaId IS NOT NULL
  BEGIN
	SELECT P.FormaPagoCuotaId,P.FormaPagoId,P.Cuota,P.Porcentaje,P.Descuento,P.FechaDesde,P.FechaHasta,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM FormasPagosCuotas P
	WHERE P.FormaPagoCuotaId = @FormaPagoCuotaId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	  SELECT P.FormaPagoCuotaId,P.FormaPagoId,P.Cuota,P.Porcentaje,P.Descuento,P.FechaDesde,P.FechaHasta,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM FormasPagosCuotas P
	  WHERE P.FormaPagoId = @FormaPagoId
	  AND
		(
			CONVERT(Varchar(10),P.FormaPagoCuotaId) LIKE @BusquedaLike + '%'
		OR
			P.Cuota LIKE @BusquedaLike + '%'
		)
	  ORDER BY P.Cuota
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	  SELECT P.FormaPagoCuotaId,P.FormaPagoId,P.Cuota,P.Porcentaje,P.Descuento,P.FechaDesde,P.FechaHasta,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM FormasPagosCuotas P
	  WHERE P.FormaPagoId = @FormaPagoId
	  AND
		(
			CONVERT(Varchar(10),P.FormaPagoCuotaId) LIKE @BusquedaLike + '%'
		OR
			P.Cuota LIKE @BusquedaLike + '%'
		)
	  ORDER BY P.Cuota
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	  SELECT P.FormaPagoCuotaId,P.FormaPagoId,P.Cuota,P.Porcentaje,P.Descuento,P.FechaDesde,P.FechaHasta,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM FormasPagosCuotas P
	  WHERE P.FormaPagoId = @FormaPagoId AND P.Cuota = @Busqueda	  
  END
  
END