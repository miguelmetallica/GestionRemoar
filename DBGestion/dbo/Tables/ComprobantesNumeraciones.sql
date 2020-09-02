CREATE TABLE [dbo].[ComprobantesNumeraciones] (
    [Id]                NVARCHAR (150) NOT NULL,
    [SucursalId]        NVARCHAR (150) NULL,
    [TipoComprobanteId] NVARCHAR (150) NULL,
    [Letra]             NCHAR (1)      NULL,
    [PuntoVenta]        INT            NULL,
    [Numero]            NUMERIC (10)   NULL,
    [Estado]            BIT            NULL,
    [FechaAlta]         DATETIME       NULL,
    [UsuarioAlta]       NVARCHAR (450) NULL,
    CONSTRAINT [PK_ComprobantesNumeraciones] PRIMARY KEY CLUSTERED ([Id] ASC)
);

