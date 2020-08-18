
CREATE PROCEDURE [ParamTiposResponsables_AddEdit]
	@TipoResponsableId int,
	@TipoResponsableCodigo varchar(5),
	@TipoResponsable varchar(150),	
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @TipoResponsableId = 0
		BEGIN
			EXEC @TipoResponsableId = NextNumber 'ParamTiposResponsables'
			INSERT INTO ParamTiposResponsables(TipoResponsableId,TipoResponsableCodigo,TipoResponsable,Estado,FechaAlta,UsuarioAlta)
			VALUES(@TipoResponsableId,UPPER(@TipoResponsableCodigo),UPPER(@TipoResponsable),@Estado,GETDATE(),UPPER(@Usuario))
		END
		
		IF @TipoResponsableId > 0
		BEGIN
			UPDATE ParamTiposResponsables
			SET TipoResponsableCodigo = UPPER(@TipoResponsableCodigo),
				TipoResponsable = UPPER(@TipoResponsable),
				Estado = @Estado
			WHERE TipoResponsableId = @TipoResponsableId
		END

	COMMIT;

	SELECT @TipoResponsableId Id

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