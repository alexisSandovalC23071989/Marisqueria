CREATE PROCEDURE [dbo].[EditarProductoProveedor]
    @IdProveedor INT,
    @IdProducto INT,
    @PrecioCompra DECIMAL(18, 2),
    @UnidadMedida VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.PRODUCTO_PROVEEDOR
    SET
        precio_compra = @PrecioCompra,
        unidad_medida = @UnidadMedida
    WHERE id_producto = @IdProducto
        AND id_proveedor = @IdProveedor;

    SELECT @IdProducto AS IdProducto;
END
