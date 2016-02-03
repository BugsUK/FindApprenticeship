create procedure [sp_MSins_dboLocalAuthorityGroup]
    @c1 int,
    @c2 nvarchar(3),
    @c3 nvarchar(50),
    @c4 nvarchar(100),
    @c5 int,
    @c6 int,
    @c7 int
as
begin  
	insert into [dbo].[LocalAuthorityGroup](
		[LocalAuthorityGroupID],
		[CodeName],
		[ShortName],
		[FullName],
		[LocalAuthorityGroupTypeID],
		[LocalAuthorityGroupPurposeID],
		[ParentLocalAuthorityGroupID]
	) values (
    @c1,
    @c2,
    @c3,
    @c4,
    @c5,
    @c6,
    @c7	) 
end