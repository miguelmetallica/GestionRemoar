CREATE PROCEDURE [eliasm_db].[FormasPagosEntidadesCuotasInteresGet]
	@FormaPagoId nvarchar(150) = null,
	@EntidadId nvarchar(150) = null,
	@Cuota int = 0
AS
BEGIN
	SELECT TOP 1 P.Id,P.FormaPagoId,P.EntidadId,P.Descripcion,P.Cuota,P.Interes,P.FechaDesde,P.FechaHasta,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM FormasPagosCuotas P
	WHERE P.FormaPagoId = @FormaPagoId
	AND P.EntidadId = @EntidadId
	AND P.Cuota = @Cuota
	AND CONVERT(DATE,GETDATE()) BETWEEN P.FechaDesde AND P.FechaHasta
	AND P.Estado = 1	
END;