CREATE TABLE [eliasm_db].[ParamPaises] (
    [PaisId]      INT            NOT NULL,
    [PaisCodigo]  VARCHAR (5)    NOT NULL,
    [Pais]        VARCHAR (150)  NOT NULL,
    [Estado]      BIT            NULL,
    [FechaAlta]   DATETIME       NULL,
    [UsuarioAlta] NVARCHAR (256) NULL,
    CONSTRAINT [PK_Paises] PRIMARY KEY CLUSTERED ([PaisId] ASC)
);

