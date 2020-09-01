CREATE TABLE [dbo].[ParamPresupuestosDescuentos] (
    [Id]          NVARCHAR (150)  NOT NULL,
    [Descripcion] NVARCHAR (150)  NOT NULL,
    [Porcentaje]  NUMERIC (18, 2) NULL,
    [Estado]      BIT             NULL,
    CONSTRAINT [PK_ParamPresupuestosDescuentos] PRIMARY KEY CLUSTERED ([Id] ASC)
);

