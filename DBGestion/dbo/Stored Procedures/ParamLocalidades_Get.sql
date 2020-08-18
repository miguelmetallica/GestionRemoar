
CREATE PROCEDURE [ParamLocalidades_Get]
	@ProvinciaId int = NULL,
	@LocalidadId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @LocalidadId IS NOT NULL
  BEGIN
	SELECT P.ProvinciaId,A.Provincia,P.LocalidadId,P.LocalidadCodigo,P.Localidad,P.CodigoPostal,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM ParamLocalidades P
	INNER JOIN ParamProvincias A ON A.ProvinciaId = P.ProvinciaId
	WHERE P.LocalidadId = @LocalidadId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	  SELECT P.ProvinciaId,A.Provincia,P.LocalidadId,P.LocalidadCodigo,P.Localidad,P.CodigoPostal,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamLocalidades P
	  INNER JOIN ParamProvincias A ON A.ProvinciaId = P.ProvinciaId
	  WHERE 
		(
			CONVERT(Varchar(10),P.LocalidadId) LIKE @BusquedaLike + '%'
		OR
			P.Localidad LIKE @BusquedaLike + '%'
		OR
			P.LocalidadCodigo LIKE @BusquedaLike + '%'
		OR
			P.CodigoPostal LIKE @BusquedaLike + '%'
		)
      AND (P.ProvinciaId = @ProvinciaId OR @ProvinciaId IS NULL)
	  ORDER BY P.Localidad
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	  SELECT P.ProvinciaId,A.Provincia,P.LocalidadId,P.LocalidadCodigo,P.Localidad,P.CodigoPostal,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamLocalidades P
	  INNER JOIN ParamProvincias A ON A.ProvinciaId = P.ProvinciaId
	  WHERE 
		(
			CONVERT(Varchar(10),P.LocalidadId) LIKE @BusquedaLikeActivo + '%'
		OR
			P.Localidad LIKE @BusquedaLikeActivo + '%'
		OR
			P.LocalidadCodigo LIKE @BusquedaLikeActivo + '%'
		OR
			P.CodigoPostal LIKE @BusquedaLikeActivo + '%'
		)
		AND (P.ProvinciaId = @ProvinciaId OR @ProvinciaId IS NULL)
	  ORDER BY P.Localidad
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	  SELECT P.ProvinciaId,A.Provincia,P.LocalidadId,P.LocalidadCodigo,P.Localidad,P.CodigoPostal,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamLocalidades P
	  INNER JOIN ParamProvincias A ON A.ProvinciaId = P.ProvinciaId
	  WHERE P.Localidad = @Busqueda	  
  END
  
END