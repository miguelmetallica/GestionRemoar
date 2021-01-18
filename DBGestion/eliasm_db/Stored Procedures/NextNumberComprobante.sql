CREATE PROCEDURE [eliasm_db].[NextNumberComprobante]
	@SucursalId nvarchar(150),
	@TipoComprobanteId nvarchar(150)
WITH EXECUTE AS CALLER
AS
BEGIN
	SET NOCOUNT ON;        	
	DECLARE @Numero numeric(10)

	SELECT @Numero = Numero 
	FROM ComprobantesNumeraciones 
	WHERE SucursalId = @SucursalId 
	AND TipoComprobanteId = @TipoComprobanteId
	
	BEGIN TRAN     
		SELECT @Numero = Numero 
		FROM ComprobantesNumeraciones 
		WITH (UPDLOCK,HOLDLOCK) 
		WHERE SucursalId = @SucursalId 
		AND TipoComprobanteId = @TipoComprobanteId   

		UPDATE ComprobantesNumeraciones SET Numero = Numero + 1 
		WHERE SucursalId = @SucursalId 
		AND TipoComprobanteId = @TipoComprobanteId

		SET @Numero = @Numero + 1    
	COMMIT     	

	RETURN @Numero

END