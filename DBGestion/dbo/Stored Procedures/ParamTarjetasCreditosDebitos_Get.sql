
CREATE PROCEDURE [ParamTarjetasCreditosDebitos_Get]
	@TarjetaId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @TarjetaId IS NOT NULL
  BEGIN
	SELECT P.TarjetaId,P.TarjetaCodigo,P.TarjetaNombre,P.EsDebito,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM ParamTarjetasCreditosDebitos P
	WHERE P.TarjetaId = @TarjetaId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	  SELECT P.TarjetaId,P.TarjetaCodigo,P.TarjetaNombre,P.EsDebito,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamTarjetasCreditosDebitos P
	  WHERE 
		(
			CONVERT(Varchar(10),P.TarjetaId) LIKE @BusquedaLike + '%'
		OR
			P.TarjetaNombre LIKE @BusquedaLike + '%'
		OR
			P.TarjetaCodigo LIKE @BusquedaLike + '%'
		)
	  ORDER BY p.TarjetaNombre
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	  SELECT P.TarjetaId,P.TarjetaCodigo,P.TarjetaNombre,P.EsDebito,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamTarjetasCreditosDebitos P
	  WHERE 
		(
			CONVERT(Varchar(10),P.TarjetaId) LIKE @BusquedaLikeActivo + '%'
		OR
			P.TarjetaNombre LIKE @BusquedaLikeActivo + '%'
		OR
			P.TarjetaCodigo LIKE @BusquedaLikeActivo + '%'
		)
	  ORDER BY p.TarjetaNombre
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	  SELECT P.TarjetaId,P.TarjetaCodigo,P.TarjetaNombre,P.EsDebito,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamTarjetasCreditosDebitos P
	  WHERE P.TarjetaNombre = @Busqueda	  
  END
  
END