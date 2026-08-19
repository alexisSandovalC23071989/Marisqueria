CREATE PROCEDURE [dbo].[AgregarProducto]
    @IdCategoria INT,
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(MAX),
    @PrecioVentaActual DECIMAL(18, 2),
    @Imagen VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.PRODUCTO
    (
        id_categoria,
        nombre,
        descripcion,
        precio_venta_actual,
        imagen
    )
    VALUES
    (
        @IdCategoria,
        @Nombre,
        @Descripcion,
        @PrecioVentaActual,
        @Imagen
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdProducto;
END
