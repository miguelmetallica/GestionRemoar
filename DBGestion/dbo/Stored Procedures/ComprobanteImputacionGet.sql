CREATE PROCEDURE [dbo].[ComprobanteImputacionGet]
	@Id nvarchar(150) = ''
AS
BEGIN
	--SELECT C.*,0.00 Saldo
	--FROM ComprobantesImputacion I
	--INNER JOIN Comprobantes C ON C.Id = I.ComprobanteCancelaId 
	--WHERE I.ComprobanteId = @Id
	--AND I.ImporteCancela <> 0
	--ORDER BY I.FechaCancela

	SELECT C.*,0.00 Saldo
	FROM ComprobantesImputacion I
	INNER JOIN Comprobantes C ON C.Id = I.ComprobanteCancelaId 
	WHERE I.ComprobanteId = @Id
	AND I.ImporteCancela <> 0

	UNION ALL

	SELECT C.*,0.00 Saldo
	FROM ComprobantesDetalleImputacion I
	INNER JOIN Comprobantes C ON C.Id = I.RemitoId 
	WHERE I.ComprobanteId = @Id
	AND I.RemitoId IS NOT NULL
	ORDER BY FechaComprobante

END
;