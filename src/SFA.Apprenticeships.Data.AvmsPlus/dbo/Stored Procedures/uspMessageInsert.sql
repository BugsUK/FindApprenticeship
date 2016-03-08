CREATE PROCEDURE [dbo].[uspMessageInsert]
@sender             INT,
@senderType         INT,
@recipient          INT,
@recipientType      INT,
@messageDate        DATETIME,
@messageEventId     INT,
@text               NVARCHAR(MAX),
@title              NVARCHAR(1000),
@messageCategoryId  INT,

@messageId INT OUT    
AS    
BEGIN    
    SET NOCOUNT ON

    BEGIN TRY
        INSERT INTO [dbo].[Message]
        (
            [Sender],
            [SenderType],
            [Recipient],
            [RecipientType],
            [MessageDate],
            [MessageEventId],
            [Text],
            [Title],
            [MessageCategoryId]
        )    
        VALUES 
        (  
            @sender,
            @senderType,
            @recipient,
            @recipientType,
            @messageDate,
            @messageEventId,
            @text,
            @title,
            @messageCategoryId
        )

        SET @MessageId = SCOPE_IDENTITY()
    END TRY    
    BEGIN CATCH    
        EXEC RethrowError;    
    END CATCH    

    SET NOCOUNT OFF    
END