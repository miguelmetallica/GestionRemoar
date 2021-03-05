CREATE TABLE [eliasm_db].[ProductosPrecios] (
    [ProductoPrecioId] INT             NOT NULL,
    [ProductoId]       INT             NOT NULL,
    [Precio]           NUMERIC (18, 2) NULL,
    [FechaDesde]       DATETIME        NULL,
    [Estado]           BIT             NULL,
    [FechaAlta]        DATETIME        NULL,
    [UsuarioAlta]      NVARCHAR (256)  NULL,
    [FechaEdit]        DATETIME        NULL,
    [UsuarioEdit]      NVARCHAR (256)  NULL,
    CONSTRAINT [PK_ProductosPrecios] PRIMARY KEY CLUSTERED ([ProductoPrecioId] ASC)
);

