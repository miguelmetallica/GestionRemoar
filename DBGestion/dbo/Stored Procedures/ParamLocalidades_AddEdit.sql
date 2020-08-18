
CREATE PROCEDURE [ParamLocalidades_AddEdit]
	@LocalidadId int,
	@ProvinciaId int,
	@LocalidadCodigo varchar(5),
	@Localidad varchar(150),
	@CodigoPostal varchar(8),	
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @LocalidadId = 0
		BEGIN
			EXEC @LocalidadId = NextNumber 'ParamLocalidades'
			INSERT INTO ParamLocalidades(LocalidadId,ProvinciaId,LocalidadCodigo,Localidad,CodigoPostal,Estado,FechaAlta,UsuarioAlta)
			VALUES(@LocalidadId,@ProvinciaId,UPPER(@LocalidadCodigo),UPPER(@Localidad),UPPER(@CodigoPostal),@Estado,GETDATE(),UPPER(@Usuario))
		END
		
		IF @LocalidadId > 0
		BEGIN
			UPDATE ParamLocalidades
			SET ProvinciaId = @ProvinciaId,
				LocalidadCodigo = UPPER(@LocalidadCodigo),
				Localidad = UPPER(@Localidad),
				CodigoPostal = UPPER(@CodigoPostal),
				Estado = @Estado
			WHERE LocalidadId = @LocalidadId
		END

	COMMIT;

	SELECT @LocalidadId Id

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