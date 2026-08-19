CREATE PROCEDURE [dbo].[ObtenerStockProducto]
    @IdProducto INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        @IdProducto AS IdProducto,
        ISNULL(SUM(cantidad_disponible), 0) AS StockDisponible
    FROM dbo.INVENTARIO
    WHERE id_producto = @IdProducto
      AND fecha_vencimiento >= CAST(GETDATE() AS DATE);
END