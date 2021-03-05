CREATE FUNCTION [eliasm_db].[PrecioContado](@ProductoId [NVARCHAR](150))
RETURNS NUMERIC(18,2) WITH EXECUTE AS CALLER
AS 
BEGIN
	DECLARE @PRECIO NUMERIC(18,2) = 0;
	
	SELECT @PRECIO = CONVERT(NUMERIC(18,2),CASE 
			WHEN ISNULL(P.AceptaDescuento,0) = 0 THEN
				CASE 
					WHEN ISNULL(P.PrecioRebaja,0) <> 0 
						AND CONVERT(DATE,GETDATE()) 
							BETWEEN ISNULL(P.RebajaDesde,'2000/01/01')
								AND ISNULL(P.RebajaHasta,'2100/01/01') 
					THEN ISNULL(P.PrecioRebaja,0) 
					ELSE P.PrecioVenta
				END 
			ELSE
				P.PrecioVenta - P.PrecioVenta * ISNULL(C.DescuentoPorcentaje,0) / 100
			END) 
	FROM Productos P
	LEFT JOIN ParamCategorias C ON C.ID = P.CategoriaId
	WHERE P.ID = @ProductoId
	
	RETURN @PRECIO
END