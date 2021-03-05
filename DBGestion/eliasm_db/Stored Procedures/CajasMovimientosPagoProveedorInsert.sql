CREATE PROCEDURE [eliasm_db].[CajasMovimientosPagoProveedorInsert]	
	@SucursalId nvarchar(150),
	@CajaId nvarchar(150),
	@TipoMovimientoId nvarchar(150),
	@ProveedorId nvarchar(150),
	@Observaciones nvarchar(500) = null,
	@NroComprobante nvarchar(50) = null,
	@Importe NUMERIC(18,2) = 0,
	@Usuario nvarchar(150)
AS
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @Id nvarchar(150) = NEWID();
	DECLARE @Fecha DATETIME;

	SET @Fecha = DATEADD(HH,4,GETDATE());
	BEGIN TRAN			
		INSERT INTO CajasMovimientos(Id,
									CajaId,
									Fecha,
									TipoMovimientoId,
									SucursalId,
									ProveedorId,
									NroComprobante,
									Importe,
									Observaciones,
									FechaAlta,
									UsuarioAlta
									)
		SELECT @Id,
				@CajaId,
				@Fecha,
				@TipoMovimientoId,
				@SucursalId,
				@ProveedorId,
				@NroComprobante,
				@Importe,
				@Observaciones,
				DATEADD(HH,4,GETDATE()),
				@Usuario
		
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
;