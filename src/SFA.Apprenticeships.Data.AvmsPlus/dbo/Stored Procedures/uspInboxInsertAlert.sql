CREATE PROCEDURE [dbo].[uspInboxInsertAlert]
        @MessageId	as	int,
        @From	as	int,
        @FromType	as	smallint,
        @To	as	int,
        @ToType	as	smallint,
        @Date as		datetime,
        @Event as 		smallint,
        @Text as		nvarchar
AS
BEGIN
	SET NOCOUNT ON
	
	BEGIN TRY
        Insert Into Message 
		(
-- Alex Crockett
-- Fix 15/08/09
-- Added the last 2 not nullable fields
-- and marked all other changes with AJC.
			Sender,
			SenderType,
			Recipient,
			RecipientType,
			MessageDate,
			MessageEventId,
			[Text],
			Title, --AJC
			IsRead, --AJC
			IsDeleted --AJC
		)
        values
        (
       -- @MessageId	,
        @From	,
        @FromType	,
        @To	,
        @ToType	,
        @Date ,
        @Event ,
        @Text,
		NULL, --AJC
		0, --AJC
		0 --AJC
        )
	SET @MessageId = SCOPE_IDENTITY()
   
    END TRY
  
    BEGIN CATCH
		EXEC RethrowError;
	END CATCH
    
    SET NOCOUNT OFF
END