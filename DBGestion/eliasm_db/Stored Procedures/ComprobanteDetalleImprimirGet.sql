CREATE PROCEDURE [eliasm_db].[ComprobanteDetalleImprimirGet]
	@ComprobanteId nvarchar(150) = ''
AS
BEGIN
	SELECT D.*
	FROM Comprobantes C
	INNER JOIN ComprobantesDetalle D ON D.ComprobanteId = C.Id 
	WHERE C.Id = @ComprobanteId
	ORDER BY D.ProductoCodigo
END
;