/*----------------------------------------------------------------------                               
Name  : uspGetExternalMessageStatus                  
Description :  returns external message status 

                
History:                  
--------                  
Date			Version		Author		Comment
07/03/2011		1.0			D. Kraevoy	first version
---------------------------------------------------------------------- */				

create procedure [dbo].[uspGetExternalMessageStatus] (
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
from dbo.ExternalMessages
where MessageId = @messageId;

end