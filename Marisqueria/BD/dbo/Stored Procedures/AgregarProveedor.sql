CREATE PROCEDURE [dbo].[AgregarProveedor]
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

    INSERT INTO dbo.PROVEEDOR
    (
        empresa,
        cedula_juridica,
        contacto,
        telefono,
        correo,
        direccion,
        estado
    )
    VALUES
    (
        @Empresa,
        @CedulaJuridica,
        @Contacto,
        @Telefono,
        @Correo,
        @Direccion,
        @Estado
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdProveedor;
END
