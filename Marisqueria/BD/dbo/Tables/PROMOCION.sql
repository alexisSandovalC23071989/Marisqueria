CREATE TABLE [dbo].[PROMOCION] (
    [id_promocion]        INT             IDENTITY (1, 1) NOT NULL,
    [nombre]              VARCHAR (120)   NOT NULL,
    [descripcion]         VARCHAR (500)   NULL,
    [tipo_descuento]      VARCHAR (12)    NOT NULL,
    [valor_descuento]     DECIMAL (18, 2) NOT NULL,
    [fecha_inicio]        DATETIME2 (7)   NOT NULL,
    [fecha_fin]           DATETIME2 (7)   NOT NULL,
    [activa]              BIT             CONSTRAINT [DF_PROMOCION_activa] DEFAULT ((1)) NOT NULL,
    [fecha_desactivacion] DATETIME2 (7)   NULL,
    [fecha_creacion]      DATETIME2 (7)   CONSTRAINT [DF_PROMOCION_fecha_creacion] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK_PROMOCION] PRIMARY KEY CLUSTERED ([id_promocion] ASC),
    CONSTRAINT [CK_PROMOCION_fechas] CHECK ([fecha_fin]>=[fecha_inicio]),
    CONSTRAINT [CK_PROMOCION_porcentaje] CHECK ([tipo_descuento]<>'PORCENTAJE' OR [valor_descuento]<=(100)),
    CONSTRAINT [CK_PROMOCION_tipo] CHECK ([tipo_descuento]='MONTO' OR [tipo_descuento]='PORCENTAJE'),
    CONSTRAINT [CK_PROMOCION_valor] CHECK ([valor_descuento]>(0))
);

