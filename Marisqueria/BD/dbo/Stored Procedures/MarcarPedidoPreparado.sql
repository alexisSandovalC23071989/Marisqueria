CREATE PROCEDURE [dbo].MarcarPedidoPreparado
    @IdPedido UNIQUEIDENTIFIER

AS
BEGIN
    SET NOCOUNT ON;

    UPDATE PEDIDO
    SET Estado='Preparado'
    WHERE IdPedido=@IdPedido;

    SELECT @IdPedido;

END