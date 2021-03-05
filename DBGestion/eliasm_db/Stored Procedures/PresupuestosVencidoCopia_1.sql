CREATE PROCEDURE [eliasm_db].[PresupuestosVencidoCopia]
	@Id nvarchar(150),
	@VencidoId nvarchar(150),
	@Usuario nvarchar(256) = NULL
AS
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @SucursalId nvarchar(5);
	DECLARE @Numero INT;
	DECLARE @Codigo nvarchar(20);
	DECLARE @EstadoId nvarchar(150);
	DECLARE @Validez INT;
	
	SELECT @SucursalId = S.Codigo
	FROM AspNetUsers U
	INNER JOIN Sucursales S ON S.Id = U.SucursalId
	WHERE UserName = @Usuario

	IF @SucursalId IS NULL
		SET @SucursalId = 0;
	
	SELECT @EstadoId = E.Id
	FROM SistemaConfiguraciones C
	INNER JOIN ParamPresupuestosEstados E ON E.Codigo = C.Valor
	WHERE C.Configuracion = 'PRESUPUESTO_PENDIENTE_CLIENTE'

	SELECT @Validez = C.Valor
	FROM SistemaConfiguraciones C
	WHERE C.Configuracion = 'PRESUPUESTO_VALIDEZ'

	BEGIN TRAN
		EXEC @Numero = NextNumber 'PRESUPUESTOS'
		SET @Codigo = RIGHT('000' + RTRIM(LTRIM(CONVERT(VARCHAR(3),@SucursalId))),3) + RIGHT('0000000000' + RTRIM(LTRIM(CONVERT(VARCHAR(10),@Numero))) ,8)		

		INSERT INTO Presupuestos(Id,Codigo,Fecha,FechaVencimiento,ClienteId,TipoResponsableId,
								EstadoId,DescuentoId,DescuentoPorcentaje,Estado,FechaAlta,UsuarioAlta)
		SELECT @Id,UPPER(@Codigo),DATEADD(HH,4,GETDATE()),DATEADD(DD,@Validez,DATEADD(HH,4,GETDATE())),
			P.ClienteId,P.TipoResponsableId,@EstadoId,
			NULL,0,1,DATEADD(HH,4,GETDATE()),UPPER(@Usuario)
		FROM Presupuestos P
		WHERE P.Id = @VencidoId
		
		INSERT INTO PresupuestosDetalle(Id,PresupuestoId,ProductoId,ProductoCodigo,ProductoNombre,Precio,PrecioContado,PrecioSinImpuesto,PrecioContadoSinImpuesto,Cantidad,UsuarioAlta,FechaAlta)
		SELECT NEWID(),
			@Id,
			D.ProductoId,
			D.ProductoCodigo,
			D.ProductoNombre,
			eliasm_db.PrecioLista(D.ProductoId),
			eliasm_db.PrecioContado(D.ProductoId),
			eliasm_db.PrecioLista(D.ProductoId) - (eliasm_db.PrecioLista(D.ProductoId) * (A.Porcentaje / 100)),
			eliasm_db.PrecioContado(D.ProductoId) - (eliasm_db.PrecioContado(D.ProductoId) * (A.Porcentaje / 100)),
			D.Cantidad,
			UPPER(@Usuario),
			DATEADD(HH,4,GETDATE())
		FROM PresupuestosDetalle D
		INNER JOIN Productos P ON P.Id = D.ProductoId		
		INNER JOIN ParamAlicuotas A ON A.Id = P.AlicuotaId
		WHERE D.PresupuestoId = @VencidoId

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