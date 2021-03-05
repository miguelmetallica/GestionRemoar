CREATE PROCEDURE [eliasm_db].[VentaRapidaDetalleInsertar]
	@Id nvarchar(150),
	@VentaRapidaId nvarchar(150),
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
	DECLARE @PrecioSinImpuesto NUMERIC(18,2) = 0	
	DECLARE @AceptaDescuento BIT = 0	
	
	SELECT @PorcenjateAlicuota = A.Porcentaje
	FROM Productos P
	INNER JOIN ParamAlicuotas A ON A.Id = P.AlicuotaId
	WHERE P.Id = @ProductoId

	SET @PrecioSinImpuesto = @Precio - (@Precio * (@PorcenjateAlicuota / 100))	
	
	SELECT @AceptaDescuento = ISNULL(P.AceptaDescuento,0)
	FROM Productos P
	WHERE Id = @ProductoId

	BEGIN TRAN
		INSERT INTO VentasRapidasDetalle(Id,VentaRapidaId,ProductoId,ProductoCodigo,ProductoNombre,
				Precio,PrecioSinImpuesto,Cantidad,AceptaDescuento,UsuarioAlta,FechaAlta)
		VALUES(@Id,@VentaRapidaId,@ProductoId,@ProductoCodigo,UPPER(@ProductoNombre),@Precio,
				@PrecioSinImpuesto,@Cantidad,@AceptaDescuento,UPPER(@Usuario),DATEADD(HH,4,GETDATE()))
		

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