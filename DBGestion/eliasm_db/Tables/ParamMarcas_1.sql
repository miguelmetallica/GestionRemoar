CREATE TABLE [eliasm_db].[ParamMarcas] (
    [Id]          NVARCHAR (150) NOT NULL,
    [Codigo]      NVARCHAR (5)   NULL,
    [Descripcion] NVARCHAR (150) NOT NULL,
    [Estado]      BIT            NULL,
    [FechaAlta]   DATETIME       NULL,
    [UsuarioAlta] NVARCHAR (256) NULL,
    CONSTRAINT [PK_ParamMarcas] PRIMARY KEY CLUSTERED ([Id] ASC)
);

