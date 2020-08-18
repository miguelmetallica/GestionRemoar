
CREATE PROCEDURE [ParamBancos_AddEdit]
	@BancoId int,
	@BancoCodigo varchar(5),
	@Banco varchar(150),	
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @BancoId = 0
		BEGIN
			EXEC @BancoId = NextNumber 'ParamBancos'
			INSERT INTO ParamBancos(BancoId,BancoCodigo,Banco,Estado,FechaAlta,UsuarioAlta)
			VALUES(@BancoId,UPPER(@BancoCodigo),UPPER(@Banco),@Estado,GETDATE(),UPPER(@Usuario))
		END
		
		IF @BancoId > 0
		BEGIN
			UPDATE ParamBancos
			SET BancoCodigo = UPPER(@BancoCodigo),
				Banco = UPPER(@Banco),
				Estado = @Estado
			WHERE BancoId = @BancoId
		END

	COMMIT;

	SELECT @BancoId Id

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