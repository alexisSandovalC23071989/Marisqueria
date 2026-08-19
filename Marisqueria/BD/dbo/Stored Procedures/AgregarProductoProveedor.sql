CREATE PROCEDURE [dbo].[AgregarProductoProveedor]
    @IdProveedor INT,
    @IdProducto INT,
    @PrecioCompra DECIMAL(18, 2),
    @UnidadMedida VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.PRODUCTO_PROVEEDOR
    (
        id_producto,
        id_proveedor,
        precio_compra,
        unidad_medida
    )
    VALUES
    (
        @IdProducto,
        @IdProveedor,
        @PrecioCompra,
        @UnidadMedida
    );

    SELECT @IdProducto AS IdProducto;
END
