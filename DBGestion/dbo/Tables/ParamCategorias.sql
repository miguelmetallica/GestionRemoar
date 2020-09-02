CREATE TABLE [dbo].[ParamCategorias] (
    [Id]          NVARCHAR (150) NOT NULL,
    [Descripcion] NVARCHAR (150) NULL,
    [Estado]      BIT            NULL,
    [FechaAlta]   DATETIME       NULL,
    [UsuarioAlta] NVARCHAR (256) NULL,
    [FechaEdit]   DATETIME       NULL,
    [UsuarioEdit] NVARCHAR (256) NULL,
    CONSTRAINT [PK_ParamCategorias] PRIMARY KEY CLUSTERED ([Id] ASC)
);



