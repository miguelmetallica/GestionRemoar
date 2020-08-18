CREATE PROCEDURE [Clientes_AddEdit]
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
	DECLARE @SucursalId INT;

	IF @ClienteId = 0
	BEGIN
		SELECT @SucursalId = SucursalId
		FROM SeguridadUsuarios
		WHERE Usuario = @Usuario
	END

	BEGIN TRAN
		IF @ClienteId = 0
		BEGIN
			
			EXEC @ClienteId = NextNumber 'Clientes'

			IF @SucursalId = 1
			BEGIN
				SET @CodigoCliente = '1' + RIGHT('0000000000' + RTRIM(LTRIM(CONVERT(VARCHAR(10),@ClienteId))) ,9)
			END
			ELSE 
			BEGIN
				IF @SucursalId = 2
				BEGIN
					SET @CodigoCliente = '2' + RIGHT('0000000000' + RTRIM(LTRIM(CONVERT(VARCHAR(10),@ClienteId))) ,9)
				END
				ELSE
				BEGIN
					IF @SucursalId = 3
						SET @CodigoCliente = '3' + RIGHT('0000000000' + RTRIM(LTRIM(CONVERT(VARCHAR(10),@ClienteId))) ,9)
					ELSE
						SET @CodigoCliente = '0' + RIGHT('0000000000' + RTRIM(LTRIM(CONVERT(VARCHAR(10),@ClienteId))) ,9)
				END
			END
			INSERT INTO Clientes(ClienteId,CodigoCliente,Apellido,Nombre,TipoDocumentoId,NroDocumento,FechaNacimiento,ProvinciaId,
								LocalidadId,Calle,CalleNro,OtrasReferencias,Telefono,Celular,Email,Facebook,Instagram,Twitter,
								Estado,FechaAlta,UsuarioAlta)
			VALUES(@ClienteId,UPPER(@CodigoCliente),UPPER(@Apellido),UPPER(@Nombre),
					@TipoDocumentoId,UPPER(@NroDocumento),@FechaNacimiento,@ProvinciaId,
					@LocalidadId,UPPER(@Calle),UPPER(@CalleNro),UPPER(@OtrasReferencias),
					UPPER(@Telefono),UPPER(@Celular),UPPER(@Email),UPPER(@Facebook),
					UPPER(@Instagram),UPPER(@Twitter),
					@Estado,GETDATE(),UPPER(@Usuario))
		END
		
		IF @ClienteId > 0
		BEGIN
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
		END

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