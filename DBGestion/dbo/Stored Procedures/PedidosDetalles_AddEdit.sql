


CREATE PROCEDURE [PedidosDetalles_AddEdit]
	@PedidoDetalleId int,
	@PedidoId int,
	@ProductoId int,
	@ProductoCodigo varchar(20),
	@Producto varchar(150),
	@ColorId int,
	@Color varchar(150),
	@MarcaId int,
	@Marca varchar(150),
	@CuentaVentaId int,
	@CuentaVenta varchar(150),
	@ImpuestoId int,
	@Impuesto numeric(18,4),
	@Precio numeric(18,2),
	@Cantidad int,
	@Total numeric(18,2),
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @PedidoDetalleId = 0
		BEGIN
			EXEC @PedidoDetalleId = NextNumber 'PedidosDetalles'
			INSERT INTO PedidosDetalles(PedidoDetalleId,PedidoId,ProductoId,ProductoCodigo,
				Producto,ColorId,Color,MarcaId,Marca,CuentaVentaId,CuentaVenta,
				ImpuestoId,Impuesto,Precio,Cantidad,Total,Estado,FechaAlta,UsuarioAlta)
			VALUES(@PedidoDetalleId,@PedidoId,@ProductoId,UPPER(@ProductoCodigo),UPPER(@Producto),@ColorId,
				UPPER(@Color),@MarcaId,UPPER(@Marca),@CuentaVentaId,UPPER(@CuentaVenta),@ImpuestoId,UPPER(@Impuesto),
				@Precio,@Cantidad,@Total,@Estado,GETDATE(),UPPER(@Usuario))
		END
		
		IF @PedidoId > 0
		BEGIN
			UPDATE PedidosDetalles
			SET PedidoId = @PedidoId,
			ProductoId = @ProductoId,
			ProductoCodigo = UPPER(@ProductoCodigo),
			Producto = UPPER(@Producto),
			ColorId = @ColorId,
			Color = UPPER(@Color),
			MarcaId = @MarcaId,
			Marca = UPPER(@Marca),
			CuentaVentaId = @CuentaVentaId,
			CuentaVenta = UPPER(@CuentaVenta),
			ImpuestoId = @ImpuestoId,
			Impuesto = UPPER(@Impuesto),
			Precio = @Precio,
			Cantidad = @Cantidad,
			Total = @Total,
			Estado = @Estado
			WHERE PedidoDetalleId = @PedidoDetalleId
		END

	COMMIT;

	SELECT @PedidoDetalleId Id

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