

CREATE PROCEDURE [FormasPagosCuotas_AddEdit]
	@FormaPagoCuotaId int,
	@FormaPagoId int,
	@Cuota int,
	@Porcentaje numeric(18,4),
	@Descuento numeric(18,4),
	@FechaDesde datetime,
	@FechaHasta datetime,
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @FormaPagoCuotaId = 0
		BEGIN
			EXEC @FormaPagoCuotaId = NextNumber 'FormasPagosCuotas'
			INSERT INTO FormasPagosCuotas(FormaPagoCuotaId,FormaPagoId,Cuota,Porcentaje,Descuento,FechaDesde,FechaHasta,Estado,FechaAlta,UsuarioAlta)
			VALUES(@FormaPagoCuotaId,@FormaPagoId,@Cuota,@Porcentaje,@Descuento,@FechaDesde,@FechaHasta,@Estado,GETDATE(),UPPER(@Usuario))
		END
		
		IF @FormaPagoCuotaId > 0
		BEGIN
			UPDATE FormasPagosCuotas
			SET FormaPagoId = @FormaPagoId,
				Cuota = @Cuota,
				Porcentaje = @Porcentaje,
				Descuento = @Descuento,
				FechaDesde = @FechaDesde,
				FechaHasta = @FechaHasta,
				Estado = @Estado
			WHERE FormaPagoCuotaId = @FormaPagoCuotaId
		END

	COMMIT;

	SELECT @FormaPagoCuotaId Id

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