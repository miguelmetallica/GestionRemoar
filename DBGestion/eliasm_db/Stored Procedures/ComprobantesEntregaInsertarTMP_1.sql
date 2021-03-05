CREATE PROCEDURE [eliasm_db].[ComprobantesEntregaInsertarTMP]	
	@Id nvarchar(150),
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @CANTIDAD INT = 0;
	
	SELECT @CANTIDAD = COUNT(*) FROM ComprobantesDetalleTMP I WHERE I.Id = @Id 
	
	IF(@CANTIDAD = 0)
	BEGIN
		BEGIN TRAN	
			INSERT INTO ComprobantesDetalleTMP(Id,
												ComprobanteId,
												ProductoId,
												ProductoCodigo,
												ProductoNombre,
												Cantidad,
												UnidadMedidaId,
												UnidadMedidaCodigo,
												UnidadMedida,
												CuentaVentaId,
												CuentaVentaCodigo,
												CuentaVenta,
												PrecioSinIva,
												Precio,
												SubTotalSinIva,
												SubTotal,
												PorcentajeBonificacion,
												ImporteBonificacion,
												AlicuotaIvaId,
												AlicuotaIva,
												AlicuotaIvaCodigo,
												AlicuotaProcentaje,
												ImporteNetoNoGravado,
												ImporteExento,
												ImporteNetoGravado,
												Iva27,Iva21,Iva105,Iva5,Iva25,Iva0,
												FechaAlta,UsuarioAlta)
			SELECT I.Id,D.ComprobanteId,D.ProductoId,D.ProductoCodigo,D.ProductoNombre,
				I.Cantidad,D.UnidadMedidaId,D.UnidadMedidaCodigo,D.UnidadMedida,
				D.CuentaVentaId,D.CuentaVentaCodigo,D.CuentaVenta,
				0 PrecioSinIva,0 Precio,0 SubTotalSinIva,0 SubTotal,
				0 PorcentajeBonificacion,
				0 ImporteBonificacion,
				D.AlicuotaIvaId,D.AlicuotaIva,
				D.AlicuotaIvaCodigo,
				D.AlicuotaProcentaje,0 ImporteNetoNoGravado,0 ImporteExento,0 ImporteNetoGravado,
				0 Iva27,0 Iva21,0 Iva105,0 Iva5,0 Iva25,0 Iva0,
				DATEADD(HH,4,GETDATE()) FechaAlta,@Usuario
			FROM ComprobantesDetalleImputacion I
			INNER JOIN ComprobantesDetalle D ON I.DetalleId = D.Id
			WHERE I.Id = @Id AND I.Estado = 1 AND (I.Entrega = 1 OR I.AutorizaEntrega = 1)
			AND I.Despacha = 0


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