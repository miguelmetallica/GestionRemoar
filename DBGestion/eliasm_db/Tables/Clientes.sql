CREATE TABLE [dbo].[Clientes] (
    [Id]                NVARCHAR (150) NOT NULL,
    [Codigo]            NVARCHAR (50)  NOT NULL,
    [Apellido]          NVARCHAR (150) NULL,
    [Nombre]            NVARCHAR (150) NULL,
    [RazonSocial]       NVARCHAR (300) NOT NULL,
    [TipoDocumentoId]   NVARCHAR (150) NOT NULL,
    [NroDocumento]      NVARCHAR (8)   NOT NULL,
    [CuilCuit]          NVARCHAR (11)  NULL,
    [esPersonaJuridica] BIT            NULL,
    [FechaNacimiento]   DATETIME       NULL,
    [ProvinciaId]       NVARCHAR (150) NULL,
    [Localidad]         NVARCHAR (250) NULL,
    [CodigoPostal]      NVARCHAR (10)  NULL,
    [Calle]             NVARCHAR (500) NULL,
    [CalleNro]          NVARCHAR (50)  NULL,
    [PisoDpto]          NVARCHAR (100) NULL,
    [OtrasReferencias]  NVARCHAR (500) NULL,
    [Telefono]          NVARCHAR (50)  NULL,
    [Celular]           NVARCHAR (50)  NULL,
    [Email]             NVARCHAR (150) NULL,
    [TipoResponsableId] NVARCHAR (150) NULL,
    [CategoriaId]       NVARCHAR (150) NULL,
    [Estado]            BIT            NULL,
    [FechaAlta]         DATETIME       NULL,
    [UsuarioAlta]       NVARCHAR (256) NULL,
    CONSTRAINT [PK_Clientes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Clientes_ParamProvincias] FOREIGN KEY ([ProvinciaId]) REFERENCES [dbo].[ParamProvincias] ([Id]),
    CONSTRAINT [FK_Clientes_ParamTiposDocumentos] FOREIGN KEY ([TipoDocumentoId]) REFERENCES [dbo].[ParamTiposDocumentos] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Clientes_Tipo_Nro_Documento]
    ON [dbo].[Clientes]([TipoDocumentoId] ASC, [NroDocumento] ASC);

