CREATE PROCEDURE [dbo].[PresupuestosInsertar]
	@Id nvarchar(150),
	@ClienteId nvarchar(150),
	@Usuario nvarchar(256) = NULL
AS
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @SucursalId nvarchar(5);
	DECLARE @Numero INT;
	DECLARE @Codigo nvarchar(20);
	DECLARE @EstadoId nvarchar(150);
	DECLARE @TipoResponsableId nvarchar(150);	
	DECLARE @CategoriaId nvarchar(150);
	DECLARE @Validez int;

	SELECT @SucursalId = S.Codigo
	FROM AspNetUsers U
	INNER JOIN Sucursales S ON S.Id = U.SucursalId
	WHERE UserName = @Usuario

	IF @SucursalId IS NULL
		SET @SucursalId = '000';
	
	SELECT @EstadoId = E.Id
	FROM SistemaConfiguraciones C
	INNER JOIN ParamPresupuestosEstados E ON E.Codigo = C.Valor
	WHERE C.Configuracion = 'PRESUPUESTO_PENDIENTE_CLIENTE'

	SELECT @Validez = C.Valor
	FROM SistemaConfiguraciones C
	WHERE C.Configuracion = 'PRESUPUESTO_VALIDEZ'

	SELECT TOP 1 @TipoResponsableId = TipoResponsableId,
				@CategoriaId = CategoriaId
	FROM Clientes
	WHERE Id = @ClienteId

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
		EXEC @Numero = NextNumber 'PRESUPUESTOS'
		SET @Codigo = RIGHT('000' + RTRIM(LTRIM(CONVERT(VARCHAR(3),@SucursalId))),3) + RIGHT('0000000000' + RTRIM(LTRIM(CONVERT(VARCHAR(10),@Numero))) ,8)

		INSERT INTO Presupuestos(Id,Codigo,Fecha,
							FechaVencimiento,ClienteId,
							TipoResponsableId,ClienteCategoriaId,
							EstadoId,DescuentoId,DescuentoPorcentaje,
							Estado,FechaAlta,UsuarioAlta)
					VALUES(@Id,UPPER(@Codigo),
							DATEADD(HH,4,GETDATE()),DATEADD(DD,@Validez,DATEADD(HH,4,GETDATE())),
							@ClienteId,@TipoResponsableId,@CategoriaId,@EstadoId,NULL,0,
							1,DATEADD(HH,4,GETDATE()),UPPER(@Usuario))
		

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