CREATE TABLE [dbo].[ParamCategorias] (
    [CategoriaId] INT            NOT NULL,
    [Categoria]   VARCHAR (150)  NULL,
    [Estado]      BIT            NULL,
    [FechaAlta]   DATETIME       NULL,
    [UsuarioAlta] NVARCHAR (256) NULL,
    [FechaEdit]   DATETIME       NULL,
    [UsuarioEdit] NVARCHAR (256) NULL,
    CONSTRAINT [PK_ParamCategorias] PRIMARY KEY CLUSTERED ([CategoriaId] ASC)
);

