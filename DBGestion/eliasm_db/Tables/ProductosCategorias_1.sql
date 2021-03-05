CREATE TABLE [eliasm_db].[ProductosCategorias] (
    [Id]          NVARCHAR (150) NOT NULL,
    [ProductoId]  NVARCHAR (150) NOT NULL,
    [CategoriaId] NVARCHAR (150) NOT NULL,
    CONSTRAINT [PK_ProductosCategorias] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProductosCategorias_ParamCategorias] FOREIGN KEY ([CategoriaId]) REFERENCES [eliasm_db].[ParamCategorias] ([Id]),
    CONSTRAINT [FK_ProductosCategorias_Productos] FOREIGN KEY ([ProductoId]) REFERENCES [eliasm_db].[Productos] ([Id])
);

