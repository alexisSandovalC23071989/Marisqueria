CREATE PROCEDURE [dbo].AgregarPedido
    @IdPedido UNIQUEIDENTIFIER,
    @IdUsuario UNIQUEIDENTIFIER,
    @FechaPedido DATETIME,
    @Estado VARCHAR(MAX),
    @Total DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;

BEGIN TRANSACTION

    INSERT INTO dbo.PEDIDO
    (
        IdPedido,
        IdUsuario,
        FechaPedido,
        Estado,
        Total
    )
    VALUES
    (
        @IdPedido,
        @IdUsuario,
        @FechaPedido,
        @Estado,
        @Total
    );

    SELECT @IdPedido;

COMMIT TRANSACTION
END
