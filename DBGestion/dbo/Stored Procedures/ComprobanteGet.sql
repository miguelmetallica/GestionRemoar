CREATE PROCEDURE [dbo].[ComprobanteGet]
	@Id nvarchar(150) = ''
AS
BEGIN
	SELECT P.*,
	P.Total - ISNULL((SELECT SUM(ISNULL(I.ImporteCancela,0)) FROM ComprobantesImputacion I WHERE I.ComprobanteId = P.Id),0)Saldo,
	S.Nombre SucursalNombre,
	S.Calle SucursalCalle,
	S.CalleNro SucursalCalleNro,
	S.Localidad SucursalLocalidad,
	S.CodigoPostal SucursalCodigoPostal,
	S.Telefono SucursalTelefono	
	FROM Comprobantes P
	LEFT JOIN AspNetUsers U ON U.UserName = P.UsuarioAlta
	LEFT JOIN Sucursales S ON S.Id = U.SucursalId
	WHERE P.Id = @Id
	AND P.Codigo IS NOT NULL
	ORDER BY P.FechaComprobante
END
;