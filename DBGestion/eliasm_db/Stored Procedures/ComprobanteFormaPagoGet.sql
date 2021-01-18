CREATE PROCEDURE [eliasm_db].[ComprobanteFormaPagoGet]
	@Id nvarchar(150) = ''
AS
BEGIN
	SELECT P.*
	FROM ComprobantesImputacion I
	INNER JOIN Comprobantes C ON C.Id = I.ComprobanteCancelaId 
	INNER JOIN ComprobantesFormasPagos P ON C.Id = P.ComprobanteId
	WHERE I.ComprobanteId = @Id
	AND I.ImporteCancela <> 0
	ORDER BY I.FechaCancela
END
;