
CREATE PROCEDURE [Productos_Get]
	@ProductoId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @ProductoId IS NOT NULL
  BEGIN
	SELECT P.ProductoId,
			ISNULL(P.ProductoCodigo,'')ProductoCodigo,
			ISNULL(P.Producto,'')Producto,
			ISNULL(P.Descripcion,'')Descripcion,
			ISNULL(P.DescripcionAdicional,'')DescripcionAdicional,
			ISNULL(P.CodigoBarra,'')CodigoBarra,
			ISNULL(P.PrecioCosto,0)PrecioCosto,
			0 ImpuestoId,
			ISNULL(null,'')Impuesto,
			ISNULL(P.CuentaVentaId,0)CuentaVentaId,
			ISNULL(V.CuentaVenta,'')CuentaVenta,
			ISNULL(P.CuentaCompraId,'')CuentaCompraId,
			ISNULL(C.CuentaCompra,'')CuentaCompra,
			ISNULL(P.UnidadMedidaId,'')UnidadMedidaId,'' UnidadMedida,
			P.Estado,
			ISNULL(P.FechaAlta,GETDATE())FechaAlta,
			ISNULL(P.UsuarioAlta,'ADMIN')UsuarioAlta
	FROM Productos P
	--LEFT JOIN ParamImpuestos I ON I.ImpuestoId = P.ImpuestoId
	LEFT JOIN ParamCuentasVentas V ON V.CuentaVentaId = P.CuentaVentaId
	LEFT JOIN ParamCuentasCompras C ON C.CuentaCompraId = P.CuentaCompraId
	WHERE P.ProductoId = @ProductoId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	SELECT top 50 P.ProductoId,
			ISNULL(P.ProductoCodigo,'')ProductoCodigo,
			ISNULL(P.Producto,'')Producto,
			ISNULL(P.Descripcion,'')Descripcion,
			ISNULL(P.DescripcionAdicional,'')DescripcionAdicional,
			ISNULL(P.CodigoBarra,'')CodigoBarra,
			ISNULL(P.PrecioCosto,0)PrecioCosto,
			0 ImpuestoId,
			ISNULL(null,'')Impuesto,
			ISNULL(P.CuentaVentaId,0)CuentaVentaId,
			ISNULL(V.CuentaVenta,'')CuentaVenta,
			ISNULL(P.CuentaCompraId,'')CuentaCompraId,
			ISNULL(C.CuentaCompra,'')CuentaCompra,
			ISNULL(P.UnidadMedidaId,'')UnidadMedidaId,'' UnidadMedida,
			P.Estado,
			ISNULL(P.FechaAlta,GETDATE())FechaAlta,
			ISNULL(P.UsuarioAlta,'ADMIN')UsuarioAlta
	FROM Productos P
	--LEFT JOIN ParamImpuestos I ON I.ImpuestoId = P.ImpuestoId
	LEFT JOIN ParamCuentasVentas V ON V.CuentaVentaId = P.CuentaVentaId
	LEFT JOIN ParamCuentasCompras C ON C.CuentaCompraId = P.CuentaCompraId
    WHERE 
		(
			P.ProductoCodigo LIKE @BusquedaLike + '%'
		OR
			P.Producto LIKE @BusquedaLike + '%'
		OR
			P.Descripcion LIKE @BusquedaLike + '%'
		OR
			P.DescripcionAdicional LIKE @BusquedaLike + '%'
		)
	  ORDER BY P.ProductoCodigo
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	SELECT top 50 P.ProductoId,
			ISNULL(P.ProductoCodigo,'')ProductoCodigo,
			ISNULL(P.Producto,'')Producto,
			ISNULL(P.Descripcion,'')Descripcion,
			ISNULL(P.DescripcionAdicional,'')DescripcionAdicional,
			ISNULL(P.CodigoBarra,'')CodigoBarra,
			ISNULL(P.PrecioCosto,0)PrecioCosto,
			0 ImpuestoId,
			ISNULL(null,'')Impuesto,
			ISNULL(P.CuentaVentaId,0)CuentaVentaId,
			ISNULL(V.CuentaVenta,'')CuentaVenta,
			ISNULL(P.CuentaCompraId,'')CuentaCompraId,
			ISNULL(C.CuentaCompra,'')CuentaCompra,
			ISNULL(P.UnidadMedidaId,'')UnidadMedidaId,'' UnidadMedida,
			P.Estado,
			ISNULL(P.FechaAlta,GETDATE())FechaAlta,
			ISNULL(P.UsuarioAlta,'ADMIN')UsuarioAlta
	FROM Productos P
	--LEFT JOIN ParamImpuestos I ON I.ImpuestoId = P.ImpuestoId
	LEFT JOIN ParamCuentasVentas V ON V.CuentaVentaId = P.CuentaVentaId
	LEFT JOIN ParamCuentasCompras C ON C.CuentaCompraId = P.CuentaCompraId
    WHERE 
		(
			P.ProductoCodigo LIKE @BusquedaLikeActivo + '%'
		OR
			P.Producto LIKE @BusquedaLikeActivo + '%'
		OR
			P.Descripcion LIKE @BusquedaLike + '%'
		OR
			P.DescripcionAdicional LIKE @BusquedaLikeActivo + '%'
		)
	  ORDER BY P.ProductoCodigo
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	SELECT top 50 P.ProductoId,
			ISNULL(P.ProductoCodigo,'')ProductoCodigo,
			ISNULL(P.Producto,'')Producto,
			ISNULL(P.Descripcion,'')Descripcion,
			ISNULL(P.DescripcionAdicional,'')DescripcionAdicional,
			ISNULL(P.CodigoBarra,'')CodigoBarra,
			ISNULL(P.PrecioCosto,0)PrecioCosto,
			0 ImpuestoId,
			ISNULL(null,'')Impuesto,
			ISNULL(P.CuentaVentaId,0)CuentaVentaId,
			ISNULL(V.CuentaVenta,'')CuentaVenta,
			ISNULL(P.CuentaCompraId,'')CuentaCompraId,
			ISNULL(C.CuentaCompra,'')CuentaCompra,
			ISNULL(P.UnidadMedidaId,'')UnidadMedidaId,'' UnidadMedida,
			P.Estado,
			ISNULL(P.FechaAlta,GETDATE())FechaAlta,
			ISNULL(P.UsuarioAlta,'ADMIN')UsuarioAlta
	FROM Productos P
	--LEFT JOIN ParamImpuestos I ON I.ImpuestoId = P.ImpuestoId
	LEFT JOIN ParamCuentasVentas V ON V.CuentaVentaId = P.CuentaVentaId
	LEFT JOIN ParamCuentasCompras C ON C.CuentaCompraId = P.CuentaCompraId
    WHERE p.ProductoCodigo = @Busqueda	  
  END
  
END