﻿create PROCEDURE [eliasm_db].[VentaRapidaDescuentoAplica]
	@Id nvarchar(150),
	@DescuentoId nvarchar(150),
	@Usuario nvarchar(256) = NULL
AS
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @Porcentaje NUMERIC(18,2) = 0
	
	SELECT @Porcentaje = Porcentaje
	FROM ParamPresupuestosDescuentos
	WHERE Id = @DescuentoId

	BEGIN TRAN
		UPDATE VentasRapidas
		SET DescuentoId = @DescuentoId,
			DescuentoPorcentaje = ISNULL(@Porcentaje,0),
			DescuentoUsuario = @Usuario,
			DescuentoFecha = DATEADD(HH,4,GETDATE()),
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