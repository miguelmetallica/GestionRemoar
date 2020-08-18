

CREATE PROCEDURE [Sucursales_Get]
	@SucursalId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @SucursalId IS NOT NULL
  BEGIN
	SELECT P.SucursalId,P.Sucursal,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM Sucursales P
	WHERE P.SucursalId = @SucursalId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	  SELECT P.SucursalId,P.Sucursal,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM Sucursales P
	  WHERE 
		(
			CONVERT(Varchar(10),P.SucursalId) LIKE @BusquedaLike + '%'
		OR
			P.Sucursal LIKE @BusquedaLike + '%'
		)
	  ORDER BY p.Sucursal
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	  SELECT P.SucursalId,P.Sucursal,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM Sucursales P
	  WHERE 
		(
			CONVERT(Varchar(10),P.SucursalId) LIKE @BusquedaLikeActivo + '%'
		OR
			P.Sucursal LIKE @BusquedaLikeActivo + '%'
		)
	  ORDER BY p.Sucursal
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	  SELECT P.SucursalId,P.Sucursal,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM Sucursales P
	  WHERE P.Sucursal = @Busqueda	  
  END
  
END