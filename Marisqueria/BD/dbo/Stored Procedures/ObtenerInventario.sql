CREATE PROCEDURE [dbo].[ObtenerInventario]
    @IdInventario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        inventario.id_inventario AS IdInventario,
        inventario.id_producto AS IdProducto,
        producto.nombre AS Producto,
        inventario.id_proveedor AS IdProveedor,
        proveedor.empresa AS Proveedor,
        inventario.lote AS Lote,
        inventario.cantidad_disponible AS CantidadDisponible,
        inventario.fecha_ingreso AS FechaIngreso,
        inventario.fecha_vencimiento AS FechaVencimiento,
        inventario.ubicacion AS Ubicacion
    FROM dbo.INVENTARIO inventario
        INNER JOIN dbo.PRODUCTO producto
            ON inventario.id_producto = producto.id_producto
        INNER JOIN dbo.PROVEEDOR proveedor
            ON inventario.id_proveedor = proveedor.id_proveedor
    WHERE inventario.id_inventario = @IdInventario;
END
