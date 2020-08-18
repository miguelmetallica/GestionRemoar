CREATE PROCEDURE [Clientes_Add]
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
	DECLARE @ClienteId INT;
	DECLARE @CodigoCliente varchar(20);

	SELECT @SucursalId = SucursalId
	FROM SeguridadUsuarios
	WHERE Usuario = @Usuario

	BEGIN TRAN
		EXEC @ClienteId = NextNumber 'Clientes'
		SET @CodigoCliente = RIGHT('00' + RTRIM(LTRIM(CONVERT(VARCHAR(2),@SucursalId))),2) + RIGHT('0000000000' + RTRIM(LTRIM(CONVERT(VARCHAR(10),@ClienteId))) ,8)

		INSERT INTO Clientes(ClienteId,CodigoCliente,Apellido,Nombre,TipoDocumentoId,NroDocumento,FechaNacimiento,ProvinciaId,
							LocalidadId,Calle,CalleNro,OtrasReferencias,Telefono,Celular,Email,Facebook,Instagram,Twitter,
							Estado,FechaAlta,UsuarioAlta)
			VALUES(@ClienteId,UPPER(@CodigoCliente),UPPER(@Apellido),UPPER(@Nombre),
					@TipoDocumentoId,UPPER(@NroDocumento),@FechaNacimiento,@ProvinciaId,
					@LocalidadId,UPPER(@Calle),UPPER(@CalleNro),UPPER(@OtrasReferencias),
					UPPER(@Telefono),UPPER(@Celular),UPPER(@Email),UPPER(@Facebook),
					UPPER(@Instagram),UPPER(@Twitter),@Estado,DATEADD(HH,4,GETDATE()),UPPER(@Usuario))
		

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