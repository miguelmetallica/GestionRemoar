CREATE TABLE [eliasm_db].[FormasPagosCotizacion] (
    [Id]          NVARCHAR (150)  NOT NULL,
    [Cotizacion]  NUMERIC (18, 2) NOT NULL,
    [FechaDesde]  DATETIME        NOT NULL,
    [FechaHasta]  DATETIME        NOT NULL,
    [Estado]      BIT             NULL,
    [FechaAlta]   DATETIME        NULL,
    [UsuarioAlta] NVARCHAR (256)  NULL,
    [FechaEdit]   DATETIME        NULL,
    [UsuarioEdit] NVARCHAR (256)  NULL,
    CONSTRAINT [PK_FormasPagosCotizacion] PRIMARY KEY CLUSTERED ([Id] ASC)
);

