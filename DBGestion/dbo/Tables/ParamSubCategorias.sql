CREATE TABLE [dbo].[ParamSubCategorias] (
    [SubCategoriaId] INT            NOT NULL,
    [SubCategoria]   VARCHAR (150)  NULL,
    [Estado]         BIT            NULL,
    [FechaAlta]      DATETIME       NULL,
    [UsuarioAlta]    NVARCHAR (256) NULL,
    [FechaEdit]      DATETIME       NULL,
    [UsuarioEdit]    NVARCHAR (256) NULL,
    CONSTRAINT [PK_ParamSubCategorias] PRIMARY KEY CLUSTERED ([SubCategoriaId] ASC)
);

