
CREATE PROCEDURE [Sucursales_AddEdit]
	@SucursalId int,
	@Sucursal varchar(150),	
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @SucursalId = 0
		BEGIN
			EXEC @SucursalId = NextNumber 'Sucursales'
			INSERT INTO Sucursales(SucursalId,Sucursal,Estado,FechaAlta,UsuarioAlta)
			VALUES(@SucursalId,UPPER(@Sucursal),@Estado,GETDATE(),UPPER(@Usuario))
		END
		
		IF @SucursalId > 0
		BEGIN
			UPDATE Sucursales
			SET Sucursal = UPPER(@Sucursal),
				Estado = @Estado
			WHERE SucursalId = @SucursalId
		END

	COMMIT;

	SELECT @SucursalId Id

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