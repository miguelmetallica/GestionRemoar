CREATE TABLE [dbo].[Sucursales] (
    [Id]               NVARCHAR (150) NOT NULL,
    [Codigo]           NVARCHAR (5)   NOT NULL,
    [Nombre]           NVARCHAR (250) NOT NULL,
    [ProvinciaId]      NVARCHAR (150) NULL,
    [Localidad]        NVARCHAR (250) NULL,
    [CodigoPostal]     NVARCHAR (10)  NULL,
    [Calle]            NVARCHAR (500) NULL,
    [CalleNro]         NVARCHAR (10)  NULL,
    [PisoDpto]         NVARCHAR (10)  NULL,
    [OtrasReferencias] NVARCHAR (500) NULL,
    [Telefono]         NVARCHAR (50)  NULL,
    [Email]            NVARCHAR (150) NULL,
    [Estado]           BIT            NOT NULL,
    [FechaAlta]        DATETIME       NULL,
    [UsuarioAlta]      NVARCHAR (256) NULL,
    CONSTRAINT [PK_Sucursales] PRIMARY KEY CLUSTERED ([Id] ASC)
);

