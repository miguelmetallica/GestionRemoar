﻿CREATE PROCEDURE [eliasm_db].[PresupuestosDetalleEditar]
	@Id nvarchar(150),
	@Cantidad int,
	@Precio numeric(18,2),
	@Usuario nvarchar(256) = NULL
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		UPDATE PresupuestosDetalle
		SET Cantidad = @Cantidad,
		Precio = @Precio,
		UsuarioEdit = @Usuario,
		FechaEdit = DATEADD(HH,4,GETDATE())
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