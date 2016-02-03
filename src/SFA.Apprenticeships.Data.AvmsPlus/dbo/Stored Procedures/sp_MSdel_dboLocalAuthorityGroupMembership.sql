create procedure [sp_MSdel_dboLocalAuthorityGroupMembership]
		@pkc1 int,
		@pkc2 int
as
begin  
	delete [dbo].[LocalAuthorityGroupMembership]
where [LocalAuthorityID] = @pkc1
  and [LocalAuthorityGroupID] = @pkc2
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end