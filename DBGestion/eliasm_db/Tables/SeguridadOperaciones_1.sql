CREATE TABLE [eliasm_db].[SeguridadOperaciones] (
    [OperacionId] INT            NOT NULL,
    [Operacion]   VARCHAR (150)  NOT NULL,
    [ModuloId]    INT            NULL,
    [Estado]      BIT            NOT NULL,
    [FechaAlta]   DATETIME       NULL,
    [UsuarioAlta] NVARCHAR (256) NULL,
    CONSTRAINT [PK_SeguridadOperaciones] PRIMARY KEY CLUSTERED ([OperacionId] ASC),
    CONSTRAINT [FK_SeguridadOperaciones_SeguridadModulos] FOREIGN KEY ([ModuloId]) REFERENCES [eliasm_db].[SeguridadModulos] ([ModuloId])
);

