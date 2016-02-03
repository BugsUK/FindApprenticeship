create procedure [sp_MSupd_dboLocalAuthorityGroupType]
		@c1 int = NULL,
		@c2 nvarchar(50) = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
if (substring(@bitmap,1,1) & 1 = 1)
begin 
update [dbo].[LocalAuthorityGroupType] set
		[LocalAuthorityGroupTypeID] = case substring(@bitmap,1,1) & 1 when 1 then @c1 else [LocalAuthorityGroupTypeID] end,
		[LocalAuthorityGroupTypeName] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [LocalAuthorityGroupTypeName] end
where [LocalAuthorityGroupTypeID] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end  
else
begin 
update [dbo].[LocalAuthorityGroupType] set
		[LocalAuthorityGroupTypeName] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [LocalAuthorityGroupTypeName] end
where [LocalAuthorityGroupTypeID] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end 
end