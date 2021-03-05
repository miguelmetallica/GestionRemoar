CREATE TABLE [eliasm_db].[ParamPresupuestosEstados] (
    [Id]          NVARCHAR (150) NOT NULL,
    [Codigo]      NVARCHAR (5)   NOT NULL,
    [Descripcion] NVARCHAR (150) NOT NULL,
    [Estado]      BIT            NULL,
    CONSTRAINT [PK_ParamPresupuestosEstados] PRIMARY KEY CLUSTERED ([Id] ASC)
);

