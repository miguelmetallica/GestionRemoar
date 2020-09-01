﻿CREATE TABLE [dbo].[Productos] (
    [Id]                  NVARCHAR (150)  NOT NULL,
    [Codigo]              NVARCHAR (20)   NOT NULL,
    [TipoProductoId]      NVARCHAR (150)  NULL,
    [Producto]            NVARCHAR (150)  NOT NULL,
    [DescripcionCorta]    NVARCHAR (500)  NULL,
    [DescripcionLarga]    NVARCHAR (500)  NULL,
    [CodigoBarra]         NVARCHAR (100)  NULL,
    [Peso]                NUMERIC (18, 2) NULL,
    [DimencionesLongitud] NUMERIC (18, 2) NULL,
    [DimencionesAncho]    NUMERIC (18, 2) NULL,
    [DimencionesAltura]   NUMERIC (18, 2) NULL,
    [CuentaVentaId]       NVARCHAR (150)  NULL,
    [CuentaCompraId]      NVARCHAR (150)  NULL,
    [UnidadMedidaId]      NVARCHAR (150)  NULL,
    [AlicuotaId]          NVARCHAR (150)  NULL,
    [PrecioVenta]         NUMERIC (18, 2) NULL,
    [Estado]              BIT             NULL,
    [FechaAlta]           DATETIME        NULL,
    [UsuarioAlta]         NVARCHAR (256)  NULL,
    CONSTRAINT [PK_Productos] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Productos_ParamAlicuotas] FOREIGN KEY ([AlicuotaId]) REFERENCES [dbo].[ParamAlicuotas] ([Id]),
    CONSTRAINT [FK_Productos_ParamCuentasCompras] FOREIGN KEY ([CuentaCompraId]) REFERENCES [dbo].[ParamCuentasCompras] ([Id]),
    CONSTRAINT [FK_Productos_ParamCuentasVentas] FOREIGN KEY ([CuentaVentaId]) REFERENCES [dbo].[ParamCuentasVentas] ([Id]),
    CONSTRAINT [FK_Productos_ParamTiposProductos] FOREIGN KEY ([TipoProductoId]) REFERENCES [dbo].[ParamTiposProductos] ([Id]),
    CONSTRAINT [FK_Productos_ParamUnidadesMedidas1] FOREIGN KEY ([UnidadMedidaId]) REFERENCES [dbo].[ParamUnidadesMedidas] ([Id])
);



