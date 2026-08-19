CREATE PROCEDURE [dbo].[DescontarStockProducto]
    @IdProducto INT,
    @Cantidad INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StockDisponible DECIMAL(18,2);

    SELECT @StockDisponible =
        ISNULL(SUM(cantidad_disponible), 0)
    FROM dbo.INVENTARIO
    WHERE id_producto = @IdProducto;

    IF @StockDisponible < @Cantidad
    BEGIN
        RAISERROR('No hay suficiente stock disponible.', 16, 1);
        RETURN;
    END

    DECLARE @IdInventario INT;
    DECLARE @Disponible DECIMAL(18,2);
    DECLARE @Restante DECIMAL(18,2);

    SET @Restante = @Cantidad;

    DECLARE inventario_cursor CURSOR FOR
        SELECT id_inventario, cantidad_disponible
        FROM dbo.INVENTARIO
        WHERE id_producto = @IdProducto
          AND cantidad_disponible > 0
        ORDER BY id_inventario;

    OPEN inventario_cursor;

    FETCH NEXT FROM inventario_cursor
    INTO @IdInventario, @Disponible;

    WHILE @@FETCH_STATUS = 0 AND @Restante > 0
    BEGIN
        IF @Disponible >= @Restante
        BEGIN
            UPDATE dbo.INVENTARIO
            SET cantidad_disponible =
                cantidad_disponible - @Restante
            WHERE id_inventario = @IdInventario;

            SET @Restante = 0;
        END
        ELSE
        BEGIN
            UPDATE dbo.INVENTARIO
            SET cantidad_disponible = 0
            WHERE id_inventario = @IdInventario;

            SET @Restante =
                @Restante - @Disponible;
        END

        FETCH NEXT FROM inventario_cursor
        INTO @IdInventario, @Disponible;
    END

    CLOSE inventario_cursor;
    DEALLOCATE inventario_cursor;

    SELECT @IdProducto;
END