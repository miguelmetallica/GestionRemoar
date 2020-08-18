
CREATE PROCEDURE [ParamBancos_Get]
	@BancoId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @BancoId IS NOT NULL
  BEGIN
	SELECT P.BancoId,P.BancoCodigo,P.Banco,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM ParamBancos P
	WHERE P.BancoId = @BancoId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	  SELECT P.BancoId,P.BancoCodigo,P.Banco,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamBancos P
	  WHERE 
		(
			CONVERT(Varchar(10),P.BancoId) LIKE @BusquedaLike + '%'
		OR
			P.Banco LIKE @BusquedaLike + '%'
		OR
			P.BancoCodigo LIKE @BusquedaLike + '%'
		)
	  ORDER BY p.Banco
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	  SELECT P.BancoId,P.BancoCodigo,P.Banco,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamBancos P
	  WHERE 
		(
			CONVERT(Varchar(10),P.BancoId) LIKE @BusquedaLikeActivo + '%'
		OR
			P.Banco LIKE @BusquedaLikeActivo + '%'
		OR
			P.BancoCodigo LIKE @BusquedaLikeActivo + '%'
		)
	  ORDER BY p.Banco
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	  SELECT P.BancoId,P.BancoCodigo,P.Banco,P.Estado,P.FechaAlta,P.UsuarioAlta
	  FROM ParamBancos P
	  WHERE P.Banco = @Busqueda	  
  END
  
END