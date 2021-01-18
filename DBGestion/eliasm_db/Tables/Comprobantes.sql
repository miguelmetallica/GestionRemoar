﻿CREATE TABLE [dbo].[Comprobantes] (
    [Id]                           NVARCHAR (150)  NOT NULL,
    [Codigo]                       NVARCHAR (20)   NULL,
    [TipoComprobanteId]            NVARCHAR (150)  NULL,
    [TipoComprobante]              NVARCHAR (150)  NULL,
    [TipoComprobanteCodigo]        NVARCHAR (50)   NULL,
    [PresupuestoId]                NVARCHAR (150)  NULL,
    [Letra]                        CHAR (1)        NULL,
    [PtoVenta]                     INT             NULL,
    [Numero]                       NUMERIC (12)    NULL,
    [FechaComprobante]             DATETIME        NULL,
    [ConceptoIncluidoId]           NVARCHAR (150)  NULL,
    [ConceptoIncluidoCodigo]       NVARCHAR (5)    NULL,
    [ConceptoIncluido]             NVARCHAR (150)  NULL,
    [PeriodoFacturadoDesde]        DATETIME        NULL,
    [PeriodoFacturadoHasta]        DATETIME        NULL,
    [FechaVencimiento]             DATETIME        NULL,
    [TipoResponsableId]            NVARCHAR (150)  NULL,
    [TipoResponsableCodigo]        NVARCHAR (5)    NULL,
    [TipoResponsable]              NVARCHAR (150)  NULL,
    [ClienteId]                    NVARCHAR (150)  NULL,
    [ClienteCodigo]                NVARCHAR (50)   NULL,
    [TipoDocumentoId]              NVARCHAR (150)  NULL,
    [TipoDocumentoCodigo]          NVARCHAR (5)    NULL,
    [TipoDocumento]                NVARCHAR (15)   NULL,
    [NroDocumento]                 NVARCHAR (15)   NULL,
    [CuilCuit]                     NVARCHAR (50)   NULL,
    [RazonSocial]                  NVARCHAR (300)  NULL,
    [ProvinciaId]                  NVARCHAR (150)  NULL,
    [ProvinciaCodigo]              NVARCHAR (5)    NULL,
    [Provincia]                    NVARCHAR (150)  NULL,
    [Localidad]                    NVARCHAR (150)  NULL,
    [CodigoPostal]                 NVARCHAR (10)   NULL,
    [Calle]                        NVARCHAR (500)  NULL,
    [CalleNro]                     NVARCHAR (50)   NULL,
    [PisoDpto]                     NVARCHAR (100)  NULL,
    [OtrasReferencias]             NVARCHAR (500)  NULL,
    [Email]                        NVARCHAR (150)  NULL,
    [Telefono]                     NVARCHAR (50)   NULL,
    [Celular]                      NVARCHAR (50)   NULL,
    [Total]                        NUMERIC (18, 2) NULL,
    [TotalSinImpuesto]             NUMERIC (18, 2) NULL,
    [TotalSinDescuento]            NUMERIC (18, 2) NULL,
    [TotalSinImpuestoSinDescuento] NUMERIC (18, 2) NULL,
    [DescuentoPorcentaje]          NUMERIC (18, 2) NULL,
    [DescuentoTotal]               NUMERIC (18, 2) NULL,
    [DescuentoSinImpuesto]         NUMERIC (18, 2) NULL,
    [ImporteTributos]              NUMERIC (18, 2) NULL,
    [Observaciones]                NVARCHAR (500)  NULL,
    [Confirmado]                   BIT             NULL,
    [Cobrado]                      BIT             NULL,
    [ComprobanteAnulaId]           NVARCHAR (150)  NULL,
    [Anulado]                      BIT             NULL,
    [FechaAnulacion]               DATETIME        NULL,
    [TipoComprobanteAnulaId]       NVARCHAR (150)  NULL,
    [TipoComprobanteAnulaCodigo]   NVARCHAR (5)    NULL,
    [TipoComprobanteAnula]         NVARCHAR (150)  NULL,
    [CodigoAnula]                  NVARCHAR (20)   NULL,
    [LetraAnula]                   CHAR (1)        NULL,
    [PtoVtaAnula]                  INT             NULL,
    [NumeroAnula]                  NUMERIC (12)    NULL,
    [UsuarioAnula]                 NVARCHAR (256)  NULL,
    [TipoComprobanteFiscal]        NVARCHAR (2)    NULL,
    [LetraFiscal]                  CHAR (1)        NULL,
    [PtoVentaFiscal]               INT             NULL,
    [NumeroFiscal]                 NUMERIC (12)    NULL,
    [Estado]                       BIT             NULL,
    [FechaAlta]                    DATETIME        NULL,
    [UsuarioAlta]                  NVARCHAR (256)  NULL,
    CONSTRAINT [PK_Comprobantes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Comprobantes_Clientes] FOREIGN KEY ([ClienteId]) REFERENCES [dbo].[Clientes] ([Id]),
    CONSTRAINT [FK_Comprobantes_ParamConceptosIncluidos] FOREIGN KEY ([ConceptoIncluidoId]) REFERENCES [dbo].[ParamConceptosIncluidos] ([Id]),
    CONSTRAINT [FK_Comprobantes_ParamProvincias] FOREIGN KEY ([ProvinciaId]) REFERENCES [dbo].[ParamProvincias] ([Id]),
    CONSTRAINT [FK_Comprobantes_ParamTiposComprobantes] FOREIGN KEY ([TipoComprobanteId]) REFERENCES [dbo].[ParamTiposComprobantes] ([Id]),
    CONSTRAINT [FK_Comprobantes_ParamTiposComprobantes1] FOREIGN KEY ([TipoComprobanteAnulaId]) REFERENCES [dbo].[ParamTiposComprobantes] ([Id]),
    CONSTRAINT [FK_Comprobantes_ParamTiposDocumentos] FOREIGN KEY ([TipoDocumentoId]) REFERENCES [dbo].[ParamTiposDocumentos] ([Id]),
    CONSTRAINT [FK_Comprobantes_ParamTiposResponsables] FOREIGN KEY ([TipoResponsableId]) REFERENCES [dbo].[ParamTiposResponsables] ([Id]),
    CONSTRAINT [FK_Comprobantes_Presupuestos] FOREIGN KEY ([PresupuestoId]) REFERENCES [dbo].[Presupuestos] ([Id])
);

