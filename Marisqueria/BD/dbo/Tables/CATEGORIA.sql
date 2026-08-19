CREATE TABLE [dbo].[CATEGORIA] (
    [id_categoria] INT IDENTITY (1, 1) NOT NULL,
    [nombre]       VARCHAR (100)      NOT NULL,
    CONSTRAINT [PK_CATEGORIA] PRIMARY KEY CLUSTERED ([id_categoria] ASC)
);
