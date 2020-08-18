CREATE PROCEDURE [UsuarioAutorizado]
@UsuarioId int,
@Modulo varchar(150),
@Operacion varchar(150)
AS
BEGIN
	SELECT UR.UsuarioId,M.Modulo,O.Operacion
	FROM SeguridadUsuariosRoles UR
	INNER JOIN SeguridadRolesOperaciones RO on RO.RolId = UR.RolId
	INNER JOIN SeguridadOperaciones O on RO.OperacionId = O.OperacionId
	INNER JOIN SeguridadModulos M on M.ModuloId = O.ModuloId
	WHERE UR.UsuarioId = @UsuarioId
	AND M.Modulo = @Modulo
	AND O.Operacion = @Operacion
END