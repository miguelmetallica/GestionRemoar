CREATE PROCEDURE [eliasm_db].[PresupuestoDeleteFormaPago]
	@id nvarchar(150)
AS
BEGIN
	DELETE PresupuestosFormasPagos
	WHERE Id = @Id	
END