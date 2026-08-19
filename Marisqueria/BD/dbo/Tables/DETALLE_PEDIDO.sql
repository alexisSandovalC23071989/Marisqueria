CREATE TABLE [dbo].[DETALLE_PEDIDO] (
    [IdDetalle]      UNIQUEIDENTIFIER NOT NULL,
    [IdPedido]       UNIQUEIDENTIFIER NOT NULL,
    [id_producto]    INT              NOT NULL,
    [Cantidad]       INT              NOT NULL,
    [PrecioUnitario] DECIMAL (10, 2)  NOT NULL,
    [Subtotal]       DECIMAL (10, 2)  NOT NULL,
    [PrecioOriginal] DECIMAL (10, 2)  CONSTRAINT [DF_DETALLE_PrecioOriginal] DEFAULT ((0)) NOT NULL,
    [TipoDescuento]  VARCHAR (12)     CONSTRAINT [DF_DETALLE_TipoDescuento] DEFAULT ('') NOT NULL,
    [ValorDescuento] DECIMAL (18, 2)  CONSTRAINT [DF_DETALLE_ValorDescuento] DEFAULT ((0)) NOT NULL,
    [IdPromocion] INT NULL, 
    PRIMARY KEY CLUSTERED ([IdDetalle] ASC)
);



