CREATE PROCEDURE [eliasm_db].[VentasRapidasSucursalGetsFacturadas]
	@SucursalId nvarchar(150) = ''
AS
BEGIN
	SELECT P.Id,
		P.Codigo,
		P.Fecha,
		P.FechaVencimiento,
		ISNULL(P.RazonSocial,'VENTA DE CONTADO')RazonSocial,
		P.NroDocumento,
		ISNULL(P.CuilCuit,'')CuilCuit,
		ISNULL(SUM(D.Cantidad),0)Cantidad,
		ISNULL(SUM((D.Precio * D.Cantidad) - ((D.Precio * D.Cantidad) * (ISNULL(CASE WHEN D.AceptaDescuento = 1 THEN P.DescuentoPorcentaje ELSE 0 END,0))/100)),0)Precio,
		P.UsuarioAlta,
		P.ComprobanteId
	FROM VentasRapidas P
	LEFT JOIN VentasRapidasDetalle D ON D.VentaRapidaId = P.Id
	WHERE P.SucursalId = @SucursalId
	and P.Estado = 1
	AND p.ComprobanteId IS NOT NULL
	GROUP BY P.Id,
		P.Codigo,
		P.Fecha,
		P.FechaVencimiento,
		P.RazonSocial,
		P.NroDocumento,
		ISNULL(P.CuilCuit,''),
		ISNULL(P.DescuentoPorcentaje,0),
		P.UsuarioAlta,
		p.ComprobanteId
	ORDER BY P.Fecha DESC
END