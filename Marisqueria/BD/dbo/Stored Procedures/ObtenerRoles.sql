
CREATE PROCEDURE ObtenerRoles

AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        Id,
        Nombre
    FROM ROL;

END