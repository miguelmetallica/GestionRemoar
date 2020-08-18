
CREATE PROCEDURE [Pedidos_Get]
	@PedidoId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaLikeActivo varchar(150) = NULL,
	@Busqueda varchar(150) = NULL
AS
BEGIN
  IF @PedidoId IS NOT NULL
  BEGIN
	SELECT PedidoId,PedidoCodigo,ClienteId,ClienteCodigo,Apellido,Nombre,TipoDocumentoId,
			TipoDocumento,NroDocumento,ProvinciaFacturacionId,ProvinciaFacturacion,LocalidadFacturacionId,
			LocalidadFacturacion,CalleFacturacion,NroCalleFacturacion,DatosDomicilioFacturacion,
			ProvinciaEnvioId,ProvinciaEnvio,LocalidadEnvioId,LocalidadEnvio,CalleEnvio,NroCalleEnvio,
			DatosDomicilioEnvio,TipoCondicionIvaId,TipoCondicionIva,FechaEmision,FechaVencimiento,MontoDescuento,
			MontoExento,MontoGravadoNormal,MontoNoGravado,MontoGravadoReducido,MontoInteres,IvaInscriptoNormal,
			IvaNoInscrNormal,IvaInscriptoReducido,IvaNoInscrReducido,MontoGravodo10,MontoGravado21,MontoGravado27,
			IvaInscripto10,IvaInscripto21,IvaInscripto27,IvaNoInscripto10,IvaNoInscripto21,IvaNoInscripto27,
			MontoGravodoNormal10,MontoGravadoNormal21,MontoGravadoNormal27,IvaInscriptoNormal10,IvaInscriptoNormal21,
			IvaInscriptoNormal27,IvaNoInscriptoNormal10,IvaNoInscriptoNormal21,IvaNoInscriptoNormal27,PercepcionIva,
			ImpuestoMunicipal,PercepcionImpuestoMunicipal,TipoConvenioIIBBId,FechaVigDesdeIIBB,FechaVigHastaIIBB,
			AlicuotaIIBB,PercepcionIIBB,Total,Facturado,Observaciones,Estado,FechaAlta,UsuarioAlta
	FROM Pedidos P
	WHERE P.PedidoId = @PedidoId
  END

  IF @BusquedaLike IS NOT NULL
  BEGIN
	  SELECT PedidoId,PedidoCodigo,ClienteId,ClienteCodigo,Apellido,Nombre,TipoDocumentoId,
			TipoDocumento,NroDocumento,ProvinciaFacturacionId,ProvinciaFacturacion,LocalidadFacturacionId,
			LocalidadFacturacion,CalleFacturacion,NroCalleFacturacion,DatosDomicilioFacturacion,
			ProvinciaEnvioId,ProvinciaEnvio,LocalidadEnvioId,LocalidadEnvio,CalleEnvio,NroCalleEnvio,
			DatosDomicilioEnvio,TipoCondicionIvaId,TipoCondicionIva,FechaEmision,FechaVencimiento,MontoDescuento,
			MontoExento,MontoGravadoNormal,MontoNoGravado,MontoGravadoReducido,MontoInteres,IvaInscriptoNormal,
			IvaNoInscrNormal,IvaInscriptoReducido,IvaNoInscrReducido,MontoGravodo10,MontoGravado21,MontoGravado27,
			IvaInscripto10,IvaInscripto21,IvaInscripto27,IvaNoInscripto10,IvaNoInscripto21,IvaNoInscripto27,
			MontoGravodoNormal10,MontoGravadoNormal21,MontoGravadoNormal27,IvaInscriptoNormal10,IvaInscriptoNormal21,
			IvaInscriptoNormal27,IvaNoInscriptoNormal10,IvaNoInscriptoNormal21,IvaNoInscriptoNormal27,PercepcionIva,
			ImpuestoMunicipal,PercepcionImpuestoMunicipal,TipoConvenioIIBBId,FechaVigDesdeIIBB,FechaVigHastaIIBB,
			AlicuotaIIBB,PercepcionIIBB,Total,Facturado,Observaciones,Estado,FechaAlta,UsuarioAlta
	  FROM Pedidos P
	  WHERE 
		(
			CONVERT(Varchar(10),P.PedidoId) LIKE @BusquedaLike + '%'
		OR
			P.PedidoCodigo LIKE @BusquedaLike + '%'
		)
	  ORDER BY P.PedidoCodigo
  END

  IF @BusquedaLikeActivo IS NOT NULL
  BEGIN
	  SELECT PedidoId,PedidoCodigo,ClienteId,ClienteCodigo,Apellido,Nombre,TipoDocumentoId,
			TipoDocumento,NroDocumento,ProvinciaFacturacionId,ProvinciaFacturacion,LocalidadFacturacionId,
			LocalidadFacturacion,CalleFacturacion,NroCalleFacturacion,DatosDomicilioFacturacion,
			ProvinciaEnvioId,ProvinciaEnvio,LocalidadEnvioId,LocalidadEnvio,CalleEnvio,NroCalleEnvio,
			DatosDomicilioEnvio,TipoCondicionIvaId,TipoCondicionIva,FechaEmision,FechaVencimiento,MontoDescuento,
			MontoExento,MontoGravadoNormal,MontoNoGravado,MontoGravadoReducido,MontoInteres,IvaInscriptoNormal,
			IvaNoInscrNormal,IvaInscriptoReducido,IvaNoInscrReducido,MontoGravodo10,MontoGravado21,MontoGravado27,
			IvaInscripto10,IvaInscripto21,IvaInscripto27,IvaNoInscripto10,IvaNoInscripto21,IvaNoInscripto27,
			MontoGravodoNormal10,MontoGravadoNormal21,MontoGravadoNormal27,IvaInscriptoNormal10,IvaInscriptoNormal21,
			IvaInscriptoNormal27,IvaNoInscriptoNormal10,IvaNoInscriptoNormal21,IvaNoInscriptoNormal27,PercepcionIva,
			ImpuestoMunicipal,PercepcionImpuestoMunicipal,TipoConvenioIIBBId,FechaVigDesdeIIBB,FechaVigHastaIIBB,
			AlicuotaIIBB,PercepcionIIBB,Total,Facturado,Observaciones,Estado,FechaAlta,UsuarioAlta
	  FROM Pedidos P
	  WHERE 
		(
			CONVERT(Varchar(10),P.PedidoId) LIKE @BusquedaLikeActivo + '%'
		OR
			P.PedidoCodigo LIKE @BusquedaLikeActivo + '%'
		)
	  ORDER BY P.PedidoCodigo
  END

  IF @Busqueda IS NOT NULL
  BEGIN
	  SELECT PedidoId,PedidoCodigo,ClienteId,ClienteCodigo,Apellido,Nombre,TipoDocumentoId,
			TipoDocumento,NroDocumento,ProvinciaFacturacionId,ProvinciaFacturacion,LocalidadFacturacionId,
			LocalidadFacturacion,CalleFacturacion,NroCalleFacturacion,DatosDomicilioFacturacion,
			ProvinciaEnvioId,ProvinciaEnvio,LocalidadEnvioId,LocalidadEnvio,CalleEnvio,NroCalleEnvio,
			DatosDomicilioEnvio,TipoCondicionIvaId,TipoCondicionIva,FechaEmision,FechaVencimiento,MontoDescuento,
			MontoExento,MontoGravadoNormal,MontoNoGravado,MontoGravadoReducido,MontoInteres,IvaInscriptoNormal,
			IvaNoInscrNormal,IvaInscriptoReducido,IvaNoInscrReducido,MontoGravodo10,MontoGravado21,MontoGravado27,
			IvaInscripto10,IvaInscripto21,IvaInscripto27,IvaNoInscripto10,IvaNoInscripto21,IvaNoInscripto27,
			MontoGravodoNormal10,MontoGravadoNormal21,MontoGravadoNormal27,IvaInscriptoNormal10,IvaInscriptoNormal21,
			IvaInscriptoNormal27,IvaNoInscriptoNormal10,IvaNoInscriptoNormal21,IvaNoInscriptoNormal27,PercepcionIva,
			ImpuestoMunicipal,PercepcionImpuestoMunicipal,TipoConvenioIIBBId,FechaVigDesdeIIBB,FechaVigHastaIIBB,
			AlicuotaIIBB,PercepcionIIBB,Total,Facturado,Observaciones,Estado,FechaAlta,UsuarioAlta
	  FROM Pedidos P
	  WHERE P.PedidoId = @Busqueda	  
  END
  
END