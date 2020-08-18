CREATE TABLE [dbo].[ProductosSubCategorias] (
    [ProductoSubCategoriaId] INT            NOT NULL,
    [ProductoId]             INT            NOT NULL,
    [SubCategoriaId]         INT            NOT NULL,
    [Estado]                 BIT            NULL,
    [FechaAlta]              DATETIME       NULL,
    [UsuarioAlta]            NVARCHAR (256) NULL,
    [FechaEdit]              DATETIME       NULL,
    [UsuarioEdit]            NVARCHAR (256) NULL,
    CONSTRAINT [PK_ProductosSubCategorias] PRIMARY KEY CLUSTERED ([ProductoSubCategoriaId] ASC)
);

