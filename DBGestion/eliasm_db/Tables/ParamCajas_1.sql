CREATE TABLE [eliasm_db].[ParamCajas] (
    [Id]          NVARCHAR (150) NOT NULL,
    [Codigo]      NVARCHAR (5)   NULL,
    [SucursalId]  NVARCHAR (150) NULL,
    [Estado]      BIT            NULL,
    [FechaAlta]   DATETIME       NULL,
    [UsuarioAlta] NVARCHAR (250) NULL,
    CONSTRAINT [PK_ParamCajas] PRIMARY KEY CLUSTERED ([Id] ASC)
);

