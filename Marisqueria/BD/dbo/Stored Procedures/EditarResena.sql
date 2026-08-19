CREATE PROCEDURE EditarResena
(
    @id_resena INT,
    @id_usuario UNIQUEIDENTIFIER,
    @id_producto INT,
    @calificacion INT,
    @comentario VARCHAR(MAX)
)
AS
BEGIN

    UPDATE RESENA
    SET

        id_usuario=@id_usuario,
        id_producto=@id_producto,
        calificacion=@calificacion,
        comentario=@comentario

    WHERE id_resena=@id_resena;

    SELECT @id_resena;

END
