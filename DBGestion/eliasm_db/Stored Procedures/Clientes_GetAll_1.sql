CREATE PROCEDURE [eliasm_db].[Clientes_GetAll]
	@Busqueda varchar(150) = NULL
AS
BEGIN
	DECLARE @NoClienteId nvarchar(150) = NULL
	SELECT @NoClienteId = Valor
	FROM SistemaConfiguraciones C
	WHERE C.Configuracion = 'CLIENTE_VENTA_CONTADO'
	AND C.Estado = 1

	SELECT C.Id,C.Codigo,C.RazonSocial,
	C.NroDocumento,C.CuilCuit,C.Estado,
	CASE WHEN C.Estado = 1 THEN 'ACTIVO' ELSE 'INACTIVO' END EstadoDescripcion
	FROM Clientes C
	WHERE Id <> @NoClienteId
END