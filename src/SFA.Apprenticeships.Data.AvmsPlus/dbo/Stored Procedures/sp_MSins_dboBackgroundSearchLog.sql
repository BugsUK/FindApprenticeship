create procedure [sp_MSins_dboBackgroundSearchLog]
    @c1 int,
    @c2 datetime,
    @c3 int,
    @c4 int,
    @c5 int
as
begin  
	insert into [dbo].[BackgroundSearchLog](
		[BackgroundSearchLogId],
		[Date],
		[NumberOfVacancies],
		[NumberOfCandidatesProcessed],
		[NumberOfFailures]
	) values (
    @c1,
    @c2,
    @c3,
    @c4,
    @c5	) 
end