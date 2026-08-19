CREATE PROCEDURE ObtenerUsuarioPorCorreo
    @Correo VARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        USUARIO.Id,
        USUARIO.Nombre,
        USUARIO.Correo,
        USUARIO.ContrasenaHash,
        USUARIO.IdRol,
        ROL.Nombre AS Rol
    FROM USUARIO
    INNER JOIN ROL
        ON USUARIO.IdRol = ROL.Id
    WHERE USUARIO.Correo = @Correo;
END