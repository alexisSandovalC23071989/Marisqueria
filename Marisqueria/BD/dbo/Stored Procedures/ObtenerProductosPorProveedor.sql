CREATE PROCEDURE [dbo].[ObtenerProductosPorProveedor]
    @IdProveedor INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        productoProveedor.id_producto AS IdProducto,
        productoProveedor.id_proveedor AS IdProveedor,
        productoProveedor.precio_compra AS PrecioCompra,
        productoProveedor.unidad_medida AS UnidadMedida,
        producto.nombre AS Producto
    FROM dbo.PRODUCTO_PROVEEDOR productoProveedor
        INNER JOIN dbo.PRODUCTO producto
            ON productoProveedor.id_producto = producto.id_producto
    WHERE productoProveedor.id_proveedor = @IdProveedor
    ORDER BY producto.nombre;
END
