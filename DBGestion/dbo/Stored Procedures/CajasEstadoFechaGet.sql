CREATE PROCEDURE [dbo].[CajasEstadoFechaGet]

AS
BEGIN
	SELECT Fecha Fecha,SUM(Total)Total,Sucursal,SucursalId
	FROM(
		SELECT CONVERT(DATE,C.FechaComprobante)Fecha,SUM(F.Total)Total,S.Nombre Sucursal,S.Id SucursalId
		FROM Comprobantes C
		INNER JOIN ComprobantesFormasPagos F on F.ComprobanteId = C.Id
		INNER JOIN AspNetUsers U on U.UserName = F.UsuarioAlta
		INNER JOIN Sucursales S ON S.Id = U.SucursalId
		GROUP BY CONVERT(DATE,C.FechaComprobante),S.Nombre,S.Id 
		UNION ALL
		SELECT CONVERT(DATE,Fecha)Fecha,SUM(-Importe)Total,S.Nombre Sucursal,S.Id SucursalId
		FROM CajasMovimientos C
		INNER JOIN AspNetUsers U on U.UserName = C.UsuarioAlta
		INNER JOIN Sucursales S ON S.Id = U.SucursalId
		GROUP BY CONVERT(DATE,C.Fecha),S.Nombre,S.Id 
	)C
	GROUP BY Fecha,Sucursal,SucursalId 
	ORDER BY Fecha DESC,Sucursal
END;