create procedure [sp_MSins_dboSubVacancy]
    @c1 int,
    @c2 int,
    @c3 int,
    @c4 datetime,
    @c5 nchar(12)
as
begin  
	insert into [dbo].[SubVacancy](
		[SubVacancyId],
		[VacancyId],
		[AllocatedApplicationId],
		[StartDate],
		[ILRNumber]
	) values (
    @c1,
    @c2,
    @c3,
    @c4,
    @c5	) 
end