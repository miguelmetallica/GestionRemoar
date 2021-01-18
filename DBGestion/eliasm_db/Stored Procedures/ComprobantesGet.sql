CREATE PROCEDURE [eliasm_db].[ComprobantesGet]
	@ClienteId nvarchar(150) = ''
AS
BEGIN
	SELECT P.*,
	P.Total - ISNULL((SELECT SUM(ISNULL(I.ImporteCancela,0)) FROM ComprobantesImputacion I WHERE I.ComprobanteId = P.Id),0)Saldo
	FROM Comprobantes P
	WHERE P.ClienteId = @ClienteId	
	AND P.Codigo IS NOT NULL
	AND P.PresupuestoId IS NOT NULL
END
;