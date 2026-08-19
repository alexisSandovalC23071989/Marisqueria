CREATE PROCEDURE [dbo].[EditarInventario]
    @IdInventario INT,
    @IdProducto INT,
    @IdProveedor INT,
    @Lote VARCHAR(100),
    @CantidadDisponible DECIMAL(18, 2),
    @FechaIngreso DATETIME2(0),
    @FechaVencimiento DATE,
    @Ubicacion VARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.INVENTARIO
    SET
        id_producto = @IdProducto,
        id_proveedor = @IdProveedor,
        lote = @Lote,
        cantidad_disponible = @CantidadDisponible,
        fecha_ingreso = @FechaIngreso,
        fecha_vencimiento = @FechaVencimiento,
        ubicacion = @Ubicacion
    WHERE id_inventario = @IdInventario;

    SELECT @IdInventario AS IdInventario;
END
