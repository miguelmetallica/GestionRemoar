﻿CREATE TABLE [eliasm_db].[ParamPresupuestosDescuentos] (
    [Id]          NVARCHAR (150)  NOT NULL,
    [UsuarioId]   NVARCHAR (256)  NULL,
    [Descripcion] NVARCHAR (150)  NOT NULL,
    [Porcentaje]  NUMERIC (18, 2) NULL,
    [Estado]      BIT             NULL,
    CONSTRAINT [PK_ParamPresupuestosDescuentos] PRIMARY KEY CLUSTERED ([Id] ASC)
);

