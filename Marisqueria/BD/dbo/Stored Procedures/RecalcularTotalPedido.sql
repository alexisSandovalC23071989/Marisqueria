CREATE PROCEDURE [dbo].[RecalcularTotalPedido]
    @IdPedido UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE PEDIDO
    SET Total =
    (
        SELECT ISNULL(SUM(Subtotal),0)
        FROM DETALLE_PEDIDO
        WHERE IdPedido = @IdPedido
    )
    WHERE IdPedido = @IdPedido;

    SELECT @IdPedido;
END
