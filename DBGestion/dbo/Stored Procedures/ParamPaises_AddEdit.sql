
CREATE PROCEDURE [ParamPaises_AddEdit]
	@PaisId int,
	@PaisCodigo varchar(5),
	@Pais varchar(150),	
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @PaisId = 0
		BEGIN
			EXEC @PaisId = NextNumber 'ParamPaises'
			INSERT INTO ParamPaises(PaisId,PaisCodigo,Pais,Estado,FechaAlta,UsuarioAlta)
			VALUES(@PaisId,UPPER(@PaisCodigo),UPPER(@Pais),@Estado,GETDATE(),UPPER(@Usuario))
		END
		
		IF @PaisId > 0
		BEGIN
			UPDATE ParamPaises
			SET PaisCodigo = UPPER(@PaisCodigo),
				Pais = UPPER(@Pais),
				Estado = @Estado
			WHERE PaisId = @PaisId
		END

	COMMIT;

	SELECT @PaisId Id

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