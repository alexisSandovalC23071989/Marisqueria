CREATE PROCEDURE ObtenerResenasPorProducto
(
    @id_producto INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id_resena AS IdResena,
        id_usuario AS IdUsuario,
        id_producto AS IdProducto,
        calificacion AS Calificacion,
        comentario AS Comentario,
        fecha AS Fecha
    FROM RESENA
    WHERE id_producto = @id_producto
    ORDER BY fecha DESC;
END;
GO