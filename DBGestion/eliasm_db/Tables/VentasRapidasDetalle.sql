CREATE TABLE [eliasm_db].[VentasRapidasDetalle] (
    [Id]                NVARCHAR (150)  NOT NULL,
    [VentaRapidaId]     NVARCHAR (150)  NOT NULL,
    [ProductoId]        NVARCHAR (150)  NULL,
    [ProductoCodigo]    NVARCHAR (50)   NULL,
    [ProductoNombre]    NVARCHAR (150)  NULL,
    [Precio]            NUMERIC (18, 2) NULL,
    [PrecioSinImpuesto] NUMERIC (18, 2) NULL,
    [AceptaDescuento]   BIT             NULL,
    [Cantidad]          INT             NULL,
    [UsuarioAlta]       NVARCHAR (450)  NULL,
    [FechaAlta]         DATETIME        NULL,
    [UsuarioEdit]       NVARCHAR (450)  NULL,
    [FechaEdit]         DATETIME        NULL,
    CONSTRAINT [PK_VentasRapidasDetalle] PRIMARY KEY CLUSTERED ([Id] ASC)
);

