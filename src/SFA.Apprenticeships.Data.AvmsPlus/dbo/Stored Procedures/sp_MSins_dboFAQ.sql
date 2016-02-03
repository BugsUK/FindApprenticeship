create procedure [sp_MSins_dboFAQ]
    @c1 int,
    @c2 int,
    @c3 int,
    @c4 nvarchar(100),
    @c5 nvarchar(2000)
as
begin  
	insert into [dbo].[FAQ](
		[FAQId],
		[SortOrder],
		[UserTypeId],
		[Title],
		[Content]
	) values (
    @c1,
    @c2,
    @c3,
    @c4,
    @c5	) 
end