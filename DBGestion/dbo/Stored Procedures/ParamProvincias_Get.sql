CREATE PROCEDURE [ParamProvincias_Get]
	@PaisId int = NULL,
	@ProvinciaId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @ProvinciaId IS NOT NULL
  BEGIN
	SELECT P.PaisId,A.Pais,P.ProvinciaId,P.ProvinciaCodigo,P.Provincia,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM ParamProvincias P
	INNER JOIN ParamPaises A ON A.PaisId = P.PaisId
	WHERE P.ProvinciaId = @ProvinciaId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	  SELECT P.PaisId,A.Pais,P.ProvinciaId,P.ProvinciaCodigo,P.Provincia,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamProvincias P
	  INNER JOIN ParamPaises A ON A.PaisId = P.PaisId
	  WHERE 
		(
			CONVERT(Varchar(10),P.ProvinciaId) LIKE @BusquedaLike + '%'
		OR
			P.Provincia LIKE @BusquedaLike + '%'
		OR
			P.ProvinciaCodigo LIKE @BusquedaLike + '%'
		)
      AND (P.PaisId = @PaisId OR @PaisId IS NULL)
	  ORDER BY P.Provincia
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	  SELECT P.PaisId,A.Pais,P.ProvinciaId,P.ProvinciaCodigo,P.Provincia,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamProvincias P
	  INNER JOIN ParamPaises A ON A.PaisId = P.PaisId
	  WHERE 
		(
			CONVERT(Varchar(10),P.ProvinciaId) LIKE @BusquedaLikeActivo + '%'
		OR
			P.Provincia LIKE @BusquedaLikeActivo + '%'
		OR
			P.ProvinciaCodigo LIKE @BusquedaLikeActivo + '%'
		)
		AND (P.PaisId = @PaisId OR @PaisId IS NULL)
	  ORDER BY P.Provincia
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	  SELECT P.PaisId,A.Pais,P.ProvinciaId,P.ProvinciaCodigo,P.Provincia,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamProvincias P
	  INNER JOIN ParamPaises A ON A.PaisId = P.PaisId
	  WHERE P.Provincia = @Busqueda	  
  END
  
END