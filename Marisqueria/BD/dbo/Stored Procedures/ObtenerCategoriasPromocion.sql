CREATE PROCEDURE [dbo].[ObtenerCategoriasPromocion] @IdPromocion INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.id_categoria IdCategoria, c.nombre Nombre
    FROM dbo.PROMOCION_CATEGORIA pc
    INNER JOIN dbo.CATEGORIA c ON c.id_categoria=pc.id_categoria
    WHERE pc.id_promocion=@IdPromocion
    ORDER BY c.nombre;
END
