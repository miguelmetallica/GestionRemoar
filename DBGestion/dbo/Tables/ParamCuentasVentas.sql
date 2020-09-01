CREATE TABLE [dbo].[ParamCuentasVentas] (
    [Id]          NVARCHAR (150) NOT NULL,
    [Codigo]      NVARCHAR (50)  NULL,
    [Descripcion] NVARCHAR (150) NULL,
    [Defecto]     BIT            NULL,
    [Estado]      BIT            NULL,
    [FechaAlta]   DATETIME       NULL,
    [UsuarioAlta] NVARCHAR (256) NULL,
    CONSTRAINT [PK_ParamCuentasVentas] PRIMARY KEY CLUSTERED ([Id] ASC)
);



