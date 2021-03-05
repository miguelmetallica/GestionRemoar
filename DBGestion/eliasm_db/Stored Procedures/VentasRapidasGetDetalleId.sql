CREATE PROCEDURE [eliasm_db].[VentasRapidasGetDetalleId]
	@Id varchar(150) = ''
AS
BEGIN
	SELECT 
		D.Id,
		D.VentaRapidaId,
		D.ProductoId,
		D.ProductoCodigo,
		D.ProductoNombre,
		D.Precio,
		D.PrecioSinImpuesto,
		D.Cantidad,
		D.UsuarioAlta,
		D.FechaAlta,
		D.UsuarioEdit,
		D.FechaEdit
	FROM VentasRapidasDetalle D
	WHERE D.Id = @Id
	AND D.Cantidad <> 0
	AND D.Precio <> 0
	ORDER BY D.ProductoCodigo
END