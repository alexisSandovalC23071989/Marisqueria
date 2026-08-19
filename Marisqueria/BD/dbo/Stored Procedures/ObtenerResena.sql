CREATE PROCEDURE ObtenerResena
(
    @id_resena INT
)
AS
BEGIN

    SELECT *

    FROM RESENA

    WHERE id_resena=@id_resena;

END