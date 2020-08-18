CREATE TABLE [dbo].[PedidosFormasPagosContado] (
    [PedidoContadoId]  INT             NOT NULL,
    [PedidoId]         INT             NOT NULL,
    [FormaPagoId]      INT             NOT NULL,
    [FormaPago]        VARCHAR (150)   NOT NULL,
    [FormaPagoTipo]    INT             NOT NULL,
    [FormaPagoCuotaId] INT             NOT NULL,
    [Cuota]            INT             NOT NULL,
    [Porcentaje]       NUMERIC (18, 4) NULL,
    [Descuento]        NUMERIC (18, 4) NULL,
    [Estado]           BIT             NULL,
    [FechaAlta]        DATETIME        NULL,
    [UsuarioAlta]      NVARCHAR (256)  NULL,
    [FechaEdit]        DATETIME        NULL,
    [UsuarioEdit]      NVARCHAR (256)  NULL,
    CONSTRAINT [PK_PedidosFormasPagosContado] PRIMARY KEY CLUSTERED ([PedidoContadoId] ASC)
);

