CREATE PROCEDURE [eliasm_db].[ComprobantesInsertarRecibo]	
	@ComprobanteId nvarchar(150),	
	@Observaciones NVARCHAR(500) = '',
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @Id nvarchar(150) = NEWID();
	DECLARE @SucursalId nvarchar(150);
	DECLARE @SucursalCodigo nvarchar(5);
	DECLARE @TipoComprobanteId nvarchar(150);
	DECLARE @TipoComprobante nvarchar(150);
	DECLARE @TipoComprobanteCodigo nvarchar(150);
	DECLARE @TipoComprobanteEsDebe nvarchar(150);
	DECLARE @PresupuestoId nvarchar(150);
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
	DECLARE @ClienteId nvarchar(150);
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
	DECLARE @ImporteTributos numeric(18,2);
	DECLARE @Total numeric(18,2)= 0;
	DECLARE @TotalSinImpuesto numeric(18,2)= 0;
	DECLARE @TotalSinDescuento numeric(18,2)= 0;
	DECLARE @TotalSinImpuestoSinDescuento numeric(18,2)= 0;
	DECLARE @DescuentoPorcentaje numeric(18,2)= 0;
	DECLARE @DescuentoTotal numeric(18,2)= 0;
	DECLARE @DescuentoSinImpuesto numeric(18,2)= 0;
	DECLARE @Confirmado bit;
	DECLARE @Cobrado bit;
	DECLARE @ComprobanteAnulaId nvarchar(150);
	DECLARE @Anulado bit;
	DECLARE @FechaAnulacion datetime;	
	DECLARE @TipoComprobanteAnulaId nvarchar(150);
	DECLARE @TipoComprobanteAnulaCodigo nvarchar(5);
	DECLARE @TipoComprobanteAnula nvarchar(150);
	DECLARE @CodigoAnula nvarchar(20);
	DECLARE @LetraAnula char(1);
	DECLARE @PtoVtaAnula int;
	DECLARE @NumeroAnula numeric(12,0);
	DECLARE @UsuarioAnula nvarchar(256);
	DECLARE @TipoComprobanteFiscal nvarchar(2);
	DECLARE @LetraFiscal char(1);
	DECLARE @PtoVentaFiscal int;
	DECLARE @NumeroFiscal numeric(12,0);
	DECLARE @Estado bit = 1;
	
	DECLARE @Numero numeric(10);
	DECLARE @Codigo nvarchar(20);

	DECLARE @TotalSaldo numeric(18,2) = 0;
	DECLARE @ImporteCancela NUMERIC(18,2) = 0;
	
	SELECT @SucursalId = S.Id, @SucursalCodigo = S.Codigo
	FROM AspNetUsers U
	INNER JOIN Sucursales S ON S.Id = U.SucursalId
	WHERE UserName = @Usuario

	IF @SucursalCodigo IS NULL
		SET @SucursalCodigo = '000';

	SELECT @TipoComprobanteId = T.Id,
			@TipoComprobante = T.Descripcion,
			@TipoComprobanteCodigo = T.Codigo,
			@TipoComprobanteEsDebe = isnull(T.esDebe,0),
			@Letra = N.Letra,
			@PtoVenta = N.PuntoVenta
	FROM SistemaConfiguraciones C
	INNER JOIN ParamTiposComprobantes T ON T.Codigo = C.Valor
	INNER JOIN ComprobantesNumeraciones N ON N.TipoComprobanteId = T.Id
	WHERE C.Configuracion = 'COMPROBANTES_RECIBO'
	AND N.SucursalId = @SucursalId
	AND N.Estado = 1

	SELECT TOP 1 @ConceptoIncluidoId = P.Id,
				@ConceptoIncluidoCodigo = P.Codigo,
				@ConceptoIncluido = Descripcion
	FROM ParamConceptosIncluidos P
	WHERE P.Defecto = 1

	SELECT @ClienteId = ClienteId,
			@PresupuestoId = PresupuestoId
	FROM Comprobantes P
	WHERE P.Id = @ComprobanteId

	SET @FechaComprobante = DATEADD(HH,4,GETDATE());

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
		@Celular = C.Celular,
		@TipoResponsableId = TipoResponsableId
	FROM Clientes C
	LEFT JOIN ParamTiposDocumentos D ON D.Id = C.TipoDocumentoId
	LEFT JOIN ParamProvincias PR ON PR.Id = C.ProvinciaId
	WHERE C.Id = @ClienteId

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

	SELECT @ImporteCancela = SUM(CASE WHEN FormaPagoTipo = 'E' then Total else Importe * Cuota end) 
	FROM ComprobantesFormasPagosTmp
	WHERE Comprobanteid = @ComprobanteId

	SELECT @TotalSaldo = eliasm_db.saldo(@PresupuestoId) - @ImporteCancela
	

	SET @Total = @ImporteCancela;
	
	SET @TotalSinImpuesto = @ImporteCancela;
	
	IF @TotalSaldo >= 0
	BEGIN
		BEGIN TRAN			
			EXEC @Numero = NextNumberComprobante @SucursalId,@TipoComprobanteId
			SET @Codigo = @Letra + RIGHT('0000' + RTRIM(LTRIM(CONVERT(VARCHAR(4),@PtoVenta))),3) + RIGHT('00000000' + RTRIM(LTRIM(CONVERT(VARCHAR(8),@Numero))),8)

			INSERT INTO Comprobantes(Id,
									Codigo,
									TipoComprobanteId,
									TipoComprobante,
									TipoComprobanteCodigo,
									TipoComprobanteEsDebe,
									PresupuestoId,
									Letra,
									PtoVenta,
									Numero,
									FechaComprobante,
									ConceptoIncluidoId,
									ConceptoIncluidoCodigo,
									ConceptoIncluido,
									PeriodoFacturadoDesde,
									PeriodoFacturadoHasta,
									FechaVencimiento,
									TipoResponsableId,
									TipoResponsableCodigo,
									TipoResponsable,
									ClienteId,
									ClienteCodigo,
									TipoDocumentoId,
									TipoDocumentoCodigo,
									TipoDocumento,
									NroDocumento,
									CuilCuit,
									RazonSocial,
									ProvinciaId,
									ProvinciaCodigo,
									Provincia,
									Localidad,
									CodigoPostal,
									Calle,
									CalleNro,
									PisoDpto,
									OtrasReferencias,
									Email,
									Telefono,
									Celular,
									Total,
									TotalSinImpuesto,
									TotalSinDescuento,
									TotalSinImpuestoSinDescuento,
									DescuentoPorcentaje,
									DescuentoTotal,
									DescuentoSinImpuesto,
									ImporteTributos,
									Observaciones,
									Confirmado,
									Cobrado,
									ComprobanteAnulaId,
									Anulado,
									FechaAnulacion,
									TipoComprobanteAnulaId,
									TipoComprobanteAnulaCodigo,
									TipoComprobanteAnula,
									CodigoAnula,
									LetraAnula,
									PtoVtaAnula,
									NumeroAnula,
									UsuarioAnula,
									TipoComprobanteFiscal,
									LetraFiscal,
									PtoVentaFiscal,
									NumeroFiscal,
									Estado,
									FechaAlta,
									UsuarioAlta,
									SucursalId
									)
			SELECT @Id,
				@Codigo,
				@TipoComprobanteId,
				@TipoComprobante,
				@TipoComprobanteCodigo,
				@TipoComprobanteEsDebe,
				@PresupuestoId,
				@Letra,
				@PtoVenta,
				@Numero,
				@FechaComprobante,
				@ConceptoIncluidoId,
				@ConceptoIncluidoCodigo,
				@ConceptoIncluido,
				@PeriodoFacturadoDesde,
				@PeriodoFacturadoHasta,
				@FechaVencimiento,
				@TipoResponsableId,
				@TipoResponsableCodigo,
				@TipoResponsable,
				@ClienteId,
				@ClienteCodigo,
				@TipoDocumentoId,
				@TipoDocumentoCodigo,
				@TipoDocumento,
				@NroDocumento,
				@CuilCuit,
				@RazonSocial,
				@ProvinciaId,
				@ProvinciaCodigo,
				@Provincia,
				@Localidad,
				@CodigoPostal,
				@Calle,
				@CalleNro,
				@PisoDpto,
				@OtrasReferencias,
				@Email,
				@Telefono,
				@Celular,
				@Total,
				@TotalSinImpuesto,
				@TotalSinDescuento,
				@TotalSinImpuestoSinDescuento,
				@DescuentoPorcentaje,
				@DescuentoTotal,
				@DescuentoSinImpuesto,
				@ImporteTributos,
				@Observaciones,
				@Confirmado,
				@Cobrado,
				@ComprobanteAnulaId,
				@Anulado,
				@FechaAnulacion,
				@TipoComprobanteAnulaId,
				@TipoComprobanteAnulaCodigo,
				@TipoComprobanteAnula,
				@CodigoAnula,
				@LetraAnula,
				@PtoVtaAnula,
				@NumeroAnula,
				@UsuarioAnula,
				@TipoComprobanteFiscal,
				@LetraFiscal,
				@PtoVentaFiscal,
				@NumeroFiscal,
				@Estado,
				DATEADD(HH,4,GETDATE()),
				@Usuario,
				@SucursalId

			INSERT INTO ComprobantesFormasPagos(Id,
												ComprobanteId,
												FormaPagoId,
												FormaPagoCodigo,
												FormaPagoTipo,
												FormaPago,
												Importe,
												Cuota,
												Interes,
												Descuento,
												Total,
												TarjetaId,
												TarjetaNombre,
												TarjetaCliente,
												TarjetaNumero,
												TarjetaVenceMes,
												TarjetaVenceAño,
												TarjetaCodigoSeguridad,
												TarjetaEsDebito,
												ChequeBancoId,
												ChequeBanco,
												ChequeNumero,
												ChequeFechaEmision,
												ChequeFechaVencimiento,
												ChequeCuit,
												ChequeNombre,
												ChequeCuenta,
												Otros,
												CodigoAutorizacion,
												DolarImporte,
												DolarCotizacion,
												Observaciones,
												FechaAlta,
												UsuarioAlta)
			SELECT Id,
					@Id ComprobanteId,
					FormaPagoId,
					FormaPagoCodigo,
					FormaPagoTipo,
					FormaPago,
					Importe,
					Cuota,
					Interes,
					Descuento,
					Total,
					TarjetaId,
					TarjetaNombre,
					TarjetaCliente,
					TarjetaNumero,
					TarjetaVenceMes,
					TarjetaVenceAño,
					TarjetaCodigoSeguridad,
					TarjetaEsDebito,
					ChequeBancoId,
					ChequeBanco,
					ChequeNumero,
					ChequeFechaEmision,
					ChequeFechaVencimiento,
					ChequeCuit,
					ChequeNombre,
					ChequeCuenta,
					Otros,
					CodigoAutorizacion,
					DolarImporte,
					DolarCotizacion,
					Observaciones,
					FechaAlta,
					UsuarioAlta
			FROM ComprobantesFormasPagosTmp
			WHERE ComprobanteId = @ComprobanteId			

			DELETE
			FROM ComprobantesFormasPagosTmp
			WHERE ComprobanteId = @ComprobanteId			
	
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