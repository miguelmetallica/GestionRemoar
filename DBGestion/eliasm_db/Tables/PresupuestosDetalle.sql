CREATE TABLE [dbo].[PresupuestosDetalle] (
    [Id]                            NVARCHAR (150)  NOT NULL,
    [PresupuestoId]                 NVARCHAR (150)  NOT NULL,
    [ProductoId]                    NVARCHAR (150)  NULL,
    [ProductoCodigo]                NVARCHAR (50)   NULL,
    [ProductoNombre]                NVARCHAR (150)  NULL,
    [Precio]                        NUMERIC (18, 2) NULL,
    [PrecioSinImpuesto]             NUMERIC (18, 2) NULL,
    [PrecioDtoCategoria]            NUMERIC (18, 2) NULL,
    [PrecioSinImpuestoDtoCategoria] NUMERIC (18, 2) NULL,
    [PorcentajeDtoCategoria]        NUMERIC (18, 2) NULL,
    [Cantidad]                      INT             NULL,
    [UsuarioAlta]                   NVARCHAR (450)  NULL,
    [FechaAlta]                     DATETIME        NULL,
    [UsuarioEdit]                   NVARCHAR (450)  NULL,
    [FechaEdit]                     DATETIME        NULL,
    CONSTRAINT [PK_PresupuestosDetalle] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PresupuestosDetalle_Presupuestos] FOREIGN KEY ([PresupuestoId]) REFERENCES [dbo].[Presupuestos] ([Id]),
    CONSTRAINT [FK_PresupuestosDetalle_Productos] FOREIGN KEY ([ProductoId]) REFERENCES [dbo].[Productos] ([Id])
);

