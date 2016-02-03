create procedure [sp_MSupd_dboCandidateBroadcastMessage]
		@c1 int = NULL,
		@c2 int = NULL,
		@pkc1 int = NULL,
		@pkc2 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[CandidateBroadcastMessage] set
		[CandidateId] = case substring(@bitmap,1,1) & 1 when 1 then @c1 else [CandidateId] end,
		[MessageId] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [MessageId] end
where [CandidateId] = @pkc1
  and [MessageId] = @pkc2
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end