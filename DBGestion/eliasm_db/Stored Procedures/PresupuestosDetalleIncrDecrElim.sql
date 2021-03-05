CREATE PROCEDURE [eliasm_db].[PresupuestosDetalleIncrDecrElim]
	@Id nvarchar(150),
	@Cantidad int,
	@Usuario nvarchar(256) = NULL
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @Cantidad = 0
		BEGIN
			DELETE PresupuestosDetalle
			WHERE Id = @Id
		END
		ELSE
		BEGIN
			UPDATE PresupuestosDetalle
			SET Cantidad = Cantidad + @Cantidad,
			UsuarioEdit = UPPER(@Usuario),
			FechaEdit = DATEADD(HH,4,GETDATE())
			WHERE Id = @Id
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