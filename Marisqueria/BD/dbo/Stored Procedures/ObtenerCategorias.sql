CREATE PROCEDURE [dbo].[ObtenerCategorias]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id_categoria AS IdCategoria,
        nombre AS Nombre
    FROM dbo.CATEGORIA
    ORDER BY nombre;
END
