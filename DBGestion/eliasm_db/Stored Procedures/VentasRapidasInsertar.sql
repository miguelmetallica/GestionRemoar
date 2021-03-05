CREATE PROCEDURE [eliasm_db].[VentasRapidasInsertar]
	@Id nvarchar(150),
	@ClienteId nvarchar(150),
	@Usuario nvarchar(256) = NULL
AS
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @Sucursal nvarchar(150);
	DECLARE @SucursalId nvarchar(5);
	DECLARE @Numero INT;
	DECLARE @Codigo nvarchar(20);
	DECLARE @EstadoId nvarchar(150);
	DECLARE @TipoResponsableId nvarchar(150);	
	DECLARE @CategoriaId nvarchar(150);
	DECLARE @Validez int;
	
	DECLARE @ClienteCodigo nvarchar(50);
	DECLARE @TipoDocumentoId nvarchar(150);
	DECLARE @TipoDocumentoCodigo nvarchar(5);
	DECLARE @TipoDocumento nvarchar(15);
	DECLARE @NroDocumento nvarchar(15);
	DECLARE @CuilCuit nvarchar(20);
	DECLARE @RazonSocial nvarchar(300);
	DECLARE @ProvinciaId nvarchar(150);
	DECLARE @ProvinciaCodigo nvarchar(5);
	DECLARE @Provincia nvarchar(150);
	DECLARE @Localidad nvarchar(150);
	DECLARE @CodigoPostal nvarchar(10);
	DECLARE @Calle nvarchar(500);
	DECLARE @CalleNro nvarchar(50);
	DECLARE @PisoDpto nvarchar(100);
	DECLARE @OtrasReferencias nvarchar(500);
	DECLARE @Email nvarchar(150);
	DECLARE @Telefono nvarchar(50);
	DECLARE @Celular nvarchar(50);

	SELECT @SucursalId = S.Codigo,@Sucursal = S.Id
	FROM AspNetUsers U
	INNER JOIN Sucursales S ON S.Id = U.SucursalId
	WHERE UserName = @Usuario

	IF @SucursalId IS NULL
		SET @SucursalId = '000';
	

	SET @Validez = 3
	
	SELECT @EstadoId = E.Id
	FROM SistemaConfiguraciones C
	INNER JOIN ParamPresupuestosEstados E ON E.Codigo = C.Valor
	WHERE C.Configuracion = 'PRESUPUESTO_PENDIENTE_CLIENTE'

	SELECT TOP 1 
		@TipoResponsableId = TipoResponsableId,
		@CategoriaId = CategoriaId,
		@ClienteId = P.Id,
		@ClienteCodigo = P.Codigo		
	FROM Clientes P
	LEFT JOIN ParamTiposDocumentos D ON D.Id = P.TipoDocumentoId
	LEFT JOIN ParamProvincias R ON R.Id = P.ProvinciaId	
	WHERE p.Id = @ClienteId

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
		EXEC @Numero = NextNumber 'VentasRapidas'
		SET @Codigo = RIGHT('000' + RTRIM(LTRIM(CONVERT(VARCHAR(3),@SucursalId))),3) + RIGHT('0000000000' + RTRIM(LTRIM(CONVERT(VARCHAR(10),@Numero))) ,8)

		INSERT INTO VentasRapidas(Id,
								Codigo,
								Fecha,
								FechaVencimiento,
								ClienteId,
								ClienteCodigo,
								TipoDocumentoId,
								TipoDocumentoCodigo,
								TipoDocumento,
								NroDocumento,
								CuilCuit,
								RazonSocial,
								ProvinciaId,
								ProvinciaCodigo,
								Provincia,
								Localidad,
								CodigoPostal,
								Calle,
								CalleNro,
								PisoDpto,
								OtrasReferencias,
								Email,
								Telefono,
								Celular,
								TipoResponsableId,
								ClienteCategoriaId,
								EstadoId,
								SucursalId,
								Estado,
								FechaAlta,
								UsuarioAlta,
								FechaEdit,
								UsuarioEdit)
					VALUES(@Id,
							UPPER(@Codigo),
							DATEADD(HH,4,GETDATE()),
							DATEADD(DD,@Validez,DATEADD(HH,4,GETDATE())),
							@ClienteId,
							@ClienteCodigo,
							@TipoDocumentoId,
							@TipoDocumentoCodigo,
							@TipoDocumento,
							@NroDocumento,
							@CuilCuit,
							ISNULL(@RazonSocial,'VENTA DE CONTADO'),
							@ProvinciaId,
							@ProvinciaCodigo,
							@Provincia,
							@Localidad,
							@CodigoPostal,
							@Calle,
							@CalleNro,
							@PisoDpto,
							@OtrasReferencias,
							@Email,
							@Telefono,
							@Celular,
							@TipoResponsableId,
							@CategoriaId,
							@EstadoId,
							@Sucursal,
							1,
							DATEADD(HH,4,GETDATE()),
							UPPER(@Usuario),
							null,
							null)
		

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