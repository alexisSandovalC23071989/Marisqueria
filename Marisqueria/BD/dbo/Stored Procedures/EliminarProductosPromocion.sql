CREATE PROCEDURE [dbo].[EliminarProductosPromocion] @IdPromocion INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.PROMOCION_PRODUCTO WHERE id_promocion=@IdPromocion;
END
