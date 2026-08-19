CREATE PROCEDURE [dbo].ActualizarDetallePedido
    @IdDetalle UNIQUEIDENTIFIER,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(10,2),
    @Subtotal DECIMAL(10,2),
    @PrecioOriginal DECIMAL(10,2),
    @TipoDescuento VARCHAR(12),
    @ValorDescuento DECIMAL(18,2),
    @IdPromocion INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.DETALLE_PEDIDO
    SET
        Cantidad = @Cantidad,
        PrecioUnitario = @PrecioUnitario,
        Subtotal = @Subtotal,
        PrecioOriginal = @PrecioOriginal,
        TipoDescuento = @TipoDescuento,
        ValorDescuento = @ValorDescuento,
        IdPromocion = @IdPromocion
    WHERE IdDetalle = @IdDetalle;

    SELECT @IdDetalle;
END
