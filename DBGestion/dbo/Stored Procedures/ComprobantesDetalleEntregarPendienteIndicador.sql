CREATE PROCEDURE [dbo].[ComprobantesDetalleEntregarPendienteIndicador]	
AS
BEGIN
	SELECT P.Id,P.ComprobanteId,P.DetalleId,P.ProductoId,PR.Codigo ProductoCodigo,
		PR.Producto ProductoNombre,P.Cantidad,P.Precio,
		P.ImporteImputado,P.PorcentajeImputado,P.Estado,P.Fecha,P.Usuario,
		P.Entrega,P.FechaEntrega,P.UsuarioEntrega,
		P.AutorizaEntrega,P.FechaAutoriza,P.UsuarioAutoriza,
		P.Despacha,P.FechaDespacha,P.UsuarioDespacha,RemitoId,
		P.Devolucion,P.FechaDevolucion,P.UsuarioDevolucion,P.MotivoDevolucion,
		C.Codigo CodigoComprobante,C.FechaComprobante,C.UsuarioAlta UsuarioComprobante,
		C.ClienteCodigo,C.RazonSocial,PR.Codigo CodigoPrespuesto
	FROM ComprobantesDetalleImputacion P
	INNER JOIN Productos PR ON P.ProductoId = PR.Id
	INNER JOIN Comprobantes C ON C.Id = P.ComprobanteId
	INNER JOIN Presupuestos PRE ON PRE.Id = C.PresupuestoId
	WHERE P.Estado = 1
	AND (P.Entrega = 1 OR P.AutorizaEntrega = 1)
	AND P.Despacha = 0
	ORDER BY C.FechaComprobante

END