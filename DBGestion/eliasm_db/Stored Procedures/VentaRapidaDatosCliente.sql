CREATE PROCEDURE [eliasm_db].[VentaRapidaDatosCliente]
	@Id nvarchar(150),
	@NroDocumento nvarchar(15) = NULL,
	@CuilCuit nvarchar(50) = NULL,
	@RazonSocial nvarchar(300) = NULL,
	@TipoResponsableId nvarchar(150) = NULL,
	@Usuario nvarchar(256) = NULL
AS
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @TipoDocumentoId nvarchar(150)
	DECLARE @TipoDocumentoCodigo nvarchar(5)
	DECLARE @TipoDocumento nvarchar(150)

	IF @TipoResponsableId IS NULL
	BEGIN
		SELECT TOP 1 @TipoResponsableId = Id
		FROM ParamTiposResponsables
		WHERE Defecto = 1
	END

	
	SELECT TOP 1 
		@TipoDocumentoId = Id,
		@TipoDocumentoCodigo = Codigo,
		@TipoDocumento = Descripcion
	FROM ParamTiposDocumentos
	WHERE Defecto = 1
	
	BEGIN TRAN
		UPDATE VentasRapidas
		SET TipoDocumentoId = @TipoDocumentoId,
			TipoDocumentoCodigo = @TipoDocumentoCodigo,
			TipoDocumento = @TipoDocumento,
			NroDocumento = @NroDocumento,
			CuilCuit = @CuilCuit,
			RazonSocial = upper(@RazonSocial),
			TipoResponsableId = @TipoResponsableId,
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