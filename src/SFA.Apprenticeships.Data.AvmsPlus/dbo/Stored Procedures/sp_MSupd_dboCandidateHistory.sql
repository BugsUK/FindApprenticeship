create procedure [sp_MSupd_dboCandidateHistory]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 int = NULL,
		@c4 int = NULL,
		@c5 datetime = NULL,
		@c6 nvarchar(4000) = NULL,
		@c7 nvarchar(50) = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[CandidateHistory] set
		[CandidateId] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [CandidateId] end,
		[CandidateHistoryEventTypeId] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [CandidateHistoryEventTypeId] end,
		[CandidateHistorySubEventTypeId] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [CandidateHistorySubEventTypeId] end,
		[EventDate] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [EventDate] end,
		[Comment] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [Comment] end,
		[UserName] = case substring(@bitmap,1,1) & 64 when 64 then @c7 else [UserName] end
where [CandidateHistoryId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end