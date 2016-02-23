/*----------------------------------------------------------------------                               
Name  : uspSetExternalMessageStatus                  
Description :  updates external message status 

                
History:                  
--------                  
Date			Version		Author		Comment
07/03/2011		1.0			D. Kraevoy	first version
---------------------------------------------------------------------- */				

create procedure [dbo].[uspSetExternalMessageStatus] (
	@messageId uniqueidentifier,
	@request text,
	@response text,
	@messageStatusId int
)
as
begin

SET NOCOUNT ON  

update dbo.ExternalMessages
set Request = @request,
	Response = @response,
	MessageStatusId = @messageStatusId, 
	ProcessedDate = GETDATE()
where MessageId = @messageId;

if 0 = @@ROWCOUNT 
begin
	insert into dbo.ExternalMessages( MessageId,
									  ReceivedDate,
									  ProcessedDate,
									  Request,
									  Response,
									  MessageStatusId ) 
    values( @messageId,
			GETDATE(),
			GETDATE(), 									  
			@request,
			@response,
			@messageStatusId )
end

end