CREATE PROCEDURE [eliasm_db].[FormasPagosCuotaUnoGet]
	@FormaPagoId nvarchar(150) = null	
AS
BEGIN
	SELECT TOP 1 P.Id,P.FormaPagoId,P.EntidadId,P.Descripcion,P.Cuota,P.Interes,P.FechaDesde,P.FechaHasta,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM FormasPagosCuotas P
	WHERE P.FormaPagoId = @FormaPagoId
	AND P.Cuota = 1
	AND CONVERT(DATE,GETDATE()) BETWEEN P.FechaDesde AND P.FechaHasta
	AND P.Estado = 1	
END;