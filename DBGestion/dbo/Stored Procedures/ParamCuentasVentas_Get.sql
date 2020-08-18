

CREATE PROCEDURE [ParamCuentasVentas_Get]
	@CuentaVentaId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @CuentaVentaId IS NOT NULL
  BEGIN
	SELECT P.CuentaVentaId,P.CuentaVentaCodigo,P.CuentaVenta,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM ParamCuentasVentas P
	WHERE P.CuentaVentaId = @CuentaVentaId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	  SELECT P.CuentaVentaId,P.CuentaVentaCodigo,P.CuentaVenta,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamCuentasVentas P
	  WHERE 
		(
			CONVERT(Varchar(10),P.CuentaVentaId) LIKE @BusquedaLike + '%'
		OR
			P.CuentaVenta LIKE @BusquedaLike + '%'
		OR
			P.CuentaVentaCodigo LIKE @BusquedaLike + '%'
		)
	  ORDER BY p.CuentaVenta
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	  SELECT P.CuentaVentaId,P.CuentaVentaCodigo,P.CuentaVenta,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamCuentasVentas P
	  WHERE 
		(
			CONVERT(Varchar(10),P.CuentaVentaId) LIKE @BusquedaLikeActivo + '%'
		OR
			P.CuentaVenta LIKE @BusquedaLikeActivo + '%'
		OR
			P.CuentaVentaCodigo LIKE @BusquedaLikeActivo + '%'
		)
	  ORDER BY p.CuentaVenta
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	  SELECT P.CuentaVentaId,P.CuentaVentaCodigo,P.CuentaVenta,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamCuentasVentas P
	  WHERE P.CuentaVenta = @Busqueda	  
  END
  
END