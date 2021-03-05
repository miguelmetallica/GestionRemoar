CREATE TABLE [eliasm_db].[ParamImpuestos] (
    [ImpuestoId]  INT             NOT NULL,
    [Impuesto]    VARCHAR (150)   NULL,
    [Valor]       NUMERIC (18, 4) NULL,
    [Estado]      BIT             NULL,
    [FechaAlta]   DATETIME        NULL,
    [UsuarioAlta] NVARCHAR (256)  NULL,
    CONSTRAINT [PK_ParamImpuestos] PRIMARY KEY CLUSTERED ([ImpuestoId] ASC)
);

