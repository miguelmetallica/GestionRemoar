/****** Object:  StoredProcedure [eliasm_db].[ComprobantesInsertarRecibo]    Script Date: 12/12/2020 06:35:24 a. m. ******/

CREATE PROCEDURE [eliasm_db].[ComprobantesInsertarReciboAnular]	
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
	DECLARE @TipoComprobanteEsDebe bit = 0;
	DECLARE @Letra nchar(1);
	DECLARE @PtoVenta int;
	DECLARE @FechaComprobante datetime;
	
	DECLARE @Anulado bit;
	DECLARE @FechaAnulacion datetime;
	DECLARE @TipoComprobanteAnulaId nvarchar(150);
	DECLARE @TipoComprobanteAnulaCodigo nvarchar(5);
	DECLARE @TipoComprobanteAnula nvarchar(150);
	DECLARE @LetraAnula char(1);
	DECLARE @PtoVtaAnula int;
	DECLARE @NumeroAnula numeric(12,0);
	DECLARE @Estado bit = 1;

	DECLARE @esAnulado int = 0;
	
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
			@TipoComprobanteEsDebe = isnull(T.esDebe,0),
			@Letra = N.Letra,
			@PtoVenta = N.PuntoVenta
	FROM SistemaConfiguraciones C
	INNER JOIN ParamTiposComprobantes T ON T.Codigo = C.Valor
	INNER JOIN ComprobantesNumeraciones N ON N.TipoComprobanteId = T.Id
	WHERE C.Configuracion = 'COMPROBANTES_RECIBO_ANULAR'
	AND N.SucursalId = @SucursalId
	AND N.Estado = 1

	SET @FechaComprobante = DATEADD(HH,4,GETDATE());
	
	SELECT @esAnulado = COUNT(*)
	FROM Comprobantes C
	WHERE C.Id = @ComprobanteId 
	AND ISNULL(C.Anulado,0) = 1
	;

	IF @esAnulado = 0
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
									SucursalId)
			SELECT @Id,
				@Codigo,
				@TipoComprobanteId,
				@TipoComprobante,
				@TipoComprobanteCodigo,
				@TipoComprobanteEsDebe,
				PresupuestoId,
				@Letra,
				@PtoVenta,
				@Numero,
				@FechaComprobante,
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
				@Observaciones,
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
				@Estado,
				DATEADD(HH,4,GETDATE()),
				@Usuario,
				@SucursalId
			FROM Comprobantes C
			WHERE C.Id = @ComprobanteId AND ISNULL(C.Anulado,0) = 0
			;
			
			UPDATE Comprobantes
			SET ComprobanteAnulaId = @Id,
				Anulado = 1,
				FechaAnulacion = @FechaComprobante,
				TipoComprobanteAnulaId = @TipoComprobanteId,
				TipoComprobanteAnulaCodigo = @TipoComprobanteCodigo,
				TipoComprobanteAnula = @TipoComprobante,
				CodigoAnula = @Codigo,
				LetraAnula = @Letra,
				PtoVtaAnula = @PtoVenta,
				NumeroAnula = @Numero,
				UsuarioAnula = UPPER(@Usuario)
			WHERE Id = @ComprobanteId AND ISNULL(Anulado,0) = 0
			;

			INSERT INTO ComprobantesFormasPagos(Id,ComprobanteId,
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
												DolarImporte,
												DolarCotizacion,
												Observaciones,
												FechaAlta,UsuarioAlta)
			SELECT NEWID() Id,@Id ComprobanteId,
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
				DolarImporte,
				DolarCotizacion,
				Observaciones,
				DATEADD(HH,4,GETDATE()),UPPER(@Usuario)
			FROM ComprobantesFormasPagos C
			WHERE C.ComprobanteId = @ComprobanteId
			;
			
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