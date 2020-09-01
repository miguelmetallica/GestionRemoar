CReATE PROCEDURE [dbo].[PresupuestosDetalleInsertar]
	@Id nvarchar(150),
	@PresupuestoId nvarchar(150),
	@ProductoId nvarchar(150),
	@Precio numeric(18,2),
	@Cantidad int,
	@Usuario nvarchar(256) = NULL
AS
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @PrecioSinImpuesto NUMERIC(18,2) = 0
	
	SELECT @PrecioSinImpuesto = P.PrecioVenta - (P.PrecioVenta * (A.Porcentaje / 100))
	FROM Productos P
	INNER JOIN ParamAlicuotas A ON A.Id = P.AlicuotaId
	WHERE P.Id = @ProductoId

	BEGIN TRAN
		INSERT INTO PresupuestosDetalle(Id,PresupuestoId,ProductoId,Precio,PrecioSinImpuesto,Cantidad,UsuarioAlta,FechaAlta)
					VALUES(@Id,@PresupuestoId,@ProductoId,@Precio,@PrecioSinImpuesto,@Cantidad,UPPER(@Usuario),DATEADD(HH,4,GETDATE()))
		

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