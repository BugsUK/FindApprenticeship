create procedure [sp_MSupd_dboMessage]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 int = NULL,
		@c4 int = NULL,
		@c5 int = NULL,
		@c6 datetime = NULL,
		@c7 int = NULL,
		@c8 nvarchar(max) = NULL,
		@c9 nvarchar(1000) = NULL,
		@c10 bit = NULL,
		@c11 bit = NULL,
		@c12 int = NULL,
		@c13 datetime = NULL,
		@c14 nvarchar(250) = NULL,
		@c15 nvarchar(250) = NULL,
		@c16 datetime = NULL,
		@pkc1 int = NULL,
		@bitmap binary(2)
as
begin  
update [dbo].[Message] set
		[Sender] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [Sender] end,
		[SenderType] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [SenderType] end,
		[Recipient] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [Recipient] end,
		[RecipientType] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [RecipientType] end,
		[MessageDate] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [MessageDate] end,
		[MessageEventId] = case substring(@bitmap,1,1) & 64 when 64 then @c7 else [MessageEventId] end,
		[Text] = case substring(@bitmap,1,1) & 128 when 128 then @c8 else [Text] end,
		[Title] = case substring(@bitmap,2,1) & 1 when 1 then @c9 else [Title] end,
		[IsRead] = case substring(@bitmap,2,1) & 2 when 2 then @c10 else [IsRead] end,
		[IsDeleted] = case substring(@bitmap,2,1) & 4 when 4 then @c11 else [IsDeleted] end,
		[MessageCategoryID] = case substring(@bitmap,2,1) & 8 when 8 then @c12 else [MessageCategoryID] end,
		[ReadDate] = case substring(@bitmap,2,1) & 16 when 16 then @c13 else [ReadDate] end,
		[DeletedBy] = case substring(@bitmap,2,1) & 32 when 32 then @c14 else [DeletedBy] end,
		[ReadByFirst] = case substring(@bitmap,2,1) & 64 when 64 then @c15 else [ReadByFirst] end,
		[DeletedDate] = case substring(@bitmap,2,1) & 128 when 128 then @c16 else [DeletedDate] end
where [MessageId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end