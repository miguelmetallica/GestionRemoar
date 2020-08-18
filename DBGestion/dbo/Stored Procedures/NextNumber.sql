create PROCEDURE [NextNumber]
	@Tabla [varchar](50)
WITH EXECUTE AS CALLER
AS
BEGIN
	SET NOCOUNT ON;        
	DECLARE @Numero numeric(8)

	SELECT @Numero = Numero FROM SistemaTablasSecuencias WHERE Tabla = @Tabla
	
	IF @Numero IS NULL    
		INSERT INTO SistemaTablasSecuencias (Tabla,Numero)VALUES(@Tabla,0)     
	
	BEGIN TRAN     
		SELECT @Numero = Numero FROM SistemaTablasSecuencias WITH (UPDLOCK,HOLDLOCK) WHERE Tabla = @Tabla   
		UPDATE SistemaTablasSecuencias SET Numero = Numero + 1 WHERE Tabla = @Tabla		
		SET @Numero = @Numero + 1    
	COMMIT     	

	RETURN @Numero

END