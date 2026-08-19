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
