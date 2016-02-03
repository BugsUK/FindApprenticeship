create procedure [sp_MSupd_dboLocalAuthorityGroupPurpose]
		@c1 int = NULL,
		@c2 nvarchar(50) = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
if (substring(@bitmap,1,1) & 1 = 1)
begin 
update [dbo].[LocalAuthorityGroupPurpose] set
		[LocalAuthorityGroupPurposeID] = case substring(@bitmap,1,1) & 1 when 1 then @c1 else [LocalAuthorityGroupPurposeID] end,
		[LocalAuthorityGroupPurposeName] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [LocalAuthorityGroupPurposeName] end
where [LocalAuthorityGroupPurposeID] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end  
else
begin 
update [dbo].[LocalAuthorityGroupPurpose] set
		[LocalAuthorityGroupPurposeName] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [LocalAuthorityGroupPurposeName] end
where [LocalAuthorityGroupPurposeID] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end 
end