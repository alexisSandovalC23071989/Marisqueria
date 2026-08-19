CREATE TABLE [dbo].[PROMOCION_ALERTA] (
    [id_alerta]          INT           IDENTITY (1, 1) NOT NULL,
    [id_promocion]       INT           NOT NULL,
    [fecha_generacion]   DATETIME2 (7) CONSTRAINT [DF_PROMOCION_ALERTA_fecha_generacion] DEFAULT (sysdatetime()) NOT NULL,
    [revisada]           BIT           CONSTRAINT [DF_PROMOCION_ALERTA_revisada] DEFAULT ((0)) NOT NULL,
    [fecha_revision]     DATETIME2 (7) NULL,
    CONSTRAINT [PK_PROMOCION_ALERTA] PRIMARY KEY CLUSTERED ([id_alerta] ASC),
    CONSTRAINT [UQ_PROMOCION_ALERTA_promocion] UNIQUE ([id_promocion]),
    CONSTRAINT [FK_PROMOCION_ALERTA_PROMOCION] FOREIGN KEY ([id_promocion]) REFERENCES [dbo].[PROMOCION] ([id_promocion])
);
