
CREATE PROCEDURE [Comprobantes_Cliente]
	@ComprobanteId int,
	@ClienteId int,
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY 
	DECLARE @ClienteCodigo VARCHAR(20)
	DECLARE @TipoDocumentoId INT
	DECLARE @TipoDocumento VARCHAR(150)
	DECLARE @NroDocumento VARCHAR(20)
	DECLARE @Apellido VARCHAR(150)
	DECLARE @Nombre VARCHAR(150)
	DECLARE @ProvinciaId INT
	DECLARE @Provincia VARCHAR(150)
	DECLARE @LocalidadId INT
	DECLARE @Localidad VARCHAR(150)
	DECLARE @Calle VARCHAR(1000)
	DECLARE @Nro VARCHAR(10)
	DECLARE @OtrasReferencias VARCHAR(1000)
	DECLARE @Email VARCHAR(150)
	DECLARE @Celular VARCHAR(50)
		
	SELECT @ClienteCodigo = C.CodigoCliente,
			@TipoDocumentoId = C.TipoDocumentoId,
			@TipoDocumento = TipoDocumento,
			@NroDocumento = C.NroDocumento,
			@Apellido = C.Apellido,
			@Nombre = C.Nombre,
			@ProvinciaId = C.ProvinciaId,
			@Provincia = Provincia,
			@LocalidadId = C.LocalidadId,
			@Localidad = Localidad,
			@Calle = C.Calle,
			@Nro = C.CalleNro,
			@OtrasReferencias = C.OtrasReferencias,
			@Celular = C.Celular,
			@Email = C.Email
	FROM Clientes C
	LEFT JOIN ParamTiposDocumentos T ON T.TipoDocumentoId = C.TipoDocumentoId
	LEFT JOIN ParamProvincias P ON P.ProvinciaId = C.ProvinciaId
	LEFT JOIN ParamLocalidades L ON L.LocalidadId = C.LocalidadId
	WHERE C.ClienteId = @ClienteId
	
	BEGIN TRAN
		IF @ComprobanteId > 0
		BEGIN
			UPDATE Comprobantes
			SET ClienteId = @ClienteId,
				ClienteCodigo = @ClienteCodigo,
				TipoDocumentoId = @TipoDocumentoId,
				TipoDocumento = @TipoDocumento,
				NroDocumento = @NroDocumento,
				Apellido = @Apellido,
				Nombre = @Nombre,
				ProvinciaId = @ProvinciaId,
				Provincia = @Provincia,
				LocalidadId = @LocalidadId,
				Localidad = @Localidad,
				Calle = @Calle,
				Nro = @Nro,
				OtrasReferencias = @OtrasReferencias,
				Telefono = @Celular,
				Email = @Email,
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