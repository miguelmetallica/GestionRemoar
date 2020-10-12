CREATE PROCEDURE [dbo].[ComprobantesInsertarPresupuesto]	
	@PresupuestoId nvarchar(150),
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
	DECLARE @Total numeric(18,2);
	DECLARE @TotalSinImpuesto numeric(18,2);
	DECLARE @TotalSinDescuento numeric(18,2);
	DECLARE @TotalSinImpuestoSinDescuento numeric(18,2);
	DECLARE @DescuentoPorcentaje numeric(18,2);
	DECLARE @DescuentoTotal numeric(18,2);
	DECLARE @DescuentoSinImpuesto numeric(18,2);
	DECLARE @Observaciones nvarchar(500);
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
	DECLARE @Estado bit;

	
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
	WHERE C.Configuracion = 'COMPROBANTES_PRESUPUESTO'
	AND N.Estado = 1


	SELECT TOP 1 @ConceptoIncluidoId = P.Id,
				@ConceptoIncluidoCodigo = P.Codigo,
				@ConceptoIncluido = Descripcion
	FROM ParamConceptosIncluidos P
	WHERE P.Defecto = 1

	SET @FechaComprobante = DATEADD(HH,4,GETDATE());

	SELECT 
	@PresupuestoId =P.Id,
	@ClienteId = P.ClienteId,
	@DescuentoPorcentaje = P.DescuentoPorcentaje,
	@TipoResponsableId = TP.Id,
	@TipoResponsableCodigo = TP.Codigo,
	@TipoResponsable = TP.Descripcion
	FROM Presupuestos P
	LEFT JOIN ParamTiposResponsables TP ON TP.Id = P.TipoResponsableId
	WHERE P.Id = @PresupuestoId
			

	SELECT 
	@Total = SUM(D.Cantidad * D.Precio) - (SUM(D.Cantidad * D.Precio) * @DescuentoPorcentaje / 100) ,
	@TotalSinImpuesto = SUM(D.Cantidad * D.PrecioSinImpuesto) - (SUM(D.Cantidad * D.PrecioSinImpuesto) * @DescuentoPorcentaje / 100),
	@TotalSinDescuento = SUM(D.Cantidad * D.Precio),
	@TotalSinImpuestoSinDescuento = SUM(D.Cantidad * D.PrecioSinImpuesto),
	@DescuentoTotal = (SUM(D.Cantidad * D.Precio) * @DescuentoPorcentaje / 100),
	@DescuentoSinImpuesto = (SUM(D.Cantidad * D.PrecioSinImpuesto) * @DescuentoPorcentaje / 100)
	FROM PresupuestosDetalle D
	WHERE D.PresupuestoId = @PresupuestoId

	IF ISNULL(@TipoResponsableId,'') = ''
	BEGIN
		SELECT TOP 1 
		@TipoResponsableId = P.Id,
		@TipoResponsableCodigo = P.Codigo,
		@TipoResponsable = Descripcion
		FROM ParamTiposResponsables P
		WHERE P.Defecto = 1
	END

	SELECT TOP 1 
	@ClienteId = P.Id,
	@ClienteCodigo = P.Codigo,
	@TipoDocumentoId = P.TipoDocumentoId,
	@TipoDocumentoCodigo = D.Codigo,
	@TipoDocumento = D.Descripcion,
	@NroDocumento = P.NroDocumento,
	@CuilCuit = P.CuilCuit,
	@RazonSocial = P.RazonSocial,
	@ProvinciaId = P.ProvinciaId,
	@ProvinciaCodigo = R.Id,
	@Provincia = R.Descripcion,
	@Localidad = P.Localidad,
	@CodigoPostal = P.CodigoPostal,
	@Calle = P.Calle,
	@CalleNro = P.CalleNro,
	@PisoDpto = P.PisoDpto,
	@OtrasReferencias = P.OtrasReferencias,
	@Email = P.Email,
	@Telefono = P.Telefono,
	@Celular = P.Celular
	FROM Clientes P
	LEFT JOIN ParamTiposDocumentos D ON D.Id = P.TipoDocumentoId
	LEFT JOIN ParamProvincias R ON R.Id = P.ProvinciaId	
	WHERE P.Id = @ClienteId


	BEGIN TRAN		
			EXEC @Numero = NextNumberComprobante @SucursalId,@TipoComprobanteId
			SET @Codigo = @Letra + RIGHT('0000' + RTRIM(LTRIM(CONVERT(VARCHAR(4),@PtoVenta))),3) + RIGHT('00000000' + RTRIM(LTRIM(CONVERT(VARCHAR(8),@Numero))),8)
			
			INSERT INTO Comprobantes(Id,Codigo,TipoComprobanteId,TipoComprobante,TipoComprobanteCodigo,PresupuestoId,
									Letra,PtoVenta,Numero,FechaComprobante,ConceptoIncluidoId,ConceptoIncluidoCodigo,
									ConceptoIncluido,PeriodoFacturadoDesde,PeriodoFacturadoHasta,FechaVencimiento,
									TipoResponsableId,TipoResponsableCodigo,TipoResponsable,ClienteId,ClienteCodigo,
									TipoDocumentoId,TipoDocumentoCodigo,TipoDocumento,NroDocumento,CuilCuit,RazonSocial,
									ProvinciaId,ProvinciaCodigo,Provincia,Localidad,CodigoPostal,Calle,CalleNro,
									PisoDpto,OtrasReferencias,Email,Telefono,Celular,Total,TotalSinImpuesto,
									TotalSinDescuento,TotalSinImpuestoSinDescuento,DescuentoPorcentaje,DescuentoTotal,
									DescuentoSinImpuesto,ImporteTributos,Observaciones,Confirmado,Cobrado,Anulado,
									FechaAnulacion,TipoComprobanteAnulaId,TipoComprobanteAnulaCodigo,TipoComprobanteAnula,
									LetraAnula,PtoVtaAnula,NumeroAnula,Estado,FechaAlta,UsuarioAlta) 
			SELECT @Id,@Codigo,@TipoComprobanteId,@TipoComprobante,@TipoComprobanteCodigo,@PresupuestoId,@Letra,@PtoVenta,@Numero,
			@FechaComprobante,@ConceptoIncluidoId,@ConceptoIncluidoCodigo,@ConceptoIncluido,@PeriodoFacturadoDesde,@PeriodoFacturadoHasta,
			@FechaVencimiento,@TipoResponsableId,@TipoResponsableCodigo,@TipoResponsable,@ClienteId,@ClienteCodigo,@TipoDocumentoId,@TipoDocumentoCodigo,
			@TipoDocumento,@NroDocumento,@CuilCuit,@RazonSocial,@ProvinciaId,@ProvinciaCodigo,@Provincia,@Localidad,@CodigoPostal,@Calle,@CalleNro,@PisoDpto,
			@OtrasReferencias,@Email,@Telefono,@Celular,@Total,@TotalSinImpuesto,@TotalSinDescuento,@TotalSinImpuestoSinDescuento,@DescuentoPorcentaje,
			@DescuentoTotal,@DescuentoSinImpuesto,@ImporteTributos,@Observaciones,@Confirmado,@Cobrado,@Anulado,@FechaAnulacion,@TipoComprobanteAnulaId,
			@TipoComprobanteAnulaCodigo,@TipoComprobanteAnula,@LetraAnula,@PtoVtaAnula,@NumeroAnula,@Estado,DATEADD(HH,4,GETDATE()),UPPER(@Usuario)
			

			INSERT INTO ComprobantesDetalle(Id,ComprobanteId,ProductoId,ProductoCodigo,ProductoNombre,Cantidad,UnidadMedidaId,UnidadMedidaCodigo,UnidadMedida,
										CuentaVentaId,CuentaVentaCodigo,CuentaVenta,PrecioUnitarioSinIva,PrecioUnitario,SubTotalSinIva,SubTotal,PorcentajeBonificacion,
										ImporteBonificacion,AlicuotaIvaId,AlicuotaIva,AlicuotaIvaCodigo,AlicuotaProcentaje,ImporteNetoNoGravado,ImporteExento,ImporteNetoGravado,
										Iva27,Iva21,Iva105,Iva5,Iva25,Iva0,FechaAlta,UsuarioAlta)
			SELECT NEWID() Id,@Id,D.ProductoId,P.Codigo ProductoCodigo,P.Producto ProductoNombre,D.Cantidad,P.UnidadMedidaId,U.Codigo UnidadMedidaCodigo,
			U.Descripcion UnidadMedida,P.CuentaVentaId,CV.Codigo CuentaVentaCodigo,CV.Descripcion CuentaVenta,D.PrecioSinImpuesto PrecioUnitarioSinIva,
			D.Precio PrecioUnitario,D.PrecioSinImpuesto * D.Cantidad SubTotalSinIva,D.Precio * D.Cantidad SubTotal,@DescuentoPorcentaje PorcentajeBonificacion,
			0 ImporteBonificacion,P.AlicuotaId,A.Descripcion AlicuotaIva,A.Codigo AlicuotaIvaCodigo,A.Porcentaje AlicuotaProcentaje,0 ImporteNetoNoGravado,0 ImporteExento,0 ImporteNetoGravado,
			CASE WHEN A.Porcentaje = 27 THEN (D.Precio * D.Cantidad) * 0.27 ELSE 0 END Iva27,
			CASE WHEN A.Porcentaje = 21 THEN (D.Precio * D.Cantidad) * 0.21 ELSE 0 END Iva21,
			CASE WHEN A.Porcentaje = 105 THEN (D.Precio * D.Cantidad) * 0.105 ELSE 0 END Iva105,
			CASE WHEN A.Porcentaje = 5 THEN (D.Precio * D.Cantidad) * 0.05 ELSE 0 END Iva5,
			CASE WHEN A.Porcentaje = 25 THEN (D.Precio * D.Cantidad) * 0.25 ELSE 0 END Iva25,
			CASE WHEN A.Porcentaje = 0 THEN (D.Precio * D.Cantidad) ELSE 0 END Iva0,
			DATEADD(HH,4,GETDATE()) FechaAlta,@Usuario
			FROM PresupuestosDetalle D
			INNER JOIN Productos P ON P.Id = D.ProductoId
			LEFT JOIN ParamUnidadesMedidas U ON U.Id = P.UnidadMedidaId
			LEFT JOIN ParamCuentasVentas CV ON CV.Id = P.CuentaVentaId
			LEFT JOIN ParamAlicuotas A ON A.Id = P.AlicuotaId
			WHERE D.PresupuestoId = @PresupuestoId

	COMMIT;

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