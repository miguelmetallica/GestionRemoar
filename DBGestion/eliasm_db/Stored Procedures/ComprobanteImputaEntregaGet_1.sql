CREATE PROCEDURE [eliasm_db].[ComprobanteImputaEntregaGet]
	@ComprobanteId NVARCHAR(150) = ''
AS
BEGIN TRY
	SET NOCOUNT ON;
	
	DECLARE @EXISTE INT = 0;
	DECLARE @I INT = 1
	DECLARE @Cant INT = 1

	SELECT @EXISTE = COUNT(*)
	FROM ComprobantesDetalleImputacion P
	WHERE P.ComprobanteId = @ComprobanteId
	IF(@EXISTE = 0)
	BEGIN
		DECLARE @Comp AS TABLE(Num INT IDENTITY, 				
							ComprobanteId NVARCHAR(150),
							DetalleId NVARCHAR(150),
							ProductoId NVARCHAR(150),
							Cantidad INT,
							Precio NUMERIC(18,2)) 

		INSERT INTO @Comp(ComprobanteId,DetalleId,ProductoId,Cantidad,Precio)
		SELECT P.Id ComprobanteId, D.Id DetalleId,D.ProductoId,D.Cantidad,
		ISNULL(CONVERT(NUMERIC(18,2),D.Precio - (Precio * (P.DescuentoPorcentaje / 100))),0)
		FROM Comprobantes P
		INNER JOIN ComprobantesDetalle D ON D.ComprobanteId = P.Id
		WHERE P.ID = @ComprobanteId

		BEGIN TRAN
			WHILE @I <= (SELECT COUNT(*) FROM @Comp) 
			BEGIN
				SELECT @Cant = Cantidad
				FROM @COMP
				WHERE NUM = @I

				WHILE @Cant > 0
				BEGIN
					INSERT INTO ComprobantesDetalleImputacion(Id,ComprobanteId,DetalleId,
															ProductoId,
															Cantidad,
															Precio,
															ImporteImputado,
															PorcentajeImputado,
															Estado,
															Fecha,
															Usuario,
															Entrega,
															FechaEntrega,
															UsuarioEntrega,
															AutorizaEntrega,
															FechaAutoriza,
															UsuarioAutoriza,
															Despacha,
															FechaDespacha,
															UsuarioDespacha,
															Devolucion,
															FechaDevolucion,
															UsuarioDevolucion,
															MotivoDevolucion)
					SELECT NEWID(),ComprobanteId,DetalleId,
							ProductoId,1,
							Precio,
							0 ImporteImputado,
							0 PorcentajeImputado,
							1 Estado,
							DATEADD(HH,4,GETDATE()) Fecha,
							NULL Usuario,
							0 Entrega,
							null FechaEntrega,
							null UsuarioEntrega,
							0 AutorizaEntrega,
							null FechaAutoriza,
							null UsuarioAutoriza,
							0 Despacha,
							null FechaDespacha,
							null UsuarioDespacha,
							0 Devolucion,
							null FechaDevolucion,
							null UsuarioDevolucion,
							null MotivoDevolucion
					FROM @COMP
					WHERE Num = @I
					SET @Cant = @Cant - 1
				END
				SET @I = @I + 1
			END
		COMMIT;
	END

	SELECT P.Id,P.ComprobanteId,P.DetalleId,P.ProductoId,PR.ProductoCodigo,
	PR.ProductoNombre,P.Cantidad,P.Precio,
	P.ImporteImputado,P.PorcentajeImputado,P.Estado,P.Fecha,P.Usuario,
	P.Entrega,P.FechaEntrega,P.UsuarioEntrega,
	P.AutorizaEntrega,P.FechaAutoriza,P.UsuarioAutoriza,
	P.Despacha,P.FechaDespacha,P.UsuarioDespacha,
	P.Devolucion,P.FechaDevolucion,P.UsuarioDevolucion,P.MotivoDevolucion,
	ISNULL((SELECT CONVERT(NUMERIC(18,2),VALOR)FROM SistemaConfiguraciones C WHERE C.Configuracion = 'ENTREGA_PRODUCTO_PORCENTAJE' AND Estado = 1),50)Porcentaje_Config
	FROM ComprobantesDetalleImputacion P
	INNER JOIN ComprobantesDetalle PR ON P.ProductoId = PR.ProductoId and P.ComprobanteId = PR.ComprobanteId AND PR.Id = P.DetalleId 
	WHERE P.ComprobanteId = @ComprobanteId
	AND P.Estado = 1
	ORDER BY PR.ProductoCodigo,P.Id

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