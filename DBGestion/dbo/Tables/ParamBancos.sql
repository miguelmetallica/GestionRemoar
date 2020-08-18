CREATE TABLE [dbo].[ParamBancos] (
    [BancoId]     INT            NOT NULL,
    [BancoCodigo] VARCHAR (5)    NULL,
    [Banco]       VARCHAR (150)  NULL,
    [Estado]      BIT            NULL,
    [FechaAlta]   DATETIME       NULL,
    [UsuarioAlta] NVARCHAR (256) NULL,
    CONSTRAINT [PK_ParamBancos] PRIMARY KEY CLUSTERED ([BancoId] ASC)
);

