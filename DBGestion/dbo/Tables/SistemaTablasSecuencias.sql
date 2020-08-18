CREATE TABLE [dbo].[SistemaTablasSecuencias] (
    [Tabla]  VARCHAR (50) NOT NULL,
    [Numero] INT          NULL,
    CONSTRAINT [PK_SistemaTablasSecuencias] PRIMARY KEY CLUSTERED ([Tabla] ASC)
);

