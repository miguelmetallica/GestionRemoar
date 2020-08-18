CREATE TABLE [dbo].[FormasPagos] (
    [FormaPagoId]     INT            NOT NULL,
    [FormaPagoCodigo] VARCHAR (5)    NOT NULL,
    [FormaPago]       VARCHAR (150)  NOT NULL,
    [Tipo]            CHAR (1)       NOT NULL,
    [FechaDesde]      DATETIME       NOT NULL,
    [FechaHasta]      DATETIME       NOT NULL,
    [Estado]          BIT            NULL,
    [FechaAlta]       DATETIME       NULL,
    [UsuarioAlta]     NVARCHAR (256) NULL,
    CONSTRAINT [PK_FormasPagos] PRIMARY KEY CLUSTERED ([FormaPagoId] ASC)
);

