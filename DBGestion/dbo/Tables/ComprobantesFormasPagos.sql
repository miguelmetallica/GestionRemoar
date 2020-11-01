﻿CREATE TABLE [dbo].[ComprobantesFormasPagos] (
    [Id]                     NVARCHAR (150)  NOT NULL,
    [ComprobanteId]          NVARCHAR (150)  NULL,
    [FormaPagoId]            NVARCHAR (150)  NULL,
    [FormaPagoCodigo]        NVARCHAR (5)    NULL,
    [FormaPago]              NVARCHAR (150)  NULL,
    [Importe]                NUMERIC (18, 2) NULL,
    [Cuota]                  INT             NULL,
    [Interes]                NUMERIC (18, 2) NULL,
    [Total]                  NUMERIC (18, 2) NULL,
    [TarjetaId]              NVARCHAR (150)  NULL,
    [TarjetaNombre]          NVARCHAR (150)  NULL,
    [TarjetaCliente]         NVARCHAR (300)  NULL,
    [TarjetaNumero]          NVARCHAR (50)   NULL,
    [TarjetaVenceMes]        INT             NULL,
    [TarjetaVenceAño]        INT             NULL,
    [TarjetaCodigoSeguridad] INT             NULL,
    [TarjetaEsDebito]        BIT             NULL,
    [ChequeBancoId]          NVARCHAR (150)  NULL,
    [ChequeBanco]            VARCHAR (150)   NULL,
    [ChequeNumero]           VARCHAR (50)    NULL,
    [ChequeFechaEmision]     DATETIME        NULL,
    [ChequeFechaVencimiento] DATETIME        NULL,
    [ChequeCuit]             VARCHAR (11)    NULL,
    [ChequeNombre]           VARCHAR (300)   NULL,
    [ChequeCuenta]           VARCHAR (50)    NULL,
    [Otros]                  NVARCHAR (500)  NULL,
    [FechaAlta]              DATETIME        NULL,
    [UsuarioAlta]            NVARCHAR (256)  NULL,
    CONSTRAINT [PK_ComprobantesFormasPagos] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ComprobantesFormasPagos_Comprobantes] FOREIGN KEY ([ComprobanteId]) REFERENCES [dbo].[Comprobantes] ([Id]),
    CONSTRAINT [FK_ComprobantesFormasPagos_FormasPagos] FOREIGN KEY ([FormaPagoId]) REFERENCES [dbo].[FormasPagos] ([Id]),
    CONSTRAINT [FK_ComprobantesFormasPagos_ParamBancos] FOREIGN KEY ([ChequeBancoId]) REFERENCES [dbo].[ParamBancos] ([Id])
);





