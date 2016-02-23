CREATE PROCEDURE [dbo].[uspMessageUpdateDeleteStatus]
    @messageId int,
    @deletedBy NVARCHAR(250)
AS
BEGIN
SET NOCOUNT ON

    BEGIN TRY
        BEGIN
            DECLARE @currentDeletedBy  NVARCHAR(250)
            SELECT @currentDeletedBy = DeletedBy FROM [Message] WHERE MessageId = @messageId

            IF @currentDeletedBy IS NULL
            BEGIN
                UPDATE
                    [Message]
                Set
                    IsDeleted = 1,
                    DeletedBy = @deletedBy,
                    DeletedDate = GETDATE()
                Where
                    MessageId = @messageId
            END
            -- ELSE Do nothing, everything is already set
        END
    END TRY
    BEGIN CATCH
        EXEC RethrowError;
    END CATCH
SET NOCOUNT OFF
END