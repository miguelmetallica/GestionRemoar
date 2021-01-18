CREATE PROCEDURE [dbo].[ClientesPersonaFisicaInsertar]
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
	DECLARE @SucursalId nvarchar(5);;
	DECLARE @Numero INT;
	DECLARE @CodigoCliente nvarchar(20);
	DECLARE @RazonSocial nvarchar(300);

	SELECT @SucursalId = S.Codigo
	FROM AspNetUsers U
	INNER JOIN Sucursales S ON S.Id = U.SucursalId
	WHERE UserName = @Usuario

	IF @SucursalId IS NULL
		SET @SucursalId = '000';

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
		EXEC @Numero = NextNumber 'CLIENTES'
		SET @CodigoCliente = RIGHT('000' + RTRIM(LTRIM(CONVERT(VARCHAR(3),@SucursalId))),3) + RIGHT('0000000000' + RTRIM(LTRIM(CONVERT(VARCHAR(10),@Numero))) ,8)

		INSERT INTO Clientes(Id,Codigo,Apellido,Nombre,RazonSocial,
							TipoDocumentoId,NroDocumento,CuilCuit,
							esPersonaJuridica,FechaNacimiento,ProvinciaId,
							Localidad,CodigoPostal,Calle,CalleNro,PisoDpto,
							OtrasReferencias,Telefono,Celular,Email,TipoResponsableId,CategoriaId,
							Estado,FechaAlta,UsuarioAlta)
					VALUES(@Id,UPPER(@CodigoCliente),UPPER(@Apellido),UPPER(@Nombre),UPPER(@RazonSocial),
							@TipoDocumentoId,UPPER(@NroDocumento),@CuilCuit,
							0,@FechaNacimiento,@ProvinciaId,
							UPPER(@Localidad),UPPER(@CodigoPostal),
							UPPER(@Calle),UPPER(@CalleNro),UPPER(@PisoDpto),
							UPPER(@OtrasReferencias),UPPER(@Telefono),UPPER(@Celular),
							UPPER(@Email),@TipoResponsableId,@CategoriaId,@Estado,DATEADD(HH,4,GETDATE()),UPPER(@Usuario))
		

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