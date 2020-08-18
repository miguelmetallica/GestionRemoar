

CREATE PROCEDURE [ParamUnidadesMedidas_Get]
	@UnidadMedidaId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @UnidadMedidaId IS NOT NULL
  BEGIN
	SELECT P.UnidadMedidaId,P.UnidadMedidaCodigo,P.UnidadMedida,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM ParamUnidadesMedidas P
	WHERE P.UnidadMedidaId = @UnidadMedidaId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	  SELECT P.UnidadMedidaId,P.UnidadMedidaCodigo,P.UnidadMedida,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamUnidadesMedidas P
	  WHERE 
		(
			CONVERT(Varchar(10),P.UnidadMedidaId) LIKE @BusquedaLike + '%'
		OR
			P.UnidadMedida LIKE @BusquedaLike + '%'
		OR
			P.UnidadMedidaCodigo LIKE @BusquedaLike + '%'
		)
	  ORDER BY p.UnidadMedida
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	  SELECT P.UnidadMedidaId,P.UnidadMedidaCodigo,P.UnidadMedida,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamUnidadesMedidas P
	  WHERE 
		(
			CONVERT(Varchar(10),P.UnidadMedidaId) LIKE @BusquedaLikeActivo + '%'
		OR
			P.UnidadMedida LIKE @BusquedaLikeActivo + '%'
		OR
			P.UnidadMedidaCodigo LIKE @BusquedaLikeActivo + '%'
		)
	  ORDER BY p.UnidadMedida
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	  SELECT P.UnidadMedidaId,P.UnidadMedidaCodigo,P.UnidadMedida,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamUnidadesMedidas P
	  WHERE P.UnidadMedida = @Busqueda	  
  END
  
END