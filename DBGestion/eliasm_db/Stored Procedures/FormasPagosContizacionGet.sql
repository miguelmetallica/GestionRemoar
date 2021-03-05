CREATE PROCEDURE [eliasm_db].FormasPagosContizacionGet
AS
BEGIN
	SELECT TOP 1 P.Cotizacion
	FROM FormasPagosCotizacion P 
	WHERE P.Estado = 1
	AND CONVERT(DATE,GetDate()) BETWEEN CONVERT(DATE,FechaDesde) AND CONVERT(DATE,FechaHasta)
	ORDER BY FechaAlta DESC
END