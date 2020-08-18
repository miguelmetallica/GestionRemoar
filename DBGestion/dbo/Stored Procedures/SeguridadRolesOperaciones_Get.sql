

CREATE PROCEDURE [SeguridadRolesOperaciones_Get]
	@RolId int = NULL,
	@BusquedaLike varchar(150) = NULL
AS
BEGIN
	SELECT P.RolOperacionId,P.RolId,R.Rol,P.OperacionId,O.Operacion,P.FechaAlta,P.UsuarioAlta
	FROM SeguridadRolesOperaciones P
	INNER JOIN SeguridadRoles R ON R.RolId = P.RolId
	INNER JOIN SeguridadOperaciones O ON O.OperacionId = P.OperacionId
	WHERE P.RolId = @RolId
	AND (
			CONVERT(Varchar(10),P.OperacionId) LIKE @BusquedaLike + '%'
		OR
			O.Operacion LIKE @BusquedaLike + '%'
		)
	ORDER BY O.Operacion

END