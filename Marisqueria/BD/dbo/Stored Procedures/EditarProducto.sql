CREATE PROCEDURE [dbo].[EditarProducto]
    @IdProducto INT,
    @IdCategoria INT,
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(MAX),
    @PrecioVentaActual DECIMAL(18, 2),
    @Imagen VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.PRODUCTO
    SET
        id_categoria = @IdCategoria,
        nombre = @Nombre,
        descripcion = @Descripcion,
        precio_venta_actual = @PrecioVentaActual,
        imagen = @Imagen
    WHERE id_producto = @IdProducto;

    SELECT @IdProducto AS IdProducto;
END
