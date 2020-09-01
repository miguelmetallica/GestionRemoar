﻿CREATE PROCEDURE [dbo].[ProductosEditar]
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
	@Estado bit = 0,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
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


	BEGIN TRAN
		UPDATE dbo.Productos
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