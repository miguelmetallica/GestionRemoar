CREATE PROCEDURE [eliasm_db].[FormasPagosCuotasEntidadesGet]
	@FormaPagoId nvarchar(150)
AS
BEGIN
	SELECT E.Id,E.Codigo,E.Descripcion,E.Estado,E.FechaAlta,E.UsuarioAlta
	FROM FormasPagosCuotas P
	INNER JOIN ParamEntidades E ON E.Id = P.EntidadId
	WHERE P.FormaPagoId = @FormaPagoId
	AND CONVERT(DATE,GETDATE()) BETWEEN P.FechaDesde AND P.FechaHasta
	AND P.Estado = 1
	GROUP BY E.Id,E.Codigo,E.Descripcion,E.Estado,E.FechaAlta,E.UsuarioAlta
	Order by E.Descripcion
END;