CREATE PROCEDURE [eliasm_db].[CajasEstadoImportesGet]
	@Id nvarchar(150) = ''
AS
BEGIN
	SELECT Fecha,FormaPagoTipo,SUM(Total)Total,Sucursal,SucursalId
	FROM(
		SELECT CONVERT(DATE,C.FechaComprobante)Fecha,
		CASE WHEN F.FormaPagoTipo = 'C' THEN 'TARJETAS DE CREDITO'
			WHEN F.FormaPagoTipo = 'D' THEN 'TARJETA DE DEBITO'
			WHEN F.FormaPagoTipo = 'E' THEN 'EFECTIVO'
			WHEN F.FormaPagoTipo = 'O' THEN 'OTROS'
			WHEN F.FormaPagoTipo = 'X' THEN 'CHEQUES'
			WHEN F.FormaPagoTipo = 'U' THEN 'DOLARES'
			ELSE '' END FormaPagoTipo,
		SUM(F.Total)Total,S.Nombre Sucursal,S.Id SucursalId
		FROM Comprobantes C
		INNER JOIN ComprobantesFormasPagos F on F.ComprobanteId = C.Id
		INNER JOIN AspNetUsers U on U.UserName = F.UsuarioAlta
		INNER JOIN Sucursales S ON S.Id = C.SucursalId
		WHERE CONVERT(VARCHAR(4), YEAR(C.FechaComprobante)) + CONVERT(VARCHAR(2), MONTH(C.FechaComprobante)) + CONVERT(VARCHAR(2), DAY(C.FechaComprobante)) + c.SucursalId = @Id
		and F.Total <> 0
		GROUP BY CONVERT(DATE,C.FechaComprobante),F.FormaPagoTipo,S.Nombre,S.Id
	
		UNION ALL
	
		SELECT CONVERT(DATE,C.Fecha)Fecha,
			T.Descripcion + isnull(' - ' + upper(C.Observaciones),'') FormaPagoTipo,
			SUM(Importe)Total,S.Nombre Sucursal,S.Id SucursalId
		FROM CajasMovimientos C
		INNER JOIN ParamCajasMovimientosTipos T ON T.Id = C.TipoMovimientoId
		INNER JOIN AspNetUsers U on U.UserName = C.UsuarioAlta
		INNER JOIN Sucursales S ON S.Id = C.SucursalId
		WHERE CONVERT(VARCHAR(4), YEAR(C.FECHA)) + CONVERT(VARCHAR(2), MONTH(C.FECHA)) + CONVERT(VARCHAR(2), DAY(C.FECHA)) + c.SucursalId = @Id
		and Importe <> 0
		GROUP BY CONVERT(DATE,C.Fecha),S.Nombre,S.Id,T.Descripcion,C.Observaciones
	)C
	GROUP BY Fecha,FormaPagoTipo,Sucursal,SucursalId
	ORDER BY Fecha,FormaPagoTipo,Sucursal,SucursalId
END;