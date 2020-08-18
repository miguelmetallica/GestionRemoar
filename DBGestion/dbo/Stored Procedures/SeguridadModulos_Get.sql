CREATE PROCEDURE [SeguridadModulos_Get]
	@ModuloId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @ModuloId IS NOT NULL
  BEGIN
	SELECT P.ModuloId,P.Modulo,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM SeguridadModulos P
	WHERE P.ModuloId = @ModuloId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	  SELECT P.ModuloId,P.Modulo,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM SeguridadModulos P
	  WHERE 
		(
			CONVERT(Varchar(10),P.ModuloId) LIKE @BusquedaLike + '%'
		OR
			P.Modulo LIKE @BusquedaLike + '%'
		)
	  ORDER BY p.Modulo
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	  SELECT P.ModuloId,P.Modulo,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM SeguridadModulos P
	  WHERE 
		(
			CONVERT(Varchar(10),P.ModuloId) LIKE @BusquedaLikeActivo + '%'
		OR
			P.Modulo LIKE @BusquedaLikeActivo + '%'
		)
	  ORDER BY p.Modulo
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	  SELECT P.ModuloId,P.Modulo,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM SeguridadModulos P
	  WHERE P.Modulo = @Busqueda	  
  END
  
END