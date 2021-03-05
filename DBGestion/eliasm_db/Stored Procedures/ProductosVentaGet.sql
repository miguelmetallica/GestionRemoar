CREATE PROCEDURE [eliasm_db].[ProductosVentaGet]	
AS
BEGIN
	SELECT P.Id,
	P.Codigo,
	ISNULL(T.Descripcion,'') TipoProducto,
	P.Producto,
	eliasm_db.PrecioContado(P.Id) PrecioContado,
	eliasm_db.PrecioLista(P.Id) PrecioVenta,
	P.Estado
	FROM Productos P
	LEFT JOIN ParamTiposProductos T ON T.Id = P.TipoProductoId
	WHERE P.Estado = 1
	ORDER BY P.Codigo
END