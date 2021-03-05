
CREATE PROCEDURE [eliasm_db].[VentaRapidaEditarProducto]
	@Id nvarchar(150),
	@ProductoNombre nvarchar(150),
	@Precio numeric(18,2),
	@Cantidad numeric(18,2),
	@Usuario nvarchar(256) = NULL
AS
BEGIN TRY
	SET NOCOUNT ON;
	
	BEGIN TRAN
		UPDATE VentasRapidasDetalle 
		SET ProductoNombre = @ProductoNombre,
			Precio = @Precio,
			Cantidad = @Cantidad,
			FechaEdit = DATEADD(HH,4,GETDATE()),
			UsuarioEdit = UPPER(@Usuario) 
			WHERE Id = @Id

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