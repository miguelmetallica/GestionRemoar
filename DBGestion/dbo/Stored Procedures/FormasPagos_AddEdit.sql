

CREATE PROCEDURE [FormasPagos_AddEdit]
	@FormaPagoId int,
	@FormaPagoCodigo varchar(5),
	@FormaPago varchar(150),
	@Tipo char(1),
	@FechaDesde datetime,
	@FechaHasta datetime,
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @FormaPagoId = 0
		BEGIN
			EXEC @FormaPagoId = NextNumber 'FormasPagos'
			INSERT INTO FormasPagos(FormaPagoId,FormaPagoCodigo,FormaPago,Tipo,FechaDesde,FechaHasta,Estado,FechaAlta,UsuarioAlta)
			VALUES(@FormaPagoId,UPPER(@FormaPagoCodigo),UPPER(@FormaPago),UPPER(@Tipo),@FechaDesde,@FechaHasta,@Estado,GETDATE(),UPPER(@Usuario))
		END
		
		IF @FormaPagoId > 0
		BEGIN
			UPDATE FormasPagos
			SET FormaPagoCodigo = UPPER(@FormaPagoCodigo),
				FormaPago = UPPER(@FormaPago),
				Tipo = UPPER(Tipo),
				FechaDesde = @FechaDesde,
				FechaHasta = @FechaHasta,
				Estado = @Estado
			WHERE FormaPagoId = @FormaPagoId
		END

	COMMIT;

	SELECT @FormaPagoId Id

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