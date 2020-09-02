CREATE TABLE [dbo].[ParamSubCategorias] (
    [Id]          NVARCHAR (150) NOT NULL,
    [CategoriaId] NVARCHAR (150) NULL,
    [Descripcion] NVARCHAR (150) NULL,
    [Estado]      BIT            NULL,
    [FechaAlta]   DATETIME       NULL,
    [UsuarioAlta] NVARCHAR (256) NULL,
    [FechaEdit]   DATETIME       NULL,
    [UsuarioEdit] NVARCHAR (256) NULL,
    CONSTRAINT [PK_ParamSubCategorias] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ParamSubCategorias_ParamCategorias] FOREIGN KEY ([CategoriaId]) REFERENCES [dbo].[ParamCategorias] ([Id])
);



