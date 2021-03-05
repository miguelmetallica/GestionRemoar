CREATE PROCEDURE [eliasm_db].[ProductosVentaGetOne]	
	@Id nvarchar(150) = ''
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
	LEFT JOIN ParamCategorias C ON C.Id = P.CategoriaId
	WHERE P.Id = @Id 
	AND P.Estado = 1	
END