

CREATE PROCEDURE [ParamConceptosIncluidos_AddEdit]
	@ConceptoIncluidoId int,
	@ConceptoIncluidoCodigo varchar(5),
	@ConceptoIncluido varchar(150),	
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @ConceptoIncluidoId = 0
		BEGIN
			EXEC @ConceptoIncluidoId = NextNumber 'ParamConceptosIncluidos'
			INSERT INTO ParamConceptosIncluidos(ConceptoIncluidoId,ConceptoIncluidoCodigo,ConceptoIncluido,Estado,FechaAlta,UsuarioAlta)
			VALUES(@ConceptoIncluidoId,UPPER(@ConceptoIncluidoCodigo),UPPER(@ConceptoIncluido),@Estado,GETDATE(),UPPER(@Usuario))
		END
		
		IF @ConceptoIncluidoId > 0
		BEGIN
			UPDATE ParamConceptosIncluidos
			SET ConceptoIncluidoCodigo = UPPER(@ConceptoIncluidoCodigo),
				ConceptoIncluido = UPPER(@ConceptoIncluido),
				Estado = @Estado
			WHERE ConceptoIncluidoId = @ConceptoIncluidoId
		END

	COMMIT;

	SELECT @ConceptoIncluidoId Id

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