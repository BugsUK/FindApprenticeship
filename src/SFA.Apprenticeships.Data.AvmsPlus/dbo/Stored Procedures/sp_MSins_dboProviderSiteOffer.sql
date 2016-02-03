create procedure [sp_MSins_dboProviderSiteOffer]
    @c1 int,
    @c2 int,
    @c3 int,
    @c4 bit,
    @c5 bit,
    @c6 bit
as
begin  
	insert into [dbo].[ProviderSiteOffer](
		[ProviderSiteOfferID],
		[ProviderSiteLocalAuthorityID],
		[ProviderSiteFrameworkID],
		[Apprenticeship],
		[AdvancedApprenticeship],
		[HigherApprenticeship]
	) values (
    @c1,
    @c2,
    @c3,
    @c4,
    @c5,
    @c6	) 
end