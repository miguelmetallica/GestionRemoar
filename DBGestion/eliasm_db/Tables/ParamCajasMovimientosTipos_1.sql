﻿CREATE TABLE [eliasm_db].[ParamCajasMovimientosTipos] (
    [Id]          NVARCHAR (150) NOT NULL,
    [Codigo]      NVARCHAR (5)   NULL,
    [Descripcion] NVARCHAR (150) NULL,
    [esDebe]      BIT            NULL,
    [Estado]      BIT            NULL,
    [FechaAlta]   DATETIME       NULL,
    [UsuarioAlta] NVARCHAR (256) NULL,
    CONSTRAINT [PK_ParamCajasMovimientosTipos] PRIMARY KEY CLUSTERED ([Id] ASC)
);
