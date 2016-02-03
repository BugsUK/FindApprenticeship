create procedure [sp_MSupd_dboProviderSiteRelationship]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 int = NULL,
		@c4 int = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[ProviderSiteRelationship] set
		[ProviderID] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [ProviderID] end,
		[ProviderSiteID] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [ProviderSiteID] end,
		[ProviderSiteRelationShipTypeID] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [ProviderSiteRelationShipTypeID] end
where [ProviderSiteRelationshipID] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end