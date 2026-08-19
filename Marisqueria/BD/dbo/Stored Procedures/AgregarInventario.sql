CREATE PROCEDURE [dbo].[AgregarInventario]
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

    INSERT INTO dbo.INVENTARIO
    (
        id_producto,
        id_proveedor,
        lote,
        cantidad_disponible,
        fecha_ingreso,
        fecha_vencimiento,
        ubicacion
    )
    VALUES
    (
        @IdProducto,
        @IdProveedor,
        @Lote,
        @CantidadDisponible,
        @FechaIngreso,
        @FechaVencimiento,
        @Ubicacion
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdInventario;
END
