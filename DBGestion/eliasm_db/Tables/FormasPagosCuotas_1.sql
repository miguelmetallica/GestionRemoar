CREATE TABLE [eliasm_db].[FormasPagosCuotas] (
    [Id]          NVARCHAR (150)  NOT NULL,
    [FormaPagoId] NVARCHAR (150)  NOT NULL,
    [EntidadId]   NVARCHAR (150)  NULL,
    [Descripcion] NVARCHAR (150)  NULL,
    [Cuota]       INT             NOT NULL,
    [Interes]     NUMERIC (18, 2) NOT NULL,
    [FechaDesde]  DATETIME        NULL,
    [FechaHasta]  DATETIME        NULL,
    [Estado]      BIT             NULL,
    [FechaAlta]   DATETIME        NULL,
    [UsuarioAlta] NVARCHAR (256)  NULL,
    CONSTRAINT [PK_FormasPagosCuotas] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FormasPagosCuotas_FormasPagos] FOREIGN KEY ([FormaPagoId]) REFERENCES [eliasm_db].[FormasPagos] ([Id])
);

