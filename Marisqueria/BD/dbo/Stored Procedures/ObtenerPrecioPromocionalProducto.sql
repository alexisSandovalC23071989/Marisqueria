CREATE PROCEDURE [dbo].[ObtenerPrecioPromocionalProducto] @IdProducto INT
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH PromocionesAplicables AS (
        SELECT DISTINCT p.id_promocion, p.nombre, p.tipo_descuento, p.valor_descuento,
               p.fecha_inicio, p.fecha_fin, pr.precio_venta_actual PrecioOriginal,
               CAST(CASE WHEN p.tipo_descuento='PORCENTAJE'
                    THEN pr.precio_venta_actual * (1 - p.valor_descuento / 100.0)
                    ELSE CASE WHEN pr.precio_venta_actual - p.valor_descuento < 0 THEN 0
                              ELSE pr.precio_venta_actual - p.valor_descuento END END AS DECIMAL(18,2)) PrecioPromocional
        FROM dbo.PRODUCTO pr
        INNER JOIN dbo.PROMOCION p ON p.activa=1
             AND SYSDATETIME() BETWEEN p.fecha_inicio AND p.fecha_fin
        WHERE pr.id_producto=@IdProducto
          AND (
              EXISTS (SELECT 1 FROM dbo.PROMOCION_PRODUCTO pp WHERE pp.id_promocion=p.id_promocion AND pp.id_producto=pr.id_producto)
              OR EXISTS (SELECT 1 FROM dbo.PROMOCION_CATEGORIA pc WHERE pc.id_promocion=p.id_promocion AND pc.id_categoria=pr.id_categoria)
          )
    )
    SELECT TOP 1
        @IdProducto IdProducto,
        id_promocion IdPromocion,
        nombre NombrePromocion,
        tipo_descuento TipoDescuento,
        valor_descuento ValorDescuento,
        PrecioOriginal,
        PrecioPromocional,
        (PrecioOriginal-PrecioPromocional) Ahorro,
        fecha_inicio FechaInicio,
        fecha_fin FechaFin
    FROM PromocionesAplicables
    ORDER BY PrecioPromocional ASC, fecha_fin ASC;
END
