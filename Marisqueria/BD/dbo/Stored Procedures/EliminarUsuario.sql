CREATE PROCEDURE EliminarUsuario
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM RECUPERACION_PASSWORD
        WHERE IdUsuario = @Id;

        DELETE FROM USUARIO
        WHERE Id = @Id;

        COMMIT TRANSACTION;
    END TRY

    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END

        DECLARE @MensajeError NVARCHAR(4000);
        DECLARE @Severidad INT;
        DECLARE @Estado INT;

        SELECT
            @MensajeError = ERROR_MESSAGE(),
            @Severidad = ERROR_SEVERITY(),
            @Estado = ERROR_STATE();

        RAISERROR(
            @MensajeError,
            @Severidad,
            @Estado
        );
    END CATCH
END