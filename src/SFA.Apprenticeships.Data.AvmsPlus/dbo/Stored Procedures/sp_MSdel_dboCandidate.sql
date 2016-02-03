create procedure [sp_MSdel_dboCandidate]
		@pkc1 int
as
begin  
	delete [dbo].[Candidate]
where [CandidateId] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end