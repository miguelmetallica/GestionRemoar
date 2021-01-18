CREATE PROCEDURE [dbo].[ComprobantesDetalleImputar]
	@Id nvarchar(150),
	@Cantidad int,
	@ImporteImputado numeric(18, 2),	
	@Usuario nvarchar(256) = NULL
AS
BEGIN TRY
	SET NOCOUNT ON;
	
	DECLARE @NEWId nvarchar(150) = NEWID();
	
	IF(@ImporteImputado <> 0)
	BEGIN
		BEGIN TRAN
			INSERT INTO ComprobantesDetalleImputacion(Id,ComprobanteId,DetalleId,ProductoId,Cantidad,Precio,
														ImporteImputado,PorcentajeImputado,Estado,Fecha,Usuario,
														Entrega,FechaEntrega,UsuarioEntrega,
														AutorizaEntrega,FechaAutoriza,UsuarioAutoriza,
														Despacha,FechaDespacha,UsuarioDespacha,RemitoId,
														Devolucion,FechaDevolucion,UsuarioDevolucion,MotivoDevolucion)
			SELECT @newId,ComprobanteId,DetalleId,ProductoId,Cantidad,Precio,
					@ImporteImputado + ImporteImputado,
					CONVERT(NUMERIC(18,2),ROUND(((@ImporteImputado + ImporteImputado) * 100) / Precio,2)) PorcentajeImputado,
					1 Estado,DATEADD(HH,4,GETDATE()) Fecha,UPPER(@Usuario)Usuario,
					Entrega,FechaEntrega,UsuarioEntrega,
					AutorizaEntrega,FechaAutoriza,UsuarioAutoriza,
					Despacha,FechaDespacha,UsuarioDespacha,RemitoId,
					Devolucion,FechaDevolucion,UsuarioDevolucion,MotivoDevolucion
			FROM ComprobantesDetalleImputacion
			WHERE Id = @Id
		
			UPDATE ComprobantesDetalleImputacion SET Estado = 0
			WHERE Id = @Id

		COMMIT;
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