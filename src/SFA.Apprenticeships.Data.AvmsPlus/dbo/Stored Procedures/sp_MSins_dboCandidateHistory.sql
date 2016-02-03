create procedure [sp_MSins_dboCandidateHistory]
    @c1 int,
    @c2 int,
    @c3 int,
    @c4 int,
    @c5 datetime,
    @c6 nvarchar(4000),
    @c7 nvarchar(50)
as
begin  
	insert into [dbo].[CandidateHistory](
		[CandidateHistoryId],
		[CandidateId],
		[CandidateHistoryEventTypeId],
		[CandidateHistorySubEventTypeId],
		[EventDate],
		[Comment],
		[UserName]
	) values (
    @c1,
    @c2,
    @c3,
    @c4,
    @c5,
    @c6,
    @c7	) 
end