CREATE PROCEDURE [eliasm_db].[ComprobantesDetalleImputar]
	@Id nvarchar(150),
	@Cantidad int,
	@ImporteImputado numeric(18, 2),	
	@Usuario nvarchar(256) = NULL
AS
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @PorcentajeImputacion numeric(18,2) 
	DECLARE @NEWId nvarchar(150) = NEWID();

	SELECT @PorcentajeImputacion = Valor
	FROM SistemaConfiguraciones S
	WHERE Configuracion = 'PORCENTAJE_IMPUTACION_MINIMO'
	AND Estado = 1

	IF ISNULL((SELECT (@ImporteImputado + ImporteImputado) * 100 / Precio 
		FROM ComprobantesDetalleImputacion WHERE Id = @Id),0) < @PorcentajeImputacion
	BEGIN
		SELECT @ImporteImputado = (@PorcentajeImputacion * Precio /100) - ImporteImputado 
		FROM ComprobantesDetalleImputacion 
		WHERE Id = @Id

		If @ImporteImputado is null
			 set @ImporteImputado = 0
	END

	IF ISNULL((SELECT Precio - @ImporteImputado - ImporteImputado FROM ComprobantesDetalleImputacion WHERE Id = @Id),0) >= 0
	BEGIN
		IF(@ImporteImputado <> 0)
		BEGIN
			BEGIN TRAN
				INSERT INTO ComprobantesDetalleImputacionHist(IdHist,Id,ComprobanteId,DetalleId,ProductoId,Cantidad,Precio,
													ImporteImputado,PorcentajeImputado,Estado,Fecha,Usuario,
													Entrega,FechaEntrega,UsuarioEntrega,
													AutorizaEntrega,FechaAutoriza,UsuarioAutoriza,
													Despacha,FechaDespacha,UsuarioDespacha,RemitoId,
													Devolucion,FechaDevolucion,UsuarioDevolucion,MotivoDevolucion)
				SELECT @newId,Id,ComprobanteId,DetalleId,ProductoId,Cantidad,Precio,
						ImporteImputado,PorcentajeImputado,Estado,Fecha,Usuario,
						Entrega,FechaEntrega,UsuarioEntrega,
						AutorizaEntrega,FechaAutoriza,UsuarioAutoriza,
						Despacha,FechaDespacha,UsuarioDespacha,RemitoId,
						Devolucion,FechaDevolucion,UsuarioDevolucion,MotivoDevolucion
				FROM ComprobantesDetalleImputacion
				WHERE Id = @Id
		
				UPDATE ComprobantesDetalleImputacion 
					SET Fecha = DATEADD(HH,4,GETDATE()),
						Usuario = UPPER(@Usuario),
						ImporteImputado = @ImporteImputado + ImporteImputado,
						PorcentajeImputado = CONVERT(NUMERIC(18,2),ROUND(((@ImporteImputado + ImporteImputado) * 100) / Precio,2))						
				WHERE Id = @Id								

			COMMIT;
		END
	END
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


SELECT precio - 10 - ImporteImputado,*
FROM ComprobantesDetalleImputacion
WHERE Id = @Id