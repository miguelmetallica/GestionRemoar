CREATE PROCEDURE [Clientes_Edit]
	@ClienteId int,
	@CodigoCliente varchar(20),
	@Apellido varchar(150),
	@Nombre varchar(150),
	@TipoDocumentoId int,
	@NroDocumento varchar(20),
	@FechaNacimiento datetime,
	@ProvinciaId int,
	@LocalidadId int,
	@Calle varchar(1000),
	@CalleNro varchar(10),
	@OtrasReferencias varchar(1000),
	@Telefono varchar(50),
	@Celular varchar(50),
	@Email varchar(150),
	@Facebook varchar(150),
	@Instagram varchar(150),
	@Twitter varchar(150),
	@Estado bit,
	@Usuario nvarchar(256)
AS
BEGIN TRY
	SET NOCOUNT ON;
	
	BEGIN TRAN
		UPDATE Clientes
		SET CodigoCliente = UPPER(@CodigoCliente),
			Apellido = UPPER(@Apellido),
			Nombre = UPPER(@Nombre),
			TipoDocumentoId = @TipoDocumentoId,
			NroDocumento = UPPER(@NroDocumento),
			FechaNacimiento = @FechaNacimiento,
			ProvinciaId = @ProvinciaId,
			LocalidadId = @LocalidadId,
			Calle = UPPER(@Calle),
			CalleNro = UPPER(@CalleNro),
			OtrasReferencias = UPPER(@OtrasReferencias),
			Telefono = UPPER(@Telefono),
			Celular = UPPER(@Celular),
			Email = UPPER(@Email),
			Facebook = UPPER(@Facebook),
			Instagram = UPPER(@Instagram),
			Twitter = UPPER(@Twitter),
			Estado = @Estado
		WHERE ClienteId = @ClienteId
		
	COMMIT;

	SELECT @ClienteId Id

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