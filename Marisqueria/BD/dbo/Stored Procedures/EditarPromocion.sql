CREATE PROCEDURE [dbo].[EditarPromocion]
    @IdPromocion INT, @Nombre VARCHAR(120), @Descripcion VARCHAR(500), @TipoDescuento VARCHAR(12),
    @ValorDescuento DECIMAL(18,2), @FechaInicio DATETIME2, @FechaFin DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.PROMOCION SET nombre=@Nombre, descripcion=@Descripcion, tipo_descuento=@TipoDescuento,
        valor_descuento=@ValorDescuento, fecha_inicio=@FechaInicio, fecha_fin=@FechaFin
    WHERE id_promocion=@IdPromocion;
    SELECT @IdPromocion;
END
