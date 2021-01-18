CREATE PROCEDURE [eliasm_db].[PresupuestosGetPendientes]
AS
BEGIN
	SELECT P.Id,
		P.Codigo,
		P.Fecha,
		P.FechaVencimiento,
		C.RazonSocial,
		C.NroDocumento,
		ISNULL(C.CuilCuit,'')CuilCuit,
		E.Descripcion Estado,
		ISNULL(SUM(D.Cantidad),0)Cantidad,
		ISNULL(SUM(D.Precio),0) - (ISNULL(SUM(D.Precio),0) * (ISNULL(P.DescuentoPorcentaje,0) / 100 ))Precio,
		P.UsuarioAlta
	FROM Presupuestos P
	INNER JOIN Clientes C ON C.Id = P.ClienteId
	INNER JOIN ParamPresupuestosEstados E ON E.Id = P.EstadoId
	LEFT JOIN PresupuestosDetalle D ON D.PresupuestoId = P.Id
	WHERE CONVERT(DATE,P.FechaVencimiento) >= CONVERT(DATE,GETDATE())
	AND P.Estado = 1
	AND P.EstadoId = (
						SELECT E.Id
						FROM SistemaConfiguraciones C
						INNER JOIN ParamPresupuestosEstados E ON 
						E.Codigo = C.Valor
						WHERE C.Configuracion = 'PRESUPUESTO_PENDIENTE_CLIENTE'
						AND C.Estado = 1
					)
	GROUP BY P.Id,
		P.Codigo,
		P.Fecha,
		P.FechaVencimiento,
		C.RazonSocial,
		C.NroDocumento,
		ISNULL(C.CuilCuit,''),
		ISNULL(P.DescuentoPorcentaje,0),
		E.Descripcion,
		P.UsuarioAlta
	ORDER BY P.Fecha DESC
END