CREATE TABLE [dbo].[SeguridadRoles] (
    [RolId]       INT            NOT NULL,
    [Rol]         VARCHAR (150)  NOT NULL,
    [Estado]      BIT            NOT NULL,
    [FechaAlta]   DATETIME       NULL,
    [UsuarioAlta] NVARCHAR (256) NULL,
    CONSTRAINT [PK_SeguridadRoles] PRIMARY KEY CLUSTERED ([RolId] ASC)
);

