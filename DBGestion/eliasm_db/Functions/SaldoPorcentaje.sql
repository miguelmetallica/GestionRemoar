CREATE FUNCTION [eliasm_db].[SaldoPorcentaje](@PresupuestoId [NVARCHAR](150),@ComprobanteId [NVARCHAR](150))
RETURNS NUMERIC(18,2) WITH EXECUTE AS CALLER
AS 
BEGIN
	DECLARE @PORCENTAJE NUMERIC(18,2) = 0;
	

	SELECT @PORCENTAJE = 100 - ABS(CASE WHEN P.Total = 0 THEN 0 ELSE (eliasm_db.Saldo(P.PresupuestoId)) * 100 / P.Total END)
	FROM Comprobantes P
	WHERE P.Id = @ComprobanteId
	
	RETURN @PORCENTAJE
END