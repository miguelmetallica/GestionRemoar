CREATE PROCEDURE [eliasm_db].[VentasRapidasACobrarGet]
	@Id varchar(150) = ''
AS
BEGIN
	SELECT P.Id,
			P.Codigo,
			P.Fecha,
			P.FechaVencimiento,
			P.ClienteId,
			P.ClienteCodigo ClienteCodigo,
			ISNULL(P.RazonSocial,'VENTA DE CONTADO')RazonSocial,
			P.NroDocumento,
			P.CuilCuit,
			P.TipoResponsableId,
			TR.Descripcion TipoResponsable,
			P.ClienteCategoriaId,
			CC.Descripcion ClienteCategoria,
			P.EstadoId,
			E.Descripcion Estado,
			P.DescuentoId,
			P.DescuentoPorcentaje,
			P.Estado,
			P.FechaAlta,
			P.UsuarioAlta,
			P.FechaEdit,
			P.UsuarioEdit,
			ISNULL((SELECT SUM(Precio) FROM VentasRapidasDetalle D Where D.VentaRapidaId = P.Id),0) TotalProductos,
			ISNULL((SELECT SUM(Cantidad) FROM VentasRapidasDetalle D Where D.VentaRapidaId = P.Id),0)CantidadProductos,
			ISNULL((SELECT SUM(Precio) FROM VentasRapidasDetalle D Where D.VentaRapidaId = P.Id),0) 
			- ISNULL((SELECT SUM((Importe) * Cuota) FROM VentasRapidasFormasPagos D Where D.VentaRapidaId = P.Id),0) Saldo
	FROM VentasRapidas P
	LEFT JOIN ParamTiposResponsables TR ON TR.Id = P.TipoResponsableId
	LEFT JOIN ParamClientesCategorias CC ON CC.Id = P.ClienteCategoriaId
	LEFT JOIN ParamPresupuestosEstados E ON E.Id = P.EstadoId
	WHERE P.Id = @Id	
	AND P.Estado = 1
	AND P.ComprobanteId is null
	AND P.EstadoId = (
						SELECT E.Id
						FROM SistemaConfiguraciones C
						INNER JOIN ParamPresupuestosEstados E ON 
						E.Codigo = C.Valor
						WHERE C.Configuracion = 'PRESUPUESTO_PENDIENTE_CLIENTE'
						AND C.Estado = 1
					)
	
END