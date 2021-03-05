CREATE PROCEDURE [eliasm_db].[PresupuestosResumenGet]
	@Id varchar(150) = ''
AS
BEGIN
	
	DECLARE @InteresFormaPago NUMERIC(18,2) = 0
	SELECT TOP 1 @InteresFormaPago = abs(P.Interes)
	FROM FormasPagosCuotas P
	WHERE P.FormaPagoId = '6DB16344-8DB9-4F81-903D-50913E53FE86'
	AND P.Cuota = 1
	AND CONVERT(DATE,GETDATE()) BETWEEN P.FechaDesde AND P.FechaHasta
	AND P.Estado = 1

	SELECT 
	@Id Id,
	SUM(P.CantidadProductos) CantidadProductos,
	SUM(P.SubTotalProductos) SubTotalProductos,
	SUM(P.DescuentoPorcentaje) DescuentoPorcentaje,
	SUM(P.DescuentoMonto) DescuentoMonto,
	SUM(P.TotalAPagar) TotalAPagar,
	SUM(ISNULL(P.SaldoAPagar,0)) SaldoAPagar,
	(SUM(P.TotalAPagar) - SUM(P.TotalAPagar) * (@InteresFormaPago / 100)) - sum(SaldoContado) SaldoContadoAPagar
	FROM(
		SELECT 
		SUM(D.Cantidad) CantidadProductos,
		SUM(D.Precio * D.Cantidad)SubTotalProductos,
		MAX(P.DescuentoPorcentaje)DescuentoPorcentaje,
		SUM((D.Precio * D.Cantidad) * ((CASE WHEN D.AceptaDescuento = 1 THEN P.DescuentoPorcentaje ELSE 0 END)/100)) DescuentoMonto,
		SUM((D.Precio * D.Cantidad) - ((D.Precio * D.Cantidad) * (ISNULL(CASE WHEN D.AceptaDescuento = 1 THEN P.DescuentoPorcentaje ELSE 0 END,0))/100))TotalAPagar,
		SUM((D.Precio * D.Cantidad) - ((D.Precio * D.Cantidad) * (ISNULL(CASE WHEN D.AceptaDescuento = 1 THEN P.DescuentoPorcentaje ELSE 0 END,0))/100))SaldoAPagar,
		0 SaldoContado
		FROM Presupuestos P
		INNER JOIN PresupuestosDetalle D 
		ON D.PresupuestoId = P.Id
		WHERE P.Id = @Id
		AND D.Cantidad <> 0
		and D.Precio <> 0

		UNION ALL

		SELECT 0 CantidadProductos,
		0 SubTotalProductos,
		0 DescuentoPorcentaje,
		0 DescuentoMonto,
		0 TotalAPagar,
		- SUM((D.Importe) * D.Cuota) SaldoAPagar,
	    SUM((D.Total)) Contado
		FROM Presupuestos P
		INNER JOIN PresupuestosFormasPagos D 
		ON D.PresupuestoId = P.Id
		WHERE P.Id = @Id
		AND D.Importe <> 0
		AND D.Cuota <> 0
	)P
END;