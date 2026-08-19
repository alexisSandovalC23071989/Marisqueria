CREATE PROCEDURE [dbo].[EditarPedido]
    @IdPedido UNIQUEIDENTIFIER,
    @IdUsuario UNIQUEIDENTIFIER,
    @FechaPedido DATETIME,
    @Estado VARCHAR(MAX),
    @Total DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION

    UPDATE dbo.PEDIDO
    SET
        IdUsuario = @IdUsuario,
        FechaPedido = @FechaPedido,
        Estado = @Estado,
        Total = @Total
    WHERE IdPedido = @IdPedido;

    SELECT @IdPedido;

    COMMIT TRANSACTION
END
