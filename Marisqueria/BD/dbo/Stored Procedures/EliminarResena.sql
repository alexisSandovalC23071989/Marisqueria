CREATE PROCEDURE EliminarResena
(
    @id_resena INT
)
AS
BEGIN

    DELETE FROM RESENA

    WHERE id_resena=@id_resena;

    SELECT @id_resena;

END
