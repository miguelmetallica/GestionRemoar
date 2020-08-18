
create PROCEDURE [ComprobantesDetalles_AddEdit]
	@ComprobanteDetalleId int,
	@ComprobanteId int,
	@ProductoId int,
	@Cantidad int,
	@PrecioUnitario numeric(18,2),
	@SubTotal numeric(18,2),
	@Usuario nvarchar(256)
AS
BEGIN TRY
	DECLARE @ProductoCodigo varchar(20)
	DECLARE @ProductoNombre varchar(150)
	DECLARE @UnidadMedidaId int
	DECLARE @UnidadMedidaCodigo varchar(5)
	DECLARE @UnidadMedida varchar(150)
	DECLARE @CuentaVentaId int
	DECLARE @CuentaVentaCodigo varchar(5)
	DECLARE @CuentaVenta varchar(150)
	DECLARE @PrecioUnitarioSinIva numeric(18,2)	= 0
	DECLARE @SubTotalSinIva numeric(18,2)	= 0
	DECLARE @PorcentajeBonificacion numeric(18,2)	= 0
	DECLARE @ImporteBonificacion numeric(18,2)	= 0
	DECLARE @AlicutaIvaId int
	DECLARE @AlicuotaIva varchar(150)
	DECLARE @AlicuotaPorcentaje numeric(18,2)
	DECLARE @ImporteNetoNoGravado numeric(18,2)	= 0
	DECLARE @ImporteExento numeric(18,2) = 0
	DECLARE @ImporteNetoGravado numeric(18,2) = 0
	DECLARE @Iva27 numeric(18,2) = 0
	DECLARE @Iva21 numeric(18,2) = 0
	DECLARE @Iva105 numeric(18,2) = 0
	DECLARE @Iva5 numeric(18,2) = 0
	DECLARE @Iva25 numeric(18,2) = 0
	DECLARE @Iva0 numeric(18,2) = 0

	SELECT @ProductoCodigo = ProductoCodigo, @ProductoNombre = Producto,
		@UnidadMedidaId = U.UnidadMedidaId, @UnidadMedidaCodigo = UnidadMedidaCodigo,
		@UnidadMedida = UnidadMedida,@CuentaVentaId = V.CuentaVentaId, 
		@CuentaVentaCodigo = CuentaVentaCodigo, @CuentaVenta = V.CuentaVenta,
		@AlicutaIvaId = A.AlicuotaId, @AlicuotaIva = Alicuota, 
		@AlicuotaPorcentaje = A.AlicuotaPorcentaje
	FROM Productos P
	LEFT JOIN ParamUnidadesMedidas U ON U.UnidadMedidaId = P.UnidadMedidaId
	LEFT JOIN ParamCuentasVentas V ON V.CuentaVentaId = P.CuentaVentaId
	LEFT JOIN ParamAlicuotas A ON A.AlicuotaId = P.AlicuotaId
	WHERE ProductoId = @ProductoId

	BEGIN TRAN
		IF @ComprobanteDetalleId = 0
		BEGIN
			EXEC @ComprobanteDetalleId = NextNumber 'ComprobantesDetalles'

			INSERT INTO ComprobantesDetalles(ComprobanteDetalleId,ComprobanteId,ProductoId,ProductoCodigo,ProductoNombre,
											Cantidad,UnidadMedidaId,UnidadMedidaCodigo,UnidadMedida,CuentaVentaId,CuentaVentaCodigo,
											CuentaVenta,PrecioUnitarioSinIva,PrecioUnitario,SubTotalSinIva,SubTotal,PorcentajeBonificacion,
											ImporteBonificacion,AlicuotaIvaId,AlicuotaIva,AlicuotaProcentaje,ImporteNetoNoGravado,
											ImporteExento,ImporteNetoGravado,Iva27,Iva21,Iva105,Iva5,Iva25,Iva0,FechaAlta,UsuarioAlta)
									VALUES(@ComprobanteDetalleId,@ComprobanteId,@ProductoId,@ProductoCodigo,@ProductoNombre,
											@Cantidad,@UnidadMedidaId,@UnidadMedidaCodigo,@UnidadMedida,@CuentaVentaId,@CuentaVentaCodigo,
											@CuentaVenta,@PrecioUnitarioSinIva,@PrecioUnitario,@SubTotalSinIva,@SubTotal,@PorcentajeBonificacion,
											@ImporteBonificacion,@AlicutaIvaId,@AlicuotaIva,@AlicuotaPorcentaje,@ImporteNetoNoGravado,
											@ImporteExento,@ImporteNetoGravado,@Iva27,@Iva21,@Iva105,@Iva5,@Iva25,@Iva0,GETDATE(),UPPER(@Usuario))
		END
		/*
		IF @ComprobanteDetalleId > 0
		BEGIN
			UPDATE ComprobantesDetalles
			SET TipoComprobanteId = @TipoComprobanteId,
				TipoComprobante = @TipoComprobante,
				FechaComprobante = @FechaComprobante,
				TipoResponsableId = @TipoResponsableId,
				TipoResponsable = @TipoResponsable,
				Estado = @Estado
			WHERE ComprobanteDetalleId = @ComprobanteDetalleId
		END
		*/
	COMMIT;

	SELECT @ComprobanteId Id

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