BEGIN
	-- TODO: Reseed identity columns as they are not currently set by migration.
	DECLARE @maxProviderId INT = COALESCE((SELECT MAX(ProviderId) FROM dbo.Provider) + 1, 1)
	DECLARE @maxProviderSiteId INT = COALESCE((SELECT MAX(ProviderSiteId) FROM dbo.ProviderSite) + 1, 1)
	DECLARE @maxProviderSiteRelationshipId INT = COALESCE((SELECT MAX(ProviderSiteRelationshipId) FROM dbo.ProviderSiteRelationship) + 1, 1)

	DBCC CHECKIDENT ('Provider', RESEED, @maxProviderId)
	DBCC CHECKIDENT ('ProviderSite', RESEED, @maxProviderSiteId)
	DBCC CHECKIDENT ('ProviderSiteRelationship', RESEED, @maxProviderSiteRelationshipId)

	BEGIN TRANSACTION

	DECLARE @ukprn INT = 10000000

	DECLARE @edsurn1 INT = 100339794
	DECLARE @edsurn2 INT = 903008386
	DECLARE @edsurn3 INT = 145238970

	--------------------------------------------------------------------------------
	-- Delete test data
	--
	/*
	DELETE FROM [dbo].[ProviderSiteRelationship]
	WHERE ProviderID = (SELECT ProviderId FROM [dbo].[Provider] WHERE UKPRN = @ukprn)

	DELETE FROM [dbo].[ProviderSite]
	WHERE EDSURN IN (@edsurn1, @edsurn2, @edsurn3)

	DELETE FROM [dbo].[Provider]
	WHERE UKPRN = @ukprn
	*/

	--------------------------------------------------------------------------------
	-- Provider
	--
	MERGE [dbo].[Provider] AS dest
	USING (VALUES
		(
		123, -- UPIN
		@ukprn, -- UKPRN
		'Hopwood Hall College', -- FullName
		NULL, -- TradingName
		1, -- IsContracted
		'2016-01-31', -- ContractedFrom
		'2016-12-31', -- ContractedTo
		1, -- ProviderStatusTypeID
		0, -- IsNASProvider
		456 -- OriginalUPIN
		)
	) AS src (
		UPIN,
		UKPRN,
		FullName,
		TradingName,
		IsContracted,
		ContractedFrom,
		ContractedTo,
		ProviderStatusTypeID,
		IsNASProvider,
		OriginalUPIN
	)
	ON (dest.UKPRN = src.UKPRN)
	WHEN MATCHED THEN
		UPDATE SET
			UPIN = src.UPIN,
			FullName = src.FullName,
			TradingName = src.TradingName,
			IsContracted = src.IsContracted,
			ContractedFrom = src.ContractedFrom,
			ContractedTo = src.ContractedTo,
			ProviderStatusTypeID = src.ProviderStatusTypeID,
			IsNASProvider = src.IsNASProvider,
			OriginalUPIN = src.OriginalUPIN
	WHEN NOT MATCHED THEN
		INSERT (
			UPIN,
			UKPRN,
			FullName,
			TradingName,
			IsContracted,
			ContractedFrom,
			ContractedTo,
			ProviderStatusTypeID,
			IsNASProvider,
			OriginalUPIN
		)
		VALUES (
			UPIN,
			UKPRN,
			FullName,
			TradingName,
			IsContracted,
			ContractedFrom,
			ContractedTo,
			ProviderStatusTypeID,
			IsNASProvider,
			OriginalUPIN
		)
	;

	SELECT * FROM [dbo].[Provider]
	WHERE UKPRN = @ukprn

	--------------------------------------------------------------------------------
	-- ProviderSite
	--
	MERGE [dbo].[ProviderSite] AS dest
	USING (VALUES
		(
		'Hopwood Campus',-- FullName
		'The Hopwood College', -- TradingName
		@edsurn1, -- EDSURN
		'Rochdale Campus', -- AddressLine1
		NULL, -- AddressLine2
		NULL, -- AddressLine3
		NULL, -- AddressLine4
		NULL, -- AddressLine5
		'Rochdale', -- Town
		'', -- CountyId
		'OL12 6RY', -- PostCode
		16, -- LocalAuthorityId
		NULL, -- ManagingAreaID
		NULL, -- Longitude
		NULL, -- Latitude
		NULL, -- GeocodeEasting
		NULL, -- GeocodeNorthing
		NULL, -- OwnerOrganisation
		'Hopwood Hall College offers vocational further education courses from two campuses in Middleton and Rochdale.', -- EmployerDescription
		'Hopwood Hall College offers candidates vocational further education courses from two campuses in Middleton and Rochdale.', -- CandidateDescription
		'http://www.hopwood.ac.uk/', -- WebPage
		0, -- OutofDate
		'Contact Jill Doe on 01706 678555', -- ContactDetailsForEmployer
		'Contact Bob Dime on 01706 555678', -- ContactDetailsForCandidate
		0, -- HideFromSearch
		1, -- TrainingProviderStatusTypeId
		0, -- IsRecruitmentAgency
		NULL -- ContactDetailsAsARecruitmentAgency
		),
		(
		'Hopwood Hall College',-- FullName
		'Orchard Training Solutions', -- TradingName
		@edsurn2, -- EDSURN
		'13 Drake Street', -- AddressLine1
		NULL, -- AddressLine2
		NULL, -- AddressLine3
		NULL, -- AddressLine4
		NULL, -- AddressLine5
		'Rochdale', -- Town
		'', -- CountyId
		'OL16 1RE', -- PostCode
		16, -- LocalAuthorityId
		NULL, -- ManagingAreaID
		NULL, -- Longitude
		NULL, -- Latitude
		NULL, -- GeocodeEasting
		NULL, -- GeocodeNorthing
		NULL, -- OwnerOrganisation
		'Orchard Training Solutions offers vocational further education courses from our campus in Rochdale.', -- EmployerDescription
		'Orchard Training Solutions offers candidates vocational further education courses from our campus in Rochdale.', -- CandidateDescription
		'http://www.hopwood.ac.uk/apprenticeships/', -- WebPage
		0, -- OutofDate
		'Contact Jane Doe on 01706 555123', -- ContactDetailsForEmployer
		'Contact John Smith on 01706 123555', -- ContactDetailsForCandidate
		0, -- HideFromSearch
		1, -- TrainingProviderStatusTypeId
		0, -- IsRecruitmentAgency
		NULL -- ContactDetailsAsARecruitmentAgency
		),
		(
		'Hopwood Hall College in the Community',-- FullName
		'Hopwood Hall Community College', -- TradingName
		@edsurn3, -- EDSURN
		'158 Drake Street', -- AddressLine1
		'St Mary''s Gate', -- AddressLine2
		NULL, -- AddressLine3
		NULL, -- AddressLine4
		NULL, -- AddressLine5
		'Rochdale', -- Town
		'', -- CountyId
		'OL16 1PX', -- PostCode
		16, -- LocalAuthorityId
		NULL, -- ManagingAreaID
		NULL, -- Longitude
		NULL, -- Latitude
		NULL, -- GeocodeEasting
		NULL, -- GeocodeNorthing
		NULL, -- OwnerOrganisation
		'Hopwood Hall Community College offers vocational further education courses from two campuses in Middleton and Rochdale.', -- EmployerDescription
		'Hopwood Hall Community College offers candidates vocational further education courses from two campuses in Middleton and Rochdale.', -- CandidateDescription
		'http://www.hopwood.ac.uk/19-plus-students/community-learning/', -- WebPage
		0, -- OutofDate
		'Contact Dan Deen on 01706 555321', -- ContactDetailsForEmployer
		'Contact Jan Block on 01706 123876', -- ContactDetailsForCandidate
		0, -- HideFromSearch
		1, -- TrainingProviderStatusTypeId
		0, -- IsRecruitmentAgency
		NULL -- ContactDetailsAsARecruitmentAgency
		)
	) AS src (
		FullName,
		TradingName,
		EDSURN,
		AddressLine1,
		AddressLine2,
		AddressLine3,
		AddressLine4,
		AddressLine5,
		Town,
		CountyId,
		PostCode,
		LocalAuthorityId,
		ManagingAreaID,
		Longitude,
		Latitude,
		GeocodeEasting,
		GeocodeNorthing,
		OwnerOrganisation,
		EmployerDescription,
		CandidateDescription,
		WebPage,
		OutofDate,
		ContactDetailsForEmployer,
		ContactDetailsForCandidate,
		HideFromSearch,
		TrainingProviderStatusTypeId,
		IsRecruitmentAgency,
		ContactDetailsAsARecruitmentAgency
	)
	ON (dest.EDSURN = src.EDSURN)
	WHEN MATCHED THEN
		UPDATE SET
			FullName = src.FullName,
			TradingName = src.TradingName,
			AddressLine1 = src.AddressLine1,
			AddressLine2 = src.AddressLine2,
			AddressLine3 = src.AddressLine3,
			AddressLine4 = src.AddressLine4,
			AddressLine5 = src.AddressLine5,
			Town = src.Town,
			CountyId = src.CountyId,
			PostCode = src.PostCode,
			LocalAuthorityId = src.LocalAuthorityId,
			ManagingAreaID = src.ManagingAreaID,
			Longitude = src.Longitude,
			Latitude = src.Latitude,
			GeocodeEasting = src.GeocodeEasting,
			GeocodeNorthing = src.GeocodeNorthing,
			OwnerOrganisation = src.OwnerOrganisation,
			EmployerDescription = src.EmployerDescription,
			CandidateDescription = src.CandidateDescription,
			WebPage = src.WebPage,
			OutofDate = src.OutofDate,
			ContactDetailsForEmployer = src.ContactDetailsForEmployer,
			ContactDetailsForCandidate = src.ContactDetailsForCandidate,
			HideFromSearch = src.HideFromSearch,
			TrainingProviderStatusTypeId = src.TrainingProviderStatusTypeId,
			IsRecruitmentAgency = src.IsRecruitmentAgency,
			ContactDetailsAsARecruitmentAgency = src.ContactDetailsAsARecruitmentAgency
	WHEN NOT MATCHED THEN
		INSERT (
			FullName,
			TradingName,
			EDSURN,
			AddressLine1,
			AddressLine2,
			AddressLine3,
			AddressLine4,
			AddressLine5,
			Town,
			CountyId,
			PostCode,
			LocalAuthorityId,
			ManagingAreaID,
			Longitude,
			Latitude,
			GeocodeEasting,
			GeocodeNorthing,
			OwnerOrganisation,
			EmployerDescription,
			CandidateDescription,
			WebPage,
			OutofDate,
			ContactDetailsForEmployer,
			ContactDetailsForCandidate,
			HideFromSearch,
			TrainingProviderStatusTypeId,
			IsRecruitmentAgency,
			ContactDetailsAsARecruitmentAgency
		)
		VALUES (
			FullName,
			TradingName,
			EDSURN,
			AddressLine1,
			AddressLine2,
			AddressLine3,
			AddressLine4,
			AddressLine5,
			Town,
			CountyId,
			PostCode,
			LocalAuthorityId,
			ManagingAreaID,
			Longitude,
			Latitude,
			GeocodeEasting,
			GeocodeNorthing,
			OwnerOrganisation,
			EmployerDescription,
			CandidateDescription,
			WebPage,
			OutofDate,
			ContactDetailsForEmployer,
			ContactDetailsForCandidate,
			HideFromSearch,
			TrainingProviderStatusTypeId,
			IsRecruitmentAgency,
			ContactDetailsAsARecruitmentAgency
		)
	;

	SELECT * FROM [dbo].[ProviderSite]
	WHERE EDSURN IN (@edsurn1, @edsurn2, @edsurn3)

	--------------------------------------------------------------------------------
	-- ProviderSiteRelationship
	--
	MERGE [dbo].[ProviderSiteRelationship] AS dest
	USING (VALUES
		(
			(SELECT ProviderId FROM [dbo].[Provider] WHERE UKPRN = @ukprn), -- ProviderID
			(SELECT ProviderSiteId FROM [dbo].[ProviderSite] WHERE EDSURN = @edsurn1), -- ProviderSiteID
			1 -- ProviderSiteRelationShipTypeID: Owner
		),
		(
			(SELECT ProviderId FROM [dbo].[Provider] WHERE UKPRN = @ukprn), -- ProviderID
			(SELECT ProviderSiteId FROM [dbo].[ProviderSite] WHERE EDSURN = @edsurn2), -- ProviderSiteID
			1 -- ProviderSiteRelationShipTypeID: Owner
		),
		(
			(SELECT ProviderId FROM [dbo].[Provider] WHERE UKPRN = @ukprn), -- ProviderID
			(SELECT ProviderSiteId FROM [dbo].[ProviderSite] WHERE EDSURN = @edsurn3), -- ProviderSiteID
			1 -- ProviderSiteRelationShipTypeID: Owner
		)
	) AS src (
			ProviderID,
			ProviderSiteID,
			ProviderSiteRelationShipTypeID
	)
	ON (dest.ProviderID = src.ProviderID AND dest.ProviderSiteId = src.ProviderSiteId)
	WHEN MATCHED THEN
		UPDATE SET
			ProviderSiteRelationShipTypeID = 1 -- ProviderSiteRelationShipTypeID: Owner
	WHEN NOT MATCHED THEN
		INSERT (
			ProviderID,
			ProviderSiteID,
			ProviderSiteRelationShipTypeID
		)
		VALUES (
			ProviderID,
			ProviderSiteID,
			ProviderSiteRelationShipTypeID
		)
	;

	SELECT psr.* FROM [dbo].[ProviderSiteRelationship] psr
	INNER JOIN [dbo].[Provider] p
	ON p.ProviderID = psr.ProviderID
	INNER JOIN [dbo].[ProviderSite] ps
	ON psr.ProviderID = ps.ProviderSiteID
	WHERE p.UKPRN = @ukprn

	-- VacancyOwnerRelationship
	-- Employer
	-- EmployerContact?
	-- EmployerTrainingProviderStatus

	COMMIT TRANSACTION
END
GO
