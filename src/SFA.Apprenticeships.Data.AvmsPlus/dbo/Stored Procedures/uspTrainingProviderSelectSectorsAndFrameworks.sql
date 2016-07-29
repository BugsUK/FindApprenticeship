Create Procedure [dbo].[uspTrainingProviderSelectSectorsAndFrameworks]
	@trainingProviderId int
As
Begin
	SET NOCOUNT ON;

	Select	psf.ProviderSiteFrameworkID,
			psf.ProviderSiteRelationshipID,
			psf.FrameworkId,
			--f.TrainingProviderSectorId,
			s.SectorId,
			s.PassRate
	From	[ProviderSiteFramework] psf
			JOIN ApprenticeshipFramework af  ON psf.FrameworkID = af.ApprenticeshipFrameworkID
			JOIN SectorSuccessRates s 	ON	s.sectorid = af.ApprenticeshipOccupationId
			JOIN ProvidersiteRelationship PSR on PSF.ProviderSiteRelationShipID = PSR.ProviderSiteRelationShipID
	
	And		psR.ProviderSiteID	= @trainingProviderId
	
	SET NOCOUNT OFF;
End