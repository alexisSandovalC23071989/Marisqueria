CREATE PROCEDURE [dbo].[EliminarProveedor]
    @IdProveedor INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.INVENTARIO
        WHERE id_proveedor = @IdProveedor
    )
    BEGIN
        THROW 50001,
              'No se puede eliminar el proveedor porque tiene registros asociados en inventario.',
              1;
    END;

    DELETE FROM dbo.PRODUCTO_PROVEEDOR
    WHERE id_proveedor = @IdProveedor;

    DELETE FROM dbo.PROVEEDOR
    WHERE id_proveedor = @IdProveedor;

    SELECT @IdProveedor AS IdProveedor;
END
