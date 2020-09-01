CREATE PROCEDURE [dbo].[PresupuestosRechazar]
	@Id nvarchar(150),
	@Usuario nvarchar(256) = NULL
AS
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @EstadoId nvarchar(150);

	SELECT @EstadoId = E.Id
	FROM SistemaConfiguraciones C
	INNER JOIN ParamPresupuestosEstados E ON E.Codigo = C.Valor
	WHERE C.Configuracion = 'PRESUPUESTO_RECHAZADO_CLIENTE'

	BEGIN TRAN
		UPDATE Presupuestos
		SET EstadoId = @EstadoId							
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