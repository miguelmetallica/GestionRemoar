CREATE PROCEDURE [eliasm_db].[ComprobantesGet]
	@ClienteId nvarchar(150) = ''
AS
BEGIN
	SELECT P.*,
	
	(SELECT SUM(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * Total)Importe
	--SUM(CASE WHEN P1.TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * P1.TotalLista)ImporteLista
	FROM Comprobantes p1
	WHERE P1.PresupuestoId = P.PresupuestoId)
	Saldo
	FROM Comprobantes P
	WHERE P.ClienteId = @ClienteId	
	AND P.Codigo IS NOT NULL
	AND P.PresupuestoId IS NOT NULL
END
;