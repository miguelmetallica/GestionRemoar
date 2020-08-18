CREATE TABLE [dbo].[ParamTiposResponsables] (
    [TipoResponsableId]     INT            NOT NULL,
    [TipoResponsableCodigo] VARCHAR (5)    NULL,
    [TipoResponsable]       VARCHAR (150)  NULL,
    [Estado]                BIT            NULL,
    [FechaAlta]             DATETIME       NULL,
    [UsuarioAlta]           NVARCHAR (256) NULL,
    CONSTRAINT [PK_ParamTiposResponsables] PRIMARY KEY CLUSTERED ([TipoResponsableId] ASC)
);

