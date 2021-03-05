CREATE PROCEDURE [eliasm_db].[FormasPagosEntidadesCuotasGet]
	@FormaPagoId nvarchar(150) = null,
	@EntidadId nvarchar(150) = null
AS
BEGIN
	SELECT P.Id,P.FormaPagoId,P.EntidadId,P.Descripcion,P.Cuota,P.Interes,P.FechaDesde,P.FechaHasta,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM FormasPagosCuotas P
	WHERE P.FormaPagoId = @FormaPagoId
	AND P.EntidadId = @EntidadId
	AND CONVERT(DATE,GETDATE()) BETWEEN P.FechaDesde AND P.FechaHasta
	AND P.Estado = 1
	Order By p.Cuota
END;