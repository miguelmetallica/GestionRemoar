
CREATE PROCEDURE [ParamProvincias_AddEdit]
	@ProvinciaId int,
	@PaisId int,
	@ProvinciaCodigo varchar(5),
	@Provincia varchar(150),	
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @ProvinciaId = 0
		BEGIN
			EXEC @ProvinciaId = NextNumber 'ParamProvincias'
			INSERT INTO ParamProvincias(ProvinciaId,PaisId,ProvinciaCodigo,Provincia,Estado,FechaAlta,UsuarioAlta)
			VALUES(@ProvinciaId,@PaisId,UPPER(@ProvinciaCodigo),UPPER(@Provincia),@Estado,GETDATE(),UPPER(@Usuario))
		END
		
		IF @ProvinciaId > 0
		BEGIN
			UPDATE ParamProvincias
			SET PaisId = @PaisId,
				ProvinciaCodigo = UPPER(@ProvinciaCodigo),
				Provincia = UPPER(@Provincia),
				Estado = @Estado
			WHERE ProvinciaId = @ProvinciaId
		END

	COMMIT;

	SELECT @ProvinciaId Id

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