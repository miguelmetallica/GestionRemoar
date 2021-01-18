CREATE PROCEDURE [dbo].[ComprobanteImputaEntregaProductoGet]
	@ProductoId NVARCHAR(150) = ''
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Tmp AS TABLE(ComprobanteId NVARCHAR(150), 
						Id NVARCHAR(150),
						ProductoId NVARCHAR(150),
						ProductoCodigo NVARCHAR(50),
						ProductoNombre NVARCHAR(150),
						Cantidad INT,
						PrecioUnitario NUMERIC(18,2),
						Precio NUMERIC(18,2),
						DescuentoPorcentaje NUMERIC(18,2),
						Imputado NUMERIC(18,2),
						ImputadoPorcentaje NUMERIC(18,2)
						) 

	DECLARE @Comp AS TABLE(Num INT IDENTITY, 
						Id NVARCHAR(150),
						ProductoId NVARCHAR(150),
						Cantidad INT,
						Precio NUMERIC(18,2)) 

	INSERT INTO @Comp(Id,ProductoId,Cantidad,Precio)
	SELECT D.Id,D.ProductoId,D.Cantidad,ISNULL(CONVERT(NUMERIC(18,2),D.PrecioUnitario - (PrecioUnitario * (P.DescuentoPorcentaje / 100))),0)
	FROM Comprobantes P
	INNER JOIN ComprobantesDetalle D
	ON D.ComprobanteId = P.Id
	WHERE D.Id = @ProductoId

	DECLARE @I INT = 1
	DECLARE @Cant INT = 1

	DECLARE @imputado numeric(18,2) = 0
	DECLARE @imputado_saldo numeric(18,2) = 0

	WHILE @I <= (SELECT COUNT(*) FROM @Comp) 
	BEGIN
		SELECT @Cant = D.Cantidad
		FROM Comprobantes P
		INNER JOIN ComprobantesDetalle D
		ON D.ComprobanteId = P.Id
		WHERE D.ID = (SELECT Id FROM @Comp WHERE Num = @i)

		SELECT @imputado_saldo = ISNULL(SUM(ISNULL(ImporteImputado,0)),0)
		FROM @Comp C
		LEFT JOIN ComprobantesDetalleImputacion I
		ON I.DetalleId = C.Id
		AND I.ProductoId = C.ProductoId		
		WHERE Num = @i
	
		WHILE @Cant > 0
		BEGIN
			IF(ISNULL(@imputado_saldo,0) <= ISNULL((SELECT Precio FROM @Comp WHERE Num = @i),0))
			BEGIN
				SET @imputado = ISNULL(@imputado_saldo,0)
				SET @imputado_saldo = 0
			END
			ELSE
			BEGIN
				SET @imputado = ISNULL((SELECT Precio FROM @Comp WHERE Num = @i),0)
				SET @imputado_saldo = ISNULL(@imputado_saldo,0) - ISNULL(@imputado,0)
			END

		
		
			INSERT INTO @TMP
			SELECT D.ComprobanteId, 
					D.Id,ProductoId,
					ProductoCodigo,
					ProductoNombre,
					1,
					PrecioUnitario,
					ISNULL((SELECT Precio FROM @Comp WHERE Num = @i),0) Precio,
					P.DescuentoPorcentaje,
					ISNULL(@imputado,0),				
					CONVERT(NUMERIC(18,2),ROUND((ISNULL(@imputado,0) * 100) /ISNULL((SELECT Precio FROM @Comp WHERE Num = @i),0),2))
			FROM Comprobantes P
			INNER JOIN ComprobantesDetalle D
			ON D.ComprobanteId = P.Id
			WHERE --D.Id = (SELECT Id FROM @Comp WHERE Num = @i)
			--AND 
			D.Id = @ProductoId

			SET @Cant = @Cant - 1
		END
		SET @I = @I + 1
	END
	
	SELECT * 
	FROM @TMP
END