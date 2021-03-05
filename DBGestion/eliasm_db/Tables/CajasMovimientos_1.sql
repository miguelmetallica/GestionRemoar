CREATE TABLE [eliasm_db].[CajasMovimientos] (
    [Id]               NVARCHAR (150)  NOT NULL,
    [CajaId]           NVARCHAR (150)  NULL,
    [Fecha]            DATETIME        NULL,
    [TipoMovimientoId] NVARCHAR (150)  NULL,
    [SucursalId]       NVARCHAR (150)  NULL,
    [ProveedorId]      NVARCHAR (150)  NULL,
    [NroComprobante]   NVARCHAR (50)   NULL,
    [Importe]          NUMERIC (18, 2) NULL,
    [Observaciones]    NVARCHAR (1000) NULL,
    [FechaAlta]        DATETIME        NULL,
    [UsuarioAlta]      NVARCHAR (256)  NULL,
    CONSTRAINT [PK_CajasMovimientos] PRIMARY KEY CLUSTERED ([Id] ASC)
);

