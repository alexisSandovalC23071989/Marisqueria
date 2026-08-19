CREATE PROCEDURE [dbo].[AgregarPromocionProducto] @IdPromocion INT, @IdProducto INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.PROMOCION_PRODUCTO(id_promocion, id_producto) VALUES(@IdPromocion, @IdProducto);
END
