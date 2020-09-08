CREATE PROCEDURE [dbo].[ProductosCategoriasGet]
	@ProductoId nvarchar(150) = ''
AS
BEGIN
	SELECT Id CategoriaId,
		@ProductoId ProductoId,
		Descripcion Categoria,
		ISNULL((SELECT count(*) FROM ProductosCategorias PC WHERE PC.CategoriaId = P.Id AND PC.ProductoId = @ProductoId),0) Activa
	FROM ParamCategorias P
	WHERE P.Estado = 1

END