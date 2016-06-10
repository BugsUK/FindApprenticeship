CREATE PROCEDURE [dbo].[uspGetNASSystemDownMessage] 
AS
BEGIN

	SET NOCOUNT ON

	select	mes.MessageID,
			mes.Sender,
			mes.SenderType,
			mes.Recipient,
			mes.RecipientType,
			mes.MessageDate,
			mes.MessageEventId,
			mes.[Text],
			mes.Title,
			mes.IsRead,
			mes.IsDeleted
	from	[Message] mes
			inner join MessageEvent me
				on me.MessageEventId = mes.MessageEventId
	where	me.CodeName = 'SYS' and Recipient=0
			and mes.Messagedate =	(
										select	Max(mes.MessageDate)
										from	[Message] mes
												inner join MessageEvent me
													on me.MessageEventId = mes.MessageEventId
										where	me.CodeName = 'SYS' and Recipient=0
									)				     
	SET NOCOUNT OFF

END