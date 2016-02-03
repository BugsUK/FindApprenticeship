create procedure [sp_MSins_dboVacancyOwnerRelationshipHistory]
    @c1 int,
    @c2 int,
    @c3 nvarchar(50),
    @c4 datetime,
    @c5 int,
    @c6 int,
    @c7 varchar(4000)
as
begin  
	insert into [dbo].[VacancyOwnerRelationshipHistory](
		[VacancyOwnerRelationshipHistoryId],
		[VacancyOwnerRelationshipId],
		[UserName],
		[Date],
		[EventTypeId],
		[EventSubTypeId],
		[Comments]
	) values (
    @c1,
    @c2,
    @c3,
    @c4,
    @c5,
    @c6,
    @c7	) 
end