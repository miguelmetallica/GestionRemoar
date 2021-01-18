CREATE PROCEDURE [eliasm_db].[PresupuestosDetalleInsertar]
	@Id nvarchar(150),
	@PresupuestoId nvarchar(150),
	@ProductoId nvarchar(150),
	@ProductoCodigo nvarchar(20),
	@ProductoNombre nvarchar(150),
	@Precio numeric(18,2),
	@Cantidad int,
	@Usuario nvarchar(256) = NULL
AS
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @PorcenjateAlicuota NUMERIC(18,2) = 0
	DECLARE @PorcentajeDtoCategoria NUMERIC(18,2) = 0

	DECLARE @PrecioSinImpuesto NUMERIC(18,2) = 0
	
	DECLARE @PrecioDtoCategoria NUMERIC(18,2) = 0
	DECLARE @PrecioSinImpuestoDtoCategoria NUMERIC(18,2) = 0
	
	SELECT @PorcenjateAlicuota = A.Porcentaje,
		@PorcentajeDtoCategoria = C.DescuentoPorcentaje
	FROM Productos P
	INNER JOIN ParamAlicuotas A ON A.Id = P.AlicuotaId
	INNER JOIN ParamCategorias C ON C.Id = P.CategoriaId
	WHERE P.Id = @ProductoId

	SET @PrecioSinImpuesto = @Precio - (@Precio * (@PorcenjateAlicuota / 100))	
	SET @PrecioDtoCategoria = @Precio - (@Precio * (@PorcentajeDtoCategoria / 100))
	SET @PrecioSinImpuestoDtoCategoria = @PrecioSinImpuesto - (@PrecioSinImpuesto * (@PorcentajeDtoCategoria / 100))


	BEGIN TRAN
		INSERT INTO PresupuestosDetalle(Id,PresupuestoId,ProductoId,ProductoCodigo,ProductoNombre,Precio,PrecioSinImpuesto,
				PorcentajeDtoCategoria,PrecioDtoCategoria,PrecioSinImpuestoDtoCategoria,Cantidad,UsuarioAlta,FechaAlta)
		VALUES(@Id,@PresupuestoId,@ProductoId,@ProductoCodigo,UPPER(@ProductoNombre),@Precio,@PrecioSinImpuesto,
				@PorcentajeDtoCategoria,@PrecioDtoCategoria,@PrecioSinImpuestoDtoCategoria,@Cantidad,UPPER(@Usuario),DATEADD(HH,4,GETDATE()))
		

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