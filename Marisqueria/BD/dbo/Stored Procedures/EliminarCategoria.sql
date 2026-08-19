CREATE PROCEDURE [dbo].[EliminarCategoria]
    @IdCategoria INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.PRODUCTO WHERE id_categoria = @IdCategoria)
    BEGIN
        SELECT 0 AS IdCategoria;
        RETURN;
    END

    DELETE FROM dbo.CATEGORIA
    WHERE id_categoria = @IdCategoria;

    SELECT @IdCategoria AS IdCategoria;
END
