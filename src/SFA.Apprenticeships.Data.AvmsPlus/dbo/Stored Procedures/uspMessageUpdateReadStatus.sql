CREATE PROCEDURE [dbo].[uspMessageUpdateReadStatus]    
    @messageId int,
    @readByFirst NVARCHAR(250)
AS
BEGIN
SET NOCOUNT ON

    BEGIN TRY
        BEGIN
            DECLARE @currentReadBy  NVARCHAR(250)
            SELECT @currentReadBy = ReadByFirst FROM [Message] WHERE MessageId = @messageId

            IF @currentReadBy IS NULL
            BEGIN
                UPDATE
                    [Message]
                Set
                    IsRead = 1,
                    ReadByFirst = @readByFirst,
                    ReadDate = GETDATE()
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