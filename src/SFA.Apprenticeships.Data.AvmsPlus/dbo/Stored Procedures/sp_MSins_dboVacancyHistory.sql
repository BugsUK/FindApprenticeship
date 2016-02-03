create procedure [sp_MSins_dboVacancyHistory]
    @c1 int,
    @c2 int,
    @c3 nvarchar(50),
    @c4 int,
    @c5 int,
    @c6 datetime,
    @c7 nvarchar(4000)
as
begin  
	insert into [dbo].[VacancyHistory](
		[VacancyHistoryId],
		[VacancyId],
		[UserName],
		[VacancyHistoryEventTypeId],
		[VacancyHistoryEventSubTypeId],
		[HistoryDate],
		[Comment]
	) values (
    @c1,
    @c2,
    @c3,
    @c4,
    @c5,
    @c6,
    @c7	) 
end