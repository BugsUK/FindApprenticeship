create procedure [sp_MSins_dboLocalAuthorityGroupMembership]
    @c1 int,
    @c2 int
as
begin  
	insert into [dbo].[LocalAuthorityGroupMembership](
		[LocalAuthorityID],
		[LocalAuthorityGroupID]
	) values (
    @c1,
    @c2	) 
end