CREATE TABLE [eliasm_db].[ComprobantesImputacion] (
    [Id]                   NVARCHAR (150)  NOT NULL,
    [ComprobanteId]        NVARCHAR (150)  NULL,
    [ComprobanteCancelaId] NVARCHAR (150)  NULL,
    [ImporteCancela]       NUMERIC (18, 2) NULL,
    [ImporteCancelaLista]  NUMERIC (18, 2) NULL,
    [FechaCancela]         DATETIME        NULL,
    [Estado]               BIT             NULL,
    [Observaciones]        NVARCHAR (500)  NULL,
    [FechaAlta]            DATETIME        NULL,
    [UsuarioAlta]          NVARCHAR (150)  NULL,
    CONSTRAINT [PK_ComprobantesImputacion] PRIMARY KEY CLUSTERED ([Id] ASC)
);

