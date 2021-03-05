CREATE PROCEDURE [eliasm_db].[ComprobantesDetalleDevolucionGet]
	@ComprobanteId nvarchar(150)
AS
BEGIN
	SELECT P.Id,C.Id ComprobanteId,P.DetalleId,P.ProductoId,PR.ProductoCodigo,
		PR.ProductoNombre,P.Cantidad,P.Precio,
		P.ImporteImputado,P.PorcentajeImputado,P.Estado,P.Fecha,P.Usuario,
		P.Entrega,P.FechaEntrega,P.UsuarioEntrega,
		P.AutorizaEntrega,P.FechaAutoriza,P.UsuarioAutoriza,
		P.Despacha,P.FechaDespacha,P.UsuarioDespacha,RemitoId,
		P.Devolucion,P.FechaDevolucion,P.UsuarioDevolucion,P.MotivoDevolucion,c.*
	FROM ComprobantesDetalleImputacion P
	INNER JOIN Comprobantes C ON C.Id = P.RemitoId
	INNER JOIN ComprobantesDetalle PR ON P.ProductoId = PR.ProductoId
										AND PR.ComprobanteId = P.ComprobanteId
										AND PR.Id = P.DetalleId
	WHERE P.RemitoId = @ComprobanteId
	AND P.Estado = 1
	AND P.Despacha = 1 
	AND P.Devolucion = 0
	and P.Id NOT IN (SELECT Id FROM ComprobantesDetalleTMP D WHERE D.Id = P.Id)
	ORDER BY PR.ProductoCodigo

END