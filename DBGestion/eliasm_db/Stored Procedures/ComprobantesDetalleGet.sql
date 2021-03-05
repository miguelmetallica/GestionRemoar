CREATE PROCEDURE [eliasm_db].[ComprobantesDetalleGet]
	@ComprobanteId varchar(150) = ''
AS
BEGIN
	SELECT 
		D.Id,
		D.ComprobanteId,
		D.ProductoId,
		D.ProductoCodigo,
		D.ProductoNombre,
		CONVERT(NUMERIC(18,2),ISNULL(D.Precio,0)) Precio,
		CONVERT(NUMERIC(18,2),ISNULL(D.PrecioSinIva,0)) PrecioSinIva,
		D.Cantidad,
		D.UsuarioAlta,
		D.FechaAlta		
	FROM ComprobantesDetalle D
	WHERE D.ComprobanteId = @ComprobanteId
	ORDER BY D.ProductoCodigo
END