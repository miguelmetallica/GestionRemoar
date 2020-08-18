CREATE TABLE [dbo].[ParamTiposComprobantes] (
    [TipoComprobanteId]     INT            NOT NULL,
    [TipoComprobanteCodigo] VARCHAR (5)    NULL,
    [TipoComprobante]       VARCHAR (150)  NULL,
    [Estado]                BIT            NULL,
    [FechaAlta]             DATETIME       NULL,
    [UsuarioAlta]           NVARCHAR (256) NULL,
    CONSTRAINT [PK_ParamTiposComprobantes] PRIMARY KEY CLUSTERED ([TipoComprobanteId] ASC)
);

