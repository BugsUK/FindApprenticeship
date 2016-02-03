create procedure [sp_MSins_dboLocalAuthorityGroupPurpose]
    @c1 int,
    @c2 nvarchar(50)
as
begin  
	insert into [dbo].[LocalAuthorityGroupPurpose](
		[LocalAuthorityGroupPurposeID],
		[LocalAuthorityGroupPurposeName]
	) values (
    @c1,
    @c2	) 
end