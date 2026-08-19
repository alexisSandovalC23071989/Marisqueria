CREATE TABLE [dbo].[RESENA] (
    [id_resena]    INT              IDENTITY (1, 1) NOT NULL,
    [id_usuario]   UNIQUEIDENTIFIER NOT NULL,
    [id_producto]  INT              NOT NULL,
    [calificacion] INT              NOT NULL,
    [comentario]   VARCHAR (MAX)    NOT NULL,
    [fecha]        DATETIME         DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_RESENA] PRIMARY KEY CLUSTERED ([id_resena] ASC),
    CONSTRAINT [CK_RESENA_CALIFICACION] CHECK ([calificacion]>=(1) AND [calificacion]<=(5)),
    CONSTRAINT [FK_RESENA_PRODUCTO] FOREIGN KEY ([id_producto]) REFERENCES [dbo].[PRODUCTO] ([id_producto]),
    CONSTRAINT [FK_RESENA_USUARIO] FOREIGN KEY ([id_usuario]) REFERENCES [dbo].[USUARIO] ([Id])
);

