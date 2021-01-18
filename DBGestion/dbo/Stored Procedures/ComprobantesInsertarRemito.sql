CREATE PROCEDURE [dbo].[ComprobantesInsertarRemito]	
    @ComprobanteId nvarchar(150),
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
	DECLARE @Estado bit = 1;

	
	DECLARE @Numero numeric(10);
	DECLARE @Codigo nvarchar(20);
	
	SELECT top 1 @SucursalId = S.Id, @SucursalCodigo = S.Codigo
	FROM AspNetUsers U
	INNER JOIN Sucursales S ON S.Id = U.SucursalId
	WHERE UserName = @Usuario

	IF @SucursalCodigo IS NULL
		SET @SucursalCodigo = '000';

	SELECT top 1 @TipoComprobanteId = T.Id,
			@TipoComprobante = T.Descripcion,
			@TipoComprobanteCodigo = T.Codigo,
			@Letra = N.Letra,
			@PtoVenta = N.PuntoVenta
	FROM SistemaConfiguraciones C
	INNER JOIN ParamTiposComprobantes T ON T.Codigo = C.Valor
	INNER JOIN ComprobantesNumeraciones N ON N.TipoComprobanteId = T.Id
	WHERE C.Configuracion = 'COMPROBANTES_REMITO'
	AND N.SucursalId = @SucursalId
	AND N.Estado = 1

	SET @FechaComprobante = DATEADD(HH,4,GETDATE());

	

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
			SELECT @Id,@Codigo,@TipoComprobanteId,@TipoComprobante,@TipoComprobanteCodigo,NULL PresupuestoId,@Letra,@PtoVenta,@Numero,
					@FechaComprobante,ConceptoIncluidoId,ConceptoIncluidoCodigo,ConceptoIncluido,PeriodoFacturadoDesde,PeriodoFacturadoHasta,
					NULL FechaVencimiento,TipoResponsableId,TipoResponsableCodigo,TipoResponsable,ClienteId,ClienteCodigo,TipoDocumentoId,TipoDocumentoCodigo,
					TipoDocumento,NroDocumento,CuilCuit,RazonSocial,ProvinciaId,ProvinciaCodigo,Provincia,Localidad,CodigoPostal,Calle,CalleNro,PisoDpto,
					OtrasReferencias,Email,Telefono,Celular,0 Total,0 TotalSinImpuesto,0 TotalSinDescuento,0 TotalSinImpuestoSinDescuento,0 DescuentoPorcentaje,
					0 DescuentoTotal,0 DescuentoSinImpuesto,0 ImporteTributos,Observaciones,Confirmado,Cobrado,Anulado,FechaAnulacion,TipoComprobanteAnulaId,
					TipoComprobanteAnulaCodigo,TipoComprobanteAnula,LetraAnula,PtoVtaAnula,NumeroAnula,@Estado,DATEADD(HH,4,GETDATE()),UPPER(@Usuario)
			FROM Comprobantes
			WHERE Id = @ComprobanteId
			AND EXISTS (SELECT 1 FROM ComprobantesDetalleTMP D WHERE ComprobanteId = @ComprobanteId)

			INSERT INTO ComprobantesDetalle(Id,ComprobanteId,ProductoId,ProductoCodigo,ProductoNombre,Cantidad,UnidadMedidaId,UnidadMedidaCodigo,UnidadMedida,
										CuentaVentaId,CuentaVentaCodigo,CuentaVenta,PrecioUnitarioSinIva,PrecioUnitario,SubTotalSinIva,SubTotal,PorcentajeBonificacion,
										ImporteBonificacion,AlicuotaIvaId,AlicuotaIva,AlicuotaIvaCodigo,AlicuotaProcentaje,ImporteNetoNoGravado,ImporteExento,ImporteNetoGravado,
										Iva27,Iva21,Iva105,Iva5,Iva25,Iva0,FechaAlta,UsuarioAlta)
			SELECT Id,@Id ComprobanteId,ProductoId,ProductoCodigo,ProductoNombre,Cantidad,UnidadMedidaId,UnidadMedidaCodigo,UnidadMedida,
				CuentaVentaId,CuentaVentaCodigo,CuentaVenta,PrecioUnitarioSinIva,PrecioUnitario,SubTotalSinIva,SubTotal,PorcentajeBonificacion,
				ImporteBonificacion,AlicuotaIvaId,AlicuotaIva,AlicuotaIvaCodigo,AlicuotaProcentaje,ImporteNetoNoGravado,ImporteExento,ImporteNetoGravado,
				Iva27,Iva21,Iva105,Iva5,Iva25,Iva0,FechaAlta,UsuarioAlta
			FROM ComprobantesDetalleTMP D
			WHERE D.ComprobanteId = @ComprobanteId

			INSERT INTO ComprobantesDetalleImputacion(Id,ComprobanteId,DetalleId,ProductoId,Cantidad,Precio,
													ImporteImputado,PorcentajeImputado,Estado,Fecha,Usuario,
													Entrega,FechaEntrega,UsuarioEntrega,
													AutorizaEntrega,FechaAutoriza,UsuarioAutoriza,
													Despacha,FechaDespacha,UsuarioDespacha,RemitoId,
													Devolucion,FechaDevolucion,UsuarioDevolucion,MotivoDevolucion)
			SELECT NEWID(),ComprobanteId,DetalleId,ProductoId,Cantidad,Precio,ImporteImputado,PorcentajeImputado,
					1 Estado,DATEADD(HH,4,GETDATE()) Fecha,UPPER(@Usuario)Usuario,
					Entrega,FechaEntrega,UsuarioEntrega,
					AutorizaEntrega,FechaAutoriza,UsuarioAutoriza,
					1 Despacha,DATEADD(HH,4,GETDATE()) FechaDespacha,UPPER(@Usuario) UsuarioDespacha,@Id,
					Devolucion,FechaDevolucion,UsuarioDevolucion,MotivoDevolucion
			FROM ComprobantesDetalleImputacion
			WHERE Id IN (
						SELECT Id
						FROM ComprobantesDetalleTMP D
						WHERE D.ComprobanteId = @ComprobanteId
						)
		
			UPDATE ComprobantesDetalleImputacion SET Estado = 0
			WHERE Id in (
						SELECT Id
						FROM ComprobantesDetalleTMP D
						WHERE D.ComprobanteId = @ComprobanteId
						)
			
			DELETE ComprobantesDetalleTMP 
			WHERE ComprobanteId = @ComprobanteId

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