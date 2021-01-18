CREATE TABLE [dbo].[PedidosDetalles] (
    [PedidoDetalleId] INT             NOT NULL,
    [PedidoId]        INT             NOT NULL,
    [ProductoId]      INT             NOT NULL,
    [ProductoCodigo]  VARCHAR (20)    NOT NULL,
    [Producto]        VARCHAR (150)   NOT NULL,
    [ColorId]         INT             NULL,
    [Color]           VARCHAR (150)   NULL,
    [MarcaId]         INT             NULL,
    [Marca]           VARCHAR (150)   NULL,
    [CuentaVentaId]   INT             NULL,
    [CuentaVenta]     VARCHAR (150)   NULL,
    [ImpuestoId]      INT             NULL,
    [Impuesto]        NUMERIC (18, 4) NULL,
    [Precio]          NUMERIC (18, 2) NULL,
    [Cantidad]        INT             NULL,
    [Total]           NUMERIC (18, 2) NULL,
    [Estado]          BIT             NULL,
    [FechaAlta]       DATETIME        NULL,
    [UsuarioAlta]     NVARCHAR (256)  NULL,
    CONSTRAINT [PK_PedidosDetalles] PRIMARY KEY CLUSTERED ([PedidoDetalleId] ASC),
    CONSTRAINT [FK_PedidosDetalles_Pedidos] FOREIGN KEY ([PedidoId]) REFERENCES [dbo].[Pedidos] ([PedidoId])
);

