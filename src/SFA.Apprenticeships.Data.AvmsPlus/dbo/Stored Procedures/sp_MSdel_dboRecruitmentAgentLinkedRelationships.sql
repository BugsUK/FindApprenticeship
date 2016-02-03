create procedure [sp_MSdel_dboRecruitmentAgentLinkedRelationships]
		@pkc1 int,
		@pkc2 int
as
begin  
	delete [dbo].[RecruitmentAgentLinkedRelationships]
where [ProviderSiteRelationshipID] = @pkc1
  and [VacancyOwnerRelationshipID] = @pkc2
if @@rowcount = 0
    if @@microsoftversion>0x07320000
        exec sp_MSreplraiserror 20598
end