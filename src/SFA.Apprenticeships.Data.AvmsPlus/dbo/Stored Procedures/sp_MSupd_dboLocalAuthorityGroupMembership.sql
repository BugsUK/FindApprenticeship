create procedure [sp_MSupd_dboLocalAuthorityGroupMembership]
		@c1 int = NULL,
		@c2 int = NULL,
		@pkc1 int = NULL,
		@pkc2 int = NULL,
		@bitmap binary(1)
as
begin  
update [dbo].[LocalAuthorityGroupMembership] set
		[LocalAuthorityID] = case substring(@bitmap,1,1) & 1 when 1 then @c1 else [LocalAuthorityID] end,
		[LocalAuthorityGroupID] = case substring(@bitmap,1,1) & 2 when 2 then @c2 else [LocalAuthorityGroupID] end
where [LocalAuthorityID] = @pkc1
  and [LocalAuthorityGroupID] = @pkc2
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end