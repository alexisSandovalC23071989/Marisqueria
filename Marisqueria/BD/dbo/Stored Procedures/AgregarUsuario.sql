CREATE   PROCEDURE AgregarUsuario
    @Nombre VARCHAR(100),
    @Correo VARCHAR(150),
    @ContrasenaHash VARCHAR(MAX),
    @Telefono VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Id UNIQUEIDENTIFIER = NEWID();
    DECLARE @IdRol UNIQUEIDENTIFIER;

    SELECT @IdRol = Id
    FROM dbo.ROL
    WHERE Nombre = 'Cliente';

    IF @IdRol IS NULL
    BEGIN
        THROW 50001, 'No existe el rol Cliente en la base de datos.', 1;
    END;

    INSERT INTO dbo.USUARIO
    (
        Id,
        IdRol,
        Nombre,
        Correo,
        ContrasenaHash,
        Telefono
    )
    VALUES
    (
        @Id,
        @IdRol,
        @Nombre,
        @Correo,
        @ContrasenaHash,
        @Telefono
    );

    SELECT @Id;
END