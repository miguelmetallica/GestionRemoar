CREATE PROCEDURE [dbo].[PresupuestosEditar]
	@Id nvarchar(150),
	@ClienteId nvarchar(150),
	@Usuario nvarchar(256) = NULL
AS
BEGIN TRY	
	SET NOCOUNT ON;
	DECLARE @TipoResponsableId nvarchar(150);	
	DECLARE @CategoriaId nvarchar(150);

	SELECT TOP 1 @TipoResponsableId = TipoResponsableId,
				@CategoriaId = CategoriaId
	FROM Clientes
	WHERE Id = @ClienteId

	IF @TipoResponsableId IS NULL
	BEGIN
		SELECT TOP 1 @TipoResponsableId = Id
		FROM ParamTiposResponsables
		WHERE Defecto = 1
	END

	IF @CategoriaId IS NULL
	BEGIN
		SELECT TOP 1 @CategoriaId = Id
		FROM ParamClientesCategorias
		WHERE Defecto = 1
	END

	BEGIN TRAN
		UPDATE Presupuestos
		SET ClienteId = @ClienteId,
			TipoResponsableId = @TipoResponsableId,
			ClienteCategoriaId = @CategoriaId,
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