CREATE TABLE [dbo].[ProductosSubCategorias] (
    [Id]             NVARCHAR (150) NOT NULL,
    [ProductoId]     NVARCHAR (150) NOT NULL,
    [SubCategoriaId] NVARCHAR (150) NOT NULL,
    [Estado]         BIT            NULL,
    [FechaAlta]      DATETIME       NULL,
    [UsuarioAlta]    NVARCHAR (256) NULL,
    [FechaEdit]      DATETIME       NULL,
    [UsuarioEdit]    NVARCHAR (256) NULL,
    CONSTRAINT [PK_ProductosSubCategorias] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProductosSubCategorias_ParamSubCategorias] FOREIGN KEY ([SubCategoriaId]) REFERENCES [dbo].[ParamSubCategorias] ([Id]),
    CONSTRAINT [FK_ProductosSubCategorias_Productos] FOREIGN KEY ([ProductoId]) REFERENCES [dbo].[Productos] ([Id])
);



