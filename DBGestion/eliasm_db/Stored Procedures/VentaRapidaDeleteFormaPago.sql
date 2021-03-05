CREATE PROCEDURE [eliasm_db].[VentaRapidaDeleteFormaPago]
	@id nvarchar(150)
AS
BEGIN
	DELETE VentasRapidasFormasPagos
	WHERE Id = @Id	
END