create PROCEDURE [dbo].[ComprobantesTmpDeleteFormaPago]
	@id nvarchar(150)
AS
BEGIN
	DELETE ComprobantesFormasPagosTmp
	WHERE Id = @Id	
END