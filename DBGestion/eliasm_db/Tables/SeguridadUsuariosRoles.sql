CREATE TABLE [dbo].[SeguridadUsuariosRoles] (
    [UsuarioRolId] INT            NOT NULL,
    [UsuarioId]    INT            NOT NULL,
    [RolId]        INT            NOT NULL,
    [FechaAlta]    DATETIME       NULL,
    [UsuarioAlta]  NVARCHAR (256) NULL,
    CONSTRAINT [PK_UsuariosRoles] PRIMARY KEY CLUSTERED ([UsuarioId] ASC, [RolId] ASC),
    CONSTRAINT [FK_SeguridadUsuariosRoles_SeguridadRoles] FOREIGN KEY ([RolId]) REFERENCES [dbo].[SeguridadRoles] ([RolId]),
    CONSTRAINT [FK_SeguridadUsuariosRoles_SeguridadUsuarios] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[SeguridadUsuarios] ([UsuarioId])
);

