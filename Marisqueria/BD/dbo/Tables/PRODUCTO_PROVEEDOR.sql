CREATE TABLE [dbo].[PRODUCTO_PROVEEDOR] (
    [id_producto]   INT             NOT NULL,
    [id_proveedor]  INT             NOT NULL,
    [precio_compra] DECIMAL (18, 2) NOT NULL,
    [unidad_medida] VARCHAR (50)    NOT NULL,
    CONSTRAINT [PK_PRODUCTO_PROVEEDOR] PRIMARY KEY CLUSTERED ([id_producto] ASC, [id_proveedor] ASC),
    CONSTRAINT [FK_PRODUCTO_PROVEEDOR_PRODUCTO] FOREIGN KEY ([id_producto]) REFERENCES [dbo].[PRODUCTO] ([id_producto]),
    CONSTRAINT [FK_PRODUCTO_PROVEEDOR_PROVEEDOR] FOREIGN KEY ([id_proveedor]) REFERENCES [dbo].[PROVEEDOR] ([id_proveedor])
);
