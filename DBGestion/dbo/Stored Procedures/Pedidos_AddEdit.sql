

CREATE PROCEDURE [Pedidos_AddEdit]
	@PedidoId int,
	@PedidoCodigo varchar(10),
	@ClienteId int,
	@ClienteCodigo varchar(20),
	@Apellido varchar(150),
	@Nombre varchar(150),
	@TipoDocumentoId int,
	@TipoDocumento varchar(150),
	@NroDocumento varchar(20),
	@ProvinciaFacturacionId int,
	@ProvinciaFacturacion varchar(150),
	@LocalidadFacturacionId int,
	@LocalidadFacturacion varchar(150),
	@CalleFacturacion varchar(500),
	@NroCalleFacturacion varchar(8),
	@DatosDomicilioFacturacion varchar(200),
	@ProvinciaEnvioId int,
	@ProvinciaEnvio varchar(150),
	@LocalidadEnvioId int,
	@LocalidadEnvio varchar(150),
	@CalleEnvio varchar(500),
	@NroCalleEnvio varchar(8),
	@DatosDomicilioEnvio varchar(200),
	@TipoCondicionIvaId int,
	@TipoCondicionIva varchar(150),
	@FechaEmision datetime,
	@FechaVencimiento datetime,
	@MontoDescuento decimal,
	@MontoExento decimal,
	@MontoGravadoNormal decimal,
	@MontoNoGravado decimal,
	@MontoGravadoReducido decimal,
	@MontoInteres decimal,
	@IvaInscriptoNormal decimal,
	@IvaNoInscrNormal decimal,
	@IvaInscriptoReducido decimal,
	@IvaNoInscrReducido decimal,
	@MontoGravodo10 decimal,
	@MontoGravado21 decimal,
	@MontoGravado27 decimal,
	@IvaInscripto10 decimal,
	@IvaInscripto21 decimal,
	@IvaInscripto27 decimal,
	@IvaNoInscripto10 decimal,
	@IvaNoInscripto21 decimal,
	@IvaNoInscripto27 decimal,
	@MontoGravodoNormal10 decimal,
	@MontoGravadoNormal21 decimal,
	@MontoGravadoNormal27 decimal,
	@IvaInscriptoNormal10 decimal,
	@IvaInscriptoNormal21 decimal,
	@IvaInscriptoNormal27 decimal,
	@IvaNoInscriptoNormal10 decimal,
	@IvaNoInscriptoNormal21 decimal,
	@IvaNoInscriptoNormal27 decimal,
	@PercepcionIva decimal,
	@ImpuestoMunicipal decimal,
	@PercepcionImpuestoMunicipal decimal,
	@TipoConvenioIIBBId int,
	@FechaVigDesdeIIBB datetime,
	@FechaVigHastaIIBB datetime,
	@AlicuotaIIBB decimal,
	@PercepcionIIBB decimal,
	@Total decimal,
	@Facturado bit,
	@Observaciones varchar(200),	
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	BEGIN TRAN
		IF @PedidoId = 0
		BEGIN
			EXEC @PedidoId = NextNumber 'Pedidos'
			INSERT INTO Pedidos(PedidoId,PedidoCodigo,ClienteId,ClienteCodigo,Apellido,
								Nombre,TipoDocumentoId,TipoDocumento,NroDocumento,ProvinciaFacturacionId,
								ProvinciaFacturacion,LocalidadFacturacionId,LocalidadFacturacion,
								CalleFacturacion,NroCalleFacturacion,DatosDomicilioFacturacion,
								ProvinciaEnvioId,ProvinciaEnvio,LocalidadEnvioId,LocalidadEnvio,
								CalleEnvio,NroCalleEnvio,DatosDomicilioEnvio,TipoCondicionIvaId,TipoCondicionIva,
								FechaEmision,FechaVencimiento,MontoDescuento,MontoExento,
								MontoGravadoNormal,MontoNoGravado,MontoGravadoReducido,MontoInteres,
								IvaInscriptoNormal,ivaNoInscrNormal,IvaInscriptoReducido,IvaNoInscrReducido,
								MontoGravodo10,MontoGravado21,MontoGravado27,IvaInscripto10,
								IvaInscripto21,IvaInscripto27,IvaNoInscripto10,IvaNoInscripto21,
								IvaNoInscripto27,MontoGravodoNormal10,MontoGravadoNormal21,
								MontoGravadoNormal27,IvaInscriptoNormal10,IvaInscriptoNormal21,
								IvaInscriptoNormal27,IvaNoInscriptoNormal10,IvaNoInscriptoNormal21,
								IvaNoInscriptoNormal27,PercepcionIva,ImpuestoMunicipal,
								PercepcionImpuestoMunicipal,TipoConvenioIIBBId,FechaVigDesdeIIBB,
								FechaVigHastaIIBB,AlicuotaIIBB,PercepcionIIBB,Total,Facturado,
								Observaciones,Estado,FechaAlta,UsuarioAlta)
						VALUES(@PedidoId,UPPER(@PedidoCodigo),@ClienteId,UPPER(@ClienteCodigo),UPPER(@Apellido),
								UPPER(@Nombre),UPPER(@TipoDocumentoId),UPPER(@TipoDocumento),UPPER(@NroDocumento),
								@ProvinciaFacturacionId,UPPER(@ProvinciaFacturacion),@LocalidadFacturacionId,
								UPPER(@LocalidadFacturacion),UPPER(@CalleFacturacion),UPPER(@NroCalleFacturacion),
								UPPER(@DatosDomicilioFacturacion),@ProvinciaEnvioId,UPPER(@ProvinciaEnvio),
								@LocalidadEnvioId,UPPER(@LocalidadEnvio),UPPER(@CalleEnvio),
								UPPER(@NroCalleEnvio),UPPER(@DatosDomicilioEnvio),@TipoCondicionIvaId,
								UPPER(@TipoCondicionIva),@FechaEmision,@FechaVencimiento,@MontoDescuento,@MontoExento,
								@MontoGravadoNormal,@MontoNoGravado,@MontoGravadoReducido,@MontoInteres,
								@IvaInscriptoNormal,@IvaNoInscrNormal,@IvaInscriptoReducido,@IvaNoInscrReducido,
								@MontoGravodo10,@MontoGravado21,@MontoGravado27,@IvaInscripto10,
								@IvaInscripto21,@IvaInscripto27,@IvaNoInscripto10,@IvaNoInscripto21,
								@IvaNoInscripto27,@MontoGravodoNormal10,@MontoGravadoNormal21,
								@MontoGravadoNormal27,@IvaInscriptoNormal10,@IvaInscriptoNormal21,
								@IvaInscriptoNormal27,@IvaNoInscriptoNormal10,@IvaNoInscriptoNormal21,
								@IvaNoInscriptoNormal27,@PercepcionIva,@ImpuestoMunicipal,
								@PercepcionImpuestoMunicipal,@TipoConvenioIIBBId,@FechaVigDesdeIIBB,
								@FechaVigHastaIIBB,@AlicuotaIIBB,@PercepcionIIBB,@Total,@Facturado,
								UPPER(@Observaciones),@Estado,GETDATE(),UPPER(@Usuario))
		END
		
		IF @PedidoId > 0
		BEGIN
			UPDATE Pedidos
			SET PedidoCodigo = UPPER(@PedidoCodigo),
			ClienteId = @ClienteId,
			ClienteCodigo = UPPER(@ClienteCodigo),
			Apellido = UPPER(@Apellido),
			Nombre = UPPER(@Nombre),
			TipoDocumentoId = @TipoDocumentoId,
			TipoDocumento = UPPER(@TipoDocumento),
			NroDocumento = UPPER(@NroDocumento),
			ProvinciaFacturacionId = @ProvinciaFacturacionId,
			ProvinciaFacturacion = UPPER(@ProvinciaFacturacion),
			LocalidadFacturacionId = @LocalidadFacturacionId,
			LocalidadFacturacion = UPPER(@LocalidadFacturacion),
			CalleFacturacion = UPPER(@CalleFacturacion),
			NroCalleFacturacion = UPPER(@NroCalleFacturacion),
			DatosDomicilioFacturacion = UPPER(@DatosDomicilioFacturacion),
			ProvinciaEnvioId = @ProvinciaEnvioId,
			ProvinciaEnvio = UPPER(@ProvinciaEnvio),
			LocalidadEnvioId = @LocalidadEnvioId,
			LocalidadEnvio = UPPER(@LocalidadEnvio),
			CalleEnvio = UPPER(@CalleEnvio),
			NroCalleEnvio = UPPER(@NroCalleEnvio),
			DatosDomicilioEnvio = UPPER(@DatosDomicilioEnvio),
			TipoCondicionIvaId = @TipoCondicionIvaId,
			TipoCondicionIva = UPPER(@TipoCondicionIva),
			FechaEmision = @FechaEmision,
			FechaVencimiento = @FechaVencimiento,
			MontoDescuento = @MontoDescuento,
			MontoExento = @MontoExento,
			MontoGravadoNormal = @MontoGravadoNormal,
			MontoNoGravado = @MontoNoGravado,
			MontoGravadoReducido = @MontoGravadoReducido,
			MontoInteres = @MontoInteres,
			IvaInscriptoNormal = @IvaInscriptoNormal,
			IvaNoInscrNormal = @IvaNoInscrNormal,
			IvaInscriptoReducido = @IvaInscriptoReducido,
			IvaNoInscrReducido = @IvaNoInscrReducido,
			MontoGravodo10 = @MontoGravodo10,
			MontoGravado21 = @MontoGravado21,
			MontoGravado27 = @MontoGravado27,
			IvaInscripto10 = @IvaInscripto10,
			IvaInscripto21 = @IvaInscripto21,
			IvaInscripto27 = @IvaInscripto27,
			IvaNoInscripto10 = @IvaNoInscripto10,
			IvaNoInscripto21 = @IvaNoInscripto21,
			IvaNoInscripto27 = @IvaNoInscripto27,
			MontoGravodoNormal10 = @MontoGravodoNormal10,
			MontoGravadoNormal21 = @MontoGravadoNormal21,
			MontoGravadoNormal27 = @MontoGravadoNormal27,
			IvaInscriptoNormal10 = @IvaInscriptoNormal10,
			IvaInscriptoNormal21 = @IvaInscriptoNormal21,
			IvaInscriptoNormal27 = @IvaInscriptoNormal27,
			IvaNoInscriptoNormal10 = @IvaNoInscriptoNormal10,
			IvaNoInscriptoNormal21 = @IvaNoInscriptoNormal21,
			IvaNoInscriptoNormal27 = @IvaNoInscriptoNormal27,
			PercepcionIva = @PercepcionIva,
			ImpuestoMunicipal = @ImpuestoMunicipal,
			PercepcionImpuestoMunicipal = @PercepcionImpuestoMunicipal,
			TipoConvenioIIBBId = @TipoConvenioIIBBId,
			FechaVigDesdeIIBB = @FechaVigDesdeIIBB,
			FechaVigHastaIIBB = @FechaVigHastaIIBB,
			AlicuotaIIBB = @AlicuotaIIBB,
			PercepcionIIBB = @PercepcionIIBB,
			Total = @Total,
			Facturado = @Facturado,
			Observaciones = UPPER(@Observaciones),
			Estado = @Estado
			WHERE PedidoId = @PedidoId
		END

	COMMIT;

	SELECT @PedidoId Id

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