CREATE PROCEDURE [dbo].[uspGetPreviousTrainingProviderForFramework]
	@framework_id int
AS
select tps.ProviderID
	from 

	[ProviderSiteFramework] PSF
	INNER JOIN ProviderSiteRelationship PSR ON PSF.ProviderSiteRelationshipID = PSR.ProviderSiteRelationshipID
	INNER JOIN Provider P ON PSR.ProviderID = P.ProviderID
	
	 inner join SectorSuccessRates tps 
	on P.ProviderID=tps.ProviderID
	inner join ApprenticeshipFramework f on f.PreviousApprenticeshipOccupationId=tps.SectorID
	where f.ApprenticeshipFrameworkId = @framework_id