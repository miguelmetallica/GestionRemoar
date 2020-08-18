

CREATE PROCEDURE [ParamConceptosIncluidos_Get]
	@ConceptoIncluidoId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @ConceptoIncluidoId IS NOT NULL
  BEGIN
	SELECT P.ConceptoIncluidoId,P.ConceptoIncluidoCodigo,P.ConceptoIncluido,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM ParamConceptosIncluidos P
	WHERE P.ConceptoIncluidoId = @ConceptoIncluidoId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	  SELECT P.ConceptoIncluidoId,P.ConceptoIncluidoCodigo,P.ConceptoIncluido,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamConceptosIncluidos P
	  WHERE 
		(
			CONVERT(Varchar(10),P.ConceptoIncluidoId) LIKE @BusquedaLike + '%'
		OR
			P.ConceptoIncluido LIKE @BusquedaLike + '%'
		OR
			P.ConceptoIncluidoCodigo LIKE @BusquedaLike + '%'
		)
	  ORDER BY p.ConceptoIncluido
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	  SELECT P.ConceptoIncluidoId,P.ConceptoIncluidoCodigo,P.ConceptoIncluido,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamConceptosIncluidos P
	  WHERE 
		(
			CONVERT(Varchar(10),P.ConceptoIncluidoId) LIKE @BusquedaLikeActivo + '%'
		OR
			P.ConceptoIncluido LIKE @BusquedaLikeActivo + '%'
		OR
			P.ConceptoIncluidoCodigo LIKE @BusquedaLikeActivo + '%'
		)
	  ORDER BY p.ConceptoIncluido
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	  SELECT P.ConceptoIncluidoId,P.ConceptoIncluidoCodigo,P.ConceptoIncluido,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamConceptosIncluidos P
	  WHERE P.ConceptoIncluido = @Busqueda	  
  END
  
END