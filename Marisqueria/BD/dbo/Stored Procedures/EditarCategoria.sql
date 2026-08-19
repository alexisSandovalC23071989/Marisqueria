CREATE PROCEDURE [dbo].[EditarCategoria]
    @IdCategoria INT,
    @Nombre VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.CATEGORIA
    SET nombre = @Nombre
    WHERE id_categoria = @IdCategoria;

    SELECT @IdCategoria AS IdCategoria;
END
