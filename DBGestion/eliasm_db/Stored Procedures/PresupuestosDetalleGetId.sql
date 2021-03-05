CREATE PROCEDURE [eliasm_db].[PresupuestosDetalleGetId]
	@Id varchar(150) = ''
AS
BEGIN
	SELECT 
		D.Id,
		D.PresupuestoId,
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
	FROM PresupuestosDetalle D
	WHERE D.Id = @Id
	
END