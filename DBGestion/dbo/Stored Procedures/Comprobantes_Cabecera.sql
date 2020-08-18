
CREATE PROCEDURE [Comprobantes_Cabecera]
	@ComprobanteId int,
	@TipoComprobanteId int,
	@FechaComprobante datetime,
	@ConceptoIncluidoId int,
	@PeriodoFacturadoDesde datetime,
	@PeriodoFacturadoHasta datetime,
	@FechaVencimiento datetime,
	@TipoResponsableId int,
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	DECLARE @TipoComprobante Varchar(150)
	DECLARE @TipoComprobanteCodigo Varchar(5)
	DECLARE @ConceptoIncluido Varchar(150)
	DECLARE @TipoResponsable Varchar(150)
	DECLARE @SucursalId Varchar(150)
	DECLARE @ComprobanteCodigo Varchar(20)

	DECLARE @Letra Varchar(1) = 'X'
	DECLARE @PtoVenta int
	DECLARE @Numero numeric(10)
	
	SELECT @TipoComprobante = TipoComprobante,@TipoComprobanteCodigo = TipoComprobanteCodigo
	FROM ParamTiposComprobantes 
	WHERE TipoComprobanteId = @TipoComprobanteId

	SELECT @ConceptoIncluido = ConceptoIncluido
	FROM ParamConceptosIncluidos 
	WHERE ConceptoIncluidoId = @ConceptoIncluidoId

	SELECT @TipoResponsable = TipoResponsable
	FROM ParamTiposResponsables
	WHERE TipoResponsableId = @TipoResponsableId

	SELECT @SucursalId = SucursalId
	FROM SeguridadUsuarios
	WHERE Usuario = @Usuario

	BEGIN TRAN
		IF @ComprobanteId = 0
		BEGIN
			EXEC @ComprobanteId = NextNumber 'Comprobantes'

			set @Numero = @ComprobanteId 

			IF @SucursalId = 1
			begin
				SET @ComprobanteCodigo = '1' + RIGHT('0000000000' + RTRIM(LTRIM(CONVERT(VARCHAR(10),@ComprobanteId))) ,9)
				set @PtoVenta = 1
			end
			IF @SucursalId = 2
			begin
				SET @ComprobanteCodigo = '2' + RIGHT('0000000000' + RTRIM(LTRIM(CONVERT(VARCHAR(10),@ComprobanteId))) ,9)
				set @PtoVenta = 2
			end
			IF @SucursalId = 3
			begin
				SET @ComprobanteCodigo = '3' + RIGHT('0000000000' + RTRIM(LTRIM(CONVERT(VARCHAR(10),@ComprobanteId))) ,9)
				set @PtoVenta = 3
			end

			INSERT INTO Comprobantes(ComprobanteId,ComprobanteCodigo,TipoComprobanteId,TipoComprobante,TipoComprobanteCodigo,
									FechaComprobante,ConceptoIncluidoId,ConceptoIncluido,PeriodoFacturadoDesde,
									PeriodoFacturadoHasta,FechaVencimiento,TipoResponsableId,TipoResponsable,Confirmado,Cobrado,
									Estado,FechaAlta,UsuarioAlta,Letra,PtoVenta,Numero )
						VALUES(@ComprobanteId,@ComprobanteCodigo,@TipoComprobanteId,@TipoComprobante,@TipoComprobanteCodigo,
								@FechaComprobante,@ConceptoIncluidoId,@ConceptoIncluido,@PeriodoFacturadoDesde,
								@PeriodoFacturadoHasta,@FechaVencimiento,@TipoResponsableId,@TipoResponsable,0,0,
								@Estado,GETDATE(),UPPER(@Usuario),@Letra,@PtoVenta,@Numero)
		END
		
		IF @ComprobanteId > 0
		BEGIN
			UPDATE Comprobantes
			SET TipoComprobanteId = @TipoComprobanteId,
				TipoComprobante = @TipoComprobante,
				FechaComprobante = @FechaComprobante,
				TipoResponsableId = @TipoResponsableId,
				TipoResponsable = @TipoResponsable,
				Estado = @Estado
			WHERE ComprobanteId = @ComprobanteId
		END

	COMMIT;

	SELECT @ComprobanteId Id

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