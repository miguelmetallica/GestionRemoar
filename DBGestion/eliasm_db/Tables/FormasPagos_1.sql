CREATE TABLE [eliasm_db].[FormasPagos] (
    [Id]          NVARCHAR (150) NOT NULL,
    [Codigo]      NVARCHAR (50)  NOT NULL,
    [Descripcion] NVARCHAR (150) NOT NULL,
    [Tipo]        CHAR (1)       NOT NULL,
    [FechaDesde]  DATETIME       NOT NULL,
    [FechaHasta]  DATETIME       NOT NULL,
    [Estado]      BIT            NULL,
    [FechaAlta]   DATETIME       NULL,
    [UsuarioAlta] NVARCHAR (256) NULL,
    CONSTRAINT [PK_FormasPagos] PRIMARY KEY CLUSTERED ([Id] ASC)
);

