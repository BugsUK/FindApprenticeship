create procedure [sp_MSins_dboProviderSiteLocalAuthority]
    @c1 int,
    @c2 int,
    @c3 int
as
begin  
	insert into [dbo].[ProviderSiteLocalAuthority](
		[ProviderSiteLocalAuthorityID],
		[ProviderSiteRelationshipID],
		[LocalAuthorityId]
	) values (
    @c1,
    @c2,
    @c3	) 
end