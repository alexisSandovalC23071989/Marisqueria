CREATE PROCEDURE ObtenerUsuario
    @Id UNIQUEIDENTIFIER
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        USUARIO.Id,
        USUARIO.Nombre,
        USUARIO.Correo,
        USUARIO.Telefono,
        USUARIO.ContrasenaHash,
        ROL.Nombre AS Rol
    FROM USUARIO
        INNER JOIN ROL
            ON USUARIO.IdRol = ROL.Id
    WHERE USUARIO.Id = @Id;

END