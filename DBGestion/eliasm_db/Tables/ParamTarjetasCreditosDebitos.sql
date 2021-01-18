CREATE TABLE [dbo].[ParamTarjetasCreditosDebitos] (
    [TarjetaId]     INT            NOT NULL,
    [TarjetaCodigo] VARCHAR (5)    NOT NULL,
    [TarjetaNombre] VARCHAR (150)  NULL,
    [EsDebito]      BIT            NULL,
    [Estado]        BIT            NULL,
    [FechaAlta]     DATETIME       NULL,
    [UsuarioAlta]   NVARCHAR (256) NULL,
    CONSTRAINT [PK_ParamTarjetasCreditosDebitos] PRIMARY KEY CLUSTERED ([TarjetaId] ASC)
);

