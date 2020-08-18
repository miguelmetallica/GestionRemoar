

CREATE PROCEDURE [SeguridadUsuariosRoles_Get]
	@UsuarioId int = NULL,
	@BusquedaLike varchar(150) = NULL,
	@BusquedaDisponible varchar(150) = NULL
AS
BEGIN
	IF @BusquedaLike IS NOT NULL
	BEGIN
		SELECT P.UsuarioRolId,P.UsuarioId,U.Usuario,P.RolId,R.Rol,P.FechaAlta,P.UsuarioAlta
		FROM SeguridadUsuariosRoles P
		INNER JOIN SeguridadRoles R ON R.RolId = P.RolId
		INNER JOIN SeguridadUsuarios U ON U.UsuarioId = P.UsuarioId
		WHERE P.UsuarioId = @UsuarioId
		AND (
				CONVERT(Varchar(10),P.RolId) LIKE @BusquedaLike + '%'
			OR
				R.Rol LIKE @BusquedaLike + '%'
			)
		ORDER BY R.Rol
	END

	IF @BusquedaDisponible IS NOT NULL
	BEGIN
		SELECT 0 UsuarioRolId,@UsuarioId UsuarioId,'' Usuario,R.RolId,R.Rol,R.FechaAlta,R.UsuarioAlta
		FROM SeguridadRoles R
		WHERE R.RolId NOT IN (SELECT P.RolId FROM SeguridadUsuariosRoles P WHERE P.UsuarioId = @UsuarioId)
		AND (
				CONVERT(Varchar(10),R.RolId) LIKE @BusquedaDisponible + '%'
			OR
				R.Rol LIKE @BusquedaDisponible + '%'
			)
		ORDER BY R.Rol
	END
END