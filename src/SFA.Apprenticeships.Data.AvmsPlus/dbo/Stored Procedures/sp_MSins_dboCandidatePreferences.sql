create procedure [sp_MSins_dboCandidatePreferences]
    @c1 int,
    @c2 int,
    @c3 int,
    @c4 int,
    @c5 int,
    @c6 int
as
begin  
	insert into [dbo].[CandidatePreferences](
		[CandidatePreferenceId],
		[CandidateId],
		[FirstFrameworkId],
		[FirstOccupationId],
		[SecondFrameworkId],
		[SecondOccupationId]
	) values (
    @c1,
    @c2,
    @c3,
    @c4,
    @c5,
    @c6	) 
end