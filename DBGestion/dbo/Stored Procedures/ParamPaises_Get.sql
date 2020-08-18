
CREATE PROCEDURE [ParamPaises_Get]
	@PaisId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @PaisId IS NOT NULL
  BEGIN
	SELECT P.PaisId,P.PaisCodigo,P.Pais,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM ParamPaises P
	WHERE P.PaisId = @PaisId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	  SELECT P.PaisId,P.PaisCodigo,P.Pais,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamPaises P
	  WHERE 
		(
			CONVERT(Varchar(10),P.PaisId) LIKE @BusquedaLike + '%'
		OR
			P.Pais LIKE @BusquedaLike + '%'
		OR
			P.PaisCodigo LIKE @BusquedaLike + '%'
		)
	  ORDER BY p.Pais
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	  SELECT P.PaisId,P.PaisCodigo,P.Pais,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamPaises P
	  WHERE 
		(
			CONVERT(Varchar(10),P.PaisId) LIKE @BusquedaLikeActivo + '%'
		OR
			P.Pais LIKE @BusquedaLikeActivo + '%'
		OR
			P.PaisCodigo LIKE @BusquedaLikeActivo + '%'
		)
	  ORDER BY p.Pais
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	  SELECT P.PaisId,P.PaisCodigo,P.Pais,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamPaises P
	  WHERE P.Pais = @Busqueda	  
  END
  
END