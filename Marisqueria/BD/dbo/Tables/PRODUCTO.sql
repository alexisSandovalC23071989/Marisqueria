CREATE TABLE [dbo].[PRODUCTO] (
    [id_producto]          INT IDENTITY (1, 1) NOT NULL,
    [id_categoria]         INT                 NOT NULL,
    [nombre]               VARCHAR (100)       NOT NULL,
    [descripcion]          VARCHAR (MAX)       NOT NULL,
    [precio_venta_actual]  DECIMAL (18, 2)     NOT NULL,
    [imagen]               VARCHAR (MAX)       NOT NULL,
    CONSTRAINT [PK_PRODUCTO] PRIMARY KEY CLUSTERED ([id_producto] ASC),
    CONSTRAINT [FK_PRODUCTO_CATEGORIA] FOREIGN KEY ([id_categoria]) REFERENCES [dbo].[CATEGORIA] ([id_categoria])
);
