create procedure [sp_MSins_dboProviderSiteRelationshipType]
    @c1 int,
    @c2 nvarchar(100)
as
begin  
	insert into [dbo].[ProviderSiteRelationshipType](
		[ProviderSiteRelationshipTypeID],
		[ProviderSiteRelationshipTypeName]
	) values (
    @c1,
    @c2	) 
end