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

