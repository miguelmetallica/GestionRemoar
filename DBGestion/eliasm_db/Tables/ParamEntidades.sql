CREATE TABLE [eliasm_db].[ParamEntidades] (
    [Id]                NVARCHAR (150) NOT NULL,
    [Codigo]            NVARCHAR (50)  NULL,
    [Descripcion]       NVARCHAR (150) NULL,
    [VerCheque]         BIT            NULL,
    [VerTarjetaCredito] BIT            NULL,
    [VerTarjetaDebito]  BIT            NULL,
    [Estado]            BIT            NULL,
    [FechaAlta]         DATETIME       NULL,
    [UsuarioAlta]       NVARCHAR (256) NULL,
    CONSTRAINT [PK_ParamBancos] PRIMARY KEY CLUSTERED ([Id] ASC)
);

