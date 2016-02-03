create procedure [sp_MSins_dboSearchFrameworks]
    @c1 int,
    @c2 int,
    @c3 int
as
begin  
	insert into [dbo].[SearchFrameworks](
		[SearchFrameworksId],
		[FrameworkId],
		[SavedSearchCriteriaId]
	) values (
    @c1,
    @c2,
    @c3	) 
end