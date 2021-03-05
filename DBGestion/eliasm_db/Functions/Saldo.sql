CREATE FUNCTION [eliasm_db].[Saldo](@PresupuestoId [NVARCHAR](150))
RETURNS NUMERIC(18,2) WITH EXECUTE AS CALLER
AS 
BEGIN
	DECLARE @SALDO NUMERIC(18,2) = 0;
	
	SELECT @SALDO = SUM(Saldo)
	FROM(
		SELECT (CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * Total) Saldo
		FROM Comprobantes P
		WHERE P.PresupuestoId = @PresupuestoId
		AND P.TipoComprobanteId in (
										SELECT T.Id
										FROM SistemaConfiguraciones C
										INNER JOIN ParamTiposComprobantes T ON T.Codigo = C.Valor
										WHERE C.Configuracion = 'COMPROBANTES_PRESUPUESTO'
										)
		UNION ALL
		SELECT SUM(CASE WHEN TipoComprobanteEsDebe = 1 THEN 1 ELSE -1 END * f.Importe * cuota)	
		FROM Comprobantes P
		LEFT JOIN ComprobantesFormasPagos F ON F.ComprobanteId = P.Id
		WHERE P.PresupuestoId = @PresupuestoId
		AND P.TipoComprobanteId not in (
										SELECT T.Id
										FROM SistemaConfiguraciones C
										INNER JOIN ParamTiposComprobantes T ON T.Codigo = C.Valor
										WHERE C.Configuracion = 'COMPROBANTES_PRESUPUESTO'
										)
		UNION ALL
		SELECT SUM(CASE WHEN TipoComprobanteEsDebe = 1 THEN -1 ELSE 1 END * f.Importe * cuota)	
		FROM Comprobantes P
		LEFT JOIN ComprobantesFormasPagos F ON F.ComprobanteId = P.Id
		WHERE P.PresupuestoId = @PresupuestoId
		AND P.TipoComprobanteId in (
										SELECT T.Id
										FROM SistemaConfiguraciones C
										INNER JOIN ParamTiposComprobantes T ON T.Codigo = C.Valor
										WHERE C.Configuracion = 'COMPROBANTES_PRESUPUESTO'
										)
	)Q
	
	RETURN @SALDO
END