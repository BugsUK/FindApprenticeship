create procedure [sp_MSins_dboProviderSiteRelationship]
    @c1 int,
    @c2 int,
    @c3 int,
    @c4 int
as
begin  
	insert into [dbo].[ProviderSiteRelationship](
		[ProviderSiteRelationshipID],
		[ProviderID],
		[ProviderSiteID],
		[ProviderSiteRelationShipTypeID]
	) values (
    @c1,
    @c2,
    @c3,
    @c4	) 
end