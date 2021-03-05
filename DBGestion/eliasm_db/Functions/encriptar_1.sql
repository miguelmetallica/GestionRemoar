CREATE FUNCTION [eliasm_db].[encriptar](@texto [nvarchar](100))
RETURNS [nvarchar](100) WITH EXECUTE AS CALLER
AS 
BEGIN
	-- Declare the return variable here
	DECLARE @N as int
	DECLARE @i as int=1
	DECLARE @Char as int
	declare @mod1 as int
	declare @mod2 as int
	declare @llave as char(7)='@dmin'
	declare @devuelve as nvarchar(100)=''
	if @texto is not null
	begin	
		set @texto=ltrim(rtrim(@texto))
		set @n=len(@llave)
		while @i<=len(@texto)
		begin
			set @mod1=@i % @n
			set @mod2=case @i%@n
							when 0 then -1
							else 0
							end 
			set @char=ascii(substring(@llave,@mod1-@n*@mod2,1))
			set @devuelve=@devuelve+char(ascii(substring(@texto,@i,1))^@char)
			set @i=@i+1
		end
	end
	else
	begin
		set @devuelve=''''
	end
	RETURN @devuelve
END