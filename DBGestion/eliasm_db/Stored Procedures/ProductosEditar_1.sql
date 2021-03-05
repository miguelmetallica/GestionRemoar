CREATE PROCEDURE [eliasm_db].[ProductosEditar]
	@Id nvarchar(150),
	@TipoProductoId nvarchar(150) = NULL,
	@Producto nvarchar(150) = NULL,
	@DescripcionCorta nvarchar(150) = NULL,
	@DescripcionLarga nvarchar(150) = NULL,
	@CodigoBarra nvarchar(100) = NULL,
	@Peso numeric(18,2) = 0,
	@DimencionesLongitud numeric(18,2) = 0,
	@DimencionesAncho numeric(18,2) = 0,
	@DimencionesAltura numeric(18,2) = 0,
	@CuentaCompraId nvarchar(150) = NULL,
	@CuentaVentaId nvarchar(150) = NULL,
	@UnidadMedidaId nvarchar(150) = NULL,
	@AlicuotaId nvarchar(150) = NULL,
	@PrecioVenta numeric(18,2) = 0,
	@ProveedorId nvarchar(150) = NULL,
	@CategoriaId nvarchar(150) = NULL,
	@AceptaDescuento bit = 0,
	@PrecioRebaja numeric(18,2) = 0,
	@RebajaDesde datetime = null,
	@RebajaHasta datetime = null,
	@ControlaStock bit = 0,
	@Estado bit = 0,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @ProveedorCodigo nvarchar(150) = NULL
	DECLARE @CategoriaCodigo nvarchar(150) = NULL

	IF @TipoProductoId IS NULL
	BEGIN
		SELECT @TipoProductoId = MIN(A.Id)
		FROM ParamTiposProductos A 
		WHERE A.Defecto = 1		
	END

	IF @CuentaCompraId IS NULL
	BEGIN
		SELECT @CuentaCompraId = MIN(A.Id)
		FROM ParamCuentasCompras A 
		WHERE A.Defecto = 1		
	END

	IF @CuentaVentaId IS NULL
	BEGIN
		SELECT @CuentaVentaId = MIN(A.Id)
		FROM ParamCuentasVentas A 
		WHERE A.Defecto = 1		
	END

	IF @UnidadMedidaId IS NULL
	BEGIN
		SELECT @UnidadMedidaId = MIN(A.Id)
		FROM ParamUnidadesMedidas A 
		WHERE A.Defecto = 1		
	END

	IF @AlicuotaId IS NULL
	BEGIN
		SELECT @AlicuotaId = MIN(A.Id)
		FROM ParamAlicuotas A 
		WHERE A.Defecto = 1		
	END

	IF @ProveedorId IS NULL
	BEGIN
		SET @ProveedorCodigo = '000';		
	END
	ELSE
	BEGIN
		SELECT @ProveedorCodigo = MIN(A.Codigo)
		FROM Proveedores A 
		WHERE A.Id = @ProveedorId		
	END

	IF @CategoriaId IS NULL
	BEGIN
		SELECT @CategoriaCodigo = MIN(A.Codigo)
		FROM ParamCategorias A 
		WHERE A.Defecto = 1		
	END
	ELSE
	BEGIN
		SELECT @CategoriaCodigo = MIN(A.Codigo)
		FROM ParamCategorias A 
		WHERE A.Id = @CategoriaId		
	END

	IF @CategoriaId IS NULL
	BEGIN
		SET @CategoriaCodigo = '000'		
	END

	
	BEGIN TRAN
		UPDATE Productos
		SET TipoProductoId = @TipoProductoId,
		Producto = UPPER(@Producto),
		DescripcionCorta = UPPER(@DescripcionCorta),
		DescripcionLarga = UPPER(@DescripcionLarga),
		CodigoBarra = UPPER(@CodigoBarra),
		Peso = @Peso,
		DimencionesLongitud = @DimencionesLongitud,
		DimencionesAncho = @DimencionesAncho,
		DimencionesAltura = @DimencionesAltura,
		CuentaVentaId = @CuentaVentaId,
		CuentaCompraId = @CuentaCompraId,
		UnidadMedidaId = @UnidadMedidaId,
		AlicuotaId = @AlicuotaId,
		PrecioVenta = @PrecioVenta,
		ProveedorId = @ProveedorId,
		CategoriaId = @CategoriaId,
		Codigo = RIGHT('000' + RTRIM(LTRIM(CONVERT(VARCHAR(3),@ProveedorCodigo))),3) + RIGHT('000' + RTRIM(LTRIM(CONVERT(VARCHAR(3),@CategoriaCodigo))),3) + RIGHT('000000' + RTRIM(LTRIM(Codigo)) ,6),
		AceptaDescuento = @AceptaDescuento,
		PrecioRebaja = @PrecioRebaja,
		RebajaDesde = @RebajaDesde,
		RebajaHasta = @RebajaHasta,
		ControlaStock = @ControlaStock,
		Estado = @Estado
		WHERE Id = @Id
		
	COMMIT;

	SELECT 1 Id

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