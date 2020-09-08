CREATE TABLE [dbo].[ProductosImagenes] (
    [Id]         NVARCHAR (150) NOT NULL,
    [ProductoId] NVARCHAR (150) NULL,
    [ImagenUrl]  NVARCHAR (500) NULL,
    CONSTRAINT [PK_ProductosImagenes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

