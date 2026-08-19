CREATE PROCEDURE [dbo].[MarcarAlertaPromocionRevisada] @IdAlerta INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.PROMOCION_ALERTA
    SET revisada=1, fecha_revision=SYSDATETIME()
    WHERE id_alerta=@IdAlerta;
    SELECT @@ROWCOUNT;
END
