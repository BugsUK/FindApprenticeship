create procedure [sp_MSdel_dboLocalAuthorityGroupPurpose]
		@pkc1 int
as
begin  
	delete [dbo].[LocalAuthorityGroupPurpose]
where [LocalAuthorityGroupPurposeID] = @pkc1
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end