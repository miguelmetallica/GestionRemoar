CREATE PROCEDURE [dbo].[ProductosGet]	
AS
BEGIN
	SELECT P.Id,
	P.Codigo,
	ISNULL(T.Descripcion,'') TipoProducto,
	P.Producto,
	P.PrecioVenta,
	P.Estado
	FROM Productos P
	LEFT JOIN ParamTiposProductos T ON T.Id = P.TipoProductoId
	ORDER BY P.Codigo
END