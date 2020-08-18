CREATE TABLE [dbo].[Presupuestos] (
    [Id]               NVARCHAR (150) NOT NULL,
    [Codigo]           NVARCHAR (150) NULL,
    [Fecha]            DATETIME       NULL,
    [FechaVencimiento] DATETIME       NULL,
    [ClienteId]        NVARCHAR (150) NULL,
    [EstadoId]         NVARCHAR (150) NULL,
    [Estado]           BIT            NULL,
    [FechaAlta]        DATETIME       NULL,
    [UsuarioAlta]      NVARCHAR (450) NULL,
    CONSTRAINT [PK_Presupuestos] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Presupuestos_Clientes] FOREIGN KEY ([ClienteId]) REFERENCES [dbo].[Clientes] ([Id]),
    CONSTRAINT [FK_Presupuestos_ParamPresupuestosEstados] FOREIGN KEY ([EstadoId]) REFERENCES [dbo].[ParamPresupuestosEstados] ([Id])
);

