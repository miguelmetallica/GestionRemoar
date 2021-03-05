CREATE PROCEDURE [eliasm_db].[VentaRapidaDetalleEditar]
	@Id nvarchar(150),
	@ProductoId nvarchar(150),
	@Cantidad int,
	@Precio numeric(18,2) = null,
	@Producto nvarchar(150) = null,
	@Usuario nvarchar(256) = NULL
AS
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @PorcenjateAlicuota NUMERIC(18,2) = 0	
	DECLARE @PrecioSinImpuesto NUMERIC(18,2) = 0	
	DECLARE @AceptaDescuento BIT = 0	
	DECLARE @ProductoCodigo NUMERIC(18,2) = 0

	SELECT @PorcenjateAlicuota = A.Porcentaje
	FROM Productos P
	INNER JOIN ParamAlicuotas A ON A.Id = P.AlicuotaId
	WHERE P.Id = @ProductoId

	SET @PrecioSinImpuesto = @Precio - (@Precio * (@PorcenjateAlicuota / 100))	
	
	SELECT @AceptaDescuento = ISNULL(P.AceptaDescuento,0),@ProductoCodigo = P.Codigo
	FROM Productos P
	WHERE Id = @ProductoId

	
	BEGIN TRAN
	IF	@ProductoCodigo = '999999999999'
	begin
		UPDATE VentasRapidasDetalle
		SET Cantidad = @Cantidad,
		Precio = @Precio,
		PrecioSinImpuesto = @PrecioSinImpuesto,
		AceptaDescuento = @AceptaDescuento,
		UsuarioEdit = @Usuario,
		ProductoNombre = upper(@Producto),
		FechaEdit = DATEADD(HH,4,GETDATE())
		WHERE Id = @Id		
	end
	else
	begin
		UPDATE VentasRapidasDetalle
		SET Cantidad = @Cantidad,
		UsuarioEdit = @Usuario,
		FechaEdit = DATEADD(HH,4,GETDATE())
		WHERE Id = @Id
	end
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