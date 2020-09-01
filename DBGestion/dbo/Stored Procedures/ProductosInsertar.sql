CREATE PROCEDURE [dbo].[ProductosInsertar]
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
	@EsVendedor bit = 0,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @SucursalId nvarchar(5);
	DECLARE @Numero INT;
	DECLARE @Codigo nvarchar(20);
	
	SELECT @SucursalId = S.Codigo
	FROM AspNetUsers U
	INNER JOIN Sucursales S ON S.Id = U.SucursalId
	WHERE UserName = @Usuario

	IF @SucursalId IS NULL
		SET @SucursalId = 0;

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
			EXEC @Numero = NextNumber 'PRODUCTOS'
			IF @EsVendedor = 1
				SET @Codigo = 'P' + RIGHT('000' + RTRIM(LTRIM(CONVERT(VARCHAR(3),@SucursalId))),3) + RIGHT('0000000000' + RTRIM(LTRIM(CONVERT(VARCHAR(10),@Numero))) ,8)
			ELSE
				SET @Codigo = RIGHT('000' + RTRIM(LTRIM(CONVERT(VARCHAR(3),@SucursalId))),3) + RIGHT('0000000000' + RTRIM(LTRIM(CONVERT(VARCHAR(10),@Numero))) ,8)
			
			INSERT INTO Productos(Id,TipoProductoId,Codigo,Producto,
								DescripcionCorta,DescripcionLarga,
								CodigoBarra,Peso,DimencionesLongitud,
								DimencionesAltura,DimencionesAncho,
								CuentaCompraId,CuentaVentaId,
								UnidadMedidaId,AlicuotaId,PrecioVenta,
								Estado,FechaAlta,UsuarioAlta)
						VALUES(@Id,@TipoProductoId,UPPER(@Codigo),UPPER(@Producto),
								UPPER(@DescripcionCorta),UPPER(@DescripcionLarga),
								UPPER(@CodigoBarra),@Peso,@DimencionesLongitud,
								@DimencionesAltura,@DimencionesAncho,
								@CuentaCompraId,@CuentaVentaId,
								@UnidadMedidaId,@AlicuotaId,@PrecioVenta,						
								@Estado,DATEADD(HH,4,GETDATE()),UPPER(@Usuario))		

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