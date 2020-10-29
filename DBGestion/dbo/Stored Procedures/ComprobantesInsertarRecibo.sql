CREATE PROCEDURE [dbo].[ComprobantesInsertarRecibo]	
	@ClienteId nvarchar(150),
	@ImporteCancela NUMERIC(18,2),
	@Observaciones NVARCHAR(500) = '',
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @TotalPagado NUMERIC(18,2) = 0
	DECLARE @ComprobanteId NVARCHAR(150) = NULL
	DECLARE @Saldo NUMERIC(18,2) = 0
	DECLARE @TotalSaldo NUMERIC(18,2) = 0

	DECLARE @Id nvarchar(150) = NEWID();
	DECLARE @SucursalId nvarchar(150);
	DECLARE @SucursalCodigo nvarchar(5);
	DECLARE @TipoComprobanteId nvarchar(150);
	DECLARE @TipoComprobante nvarchar(150);
	DECLARE @TipoComprobanteCodigo nvarchar(150);
	DECLARE @Letra nchar(1);
	DECLARE @PtoVenta int;
	DECLARE @FechaComprobante datetime;
	DECLARE @ConceptoIncluidoId nvarchar(150);
	DECLARE @ConceptoIncluidoCodigo nvarchar(5);
	DECLARE @ConceptoIncluido nvarchar(150);
	DECLARE @PeriodoFacturadoDesde datetime;
	DECLARE @PeriodoFacturadoHasta datetime;
	DECLARE @FechaVencimiento datetime;
	DECLARE @TipoResponsableId nvarchar(150);
	DECLARE @TipoResponsableCodigo nvarchar(5);
	DECLARE @TipoResponsable nvarchar(150);
	DECLARE @PresupuestoId nvarchar(150);
	DECLARE @ClienteCodigo nvarchar(50);
	DECLARE @TipoDocumentoId nvarchar(150);
	DECLARE @TipoDocumentoCodigo nvarchar(5);
	DECLARE @TipoDocumento nvarchar(15);
	DECLARE @NroDocumento nvarchar(15);
	DECLARE @CuilCuit nvarchar(20);
	DECLARE @RazonSocial nvarchar(300);
	DECLARE @ProvinciaId nvarchar(150);
	DECLARE @ProvinciaCodigo nvarchar(5);
	DECLARE @Provincia nvarchar(150);
	DECLARE @Localidad nvarchar(150);
	DECLARE @CodigoPostal nvarchar(10);
	DECLARE @Calle nvarchar(500);
	DECLARE @CalleNro nvarchar(50);
	DECLARE @PisoDpto nvarchar(100);
	DECLARE @OtrasReferencias nvarchar(500);
	DECLARE @Email nvarchar(150);
	DECLARE @Telefono nvarchar(50);
	DECLARE @Celular nvarchar(50);
	DECLARE @SubTotal numeric(18,2);
	DECLARE @ImporteTributos numeric(18,2) = 0;
	DECLARE @Total numeric(18,2);
	DECLARE @TotalSinImpuesto numeric(18,2);
	DECLARE @TotalSinDescuento numeric(18,2);
	DECLARE @TotalSinImpuestoSinDescuento numeric(18,2);
	DECLARE @DescuentoPorcentaje numeric(18,2);
	DECLARE @DescuentoTotal numeric(18,2);
	DECLARE @DescuentoSinImpuesto numeric(18,2);
	DECLARE @Confirmado bit;
	DECLARE @Cobrado bit;
	DECLARE @Anulado bit;
	DECLARE @FechaAnulacion datetime;
	DECLARE @TipoComprobanteAnulaId nvarchar(150);
	DECLARE @TipoComprobanteAnulaCodigo nvarchar(5);
	DECLARE @TipoComprobanteAnula nvarchar(150);
	DECLARE @LetraAnula char(1);
	DECLARE @PtoVtaAnula int;
	DECLARE @NumeroAnula numeric(12,0);
	DECLARE @Estado bit = 1;
	
	DECLARE @Numero numeric(10);
	DECLARE @Codigo nvarchar(20);

	
	SELECT @SucursalId = S.Id, @SucursalCodigo = S.Codigo
	FROM AspNetUsers U
	INNER JOIN Sucursales S ON S.Id = U.SucursalId
	WHERE UserName = @Usuario

	IF @SucursalCodigo IS NULL
		SET @SucursalCodigo = '000';

	SELECT @TipoComprobanteId = T.Id,
			@TipoComprobante = T.Descripcion,
			@TipoComprobanteCodigo = T.Codigo,
			@Letra = N.Letra,
			@PtoVenta = N.PuntoVenta
	FROM SistemaConfiguraciones C
	INNER JOIN ParamTiposComprobantes T ON T.Codigo = C.Valor
	INNER JOIN ComprobantesNumeraciones N ON N.TipoComprobanteId = T.Id
	WHERE C.Configuracion = 'COMPROBANTES_RECIBO_A'
	AND N.Estado = 1

	SELECT TOP 1 @ConceptoIncluidoId = P.Id,
				@ConceptoIncluidoCodigo = P.Codigo,
				@ConceptoIncluido = Descripcion
	FROM ParamConceptosIncluidos P
	WHERE P.Defecto = 1

	SET @FechaComprobante = DATEADD(HH,4,GETDATE());

	IF ISNULL(@TipoResponsableId,'') = ''
	BEGIN
		SELECT TOP 1 
			@TipoResponsableId = P.Id,
			@TipoResponsableCodigo = P.Codigo,
			@TipoResponsable = Descripcion
		FROM ParamTiposResponsables P
		WHERE P.Defecto = 1
	END
	ELSE
	BEGIN
		SELECT TOP 1 
			@TipoResponsableId = P.Id,
			@TipoResponsableCodigo = P.Codigo,
			@TipoResponsable = Descripcion
		FROM ParamTiposResponsables P
		WHERE P.Id = @TipoResponsableId
	END

	SELECT @ClienteCodigo = C.Codigo,
		@TipoDocumentoId = C.TipoDocumentoId,
		@TipoDocumentoCodigo = D.Codigo,
		@TipoDocumento = D.Descripcion,
		@NroDocumento = C.NroDocumento,
		@CuilCuit = C.CuilCuit,
		@RazonSocial = C.RazonSocial,
		@ProvinciaId = C.ProvinciaId,
		@ProvinciaCodigo = PR.Codigo,
		@Provincia = PR.Descripcion,
		@Localidad = C.Localidad,
		@CodigoPostal = C.CodigoPostal,
		@Calle = C.Calle,
		@CalleNro = C.CalleNro,
		@PisoDpto = C.PisoDpto,
		@OtrasReferencias = C.OtrasReferencias,
		@Email = C.Email,
		@Telefono = C.Telefono,
		@Celular = C.Celular
	FROM Clientes C
	LEFT JOIN ParamTiposDocumentos D ON D.Id = C.TipoDocumentoId
	LEFT JOIN ParamProvincias PR ON PR.Id = C.ProvinciaId
	WHERE C.Id = @ClienteId

	SELECT @TotalSaldo = SUM(Saldo)	
	FROM (
	SELECT P.Codigo, P.Id,P.FechaComprobante,P.Total,P.Total - ISNULL((SELECT SUM(ISNULL(I.ImporteCancela,0)) FROM ComprobantesImputacion I WHERE I.ComprobanteId = P.Id),0)Saldo
	FROM Comprobantes P
	WHERE P.ClienteId = @ClienteId AND 
		P.Codigo IS NOT NULL
	)P
	WHERE P.Saldo <> 0

	IF @TotalSaldo > 0
	BEGIN
		BEGIN TRAN			
			WHILE @ImporteCancela >= 0
			BEGIN
				SET @Saldo = 0
				SET @ComprobanteId = NULL
	
				SELECT TOP 1 @ComprobanteId = Id, @Saldo = Saldo	
				FROM (
				SELECT P.Codigo, P.Id,P.FechaComprobante,P.Total,P.Total - ISNULL((SELECT SUM(ISNULL(I.ImporteCancela,0)) FROM ComprobantesImputacion I WHERE I.ComprobanteId = P.Id),0)Saldo
				FROM Comprobantes P
				WHERE P.ClienteId = @ClienteId AND 
					P.Codigo IS NOT NULL
				)P
				WHERE P.Saldo <> 0
				ORDER BY P.FechaComprobante
	
				SET @ImporteCancela = @ImporteCancela - ABS(@Saldo)
	
				IF @Saldo > 0 
				BEGIN
					IF @ImporteCancela > 0 
					BEGIN
						INSERT INTO ComprobantesImputacion
						SELECT NEWID() Id,@ComprobanteId ComprobanteId,@Id ComprobanteCancelaId,@Saldo ImporteCancela,@FechaComprobante FechaCancela,1 Estado,@Observaciones Observaciones,DATEADD(HH,4,GETDATE()) FechaAlta,UPPER(@Usuario) UsuarioAlta

						SET @TotalPagado = @TotalPagado + @Saldo
					END
					ELSE
					BEGIN
						INSERT INTO ComprobantesImputacion
						SELECT NEWID() Id,@ComprobanteId ComprobanteId,@Id ComprobanteCancelaId,@Saldo - ABS(@ImporteCancela) ImporteCancela,@FechaComprobante FechaCancela,1 Estado,@Observaciones Observaciones,DATEADD(HH,4,GETDATE()) FechaAlta,UPPER(@Usuario) UsuarioAlta				

						SET @TotalPagado = @TotalPagado + @Saldo - ABS(@ImporteCancela)
					END
				END
				ELSE
					SET @ImporteCancela = -1

			END

			SELECT @Total = SUM(ImporteCancela),
				@TotalSinImpuesto = SUM(ImporteCancelaSinImp),
				@TotalSinDescuento = 0,
				@TotalSinImpuestoSinDescuento = 0,
				@DescuentoPorcentaje = 0,
				@DescuentoTotal = 0,
				@DescuentoSinImpuesto = 0,
				@ImporteTributos = 0
			FROM(
				SELECT 
				C.Codigo,
				I.ImporteCancela,
				SUM(D.SubTotal)SubTotal,
				SUM(D.SubTotalSinIva)SubTotalSinIva,
				I.ImporteCancela - (I.ImporteCancela * SUM(D.AlicuotaProcentaje/100))ImporteCancelaSinImp
				FROM ComprobantesImputacion I
				INNER JOIN Comprobantes C ON C.Id = I.ComprobanteId
				INNER JOIN ComprobantesDetalle D ON D.ComprobanteId = C.Id
				WHERE I.ComprobanteCancelaId = @Id
				GROUP BY C.Codigo,I.ImporteCancela
			)P

			EXEC @Numero = NextNumberComprobante @SucursalId,@TipoComprobanteId
			SET @Codigo = @Letra + RIGHT('0000' + RTRIM(LTRIM(CONVERT(VARCHAR(4),@PtoVenta))),3) + RIGHT('00000000' + RTRIM(LTRIM(CONVERT(VARCHAR(8),@Numero))),8)

			INSERT INTO Comprobantes
			SELECT @Id,@Codigo,@TipoComprobanteId,@TipoComprobante,@TipoComprobanteCodigo,@PresupuestoId,@Letra,@PtoVenta,@Numero,
					@FechaComprobante,@ConceptoIncluidoId,@ConceptoIncluidoCodigo,@ConceptoIncluido,@PeriodoFacturadoDesde,@PeriodoFacturadoHasta,
					@FechaVencimiento,@TipoResponsableId,@TipoResponsableCodigo,@TipoResponsable,@ClienteId,@ClienteCodigo,@TipoDocumentoId,@TipoDocumentoCodigo,
					@TipoDocumento,@NroDocumento,@CuilCuit,@RazonSocial,@ProvinciaId,@ProvinciaCodigo,@Provincia,@Localidad,@CodigoPostal,@Calle,@CalleNro,@PisoDpto,
					@OtrasReferencias,@Email,@Telefono,@Celular,@Total,@TotalSinImpuesto,@TotalSinDescuento,@TotalSinImpuestoSinDescuento,@DescuentoPorcentaje,
					@DescuentoTotal,@DescuentoSinImpuesto,@ImporteTributos,@Observaciones,@Confirmado,@Cobrado,@Anulado,@FechaAnulacion,@TipoComprobanteAnulaId,
					@TipoComprobanteAnulaCodigo,@TipoComprobanteAnula,@LetraAnula,@PtoVtaAnula,@NumeroAnula,@Estado,DATEADD(HH,4,GETDATE()),UPPER(@Usuario)

	
		COMMIT;
	END;


	SELECT 1 Id

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 
		ROLLBACK;

	DECLARE @ErrorMessage NVARCHAR(4000);  
	DECLARE @ErrorSeverity INT;  
	DECLARE @ErrorState INT;  

	SELECT @ErrorMessage = ERROR_MESSAGE(),@ErrorSeverity = ERROR_SEVERITY(),@ErrorState = ERROR_STATE();  

	RAISERROR (@ErrorMessage, -- Message text.  
				@ErrorSeverity, -- Severity.  
				@ErrorState -- State.  
			);  
END CATCH
;