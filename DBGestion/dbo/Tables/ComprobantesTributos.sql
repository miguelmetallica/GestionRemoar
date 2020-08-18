CREATE TABLE [dbo].[ComprobantesTributos] (
    [ComprobanteTributoId] INT             NOT NULL,
    [ComprobanteId]        INT             NULL,
    [TributoId]            INT             NULL,
    [Tributo]              VARCHAR (150)   NULL,
    [Detalle]              VARCHAR (150)   NULL,
    [BaseImponible]        NUMERIC (18, 2) NULL,
    [Alicuota]             NUMERIC (18, 2) NULL,
    [Importe]              NUMERIC (18, 2) NULL,
    [FechaAlta]            DATETIME        NULL,
    [UsuarioAlta]          NVARCHAR (256)  NULL,
    CONSTRAINT [PK_ComprobantesTributos] PRIMARY KEY CLUSTERED ([ComprobanteTributoId] ASC),
    CONSTRAINT [FK_ComprobantesTributos_Comprobantes] FOREIGN KEY ([ComprobanteId]) REFERENCES [dbo].[Comprobantes] ([ComprobanteId])
);

