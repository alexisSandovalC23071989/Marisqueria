CREATE PROCEDURE [dbo].[EliminarProductoProveedor]
    @IdProveedor INT,
    @IdProducto INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.PRODUCTO_PROVEEDOR
    WHERE id_producto = @IdProducto
        AND id_proveedor = @IdProveedor;

    SELECT @IdProducto AS IdProducto;
END
