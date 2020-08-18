

CREATE PROCEDURE [SeguridadUsuarios_Get]
	@UsuarioId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @UsuarioId IS NOT NULL
  BEGIN
	SELECT P.UsuarioId,P.Usuario,P.Apellido,P.Nombre,P.Telefono,P.Celular,P.Puesto,P.SucursalId,S.Sucursal,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM SeguridadUsuarios P
	INNER JOIN Sucursales S ON S.SucursalId = P.SucursalId
	WHERE P.UsuarioId = @UsuarioId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	SELECT P.UsuarioId,P.Usuario,P.Apellido,P.Nombre,P.Telefono,P.Celular,P.Puesto,P.SucursalId,S.Sucursal,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM SeguridadUsuarios P
	INNER JOIN Sucursales S ON S.SucursalId = P.SucursalId
	WHERE 
	(
		CONVERT(Varchar(10),P.UsuarioId) LIKE @BusquedaLike + '%'
	OR
		P.Usuario LIKE @BusquedaLike + '%'
	OR
		P.Apellido LIKE @BusquedaLike + '%'
	OR
		P.Nombre LIKE @BusquedaLike + '%'
	OR
		P.Puesto LIKE @BusquedaLike + '%'
	OR
		S.Sucursal LIKE @BusquedaLike + '%'
	)
	ORDER BY p.Usuario
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	SELECT P.UsuarioId,P.Usuario,P.Apellido,P.Nombre,P.Telefono,P.Celular,P.Puesto,P.SucursalId,S.Sucursal,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM SeguridadUsuarios P
	INNER JOIN Sucursales S ON S.SucursalId = P.SucursalId
	WHERE 
	(
		CONVERT(Varchar(10),P.UsuarioId) LIKE @BusquedaLikeActivo + '%'
	OR
		P.Usuario LIKE @BusquedaLike + '%'
	OR
		P.Apellido LIKE @BusquedaLike + '%'
	OR
		P.Nombre LIKE @BusquedaLike + '%'
	OR
		P.Puesto LIKE @BusquedaLike + '%'
	OR
		S.Sucursal LIKE @BusquedaLike + '%'
	)
	ORDER BY p.UsuarioId
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	SELECT P.UsuarioId,P.Usuario,P.Apellido,P.Nombre,P.Telefono,P.Celular,P.Puesto,P.SucursalId,S.Sucursal,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM SeguridadUsuarios P
	INNER JOIN Sucursales S ON S.SucursalId = P.SucursalId
	WHERE P.Usuario = @Busqueda	  
  END
  
END