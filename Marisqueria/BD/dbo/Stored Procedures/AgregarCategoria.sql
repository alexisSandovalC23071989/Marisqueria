CREATE PROCEDURE [dbo].[AgregarCategoria]
    @Nombre VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.CATEGORIA (nombre)
    VALUES (@Nombre);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdCategoria;
END
