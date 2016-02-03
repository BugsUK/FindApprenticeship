create procedure [sp_MSins_dboRecruitmentAgentLinkedRelationships]
    @c1 int,
    @c2 int
as
begin  
	insert into [dbo].[RecruitmentAgentLinkedRelationships](
		[ProviderSiteRelationshipID],
		[VacancyOwnerRelationshipID]
	) values (
    @c1,
    @c2	) 
end