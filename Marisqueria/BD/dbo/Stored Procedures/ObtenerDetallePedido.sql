CREATE PROCEDURE [dbo].[ObtenerDetallePedido]
    @IdPedido UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        DP.IdDetalle,
        DP.IdPedido,
        DP.id_producto AS IdProducto,

        PR.nombre AS NombreProducto,

        DP.Cantidad,
        DP.PrecioOriginal,
        DP.TipoDescuento,
        DP.ValorDescuento,
        DP.PrecioUnitario,
        DP.Subtotal,
        DP.IdPromocion,

        P.nombre AS NombrePromocion

    FROM dbo.DETALLE_PEDIDO DP

    INNER JOIN dbo.PRODUCTO PR
        ON PR.id_producto = DP.id_producto

    LEFT JOIN dbo.PROMOCION P
        ON P.id_promocion = DP.IdPromocion

    WHERE DP.IdPedido = @IdPedido;
END