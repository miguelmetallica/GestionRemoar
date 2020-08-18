CREATE PROCEDURE [ParamImpuestos_Get]
	@ImpuestoId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @ImpuestoId IS NOT NULL
  BEGIN
	SELECT P.ImpuestoId,P.Impuesto,P.Valor,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM ParamImpuestos P
	WHERE P.ImpuestoId = @ImpuestoId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	  SELECT P.ImpuestoId,P.Impuesto,P.Valor,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamImpuestos P
	  WHERE 
		(
			CONVERT(Varchar(10),P.ImpuestoId) LIKE @BusquedaLike + '%'
		OR
			P.Impuesto LIKE @BusquedaLike + '%'
		OR
			CONVERT(Varchar(10),P.Valor) LIKE @BusquedaLike + '%'
		)
	  ORDER BY p.Impuesto
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	  SELECT P.ImpuestoId,P.Impuesto,P.Valor,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamImpuestos P
	  WHERE 
		(
			CONVERT(Varchar(10),P.ImpuestoId) LIKE @BusquedaLikeActivo + '%'
		OR
			P.Impuesto LIKE @BusquedaLikeActivo + '%'
		OR
			CONVERT(Varchar(10),P.Valor) LIKE @BusquedaLikeActivo + '%'
		)
	  ORDER BY p.Impuesto
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	  SELECT P.ImpuestoId,P.Impuesto,P.Valor,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamImpuestos P
	  WHERE P.Impuesto = @Busqueda	  
  END
  
END