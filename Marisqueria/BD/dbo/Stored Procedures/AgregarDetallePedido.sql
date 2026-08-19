CREATE PROCEDURE [dbo].[AgregarDetallePedido]
    @IdDetalle UNIQUEIDENTIFIER,
    @IdPedido UNIQUEIDENTIFIER,
    @IdProducto INT,
    @Cantidad INT,
    @PrecioOriginal DECIMAL(10,2),
    @TipoDescuento VARCHAR(12),
    @ValorDescuento DECIMAL(18,2),
    @PrecioUnitario DECIMAL(10,2),
    @Subtotal DECIMAL(10,2),
    @IdPromocion INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.DETALLE_PEDIDO
    (
        IdDetalle,
        IdPedido,
        id_producto,
        Cantidad,
        PrecioOriginal,
        TipoDescuento,
        ValorDescuento,
        PrecioUnitario,
        Subtotal,
        IdPromocion
    )
    VALUES
    (
        @IdDetalle,
        @IdPedido,
        @IdProducto,
        @Cantidad,
        @PrecioOriginal,
        @TipoDescuento,
        @ValorDescuento,
        @PrecioUnitario,
        @Subtotal,
        @IdPromocion
    );

    SELECT @IdDetalle;
END