-- =====================================================================
-- BASE DE DATOS: MARISQUERIA
-- =====================================================================
USE Marisqueria;
GO


-- =====================================================================
-- SECCION 1: TABLAS - CATALOGO DE PRODUCTOS
-- =====================================================================

CREATE TABLE [dbo].[CATEGORIA] (
    [id_categoria] INT IDENTITY (1, 1) NOT NULL,
    [nombre]       VARCHAR (100)      NOT NULL,
    CONSTRAINT [PK_CATEGORIA] PRIMARY KEY CLUSTERED ([id_categoria] ASC)
);
GO

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
GO


-- =====================================================================
-- SECCION 2: TABLAS - PROVEEDORES
-- =====================================================================

CREATE TABLE [dbo].[PROVEEDOR] (
    [id_proveedor]    INT IDENTITY (1, 1) NOT NULL,
    [empresa]         VARCHAR (150)       NOT NULL,
    [cedula_juridica] VARCHAR (20)        NOT NULL,
    [contacto]        VARCHAR (100)       NOT NULL,
    [telefono]        VARCHAR (20)        NOT NULL,
    [correo]          VARCHAR (150)       NOT NULL,
    [direccion]       VARCHAR (MAX)       NOT NULL,
    [estado]          VARCHAR (20)        NOT NULL,
    CONSTRAINT [PK_PROVEEDOR] PRIMARY KEY CLUSTERED ([id_proveedor] ASC),
    CONSTRAINT [UQ_PROVEEDOR_CEDULA_JURIDICA] UNIQUE ([cedula_juridica])
);
GO

-- Relacion "suministra": que productos vende cada proveedor, a que precio y en que unidad
CREATE TABLE [dbo].[PRODUCTO_PROVEEDOR] (
    [id_producto]   INT             NOT NULL,
    [id_proveedor]  INT             NOT NULL,
    [precio_compra] DECIMAL (18, 2) NOT NULL,
    [unidad_medida] VARCHAR (50)    NOT NULL,
    CONSTRAINT [PK_PRODUCTO_PROVEEDOR] PRIMARY KEY CLUSTERED ([id_producto] ASC, [id_proveedor] ASC),
    CONSTRAINT [FK_PRODUCTO_PROVEEDOR_PRODUCTO] FOREIGN KEY ([id_producto]) REFERENCES [dbo].[PRODUCTO] ([id_producto]),
    CONSTRAINT [FK_PRODUCTO_PROVEEDOR_PROVEEDOR] FOREIGN KEY ([id_proveedor]) REFERENCES [dbo].[PROVEEDOR] ([id_proveedor])
);
GO


-- =====================================================================
-- SECCION 3: TABLAS - USUARIOS Y AUTENTICACION
-- =====================================================================

CREATE TABLE [dbo].[ROL] (
    [Id]     UNIQUEIDENTIFIER NOT NULL,
    [Nombre] VARCHAR (50)     NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE TABLE [dbo].[USUARIO] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [IdRol]          UNIQUEIDENTIFIER NOT NULL,
    [Nombre]         VARCHAR (100)    NOT NULL,
    [Correo]         VARCHAR (150)    NOT NULL,
    [ContrasenaHash] VARCHAR (MAX)    NOT NULL,
    [Telefono]       VARCHAR (20)     NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_USUARIO_ROL] FOREIGN KEY ([IdRol]) REFERENCES [dbo].[ROL] ([Id])
);
GO

CREATE TABLE [dbo].[RECUPERACION_PASSWORD] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [IdUsuario]       UNIQUEIDENTIFIER NOT NULL,
    [Token]           VARCHAR (250)    NOT NULL,
    [FechaCreacion]   DATETIME         NOT NULL,
    [FechaExpiracion] DATETIME         NOT NULL,
    [Utilizado]       BIT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RECUPERACION_PASSWORD_USUARIO] FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[USUARIO] ([Id])
);
GO


-- =====================================================================
-- SECCION 4: STORED PROCEDURES - CATEGORIAS
-- =====================================================================

CREATE PROCEDURE [dbo].[ObtenerCategorias]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id_categoria AS IdCategoria,
        nombre AS Nombre
    FROM dbo.CATEGORIA
    ORDER BY nombre;
END
GO


-- =====================================================================
-- SECCION 5: STORED PROCEDURES - PRODUCTOS
-- =====================================================================

CREATE PROCEDURE [dbo].[ObtenerProductos]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        producto.id_producto AS IdProducto,
        producto.id_categoria AS IdCategoria,
        producto.nombre AS Nombre,
        producto.descripcion AS Descripcion,
        producto.precio_venta_actual AS PrecioVentaActual,
        producto.imagen AS Imagen,
        categoria.nombre AS Categoria
    FROM dbo.PRODUCTO producto
        INNER JOIN dbo.CATEGORIA categoria
            ON producto.id_categoria = categoria.id_categoria
    ORDER BY producto.nombre;
END
GO

