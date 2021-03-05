
CREATE PROCEDURE [eliasm_db].[ProductoGetCodigo]	
	@Codigo nvarchar(50) = ''
AS
BEGIN
	SELECT 
		P.Id,
		P.Codigo,
		P.TipoProductoId,
		T.Descripcion TipoProducto,
		P.Producto,
		isnull(P.DescripcionCorta,'')DescripcionCorta,
		isnull(P.DescripcionLarga,'')DescripcionLarga,
		isnull(P.CodigoBarra,'')CodigoBarra,
		P.Peso,
		P.DimencionesLongitud,
		P.DimencionesAncho,
		P.DimencionesAltura,
		P.CuentaVentaId,
		V.Descripcion CuentaVenta,
		P.CuentaCompraId,
		C.Descripcion CuentaCompra,
		P.UnidadMedidaId,
		U.Descripcion UnidadMedida,
		P.AlicuotaId,
		A.Descripcion Alicuota,
		P.PrecioVenta,
		P.ProveedorId,
		R.RazonSocial Proveedor,
		ISNULL(P.CategoriaId,'') CategoriaId,
		ISNULL(G.Descripcion,'') Categoria,
		P.ControlaStock,
		P.AceptaDescuento,
		P.PrecioRebaja,
		P.RebajaDesde,
		P.RebajaHasta,
		P.Estado,
		P.FechaAlta,
		P.UsuarioAlta
	FROM Productos P
	LEFT JOIN ParamTiposProductos T ON T.Id = P.TipoProductoId
	LEFT JOIN ParamCuentasVentas V ON V.Id = P.CuentaVentaId
	LEFT JOIN ParamCuentasCompras C ON C.Id = P.CuentaCompraId
	LEFT JOIN ParamUnidadesMedidas U ON U.Id = P.UnidadMedidaId
	LEFT JOIN ParamAlicuotas A ON A.Id = P.AlicuotaId
	LEFT JOIN Proveedores R ON R.Id = P.ProveedorId
	LEFT JOIN ParamCategorias G ON G.Id = P.CategoriaId
	WHERE P.Codigo = @Codigo
END