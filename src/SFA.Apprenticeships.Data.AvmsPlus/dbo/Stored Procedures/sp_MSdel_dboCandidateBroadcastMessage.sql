create procedure [sp_MSdel_dboCandidateBroadcastMessage]
		@pkc1 int,
		@pkc2 int
as
begin  
	delete [dbo].[CandidateBroadcastMessage]
where [CandidateId] = @pkc1
  and [MessageId] = @pkc2
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end