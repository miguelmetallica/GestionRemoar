
CREATE PROCEDURE [ParamTarjetasCreditosDebitos_AddEdit]
	@TarjetaId int,
	@TarjetaCodigo varchar(5),
	@TarjetaNombre varchar(150),	
	@EsDebito bit,
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @TarjetaId = 0
		BEGIN
			EXEC @TarjetaId = NextNumber 'ParamTarjetasCreditosDebitos'
			INSERT INTO ParamTarjetasCreditosDebitos(TarjetaId,TarjetaCodigo,TarjetaNombre,EsDebito,Estado,FechaAlta,UsuarioAlta)
			VALUES(@TarjetaId,UPPER(@TarjetaCodigo),UPPER(@TarjetaNombre),@EsDebito,@Estado,GETDATE(),UPPER(@Usuario))
		END
		
		IF @TarjetaId > 0
		BEGIN
			UPDATE ParamTarjetasCreditosDebitos
			SET TarjetaCodigo = UPPER(@TarjetaCodigo),
				TarjetaNombre = UPPER(@TarjetaNombre),
				EsDebito = @EsDebito,
				Estado = @Estado
			WHERE TarjetaId = @TarjetaId
		END

	COMMIT;

	SELECT @TarjetaId Id

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