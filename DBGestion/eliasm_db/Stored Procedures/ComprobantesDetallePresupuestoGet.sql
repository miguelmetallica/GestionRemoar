CREATE PROCEDURE [eliasm_db].[ComprobantesDetallePresupuestoGet]
	@PresupuestoId varchar(150) = ''
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
	FROM Comprobantes C
	INNER JOIN ComprobantesDetalle D ON D.ComprobanteId = C.Id
	WHERE C.PresupuestoId = @PresupuestoId
	ORDER BY D.ProductoCodigo
END