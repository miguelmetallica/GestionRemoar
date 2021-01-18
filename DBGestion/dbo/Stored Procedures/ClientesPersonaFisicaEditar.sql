CREATE PROCEDURE [dbo].[ClientesPersonaFisicaEditar]
	@Id nvarchar(150),
	@Apellido nvarchar(150),
	@Nombre nvarchar(150),
	@TipoDocumentoId nvarchar(150),
	@NroDocumento nvarchar(20),
	@CuilCuit nvarchar(20) = NULL,
	@FechaNacimiento datetime = NULL,
	@ProvinciaId nvarchar(150) = NULL,
	@Localidad nvarchar(250) = NULL,
	@CodigoPostal nvarchar(10) = NULL,
	@Calle nvarchar(500) = NULL,
	@CalleNro nvarchar(50) = NULL,
	@PisoDpto nvarchar(100) = NULL,
	@OtrasReferencias nvarchar(500) = NULL,
	@Telefono nvarchar(50) = NULL,
	@Celular nvarchar(50) = NULL,
	@Email nvarchar(150) = NULL,
	@TipoResponsableId nvarchar(150) = NULL,
	@CategoriaId nvarchar(150) = NULL,
	@Estado bit = 0,
	@Usuario nvarchar(256) = NULL
AS
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @RazonSocial nvarchar(300);
	
	SET @RazonSocial = CONVERT(VARCHAR(140),@Apellido) + ' ' + @Nombre

	IF @TipoResponsableId IS NULL
	BEGIN
		SELECT TOP 1 @TipoResponsableId = Id
		FROM ParamTiposResponsables
		WHERE Defecto = 1
	END

	IF @CategoriaId IS NULL
	BEGIN
		SELECT TOP 1 @CategoriaId = Id
		FROM ParamClientesCategorias
		WHERE Defecto = 1
	END

	BEGIN TRAN
		UPDATE Clientes
		SET Apellido = UPPER(@Apellido),
		Nombre = UPPER(@Nombre),
		RazonSocial = UPPER(@RazonSocial),
		TipoDocumentoId = @TipoDocumentoId,
		NroDocumento = UPPER(@NroDocumento),
		CuilCuit = @CuilCuit,
		esPersonaJuridica = 0,
		FechaNacimiento = @FechaNacimiento,
		ProvinciaId = @ProvinciaId,
		Localidad = UPPER(@Localidad),
		CodigoPostal = UPPER(@CodigoPostal),
		Calle = UPPER(@Calle),
		CalleNro = UPPER(@CalleNro),
		PisoDpto = UPPER(@PisoDpto),
		OtrasReferencias = UPPER(@OtrasReferencias),
		Telefono = UPPER(@Telefono),
		Celular = UPPER(@Celular),
		Email = UPPER(@Email),
		TipoResponsableId = @TipoResponsableId,
		CategoriaId = @CategoriaId,
		Estado = @Estado
		WHERE Id = @Id

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