CREATE PROCEDURE [dbo].[EliminarInventario]
    @IdInventario INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.INVENTARIO
    WHERE id_inventario = @IdInventario;

    SELECT @IdInventario AS IdInventario;
END
