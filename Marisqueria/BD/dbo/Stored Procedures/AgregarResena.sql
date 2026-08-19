CREATE PROCEDURE AgregarResena
(
    @id_usuario UNIQUEIDENTIFIER,
    @id_producto INT,
    @calificacion INT,
    @comentario VARCHAR(MAX)
)
AS
BEGIN

    INSERT INTO RESENA
    (
        id_usuario,
        id_producto,
        calificacion,
        comentario,
        fecha
    )
    VALUES
    (
        @id_usuario,
        @id_producto,
        @calificacion,
        @comentario,
        GETDATE()
    );

    SELECT SCOPE_IDENTITY();

END