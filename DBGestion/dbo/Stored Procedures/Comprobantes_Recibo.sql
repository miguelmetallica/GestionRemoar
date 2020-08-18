
create PROCEDURE [Comprobantes_Recibo]
	@ComprobanteId int
AS
BEGIN 
	SELECT SUM(Q.ProductosCantidad)ProductosCantidad,
	CONVERT(NUMERIC(18,2),SUM(Q.ProductosTotal))ProductosTotal,
	CONVERT(NUMERIC(18,2),SUM(Q.TotalCapital))TotalCapital,
	CONVERT(NUMERIC(18,2),SUM(Q.TotalInteres)) TotalInteres,
	CONVERT(NUMERIC(18,2),ROUND(SUM(Q.ProductosTotal) - SUM(Q.TotalCapital),2))TotalPendiente
	FROM(
		SELECT SUM(D.Cantidad)ProductosCantidad,ROUND(SUM(D.SubTotal),2)ProductosTotal,0 TotalCapital,0 TotalInteres 
		FROM ComprobantesDetalles D
		WHERE D.ComprobanteId = @ComprobanteId
		UNION ALL 
		SELECT 0,0,ROUND(SUM(P.Importe),2)TotalCapital,ROUND(SUM(P.Total),2)TotalInteres
		FROM ComprobantesFormasPagos P 
		WHERE P.ComprobanteId = @ComprobanteId
	)Q
END