CREATE TABLE [dbo].[ParamColores] (
    [ColorId]     INT            NOT NULL,
    [Color]       VARCHAR (150)  NOT NULL,
    [Estado]      BIT            NULL,
    [FechaAlta]   DATETIME       NULL,
    [UsuarioAlta] NVARCHAR (256) NULL,
    [FechaEdit]   DATETIME       NULL,
    [UsuarioEdit] NVARCHAR (256) NULL,
    CONSTRAINT [PK_ParamColores] PRIMARY KEY CLUSTERED ([ColorId] ASC)
);

