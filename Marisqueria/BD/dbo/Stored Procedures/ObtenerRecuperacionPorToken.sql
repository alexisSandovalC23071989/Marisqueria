CREATE PROCEDURE ObtenerRecuperacionPorToken
    @Token VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        IdUsuario,
        Token,
        FechaCreacion,
        FechaExpiracion,
        Utilizado
    FROM RECUPERACION_PASSWORD
    WHERE Token = @Token;
END