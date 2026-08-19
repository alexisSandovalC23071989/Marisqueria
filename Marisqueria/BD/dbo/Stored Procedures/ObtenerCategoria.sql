CREATE PROCEDURE [dbo].[ObtenerCategoria]
    @IdCategoria INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id_categoria AS IdCategoria,
        nombre AS Nombre
    FROM dbo.CATEGORIA
    WHERE id_categoria = @IdCategoria;
END
