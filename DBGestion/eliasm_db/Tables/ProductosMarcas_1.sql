CREATE TABLE [eliasm_db].[ProductosMarcas] (
    [ProductoMarcaId] INT            NOT NULL,
    [ProductoId]      INT            NOT NULL,
    [MarcaId]         INT            NOT NULL,
    [Estado]          BIT            NULL,
    [FechaAlta]       DATETIME       NULL,
    [UsuarioAlta]     NVARCHAR (256) NULL,
    [FechaEdit]       DATETIME       NULL,
    [UsuarioEdit]     NVARCHAR (256) NULL,
    CONSTRAINT [PK_ProductosMarcas] PRIMARY KEY CLUSTERED ([ProductoMarcaId] ASC)
);

