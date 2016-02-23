create procedure [dbo].[uspValidateTrainingProviderVacancyRelationship]
	@vacancyReference int,
	@ukprn int
as

-- check that training provider can manage the vacancy

select v.VacancyId 
from Vacancy v
join VacancyOwnerRelationship vor ON v.VacancyOwnerRelationshipId = vor.VacancyOwnerRelationshipId
join ProviderSiteRelationship psr on psr.ProviderSiteID = vor.ProviderSiteID
join ProviderSiteRelationshipType psrt on psrt.ProviderSiteRelationshipTypeID = psr.ProviderSiteRelationShipTypeID
join Provider p on p.ProviderID = psr.ProviderID
where 
	v.VacancyReferenceNumber = @vacancyReference
	and p.UKPRN = @ukprn
	AND p.ProviderStatusTypeID <> 2
	and psrt.ProviderSiteRelationshipTypeName in ('Owner', 'Subcontractor')

return 0