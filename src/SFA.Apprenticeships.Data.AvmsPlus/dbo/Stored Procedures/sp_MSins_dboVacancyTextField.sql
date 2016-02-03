create procedure [sp_MSins_dboVacancyTextField]
    @c1 int,
    @c2 int,
    @c3 int,
    @c4 nvarchar(max)
as
begin  
	insert into [dbo].[VacancyTextField](
		[VacancyTextFieldId],
		[VacancyId],
		[Field],
		[Value]
	) values (
    @c1,
    @c2,
    @c3,
    @c4	) 
end