CREATE PROCEDURE [dbo].[ObtenerPromociones] @Estado VARCHAR(20)=NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.id_promocion IdPromocion, p.nombre Nombre, ISNULL(p.descripcion,'') Descripcion,
           p.tipo_descuento TipoDescuento, p.valor_descuento ValorDescuento,
           p.fecha_inicio FechaInicio, p.fecha_fin FechaFin, p.activa Activa,
           p.fecha_desactivacion FechaDesactivacion,
           CASE WHEN p.activa=0 THEN 'DESACTIVADA'
                WHEN SYSDATETIME() < p.fecha_inicio THEN 'PROGRAMADA'
                WHEN SYSDATETIME() > p.fecha_fin THEN 'FINALIZADA'
                ELSE 'ACTIVA' END Estado
    FROM dbo.PROMOCION p
    WHERE @Estado IS NULL OR @Estado='' OR
          @Estado = CASE WHEN p.activa=0 THEN 'DESACTIVADA'
                         WHEN SYSDATETIME() < p.fecha_inicio THEN 'PROGRAMADA'
                         WHEN SYSDATETIME() > p.fecha_fin THEN 'FINALIZADA'
                         ELSE 'ACTIVA' END
    ORDER BY p.fecha_inicio DESC, p.id_promocion DESC;
END
