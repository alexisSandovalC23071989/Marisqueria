CREATE PROCEDURE [dbo].[EliminarCategoriasPromocion] @IdPromocion INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.PROMOCION_CATEGORIA WHERE id_promocion=@IdPromocion;
END
