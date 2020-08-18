

CREATE PROCEDURE [ParamImpuestos_AddEdit]
	@ImpuestoId int,
	@Impuesto varchar(150),	
	@Valor numeric(18,4),	
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @ImpuestoId = 0
		BEGIN
			EXEC @ImpuestoId = NextNumber 'ParamImpuestos'
			INSERT INTO ParamImpuestos(ImpuestoId,Impuesto,Valor,Estado,FechaAlta,UsuarioAlta)
			VALUES(@ImpuestoId,UPPER(@Impuesto),@Valor,@Estado,GETDATE(),UPPER(@Usuario))
		END
		
		IF @ImpuestoId > 0
		BEGIN
			UPDATE ParamImpuestos
			SET Impuesto = UPPER(@Impuesto),
				Valor = @Valor,
				Estado = @Estado
			WHERE ImpuestoId = @ImpuestoId
		END

	COMMIT;

	SELECT @ImpuestoId Id

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