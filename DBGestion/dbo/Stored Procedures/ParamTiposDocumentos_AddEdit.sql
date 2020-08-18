
CREATE PROCEDURE [ParamTiposDocumentos_AddEdit]
	@TipoDocumentoId int,
	@TipoDocumentoCodigo varchar(5),
	@TipoDocumento varchar(150),	
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @TipoDocumentoId = 0
		BEGIN
			EXEC @TipoDocumentoId = NextNumber 'ParamTiposDocumentos'
			INSERT INTO ParamTiposDocumentos(TipoDocumentoId,TipoDocumentoCodigo,TipoDocumento,Estado,FechaAlta,UsuarioAlta)
			VALUES(@TipoDocumentoId,UPPER(@TipoDocumentoCodigo),UPPER(@TipoDocumento),@Estado,GETDATE(),UPPER(@Usuario))
		END
		
		IF @TipoDocumentoId > 0
		BEGIN
			UPDATE ParamTiposDocumentos
			SET TipoDocumentoCodigo = UPPER(@TipoDocumentoCodigo),
				TipoDocumento = UPPER(@TipoDocumento),
				Estado = @Estado
			WHERE TipoDocumentoId = @TipoDocumentoId
		END

	COMMIT;

	SELECT @TipoDocumentoId Id

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