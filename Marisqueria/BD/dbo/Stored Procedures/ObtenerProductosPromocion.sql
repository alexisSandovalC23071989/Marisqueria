CREATE PROCEDURE [dbo].[ObtenerProductosPromocion] @IdPromocion INT
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH ProductosAplicables AS (
        SELECT pp.id_producto
        FROM dbo.PROMOCION_PRODUCTO pp
        WHERE pp.id_promocion=@IdPromocion
        UNION
        SELECT pr.id_producto
        FROM dbo.PROMOCION_CATEGORIA pc
        INNER JOIN dbo.PRODUCTO pr ON pr.id_categoria=pc.id_categoria
        WHERE pc.id_promocion=@IdPromocion
    )
    SELECT pr.id_producto IdProducto, pr.nombre Nombre, pr.precio_venta_actual PrecioOriginal,
           CAST(CASE WHEN p.tipo_descuento='PORCENTAJE'
                THEN pr.precio_venta_actual * (1 - p.valor_descuento / 100.0)
                ELSE CASE WHEN pr.precio_venta_actual - p.valor_descuento < 0 THEN 0
                          ELSE pr.precio_venta_actual - p.valor_descuento END END AS DECIMAL(18,2)) PrecioPromocional,
           CAST(CASE WHEN EXISTS (
                SELECT 1 FROM dbo.PROMOCION_PRODUCTO pp
                WHERE pp.id_promocion=@IdPromocion AND pp.id_producto=pr.id_producto
           ) THEN 1 ELSE 0 END AS BIT) AsignacionDirecta
    FROM ProductosAplicables pa
    INNER JOIN dbo.PRODUCTO pr ON pr.id_producto=pa.id_producto
    INNER JOIN dbo.PROMOCION p ON p.id_promocion=@IdPromocion
    ORDER BY pr.nombre;
END
