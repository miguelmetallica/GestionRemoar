CREATE TABLE [dbo].[SistemaConfiguraciones] (
    [Id]            NVARCHAR (150) NOT NULL,
    [Configuracion] NVARCHAR (150) NOT NULL,
    [Valor]         NVARCHAR (150) NOT NULL,
    [Estado]        BIT            NULL,
    CONSTRAINT [PK_SistemaConfiguraciones] PRIMARY KEY CLUSTERED ([Id] ASC)
);

