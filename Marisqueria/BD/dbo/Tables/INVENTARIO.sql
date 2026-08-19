CREATE TABLE [dbo].[INVENTARIO] (
    [id_inventario]       INT             IDENTITY (1, 1) NOT NULL,
    [id_producto]         INT                             NOT NULL,
    [id_proveedor]        INT                             NOT NULL,
    [lote]                VARCHAR (100)                   NOT NULL,
    [cantidad_disponible] DECIMAL (18, 2)                 NOT NULL,
    [fecha_ingreso]       DATETIME2 (0)                   NOT NULL,
    [fecha_vencimiento]   DATE                            NOT NULL,
    [ubicacion]           VARCHAR (150)                   NOT NULL,
    CONSTRAINT [PK_INVENTARIO] PRIMARY KEY CLUSTERED ([id_inventario] ASC),
    CONSTRAINT [FK_INVENTARIO_PRODUCTO] FOREIGN KEY ([id_producto]) REFERENCES [dbo].[PRODUCTO] ([id_producto]),
    CONSTRAINT [FK_INVENTARIO_PROVEEDOR] FOREIGN KEY ([id_proveedor]) REFERENCES [dbo].[PROVEEDOR] ([id_proveedor]),
    CONSTRAINT [CK_INVENTARIO_CANTIDAD] CHECK ([cantidad_disponible] >= 0),
    CONSTRAINT [CK_INVENTARIO_FECHAS] CHECK ([fecha_vencimiento] >= CONVERT(DATE, [fecha_ingreso]))
);
