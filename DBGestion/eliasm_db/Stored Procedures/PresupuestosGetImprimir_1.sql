CREATE PROCEDURE [eliasm_db].[PresupuestosGetImprimir]
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
			ISNULL((SELECT SUM(Precio * d.cantidad) FROM PresupuestosDetalle D Where D.PresupuestoId = P.Id),0)Precio,
			ISNULL((SELECT SUM(Cantidad) FROM PresupuestosDetalle D Where D.PresupuestoId = P.Id),0)CantidadProductos,
			S.Nombre SucursalNombre,
			S.Calle SucursalCalle,
			S.CalleNro SucursalCalleNro,
			S.Localidad SucursalLocalidad,
			S.CodigoPostal SucursalCodigoPostal,
			S.Telefono SucursalTelefono,
			(
				SELECT RIGHT('0000'+ CONVERT(VARCHAR(4),DATEPART(YEAR,MAX(Fecha))),4) + RIGHT('00'+ CONVERT(VARCHAR(2),DATEPART(MONTH,MAX(Fecha))),2) + RIGHT('00'+ CONVERT(VARCHAR(2),DATEPART(DAY,MAX(Fecha))),2) + RIGHT('00'+ CONVERT(VARCHAR(2),DATEPART(HOUR,MAX(Fecha))),2) + RIGHT('00'+ CONVERT(VARCHAR(2),DATEPART(MINUTE,MAX(Fecha))),2) + RIGHT('00'+ CONVERT(VARCHAR(2),DATEPART(SECOND,MAX(Fecha))),2)
				FROM (
					SELECT MAX(FechaAlta)Fecha FROM Presupuestos WHERE Id = P.Id
					UNION ALL
					SELECT MAX(FechaAlta) FROM PresupuestosDetalle WHERE PresupuestoId = P.Id
					UNION ALL
					SELECT MAX(FechaEdit)Fecha FROM Presupuestos WHERE Id = P.Id
					UNION ALL
					SELECT MAX(FechaEdit) FROM PresupuestosDetalle WHERE PresupuestoId = P.Id
				)P
			)VersionImpresion
	FROM Presupuestos P
	INNER JOIN Clientes C ON C.Id = P.ClienteId
	INNER JOIN ParamPresupuestosEstados E ON E.Id = P.EstadoId
	LEFT JOIN ParamTiposResponsables TR ON TR.Id = P.TipoResponsableId
	LEFT JOIN ParamClientesCategorias CC ON CC.Id = P.ClienteCategoriaId
	LEFT JOIN AspNetUsers U ON U.UserName = P.UsuarioAlta
	LEFT JOIN Sucursales S ON S.Id = U.SucursalId
	WHERE P.Id = @Id
	AND P.Estado = 1
	
END