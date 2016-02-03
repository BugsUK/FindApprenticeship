create procedure [sp_MSins_dboSavedSearchCriteriaSearchType]
    @c1 int,
    @c2 nvarchar(3),
    @c3 nvarchar(100),
    @c4 nvarchar(200)
as
begin  
	insert into [dbo].[SavedSearchCriteriaSearchType](
		[SavedSearchCriteriaSearchTypeId],
		[CodeName],
		[ShortName],
		[FullName]
	) values (
    @c1,
    @c2,
    @c3,
    @c4	) 
end