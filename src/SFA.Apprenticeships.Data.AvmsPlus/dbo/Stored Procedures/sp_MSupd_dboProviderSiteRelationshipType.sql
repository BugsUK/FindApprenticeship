create procedure [sp_MSupd_dboProviderSiteRelationshipType]
		@c1 int = NULL,
		@c2 nvarchar(100) = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
if (substring(@bitmap,1,1) & 1 = 1)
begin 
update [dbo].[ProviderSiteRelationshipType] set
		[ProviderSiteRelationshipTypeID] = case substring(@bitmap,1,1) & 1 when 1 then @c1 else [ProviderSiteRelationshipTypeID] end,
		[ProviderSiteRelationshipTypeName] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [ProviderSiteRelationshipTypeName] end
where [ProviderSiteRelationshipTypeID] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end  
else
begin 
update [dbo].[ProviderSiteRelationshipType] set
		[ProviderSiteRelationshipTypeName] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [ProviderSiteRelationshipTypeName] end
where [ProviderSiteRelationshipTypeID] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end 
end