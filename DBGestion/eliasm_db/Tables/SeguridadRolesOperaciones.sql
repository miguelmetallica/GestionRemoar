CREATE TABLE [dbo].[SeguridadRolesOperaciones] (
    [RolOperacionId] INT            IDENTITY (1, 1) NOT NULL,
    [RolId]          INT            NOT NULL,
    [OperacionId]    INT            NOT NULL,
    [FechaAlta]      DATETIME       NULL,
    [UsuarioAlta]    NVARCHAR (256) NULL,
    CONSTRAINT [PK_SeguridadRolesOperaciones] PRIMARY KEY CLUSTERED ([RolOperacionId] ASC),
    CONSTRAINT [FK_SeguridadRolesOperaciones_SeguridadOperaciones] FOREIGN KEY ([OperacionId]) REFERENCES [dbo].[SeguridadOperaciones] ([OperacionId]),
    CONSTRAINT [FK_SeguridadRolesOperaciones_SeguridadRoles] FOREIGN KEY ([RolId]) REFERENCES [dbo].[SeguridadRoles] ([RolId])
);

