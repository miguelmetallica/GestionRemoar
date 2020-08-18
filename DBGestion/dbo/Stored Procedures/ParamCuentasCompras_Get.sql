

CREATE PROCEDURE [ParamCuentasCompras_Get]
	@CuentaCompraId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @CuentaCompraId IS NOT NULL
  BEGIN
	SELECT P.CuentaCompraId,P.CuentaCompraCodigo,P.CuentaCompra,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM ParamCuentasCompras P
	WHERE P.CuentaCompraId = @CuentaCompraId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	  SELECT P.CuentaCompraId,P.CuentaCompraCodigo,P.CuentaCompra,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamCuentasCompras P
	  WHERE 
		(
			CONVERT(Varchar(10),P.CuentaCompraId) LIKE @BusquedaLike + '%'
		OR
			P.CuentaCompra LIKE @BusquedaLike + '%'
		OR
			P.CuentaCompraCodigo LIKE @BusquedaLike + '%'
		)
	  ORDER BY p.CuentaCompra
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	  SELECT P.CuentaCompraId,P.CuentaCompraCodigo,P.CuentaCompra,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamCuentasCompras P
	  WHERE 
		(
			CONVERT(Varchar(10),P.CuentaCompraId) LIKE @BusquedaLikeActivo + '%'
		OR
			P.CuentaCompra LIKE @BusquedaLikeActivo + '%'
		OR
			P.CuentaCompraCodigo LIKE @BusquedaLikeActivo + '%'
		)
	  ORDER BY p.CuentaCompra
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	  SELECT P.CuentaCompraId,P.CuentaCompraCodigo,P.CuentaCompra,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamCuentasCompras P
	  WHERE P.CuentaCompra = @Busqueda	  
  END
  
END