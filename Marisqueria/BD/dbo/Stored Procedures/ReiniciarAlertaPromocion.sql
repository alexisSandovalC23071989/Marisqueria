CREATE PROCEDURE [dbo].[ReiniciarAlertaPromocion] @IdPromocion INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.PROMOCION_ALERTA WHERE id_promocion=@IdPromocion;
END
