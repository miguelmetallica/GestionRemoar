CREATE PROCEDURE [dbo].[ComprobantesDetalleEntregarGet]
	@ComprobanteId nvarchar(150)
AS
BEGIN
	SELECT P.Id,P.ComprobanteId,P.DetalleId,P.ProductoId,PR.Codigo ProductoCodigo,
		PR.Producto ProductoNombre,P.Cantidad,P.Precio,
		P.ImporteImputado,P.PorcentajeImputado,P.Estado,P.Fecha,P.Usuario,
		P.Entrega,P.FechaEntrega,P.UsuarioEntrega,
		P.AutorizaEntrega,P.FechaAutoriza,P.UsuarioAutoriza,
		P.Despacha,P.FechaDespacha,P.UsuarioDespacha,RemitoId,
		P.Devolucion,P.FechaDevolucion,P.UsuarioDevolucion,P.MotivoDevolucion
	FROM ComprobantesDetalleImputacion P
	INNER JOIN Productos PR ON P.ProductoId = PR.Id
	WHERE P.ComprobanteId = @ComprobanteId
	AND P.Estado = 1
	AND (P.Entrega = 1 OR P.AutorizaEntrega = 1)
	AND P.Despacha = 0
	ORDER BY PR.Codigo

END