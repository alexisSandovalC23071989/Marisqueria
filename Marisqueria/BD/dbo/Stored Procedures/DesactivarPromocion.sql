CREATE PROCEDURE [dbo].[DesactivarPromocion] @IdPromocion INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.PROMOCION SET activa=0, fecha_desactivacion=SYSDATETIME() WHERE id_promocion=@IdPromocion;
    SELECT @IdPromocion;
END
