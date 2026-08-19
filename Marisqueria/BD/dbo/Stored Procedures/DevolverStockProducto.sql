CREATE PROCEDURE [dbo].[DevolverStockProducto]
    @IdProducto INT,
    @Cantidad INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdInventario INT;

    SELECT TOP 1
        @IdInventario = id_inventario
    FROM dbo.INVENTARIO
    WHERE id_producto = @IdProducto
    ORDER BY id_inventario;

    IF @IdInventario IS NULL
    BEGIN
        RAISERROR(
            'No existe un registro de inventario para este producto.',
            16,
            1
        );

        RETURN;
    END

    UPDATE dbo.INVENTARIO
    SET cantidad_disponible =
        cantidad_disponible + @Cantidad
    WHERE id_inventario = @IdInventario;

    SELECT @IdProducto;
END