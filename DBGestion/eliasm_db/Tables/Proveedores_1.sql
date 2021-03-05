CREATE TABLE [eliasm_db].[Proveedores] (
    [Id]          NVARCHAR (150) NOT NULL,
    [Codigo]      NVARCHAR (50)  NULL,
    [RazonSocial] NVARCHAR (300) NULL,
    [Cuit]        NVARCHAR (11)  NULL,
    [Estado]      BIT            NULL,
    [FechaAlta]   DATETIME       NULL,
    [UsuarioAlta] NVARCHAR (256) NULL,
    CONSTRAINT [PK_Proveedores] PRIMARY KEY CLUSTERED ([Id] ASC)
);

