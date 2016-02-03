create procedure [sp_MSupd_dboApplicationHistory]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 nvarchar(50) = NULL,
		@c4 datetime = NULL,
		@c5 int = NULL,
		@c6 int = NULL,
		@c7 nvarchar(4000) = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[ApplicationHistory] set
		[ApplicationId] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [ApplicationId] end,
		[UserName] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [UserName] end,
		[ApplicationHistoryEventDate] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [ApplicationHistoryEventDate] end,
		[ApplicationHistoryEventTypeId] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [ApplicationHistoryEventTypeId] end,
		[ApplicationHistoryEventSubTypeId] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [ApplicationHistoryEventSubTypeId] end,
		[Comment] = case substring(@bitmap,1,1) & 64 when 64 then @c7 else [Comment] end
where [ApplicationHistoryId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end