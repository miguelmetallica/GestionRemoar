CREATE PROCEDURE [dbo].[ProductosCategoriasEliminar]
	@ProductoId nvarchar(150),
	@CategoriaId nvarchar(150)
AS
BEGIN
	DELETE ProductosCategorias
	WHERE ProductoId = @ProductoId
	AND CategoriaId = @CategoriaId
END