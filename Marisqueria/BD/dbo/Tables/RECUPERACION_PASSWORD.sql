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