CREATE PROCEDURE [dbo].[ObtenerProducto]
    @IdProducto INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        producto.id_producto AS IdProducto,
        producto.id_categoria AS IdCategoria,
        producto.nombre AS Nombre,
        producto.descripcion AS Descripcion,
        producto.precio_venta_actual AS PrecioVentaActual,
        producto.imagen AS Imagen,
        categoria.nombre AS Categoria
    FROM dbo.PRODUCTO producto
        INNER JOIN dbo.CATEGORIA categoria
            ON producto.id_categoria = categoria.id_categoria
    WHERE producto.id_producto = @IdProducto;
END
GO

CREATE PROCEDURE [dbo].[AgregarProducto]
    @IdCategoria INT,
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(MAX),
    @PrecioVentaActual DECIMAL(18, 2),
    @Imagen VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.PRODUCTO
    (
        id_categoria,
        nombre,
        descripcion,
        precio_venta_actual,
        imagen
    )
    VALUES
    (
        @IdCategoria,
        @Nombre,
        @Descripcion,
        @PrecioVentaActual,
        @Imagen
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdProducto;
END
GO

CREATE PROCEDURE [dbo].[EditarProducto]
    @IdProducto INT,
    @IdCategoria INT,
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(MAX),
    @PrecioVentaActual DECIMAL(18, 2),
    @Imagen VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.PRODUCTO
    SET
        id_categoria = @IdCategoria,
        nombre = @Nombre,
        descripcion = @Descripcion,
        precio_venta_actual = @PrecioVentaActual,
        imagen = @Imagen
    WHERE id_producto = @IdProducto;

    SELECT @IdProducto AS IdProducto;
END
GO

CREATE PROCEDURE [dbo].[EliminarProducto]
    @IdProducto INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.PRODUCTO
    WHERE id_producto = @IdProducto;

    SELECT @IdProducto AS IdProducto;
END
GO


-- =====================================================================
-- SECCION 6: STORED PROCEDURES - PROVEEDORES
-- =====================================================================

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
GO

CREATE PROCEDURE [dbo].[ObtenerProveedor]
    @IdProveedor INT
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
    WHERE id_proveedor = @IdProveedor;
END
GO

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
GO

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
GO

CREATE PROCEDURE [dbo].[EliminarProveedor]
    @IdProveedor INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Se eliminan primero las relaciones en PRODUCTO_PROVEEDOR para respetar la integridad referencial
    DELETE FROM dbo.PRODUCTO_PROVEEDOR
    WHERE id_proveedor = @IdProveedor;

    DELETE FROM dbo.PROVEEDOR
    WHERE id_proveedor = @IdProveedor;

    SELECT @IdProveedor AS IdProveedor;
END
GO


-- =====================================================================
-- SECCION 7: STORED PROCEDURES - PRODUCTO_PROVEEDOR (relacion "suministra")
-- =====================================================================

CREATE PROCEDURE [dbo].[ObtenerProductosPorProveedor]
    @IdProveedor INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        productoProveedor.id_producto AS IdProducto,
        productoProveedor.id_proveedor AS IdProveedor,
        productoProveedor.precio_compra AS PrecioCompra,
        productoProveedor.unidad_medida AS UnidadMedida,
        producto.nombre AS Producto
    FROM dbo.PRODUCTO_PROVEEDOR productoProveedor
        INNER JOIN dbo.PRODUCTO producto
            ON productoProveedor.id_producto = producto.id_producto
    WHERE productoProveedor.id_proveedor = @IdProveedor
    ORDER BY producto.nombre;
END
GO

CREATE PROCEDURE [dbo].[AgregarProductoProveedor]
    @IdProveedor INT,
    @IdProducto INT,
    @PrecioCompra DECIMAL(18, 2),
    @UnidadMedida VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.PRODUCTO_PROVEEDOR
    (
        id_producto,
        id_proveedor,
        precio_compra,
        unidad_medida
    )
    VALUES
    (
        @IdProducto,
        @IdProveedor,
        @PrecioCompra,
        @UnidadMedida
    );

    SELECT @IdProducto AS IdProducto;
END
GO

CREATE PROCEDURE [dbo].[EditarProductoProveedor]
    @IdProveedor INT,
    @IdProducto INT,
    @PrecioCompra DECIMAL(18, 2),
    @UnidadMedida VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.PRODUCTO_PROVEEDOR
    SET
        precio_compra = @PrecioCompra,
        unidad_medida = @UnidadMedida
    WHERE id_producto = @IdProducto
        AND id_proveedor = @IdProveedor;

    SELECT @IdProducto AS IdProducto;
END
GO

CREATE PROCEDURE [dbo].[EliminarProductoProveedor]
    @IdProveedor INT,
    @IdProducto INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.PRODUCTO_PROVEEDOR
    WHERE id_producto = @IdProducto
        AND id_proveedor = @IdProveedor;

    SELECT @IdProducto AS IdProducto;
END
GO