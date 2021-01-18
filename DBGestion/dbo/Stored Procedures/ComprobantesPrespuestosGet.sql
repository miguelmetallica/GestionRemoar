CREATE PROCEDURE [dbo].[ComprobantesPrespuestosGet]
AS
BEGIN
	SELECT P.*,PR.Codigo CodigoPrespuesto,
	P.Total - ISNULL((SELECT SUM(ISNULL(I.ImporteCancela,0)) FROM ComprobantesImputacion I WHERE I.ComprobanteId = P.Id),0)Saldo,
	CONVERT(NUMERIC(18,2),(ROUND((ISNULL((SELECT SUM(ISNULL(I.ImporteCancela,0)) FROM ComprobantesImputacion I WHERE I.ComprobanteId = P.Id),0) * 100) / P.Total,2))) Saldo_Porcentaje
	FROM Comprobantes P
	INNER JOIN Presupuestos PR ON PR.Id = P.PresupuestoId
	WHERE P.Codigo IS NOT NULL
	AND P.PresupuestoId IS NOT NULL
	AND P.Estado = 1
END
;