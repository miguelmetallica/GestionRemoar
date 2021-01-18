CREATE TABLE [dbo].[ParamCategorias] (
    [Id]                  NVARCHAR (150)  NOT NULL,
    [PadreId]             NVARCHAR (150)  NULL,
    [Codigo]              NVARCHAR (5)    NULL,
    [Descripcion]         NVARCHAR (150)  NULL,
    [Defecto]             BIT             NULL,
    [DescuentoPorcentaje] NUMERIC (18, 2) NULL,
    [Estado]              BIT             NULL,
    CONSTRAINT [PK_ParamCategorias] PRIMARY KEY CLUSTERED ([Id] ASC)
);

