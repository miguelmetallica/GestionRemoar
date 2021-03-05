CREATE TABLE [eliasm_db].[ParamAlicuotas] (
    [Id]          NVARCHAR (150)  NOT NULL,
    [Codigo]      NVARCHAR (5)    NULL,
    [Descripcion] NVARCHAR (150)  NULL,
    [Porcentaje]  NUMERIC (18, 2) NULL,
    [Defecto]     BIT             NULL,
    [Estado]      BIT             NULL,
    [FechaAlta]   DATETIME        NULL,
    [UsuarioAlta] NVARCHAR (256)  NULL,
    CONSTRAINT [PK_ParamAlicuotas] PRIMARY KEY CLUSTERED ([Id] ASC)
);

