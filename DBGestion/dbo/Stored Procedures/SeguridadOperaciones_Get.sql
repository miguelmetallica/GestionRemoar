
CREATE PROCEDURE [SeguridadOperaciones_Get]
	@OperacionId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @OperacionId IS NOT NULL
  BEGIN
	SELECT P.OperacionId,P.Operacion,P.ModuloId,M.Modulo,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM SeguridadOperaciones P
	INNER JOIN SeguridadModulos M ON M.ModuloId = P.ModuloId
	WHERE P.OperacionId = @OperacionId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	  SELECT P.OperacionId,P.Operacion,P.ModuloId,M.Modulo,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM SeguridadOperaciones P
	  INNER JOIN SeguridadModulos M ON M.ModuloId = P.ModuloId
	  WHERE 
		(
			CONVERT(Varchar(10),P.OperacionId) LIKE @BusquedaLike + '%'
		OR
			P.Operacion LIKE @BusquedaLike + '%'
		OR
			M.Modulo LIKE @BusquedaLike + '%'
		)
	  ORDER BY p.Operacion
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	  SELECT P.OperacionId,P.Operacion,P.ModuloId,M.Modulo,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM SeguridadOperaciones P
	  INNER JOIN SeguridadModulos M ON M.ModuloId = P.ModuloId
	  WHERE 
		(
			CONVERT(Varchar(10),P.OperacionId) LIKE @BusquedaLikeActivo + '%'
		OR
			P.Operacion LIKE @BusquedaLikeActivo + '%'
		OR
			M.Modulo LIKE @BusquedaLike + '%'
		)
	  ORDER BY p.Operacion
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	  SELECT P.OperacionId,P.Operacion,P.ModuloId,M.Modulo,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM SeguridadOperaciones P
	  INNER JOIN SeguridadModulos M ON M.ModuloId = P.ModuloId
	  WHERE P.Operacion = @Busqueda	  
  END
  
END