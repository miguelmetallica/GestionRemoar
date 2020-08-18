

CREATE PROCEDURE [ParamCuentasVentas_AddEdit]
	@CuentaVentaId int,
	@CuentaVentaCodigo varchar(5),
	@CuentaVenta varchar(150),	
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @CuentaVentaId = 0
		BEGIN
			EXEC @CuentaVentaId = NextNumber 'ParamCuentasVentas'
			INSERT INTO ParamCuentasVentas(CuentaVentaId,CuentaVentaCodigo,CuentaVenta,Estado,FechaAlta,UsuarioAlta)
			VALUES(@CuentaVentaId,UPPER(@CuentaVentaCodigo),UPPER(@CuentaVenta),@Estado,GETDATE(),UPPER(@Usuario))
		END
		
		IF @CuentaVentaId > 0
		BEGIN
			UPDATE ParamCuentasVentas
			SET CuentaVentaCodigo = UPPER(@CuentaVentaCodigo),
				CuentaVenta = UPPER(@CuentaVenta),
				Estado = @Estado
			WHERE CuentaVentaId = @CuentaVentaId
		END

	COMMIT;

	SELECT @CuentaVentaId Id

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