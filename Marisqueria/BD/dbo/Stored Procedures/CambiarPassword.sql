CREATE PROCEDURE CambiarPassword
    @IdUsuario UNIQUEIDENTIFIER,
    @ContrasenaHash VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE USUARIO
    SET ContrasenaHash = @ContrasenaHash
    WHERE Id = @IdUsuario;
END