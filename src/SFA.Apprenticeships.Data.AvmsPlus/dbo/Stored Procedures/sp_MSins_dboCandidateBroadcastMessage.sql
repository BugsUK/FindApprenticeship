create procedure [sp_MSins_dboCandidateBroadcastMessage]
    @c1 int,
    @c2 int
as
begin  
	insert into [dbo].[CandidateBroadcastMessage](
		[CandidateId],
		[MessageId]
	) values (
    @c1,
    @c2	) 
end