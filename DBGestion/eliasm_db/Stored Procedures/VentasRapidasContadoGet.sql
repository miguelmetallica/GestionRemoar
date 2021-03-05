CREATE PROCEDURE [eliasm_db].[VentasRapidasContadoGet]
	@Id varchar(150) = '',
	@FormaPagoId varchar(150) = ''
AS
BEGIN
	DECLARE @Descuento NUMERIC(18,2) = 0
	SELECT @Descuento =abs(C.Interes)
	FROM FormasPagos F
	INNER JOIN FormasPagosCuotas C ON C.FormaPagoId = F.Id
	WHERE F.Id = @FormaPagoId
	AND CONVERT(DATE, GetDate()) BETWEEN CONVERT(DATE, F.FechaDesde) AND CONVERT(DATE, F.FechaHasta)
	AND F.Estado = 1
	AND CONVERT(DATE, GetDate()) BETWEEN CONVERT(DATE, C.FechaDesde) AND CONVERT(DATE, C.FechaHasta)
	AND C.Estado = 1
	AND C.Cuota = 1

	IF @Descuento is null
		SET @Descuento = 0
	
	SET @Descuento = 1 - (@Descuento / 100)
	
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
			ISNULL((SELECT SUM(Precio) FROM VentasRapidasDetalle D Where D.VentaRapidaId = P.Id),0)TotalProductosSinDescuento,
			ISNULL((SELECT SUM(CASE WHEN ISNULL(AceptaDescuento,0) = 1 THEN  @Descuento * Precio ELSE Precio END) FROM VentasRapidasDetalle D Where D.VentaRapidaId = P.Id),0)TotalProductos,
			ISNULL((SELECT SUM(Cantidad) FROM VentasRapidasDetalle D Where D.VentaRapidaId = P.Id),0)CantidadProductos
	FROM VentasRapidas P
	LEFT JOIN ParamTiposResponsables TR ON TR.Id = P.TipoResponsableId
	LEFT JOIN ParamClientesCategorias CC ON CC.Id = P.ClienteCategoriaId
	LEFT JOIN ParamPresupuestosEstados E ON E.Id = P.EstadoId
	WHERE P.Id = @Id	
	AND P.Estado = 1	
	AND P.EstadoId = (
						SELECT E.Id
						FROM SistemaConfiguraciones C
						INNER JOIN ParamPresupuestosEstados E ON 
						E.Codigo = C.Valor
						WHERE C.Configuracion = 'PRESUPUESTO_PENDIENTE_CLIENTE'
						AND C.Estado = 1
					)
END