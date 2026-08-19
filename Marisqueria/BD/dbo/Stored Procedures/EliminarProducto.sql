CREATE PROCEDURE [dbo].[EliminarProducto]
    @IdProducto INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.PRODUCTO
    WHERE id_producto = @IdProducto;

    SELECT @IdProducto AS IdProducto;
END
