


CREATE PROCEDURE [SeguridadRoles_Get]
	@RolId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @RolId IS NOT NULL
  BEGIN
	SELECT P.RolId,P.Rol,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM SeguridadRoles P
	WHERE P.RolId = @RolId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	  SELECT P.RolId,P.Rol,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM SeguridadRoles P
	  WHERE 
		(
			CONVERT(Varchar(10),P.RolId) LIKE @BusquedaLike + '%'
		OR
			P.Rol LIKE @BusquedaLike + '%'
		)
	  ORDER BY p.Rol
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	  SELECT P.RolId,P.Rol,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM SeguridadRoles P
	  WHERE 
		(
			CONVERT(Varchar(10),P.RolId) LIKE @BusquedaLikeActivo + '%'
		OR
			P.Rol LIKE @BusquedaLikeActivo + '%'
		)
	  ORDER BY p.Rol
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	  SELECT P.RolId,P.Rol,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM SeguridadRoles P
	  WHERE P.Rol = @Busqueda	  
  END
  
END