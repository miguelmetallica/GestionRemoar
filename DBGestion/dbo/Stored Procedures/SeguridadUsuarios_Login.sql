

CREATE PROCEDURE [SeguridadUsuarios_Login]
	@Usuario varchar(150) = NULL,
	@Password varchar(150) = NULL	
AS
BEGIN
	SELECT P.UsuarioId,P.Usuario,P.Apellido,P.Nombre,P.Telefono,P.Celular,P.Puesto,P.SucursalId,S.Sucursal,P.Estado,P.FechaAlta,P.UsuarioAlta
	FROM SeguridadUsuarios P
	INNER JOIN Sucursales S ON S.SucursalId = P.SucursalId
	WHERE P.Usuario = @Usuario
	AND P.Password = dbo.encriptar(@Password)
END