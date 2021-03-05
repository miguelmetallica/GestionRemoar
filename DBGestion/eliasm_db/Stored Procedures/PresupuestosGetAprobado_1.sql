CREATE PROCEDURE [eliasm_db].[PresupuestosGetAprobado]
	@Id varchar(150) = ''
AS
BEGIN
	SELECT P.Id,
			P.Codigo,
			P.Fecha,
			P.FechaVencimiento,
			P.ClienteId,
			C.Codigo ClienteCodigo,
			C.RazonSocial,
			C.NroDocumento,
			C.CuilCuit,
			P.TipoResponsableId,
			TR.Descripcion TipoResponsable,
			P.ClienteCategoriaId,
			CC.Descripcion ClienteCategoria,
			P.EstadoId,
			E.Descripcion Estado,
			P.DescuentoId,
			P.DescuentoPorcentaje,
			P.FechaAprobacion,
			P.FechaRechazo,
			P.MotivoAprobacionRechazo,
			P.UsuarioAprobacionRechazo,
			P.Estado,
			P.FechaAlta,
			P.UsuarioAlta,
			P.FechaEdit,
			P.UsuarioEdit,
			ISNULL((SELECT SUM(Precio) FROM PresupuestosDetalle D Where D.PresupuestoId = P.Id),0)Precio,
			ISNULL((SELECT SUM(PrecioContado) FROM PresupuestosDetalle D Where D.PresupuestoId = P.Id),0)PrecioContado,
			ISNULL((SELECT SUM(Cantidad) FROM PresupuestosDetalle D Where D.PresupuestoId = P.Id),0)CantidadProductos
	FROM Presupuestos P
	INNER JOIN Clientes C ON C.Id = P.ClienteId
	INNER JOIN ParamPresupuestosEstados E ON E.Id = P.EstadoId
	LEFT JOIN ParamTiposResponsables TR ON TR.Id = P.TipoResponsableId
	LEFT JOIN ParamClientesCategorias CC ON CC.Id = P.ClienteCategoriaId
	WHERE P.Id = @Id
	AND P.Estado = 1
	AND P.EstadoId = (
					SELECT E.Id
					FROM SistemaConfiguraciones C
					INNER JOIN ParamPresupuestosEstados E ON 
					E.Codigo = C.Valor
					WHERE C.Configuracion = 'PRESUPUESTO_APROBADO_CLIENTE'
					AND C.Estado = 1
					)

END