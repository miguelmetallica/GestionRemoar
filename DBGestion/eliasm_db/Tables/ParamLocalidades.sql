CREATE TABLE [dbo].[ParamLocalidades] (
    [LocalidadId]     NVARCHAR (250) NOT NULL,
    [ProvinciaId]     NVARCHAR (150) NOT NULL,
    [LocalidadCodigo] VARCHAR (5)    NOT NULL,
    [Localidad]       VARCHAR (150)  NOT NULL,
    [CodigoPostal]    VARCHAR (8)    NOT NULL,
    [Estado]          BIT            NULL,
    [FechaAlta]       DATETIME       NULL,
    [UsuarioAlta]     NVARCHAR (256) NULL,
    CONSTRAINT [PK_ParamLocalidades] PRIMARY KEY CLUSTERED ([LocalidadId] ASC),
    CONSTRAINT [FK_ParamLocalidades_ParamProvincias] FOREIGN KEY ([ProvinciaId]) REFERENCES [dbo].[ParamProvincias] ([Id])
);

