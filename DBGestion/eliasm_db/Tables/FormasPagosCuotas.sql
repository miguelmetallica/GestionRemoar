CREATE TABLE [dbo].[FormasPagosCuotas] (
    [Id]          NVARCHAR (150)  NOT NULL,
    [FormaPagoId] NVARCHAR (150)  NOT NULL,
    [BancoId]     NVARCHAR (150)  NULL,
    [Cuota]       INT             NOT NULL,
    [Porcentaje]  NUMERIC (18, 4) NOT NULL,
    [Descuento]   NUMERIC (18, 4) NOT NULL,
    [FechaDesde]  DATETIME        NOT NULL,
    [FechaHasta]  DATETIME        NOT NULL,
    [Estado]      BIT             NULL,
    [FechaAlta]   DATETIME        NULL,
    [UsuarioAlta] NVARCHAR (256)  NULL,
    CONSTRAINT [PK_FormasPagosCuotas] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FormasPagosCuotas_FormasPagos] FOREIGN KEY ([FormaPagoId]) REFERENCES [dbo].[FormasPagos] ([Id]),
    CONSTRAINT [FK_FormasPagosCuotas_ParamBancos] FOREIGN KEY ([BancoId]) REFERENCES [dbo].[ParamBancos] ([Id])
);

