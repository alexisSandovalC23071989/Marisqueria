CREATE PROCEDURE [dbo].[AgregarPromocion]
    @Nombre VARCHAR(120), @Descripcion VARCHAR(500), @TipoDescuento VARCHAR(12),
    @ValorDescuento DECIMAL(18,2), @FechaInicio DATETIME2, @FechaFin DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.PROMOCION(nombre, descripcion, tipo_descuento, valor_descuento, fecha_inicio, fecha_fin)
    VALUES(@Nombre, @Descripcion, @TipoDescuento, @ValorDescuento, @FechaInicio, @FechaFin);
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
