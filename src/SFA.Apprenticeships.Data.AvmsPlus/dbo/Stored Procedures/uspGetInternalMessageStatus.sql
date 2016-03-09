/*----------------------------------------------------------------------                               
Name  : uspGetInternalMessageStatus                  
Description :  returns internal message status 

                
History:                  
--------                  
Date			Version		Author		Comment
07/03/2011		1.0			D. Kraevoy	first version
---------------------------------------------------------------------- */				

CREATE procedure [dbo].[uspGetInternalMessageStatus] (
	@messageId uniqueidentifier
)
as
begin

SET NOCOUNT ON  

select MessageId,
	   MessageStatusId,
	   Request,	 
	   Response, 
	   ReceivedDate, 
	   ProcessedDate
from dbo.InternalMessages
where MessageId = @messageId;

end