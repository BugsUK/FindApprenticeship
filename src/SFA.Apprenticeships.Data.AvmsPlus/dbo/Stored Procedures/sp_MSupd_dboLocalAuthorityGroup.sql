create procedure [sp_MSupd_dboLocalAuthorityGroup]
		@c1 int = NULL,
		@c2 nvarchar(3) = NULL,
		@c3 nvarchar(50) = NULL,
		@c4 nvarchar(100) = NULL,
		@c5 int = NULL,
		@c6 int = NULL,
		@c7 int = NULL,
		@pkc1 int = NULL,
		@bitmap binary(1)
as
begin  
if (substring(@bitmap,1,1) & 1 = 1)
begin 
update [dbo].[LocalAuthorityGroup] set
		[LocalAuthorityGroupID] = case substring(@bitmap,1,1) & 1 when 1 then @c1 else [LocalAuthorityGroupID] end,
		[CodeName] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [CodeName] end,
		[ShortName] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [ShortName] end,
		[FullName] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [FullName] end,
		[LocalAuthorityGroupTypeID] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [LocalAuthorityGroupTypeID] end,
		[LocalAuthorityGroupPurposeID] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [LocalAuthorityGroupPurposeID] end,
		[ParentLocalAuthorityGroupID] = case substring(@bitmap,1,1) & 64 when 64 then @c7 else [ParentLocalAuthorityGroupID] end
where [LocalAuthorityGroupID] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end  
else
begin 
update [dbo].[LocalAuthorityGroup] set
		[CodeName] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [CodeName] end,
		[ShortName] = case substring(@bitmap,1,1) & 4 when 4 then @c3 else [ShortName] end,
		[FullName] = case substring(@bitmap,1,1) & 8 when 8 then @c4 else [FullName] end,
		[LocalAuthorityGroupTypeID] = case substring(@bitmap,1,1) & 16 when 16 then @c5 else [LocalAuthorityGroupTypeID] end,
		[LocalAuthorityGroupPurposeID] = case substring(@bitmap,1,1) & 32 when 32 then @c6 else [LocalAuthorityGroupPurposeID] end,
		[ParentLocalAuthorityGroupID] = case substring(@bitmap,1,1) & 64 when 64 then @c7 else [ParentLocalAuthorityGroupID] end
where [LocalAuthorityGroupID] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end 
end