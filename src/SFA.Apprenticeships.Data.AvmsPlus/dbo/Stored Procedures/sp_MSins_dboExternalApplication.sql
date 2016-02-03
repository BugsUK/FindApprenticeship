create procedure [sp_MSins_dboExternalApplication]
    @c1 int,
    @c2 int,
    @c3 int,
    @c4 datetime,
    @c5 uniqueidentifier
as
begin  
	insert into [dbo].[ExternalApplication](
		[ExternalApplicationId],
		[CandidateId],
		[VacancyId],
		[ClickthroughDate],
		[ExternalTrackingId]
	) values (
    @c1,
    @c2,
    @c3,
    @c4,
    @c5	) 
end