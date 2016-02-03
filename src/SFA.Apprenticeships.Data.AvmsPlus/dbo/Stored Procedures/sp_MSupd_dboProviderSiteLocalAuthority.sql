create procedure [sp_MSupd_dboProviderSiteLocalAuthority]
		@c1 int = NULL,
		@c2 int = NULL,
		@c3 int = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[ProviderSiteLocalAuthority] set
		[ProviderSiteRelationshipID] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [ProviderSiteRelationshipID] end,
		[LocalAuthorityId] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [LocalAuthorityId] end
where [ProviderSiteLocalAuthorityID] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end