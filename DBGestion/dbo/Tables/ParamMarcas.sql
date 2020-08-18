CREATE TABLE [dbo].[ParamMarcas] (
    [MarcaId]     INT            NOT NULL,
    [Marca]       VARCHAR (150)  NOT NULL,
    [Estado]      BIT            NULL,
    [FechaAlta]   DATETIME       NULL,
    [UsuarioAlta] NVARCHAR (256) NULL,
    [FechaEdit]   DATETIME       NULL,
    [UsuarioEdit] NVARCHAR (256) NULL,
    CONSTRAINT [PK_ParamMarcas] PRIMARY KEY CLUSTERED ([MarcaId] ASC)
);

