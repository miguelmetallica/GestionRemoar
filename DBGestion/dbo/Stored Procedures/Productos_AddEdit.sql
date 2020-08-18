CREATE PROCEDURE [Productos_AddEdit]
	@ProductoId int,
	@ProductoCodigo varchar(20),
	@Producto varchar(150),
	@Descripcion varchar(500),
	@DescripcionAdicional varchar(500),
	@CodigoBarra varchar(100),
	@PrecioCosto numeric(18,2),
	@ImpuestoId int,
	@CuentaVentaId int,
	@CuentaCompraId int,
	@UnidadMedidaId int,
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		DECLARE @TipoProductoId INT = 3
		IF @ProductoId = 0
		BEGIN
			EXEC @ProductoId = NextNumber 'Productos'
			INSERT INTO Productos(ProductoId,TipoProductoId,ProductoCodigo,Producto,Descripcion,
									DescripcionAdicional,CodigoBarra,PrecioCosto,
									ImpuestoId,CuentaVentaId,CuentaCompraId,
									UnidadMedidaId,Estado,FechaAlta,UsuarioAlta)
			VALUES(@ProductoId,@TipoProductoId,UPPER(@ProductoCodigo),UPPER(@Producto),UPPER(@Descripcion),
					UPPER(@DescripcionAdicional),UPPER(@CodigoBarra),@PrecioCosto,
					@ImpuestoId,@CuentaVentaId,@CuentaCompraId,
					@UnidadMedidaId,@Estado,GETDATE(),UPPER(@Usuario))
		END
		
		IF @ProductoId > 0
		BEGIN
			UPDATE Productos
			SET TipoProductoId = @TipoProductoId,
				ProductoCodigo = UPPER(@ProductoCodigo),
				Producto = UPPER(@Producto),
				Descripcion = UPPER(@Descripcion),
				DescripcionAdicional = UPPER(@DescripcionAdicional),
				CodigoBarra = UPPER(@CodigoBarra),
				PrecioCosto = @PrecioCosto,
				ImpuestoId = @ImpuestoId,
				CuentaVentaId = @CuentaVentaId,
				CuentaCompraId = @CuentaCompraId,
				UnidadMedidaId = @UnidadMedidaId,
				Estado = @Estado
			WHERE ProductoId = @ProductoId
		END

	COMMIT;

	SELECT @ProductoId Id

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