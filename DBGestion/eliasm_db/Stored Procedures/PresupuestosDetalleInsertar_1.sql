CREATE PROCEDURE [eliasm_db].[PresupuestosDetalleInsertar]
	@Id nvarchar(150),
	@PresupuestoId nvarchar(150),
	@ProductoId nvarchar(150),
	@ProductoCodigo nvarchar(20),
	@ProductoNombre nvarchar(150),
	@Precio numeric(18,2),
	@Cantidad int,
	@Usuario nvarchar(256) = NULL
AS
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @PorcenjateAlicuota NUMERIC(18,2) = 0
	DECLARE @Existe INT = 0
	DECLARE @AceptaDescuento bit = 0
	
	DECLARE @PrecioSinImpuesto NUMERIC(18,2) = 0	
	
	SELECT @PorcenjateAlicuota = A.Porcentaje,@AceptaDescuento = P.AceptaDescuento
	FROM Productos P
	INNER JOIN ParamAlicuotas A ON A.Id = P.AlicuotaId
	WHERE P.Id = @ProductoId

	SET @PrecioSinImpuesto = @Precio - (@Precio * (@PorcenjateAlicuota / 100))	
	
	SELECT @Existe = Count(*)
	FROM PresupuestosDetalle
	WHERE PresupuestoId = @PresupuestoId
	AND ProductoId = @ProductoId
	AND ProductoId NOT IN (SELECT Id FROM Productos where Codigo = '999999999999')
	
	BEGIN TRAN
		IF @Existe = 0 
		BEGIN
			INSERT INTO PresupuestosDetalle(Id,PresupuestoId,
											ProductoId,
											ProductoCodigo,
											ProductoNombre,
											Precio,
											PrecioSinImpuesto,
											Cantidad,
											AceptaDescuento,
											UsuarioAlta,FechaAlta)
								VALUES(@Id,@PresupuestoId,
											@ProductoId,
											@ProductoCodigo,
											UPPER(@ProductoNombre),
											@Precio,
											@PrecioSinImpuesto,
											@Cantidad,
											@AceptaDescuento,
											UPPER(@Usuario),DATEADD(HH,4,GETDATE()))
		END
		ELSE
		BEGIN
			UPDATE PresupuestosDetalle
			SET Precio = @Precio,
			PrecioSinImpuesto = @PrecioSinImpuesto,
			Cantidad = Cantidad + 1,
			UsuarioEdit = UPPER(@Usuario),
			FechaEdit = DATEADD(HH,4,GETDATE())
			WHERE PresupuestoId = @PresupuestoId
			AND ProductoId = @ProductoId
		END

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