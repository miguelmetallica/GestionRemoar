﻿CREATE PROCEDURE [eliasm_db].[ProductosCategoriasInsertar]
	@Id nvarchar(150),
	@ProductoId nvarchar(150),
	@CategoriaId nvarchar(150)
AS
BEGIN
	INSERT INTO ProductosCategorias(Id,ProductoId,CategoriaId)
     VALUES(@Id,@ProductoId,@CategoriaId)           
END