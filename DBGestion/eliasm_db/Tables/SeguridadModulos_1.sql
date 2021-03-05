CREATE TABLE [eliasm_db].[SeguridadModulos] (
    [ModuloId]    INT            NOT NULL,
    [Modulo]      VARCHAR (150)  NOT NULL,
    [Estado]      BIT            NOT NULL,
    [FechaAlta]   DATETIME       NULL,
    [UsuarioAlta] NVARCHAR (256) NULL,
    CONSTRAINT [PK_SeguridadModulos] PRIMARY KEY CLUSTERED ([ModuloId] ASC)
);

