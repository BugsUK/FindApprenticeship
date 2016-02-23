/*----------------------------------------------------------------------                               
Name  : uspSetInternalMessageStatus                  
Description :  updates internal message status 

                
History:                  
--------                  
Date			Version		Author		Comment
07/03/2011		1.0			D. Kraevoy	first version
---------------------------------------------------------------------- */				

create procedure [dbo].[uspSetInternalMessageStatus] (
	@messageId uniqueidentifier,
	@request text,
	@response text,
	@messageStatusId int
)
as
begin

SET NOCOUNT ON  

update dbo.InternalMessages
set Request = @request,
	Response = @response,
	MessageStatusId = @messageStatusId, 
	ProcessedDate = GETDATE()
where MessageId = @messageId;

if 0 = @@ROWCOUNT 
begin
	insert into dbo.InternalMessages( MessageId,
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