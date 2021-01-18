CREATE TABLE [dbo].[SeguridadUsuarios] (
    [UsuarioId]   INT            NOT NULL,
    [Usuario]     NVARCHAR (256) NOT NULL,
    [Password]    NVARCHAR (100) NULL,
    [Apellido]    VARCHAR (150)  NULL,
    [Nombre]      VARCHAR (150)  NULL,
    [Telefono]    VARCHAR (50)   NULL,
    [Celular]     VARCHAR (50)   NULL,
    [Puesto]      VARCHAR (250)  NULL,
    [SucursalId]  NVARCHAR (150) NULL,
    [Estado]      BIT            NULL,
    [FechaAlta]   DATETIME       NULL,
    [UsuarioAlta] NVARCHAR (256) NULL,
    [FechaEdit]   DATETIME       NULL,
    [UsuarioEdit] NVARCHAR (256) NULL,
    CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED ([UsuarioId] ASC),
    CONSTRAINT [FK_SeguridadUsuarios_Sucursales] FOREIGN KEY ([SucursalId]) REFERENCES [dbo].[Sucursales] ([Id])
);

