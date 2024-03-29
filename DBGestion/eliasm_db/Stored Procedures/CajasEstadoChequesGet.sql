﻿CREATE PROCEDURE [eliasm_db].[CajasEstadoChequesGet]
	@Id nvarchar(150) = ''
AS
BEGIN
	SELECT C.TipoComprobante,
			C.Codigo Numero,
			CONVERT(DATE,C.FechaComprobante)Fecha,
			'CHEQUES' FormaPagoTipo,
			SUM(F.Total)Total,
			S.Nombre Sucursal,
			F.UsuarioAlta
	FROM Comprobantes C
	INNER JOIN ComprobantesFormasPagos F on F.ComprobanteId = C.Id
	INNER JOIN AspNetUsers U on U.UserName = F.UsuarioAlta
	INNER JOIN Sucursales S ON S.Id = C.SucursalId
	WHERE CONVERT(VARCHAR(4), YEAR(C.FechaComprobante)) + CONVERT(VARCHAR(2), MONTH(C.FechaComprobante)) + CONVERT(VARCHAR(2), DAY(C.FechaComprobante)) + c.SucursalId = @Id
	AND F.Total <> 0
	AND F.FormaPagoTipo = 'X'
	GROUP BY C.TipoComprobante,
		C.Codigo,
		CONVERT(DATE,C.FechaComprobante),
		S.Nombre,F.UsuarioAlta
END;