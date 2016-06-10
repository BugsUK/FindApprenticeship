CREATE PROCEDURE [dbo].[uspUpdateNASSystemDownMessage] 
(
	@text varchar(256),
	@title varchar(1000),
	@IsRead bit
)
AS
BEGIN

	SET NOCOUNT ON

	declare @count int
	declare @codeName char(3)
	declare @support int
	declare @supportType int
	declare @messageEventId int

	set @codeName = 'SYS'
	set @support = 0
	set @supportType = 3
	set @messageEventId = (select MessageEventId from MessageEvent where UPPER(CodeName) = @codeName)

	select	@count = count(*) 
	from	[message] mes
			inner join MessageEvent me
				on me.messageeventid = mes.messageeventid
	where	me.CodeName = 'SYS' and Recipient=0

	if (@count = 0)
		insert	into [message]
				(
					Sender,
					SenderType,
					Recipient,
					RecipientType,
					MessageDate,
					MessageEventId,
					[Text],
					Title,
					IsRead,
					IsDeleted
				)
		values	(
					@support,
					@supportType,
					@support,
					@supportType,
					getdate(),
					@messageEventId,
					@text,
					@title,
					@IsRead,
					0
				)
	else
		update [message]
		set			
				MessageDate = getdate(),
				[Text] = @text,
				Title = @title,
				IsRead = @IsRead
		from	[message] mes
				inner join MessageEvent me
					on me.messageeventid = mes.messageeventid
		where	me.CodeName = 'SYS'  and Recipient=0
				and mes.Messagedate =	(
											select	Max(mes.MessageDate)
											from	[Message] mes
													inner join MessageEvent me
														on me.messageeventid = mes.messageeventid
											where	me.CodeName = 'SYS' and Recipient=0
										)					
	SET NOCOUNT OFF

END