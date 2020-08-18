CREATE TABLE [dbo].[FormasPagosCuotas] (
    [FormaPagoCuotaId] INT             NOT NULL,
    [FormaPagoId]      INT             NOT NULL,
    [Cuota]            INT             NOT NULL,
    [Porcentaje]       NUMERIC (18, 4) NOT NULL,
    [Descuento]        NUMERIC (18, 4) NOT NULL,
    [FechaDesde]       DATETIME        NOT NULL,
    [FechaHasta]       DATETIME        NOT NULL,
    [Estado]           BIT             NULL,
    [FechaAlta]        DATETIME        NULL,
    [UsuarioAlta]      NVARCHAR (256)  NULL,
    CONSTRAINT [PK_FormasPagosCuotas] PRIMARY KEY CLUSTERED ([FormaPagoCuotaId] ASC)
);

