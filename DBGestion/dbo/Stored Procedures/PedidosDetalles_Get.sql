
CREATE PROCEDURE [PedidosDetalles_Get]
	@PedidoId int = NULL,
	@PedidoDetalleId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @PedidoDetalleId IS NOT NULL
  BEGIN
	SELECT PedidoDetalleId,PedidoId,ProductoId,ProductoCodigo,
			Producto,ColorId,Color,MarcaId,Marca,CuentaVentaId,CuentaVenta,
			ImpuestoId,Impuesto,Precio,Cantidad,Total,Estado,FechaAlta,UsuarioAlta
	FROM PedidosDetalles P
	WHERE P.PedidoDetalleId = @PedidoDetalleId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	  SELECT PedidoDetalleId,PedidoId,ProductoId,ProductoCodigo,
			Producto,ColorId,Color,MarcaId,Marca,CuentaVentaId,CuentaVenta,
			ImpuestoId,Impuesto,Precio,Cantidad,Total,Estado,FechaAlta,UsuarioAlta
	  FROM PedidosDetalles P
	  WHERE PedidoId = @PedidoId AND
		(
			CONVERT(Varchar(10),P.PedidoId) LIKE @BusquedaLike + '%'
		--OR
		--	P.PedidoCodigo LIKE @BusquedaLike + '%'
		)
	  ORDER BY P.PedidoDetalleId
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	  SELECT PedidoDetalleId,PedidoId,ProductoId,ProductoCodigo,
			Producto,ColorId,Color,MarcaId,Marca,CuentaVentaId,CuentaVenta,
			ImpuestoId,Impuesto,Precio,Cantidad,Total,Estado,FechaAlta,UsuarioAlta
	  FROM PedidosDetalles P
	  WHERE PedidoId = @PedidoId AND
		(
			CONVERT(Varchar(10),P.PedidoId) LIKE @BusquedaLikeActivo + '%'
		--OR
		--	P.PedidoCodigo LIKE @BusquedaLike + '%'
		)
	  ORDER BY P.PedidoDetalleId
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	  SELECT PedidoDetalleId,PedidoId,ProductoId,ProductoCodigo,
			Producto,ColorId,Color,MarcaId,Marca,CuentaVentaId,CuentaVenta,
			ImpuestoId,Impuesto,Precio,Cantidad,Total,Estado,FechaAlta,UsuarioAlta
	  FROM PedidosDetalles P
	  WHERE P.PedidoId = @Busqueda	  
  END
  
END