CREATE PROCEDURE [dbo].[EditarProveedor]
    @IdProveedor INT,
    @Empresa VARCHAR(150),
    @CedulaJuridica VARCHAR(20),
    @Contacto VARCHAR(100),
    @Telefono VARCHAR(20),
    @Correo VARCHAR(150),
    @Direccion VARCHAR(MAX),
    @Estado VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.PROVEEDOR
    SET
        empresa = @Empresa,
        cedula_juridica = @CedulaJuridica,
        contacto = @Contacto,
        telefono = @Telefono,
        correo = @Correo,
        direccion = @Direccion,
        estado = @Estado
    WHERE id_proveedor = @IdProveedor;

    SELECT @IdProveedor AS IdProveedor;
END
