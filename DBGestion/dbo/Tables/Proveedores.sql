CREATE TABLE [dbo].[Proveedores] (
    [ProveedorId] INT            NOT NULL,
    [Proveedor]   NVARCHAR (250) NULL,
    [Estado]      BIT            NULL,
    [FechaAlta]   DATETIME       NULL,
    [UsuarioAlta] NVARCHAR (256) NULL,
    [FechaEdit]   DATETIME       NULL,
    [UsuarioEdit] NVARCHAR (256) NULL,
    CONSTRAINT [PK_Proveedores] PRIMARY KEY CLUSTERED ([ProveedorId] ASC)
);

