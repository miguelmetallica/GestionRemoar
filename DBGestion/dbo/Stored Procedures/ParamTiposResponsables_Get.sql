
CREATE PROCEDURE [ParamTiposResponsables_Get]
	@TipoResponsableId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @TipoResponsableId IS NOT NULL
  BEGIN
	SELECT P.TipoResponsableId,
		P.TipoResponsableCodigo,
		P.TipoResponsable,
		P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM ParamTiposResponsables P
	WHERE P.TipoResponsableId = @TipoResponsableId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	SELECT P.TipoResponsableId,
		P.TipoResponsableCodigo,
		P.TipoResponsable,
		P.Estado,P.FechaAlta,P.UsuarioAlta	
	FROM ParamTiposResponsables P
	WHERE 
		(
			CONVERT(Varchar(10),P.TipoResponsableId) LIKE @BusquedaLike + '%'
		OR
			P.TipoResponsable LIKE @BusquedaLike + '%'
		OR
			P.TipoResponsableCodigo LIKE @BusquedaLike + '%'
		)
	  ORDER BY p.TipoResponsableCodigo
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	SELECT P.TipoResponsableId,
		P.TipoResponsableCodigo,
		P.TipoResponsable,
		P.Estado,P.FechaAlta,P.UsuarioAlta	
	FROM ParamTiposResponsables P
	WHERE 
		(
			CONVERT(Varchar(10),P.TipoResponsableId) LIKE @BusquedaLikeActivo + '%'
		OR
			P.TipoResponsable LIKE @BusquedaLikeActivo + '%'
		OR
			P.TipoResponsableCodigo LIKE @BusquedaLikeActivo + '%'
		)
	  ORDER BY p.TipoResponsableCodigo
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	SELECT P.TipoResponsableId,
		P.TipoResponsableCodigo,
		P.TipoResponsable,
		P.Estado,P.FechaAlta,P.UsuarioAlta	
	FROM ParamTiposResponsables P
	WHERE P.TipoResponsable = @Busqueda	  
  END
  
END