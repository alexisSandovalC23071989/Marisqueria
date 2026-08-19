CREATE PROCEDURE [dbo].[AgregarPromocionCategoria]
    @IdPromocion INT,
    @IdCategoria INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM dbo.PROMOCION_CATEGORIA WHERE id_promocion=@IdPromocion AND id_categoria=@IdCategoria)
        INSERT INTO dbo.PROMOCION_CATEGORIA(id_promocion,id_categoria) VALUES(@IdPromocion,@IdCategoria);
END
