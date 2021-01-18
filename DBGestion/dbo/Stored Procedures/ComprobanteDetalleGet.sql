CREATE PROCEDURE [dbo].[ComprobanteDetalleGet]
	@ComprobanteId nvarchar(150) = ''
AS
BEGIN
	SELECT D.*,0.00 Saldo
	FROM ComprobantesDetalleImputacion I
	INNER JOIN Comprobantes C ON C.Id = I.RemitoId 
	INNER JOIN ComprobantesDetalle D ON D.ComprobanteId = C.Id 
	WHERE I.ComprobanteId = @ComprobanteId
	AND I.RemitoId IS NOT NULL	
	ORDER BY D.ProductoCodigo
END
;