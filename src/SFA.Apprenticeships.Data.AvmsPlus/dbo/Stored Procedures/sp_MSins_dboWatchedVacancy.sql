create procedure [sp_MSins_dboWatchedVacancy]
    @c1 int,
    @c2 int,
    @c3 int
as
begin  
	insert into [dbo].[WatchedVacancy](
		[WatchedVacancyId],
		[CandidateId],
		[VacancyId]
	) values (
    @c1,
    @c2,
    @c3	) 
end