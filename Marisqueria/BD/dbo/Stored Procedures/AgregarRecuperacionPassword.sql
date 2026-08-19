CREATE PROCEDURE AgregarRecuperacionPassword
    @IdUsuario UNIQUEIDENTIFIER,
    @Token VARCHAR(255),
    @FechaCreacion DATETIME,
    @FechaExpiracion DATETIME,
    @Utilizado BIT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Id UNIQUEIDENTIFIER = NEWID();

    INSERT INTO RECUPERACION_PASSWORD
    (
        Id,
        IdUsuario,
        Token,
        FechaCreacion,
        FechaExpiracion,
        Utilizado
    )
    VALUES
    (
        @Id,
        @IdUsuario,
        @Token,
        @FechaCreacion,
        @FechaExpiracion,
        @Utilizado
    );

    SELECT @Id;
END