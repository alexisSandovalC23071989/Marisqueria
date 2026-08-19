CREATE PROCEDURE [dbo].[ObtenerAlertasPromociones]
    @DiasAnticipacion INT = 3,
    @SoloPendientes BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.PROMOCION_ALERTA(id_promocion)
    SELECT p.id_promocion
    FROM dbo.PROMOCION p
    WHERE p.activa=1
      AND p.fecha_fin >= SYSDATETIME()
      AND p.fecha_fin <= DATEADD(DAY, @DiasAnticipacion, SYSDATETIME())
      AND NOT EXISTS (SELECT 1 FROM dbo.PROMOCION_ALERTA a WHERE a.id_promocion=p.id_promocion);

    SELECT a.id_alerta IdAlerta,
           a.id_promocion IdPromocion,
           p.nombre NombrePromocion,
           p.fecha_fin FechaFin,
           DATEDIFF(DAY, CONVERT(date,SYSDATETIME()), CONVERT(date,p.fecha_fin)) DiasRestantes,
           a.fecha_generacion FechaGeneracion,
           a.revisada Revisada,
           a.fecha_revision FechaRevision
    FROM dbo.PROMOCION_ALERTA a
    INNER JOIN dbo.PROMOCION p ON p.id_promocion=a.id_promocion
    WHERE (@SoloPendientes=0 OR a.revisada=0)
    ORDER BY a.revisada ASC, p.fecha_fin ASC;
END
