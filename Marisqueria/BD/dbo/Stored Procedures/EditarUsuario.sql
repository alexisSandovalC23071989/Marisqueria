CREATE PROCEDURE EditarUsuario
    @Id UNIQUEIDENTIFIER,
    @IdRol UNIQUEIDENTIFIER,
    @Nombre VARCHAR(100),
    @Correo VARCHAR(150),
    @Telefono VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE USUARIO
    SET
        IdRol = @IdRol,
        Nombre = @Nombre,
        Correo = @Correo,
        Telefono = @Telefono
    WHERE Id = @Id;

    SELECT @Id;
END