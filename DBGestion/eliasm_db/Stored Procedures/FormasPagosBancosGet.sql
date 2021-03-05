CREATE PROCEDURE [eliasm_db].[FormasPagosBancosGet]	
AS
BEGIN
	SELECT E.Id,E.Codigo,E.Descripcion,E.Estado,E.FechaAlta,E.UsuarioAlta
	FROM ParamEntidades E 
	WHERE E.Estado = 1
	AND E.VerCheque = 1
	Order by E.Descripcion
END;