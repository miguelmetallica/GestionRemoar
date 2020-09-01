CREATE TABLE [dbo].[ParamProvincias] (
    [Id]          NVARCHAR (150) NOT NULL,
    [Codigo]      NVARCHAR (50)  NOT NULL,
    [Descripcion] NVARCHAR (150) NOT NULL,
    [Defecto]     BIT            NULL,
    [Estado]      BIT            NULL,
    [FechaAlta]   DATETIME       NULL,
    [UsuarioAlta] NVARCHAR (256) NULL,
    CONSTRAINT [PK_ParamProvincias] PRIMARY KEY CLUSTERED ([Id] ASC)
);



