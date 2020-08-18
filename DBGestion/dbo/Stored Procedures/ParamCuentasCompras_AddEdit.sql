

CREATE PROCEDURE [ParamCuentasCompras_AddEdit]
	@CuentaCompraId int,
	@CuentaCompraCodigo varchar(5),
	@CuentaCompra varchar(150),	
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @CuentaCompraId = 0
		BEGIN
			EXEC @CuentaCompraId = NextNumber 'ParamCuentasCompras'
			INSERT INTO ParamCuentasCompras(CuentaCompraId,CuentaCompraCodigo,CuentaCompra,Estado,FechaAlta,UsuarioAlta)
			VALUES(@CuentaCompraId,UPPER(@CuentaCompraCodigo),UPPER(@CuentaCompra),@Estado,GETDATE(),UPPER(@Usuario))
		END
		
		IF @CuentaCompraId > 0
		BEGIN
			UPDATE ParamCuentasCompras
			SET CuentaCompraCodigo = UPPER(@CuentaCompraCodigo),
				CuentaCompra = UPPER(@CuentaCompra),
				Estado = @Estado
			WHERE CuentaCompraId = @CuentaCompraId
		END

	COMMIT;

	SELECT @CuentaCompraId Id

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