create procedure [sp_MSins_dboLocalAuthorityGroupType]
    @c1 int,
    @c2 nvarchar(50)
as
begin  
	insert into [dbo].[LocalAuthorityGroupType](
		[LocalAuthorityGroupTypeID],
		[LocalAuthorityGroupTypeName]
	) values (
    @c1,
    @c2	) 
end