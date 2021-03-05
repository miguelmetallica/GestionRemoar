CREATE TABLE [eliasm_db].[ComprobantesSecuencias] (
    [ComprobanteSecuenciaId] INT            NOT NULL,
    [TipoComprobanteId]      INT            NULL,
    [Letra]                  CHAR (1)       NULL,
    [PtoVenta]               INT            NULL,
    [Numero]                 NUMERIC (10)   NULL,
    [Estado]                 BIT            NULL,
    [FechaAlta]              DATETIME       NULL,
    [UsuarioAlta]            NVARCHAR (256) NULL,
    CONSTRAINT [PK_ComprobantesSecuencias] PRIMARY KEY CLUSTERED ([ComprobanteSecuenciaId] ASC)
);

