CREATE PROCEDURE [dbo].[ObtenerPromocionesActivas]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.id_promocion IdPromocion, p.nombre Nombre, ISNULL(p.descripcion,'') Descripcion,
           p.tipo_descuento TipoDescuento, p.valor_descuento ValorDescuento,
           p.fecha_inicio FechaInicio, p.fecha_fin FechaFin, p.activa Activa,
           p.fecha_desactivacion FechaDesactivacion, 'ACTIVA' Estado
    FROM dbo.PROMOCION p
    WHERE p.activa=1 AND SYSDATETIME() BETWEEN p.fecha_inicio AND p.fecha_fin
    ORDER BY p.fecha_fin, p.nombre;
END
