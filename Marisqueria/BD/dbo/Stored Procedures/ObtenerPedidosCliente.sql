CREATE PROCEDURE [dbo].ObtenerPedidosCliente
    @IdUsuario UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM PEDIDO
    WHERE IdUsuario=@IdUsuario
    ORDER BY FechaPedido DESC;
END