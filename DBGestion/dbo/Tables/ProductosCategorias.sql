CREATE TABLE [dbo].[ProductosCategorias] (
    [Id]          NVARCHAR (150) NOT NULL,
    [ProductoId]  NVARCHAR (150) NOT NULL,
    [CategoriaId] NVARCHAR (150) NOT NULL,
    CONSTRAINT [PK_ProductosCategorias] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProductosCategorias_ParamCategorias] FOREIGN KEY ([CategoriaId]) REFERENCES [dbo].[ParamCategorias] ([Id]),
    CONSTRAINT [FK_ProductosCategorias_Productos] FOREIGN KEY ([ProductoId]) REFERENCES [dbo].[Productos] ([Id])
);





