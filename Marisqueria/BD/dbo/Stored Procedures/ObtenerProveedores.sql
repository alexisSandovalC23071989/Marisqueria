CREATE PROCEDURE [dbo].[ObtenerProveedores]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id_proveedor AS IdProveedor,
        empresa AS Empresa,
        cedula_juridica AS CedulaJuridica,
        contacto AS Contacto,
        telefono AS Telefono,
        correo AS Correo,
        direccion AS Direccion,
        estado AS Estado
    FROM dbo.PROVEEDOR
    ORDER BY empresa;
END
