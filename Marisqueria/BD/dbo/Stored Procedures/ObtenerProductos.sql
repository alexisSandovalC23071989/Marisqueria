CREATE PROCEDURE [dbo].[ObtenerProductos]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        producto.id_producto AS IdProducto,
        producto.id_categoria AS IdCategoria,
        producto.nombre AS Nombre,
        producto.descripcion AS Descripcion,
        producto.precio_venta_actual AS PrecioVentaActual,
        producto.imagen AS Imagen,
        categoria.nombre AS Categoria
    FROM dbo.PRODUCTO producto
        INNER JOIN dbo.CATEGORIA categoria
            ON producto.id_categoria = categoria.id_categoria
    ORDER BY producto.nombre;
END
