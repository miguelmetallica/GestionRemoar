CREATE PROCEDURE [eliasm_db].[CatalogoProductosGet]
AS
BEGIN
	DECLARE @IMAGEN_DEFAUT VARCHAR(500)

	SELECT @IMAGEN_DEFAUT = Valor
	FROM SistemaConfiguraciones 
	WHERE Configuracion = 'IMAGEN_DEFAULT'

	SELECT P.Id,P.Codigo,P.Producto,ISNULL(I.ImagenUrl,@IMAGEN_DEFAUT)ImagenUrl,1 Cantidad,P.PrecioVenta
	FROM Productos P
	LEFT JOIN ProductosImagenes I ON I.ProductoId = P.Id
	WHERE P.Estado = 1
END