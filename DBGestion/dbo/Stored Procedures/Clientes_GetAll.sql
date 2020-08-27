CREATE PROCEDURE [dbo].[Clientes_GetAll]
	@Busqueda varchar(150) = NULL
AS
BEGIN
	SELECT C.Id,C.Codigo,C.RazonSocial,
	C.NroDocumento,C.CuilCuit,C.Estado,
	CASE WHEN C.Estado = 1 THEN 'ACTIVO' ELSE 'INACTIVO' END EstadoDescripcion
	FROM Clientes C
END