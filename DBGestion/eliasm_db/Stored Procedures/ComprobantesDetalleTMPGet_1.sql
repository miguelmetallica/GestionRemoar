CREATE PROCEDURE [eliasm_db].[ComprobantesDetalleTMPGet]
	@ComprobanteId nvarchar(150)
AS
BEGIN
	SELECT D.Id,
		D.ComprobanteId,
		D.ProductoId,
		D.ProductoCodigo,
		D.ProductoNombre,
		CONVERT(NUMERIC(18,2),ISNULL(D.Precio,0)) Precio,
		CONVERT(NUMERIC(18,2),ISNULL(D.PrecioSinIva,0)) PrecioSinIva,
		D.Cantidad,
		D.UsuarioAlta,
		D.FechaAlta	
	FROM ComprobantesDetalleTMP d
	WHERE d.ComprobanteId = @ComprobanteId
	ORDER BY d.ProductoCodigo

END