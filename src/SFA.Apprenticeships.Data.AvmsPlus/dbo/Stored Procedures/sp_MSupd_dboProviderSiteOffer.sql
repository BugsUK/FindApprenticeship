create procedure [sp_MSupd_dboProviderSiteOffer]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 int = NULL,
		@c4 bit = NULL,
		@c5 bit = NULL,
		@c6 bit = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[ProviderSiteOffer] set
		[ProviderSiteLocalAuthorityID] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [ProviderSiteLocalAuthorityID] end,
		[ProviderSiteFrameworkID] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [ProviderSiteFrameworkID] end,
		[Apprenticeship] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [Apprenticeship] end,
		[AdvancedApprenticeship] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [AdvancedApprenticeship] end,
		[HigherApprenticeship] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [HigherApprenticeship] end
where [ProviderSiteOfferID] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end