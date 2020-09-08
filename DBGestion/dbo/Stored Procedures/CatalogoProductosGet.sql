CREATE PROCEDURE CatalogoProductosGet
AS
BEGIN
	SELECT P.Id,P.Codigo,P.Producto,I.ImagenUrl,1 Cantidad,P.PrecioVenta
	FROM Productos P
	LEFT JOIN ProductosImagenes I ON I.ProductoId = P.Id
	WHERE P.Estado = 1
END