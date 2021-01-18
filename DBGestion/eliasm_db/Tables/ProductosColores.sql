CREATE TABLE [dbo].[ProductosColores] (
    [ProductoColorId] INT            NOT NULL,
    [ProductoId]      INT            NOT NULL,
    [ColorId]         INT            NOT NULL,
    [Estado]          BIT            NULL,
    [FechaAlta]       DATETIME       NULL,
    [UsuarioAlta]     NVARCHAR (256) NULL,
    [FechaEdit]       DATETIME       NULL,
    [UsuarioEdit]     NVARCHAR (256) NULL,
    CONSTRAINT [PK_ProductosColores] PRIMARY KEY CLUSTERED ([ProductoColorId] ASC)
);

