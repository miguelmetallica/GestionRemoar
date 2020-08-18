CREATE TABLE [dbo].[ProductosCategorias] (
    [ProductoCategoriaId] INT            NOT NULL,
    [ProductoId]          INT            NOT NULL,
    [CategoriaId]         INT            NOT NULL,
    [Estado]              BIT            NULL,
    [FechaAlta]           DATETIME       NULL,
    [UsuarioAlta]         NVARCHAR (256) NULL,
    [FechaEdit]           DATETIME       NULL,
    [UsuarioEdit]         NVARCHAR (256) NULL,
    CONSTRAINT [PK_ProductosCategorias] PRIMARY KEY CLUSTERED ([ProductoCategoriaId] ASC)
);

