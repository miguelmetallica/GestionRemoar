CREATE TABLE [eliasm_db].[ParamTiposDocumentos] (
    [Id]          NVARCHAR (150) NOT NULL,
    [Codigo]      NVARCHAR (5)   NOT NULL,
    [Descripcion] NVARCHAR (150) NOT NULL,
    [Defecto]     BIT            NULL,
    [Estado]      BIT            NULL,
    [FechaAlta]   DATETIME       NULL,
    [UsuarioAlta] NVARCHAR (256) NULL,
    CONSTRAINT [PK_TIPOS_DOCUMENTOS] PRIMARY KEY CLUSTERED ([Id] ASC)
);

