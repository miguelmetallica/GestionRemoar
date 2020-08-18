CREATE TABLE [dbo].[ParamConceptosIncluidos] (
    [ConceptoIncluidoId]     INT            NOT NULL,
    [ConceptoIncluidoCodigo] VARCHAR (5)    NOT NULL,
    [ConceptoIncluido]       VARCHAR (150)  NOT NULL,
    [Estado]                 BIT            NULL,
    [FechaAlta]              DATETIME       NULL,
    [UsuarioAlta]            NVARCHAR (256) NULL,
    CONSTRAINT [PK_ConceptosIncluidos] PRIMARY KEY CLUSTERED ([ConceptoIncluidoId] ASC)
);

