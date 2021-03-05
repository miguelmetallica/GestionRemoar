CREATE PROCEDURE [eliasm_db].[ComprobantesDetalleAutorizaAnula]
	@Id nvarchar(150),
	@Usuario nvarchar(256) = NULL
AS
BEGIN TRY
	SET NOCOUNT ON;
	
	DECLARE @NEWId nvarchar(150) = NEWID();
	
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
		AND Id NOT IN (
					SELECT Id
					FROM ComprobantesDetalleTMP D
					WHERE D.Id = @Id
					)
		
		UPDATE ComprobantesDetalleImputacion 
			SET Fecha = DATEADD(HH,4,GETDATE()),
				Usuario = UPPER(@Usuario),
				AutorizaEntrega = 0,
				FechaAutoriza = NULL,
				UsuarioAutoriza = NULL
		WHERE Id = @Id
		AND Id NOT IN (
					SELECT Id
					FROM ComprobantesDetalleTMP D
					WHERE D.Id = @Id
					)

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