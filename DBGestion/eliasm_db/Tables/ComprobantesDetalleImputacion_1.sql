﻿CREATE TABLE [eliasm_db].[ComprobantesDetalleImputacion] (
    [Id]                 NVARCHAR (150)  NOT NULL,
    [ComprobanteId]      NVARCHAR (150)  NULL,
    [DetalleId]          NVARCHAR (150)  NULL,
    [ProductoId]         NVARCHAR (150)  NULL,
    [Cantidad]           INT             NULL,
    [Precio]             NUMERIC (18, 2) NULL,
    [ImporteImputado]    NUMERIC (18, 2) NULL,
    [PorcentajeImputado] NUMERIC (18, 2) NULL,
    [Estado]             BIT             NULL,
    [Fecha]              DATETIME        NULL,
    [Usuario]            NVARCHAR (256)  NULL,
    [Entrega]            BIT             NULL,
    [FechaEntrega]       DATETIME        NULL,
    [UsuarioEntrega]     NVARCHAR (256)  NULL,
    [AutorizaEntrega]    BIT             NULL,
    [FechaAutoriza]      DATETIME        NULL,
    [UsuarioAutoriza]    NVARCHAR (256)  NULL,
    [Despacha]           BIT             NULL,
    [FechaDespacha]      DATETIME        NULL,
    [UsuarioDespacha]    NVARCHAR (256)  NULL,
    [RemitoId]           NVARCHAR (150)  NULL,
    [Devolucion]         BIT             NULL,
    [FechaDevolucion]    DATETIME        NULL,
    [UsuarioDevolucion]  NVARCHAR (256)  NULL,
    [MotivoDevolucion]   NVARCHAR (1000) NULL,
    [DevolucionId]       NVARCHAR (150)  NULL,
    CONSTRAINT [PK_ComprobantesDetalleImputacion] PRIMARY KEY CLUSTERED ([Id] ASC)
);

