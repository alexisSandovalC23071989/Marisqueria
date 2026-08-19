CREATE PROCEDURE [dbo].[ObtenerPedidos]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        P.IdPedido,
        P.IdUsuario,
        ISNULL(U.Nombre, 'Usuario no disponible') AS NombreUsuario,
        P.FechaPedido,
        P.Estado,
        P.Total

    FROM dbo.PEDIDO P

    LEFT JOIN dbo.USUARIO U
        ON P.IdUsuario = U.Id

    ORDER BY P.FechaPedido DESC;
END
