CREATE PROCEDURE ObtenerRol
    @Id UNIQUEIDENTIFIER

AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        Id,
        Nombre
    FROM ROL
    WHERE Id = @Id;

END