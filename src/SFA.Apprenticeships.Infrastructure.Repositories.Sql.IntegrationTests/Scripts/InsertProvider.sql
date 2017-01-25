BEGIN
	BEGIN TRANSACTION

	DECLARE @providerId INT = 10000001
	DECLARE @ukprn INT = 10000002

	DECLARE @providerSiteId1 INT = 20000001
	DECLARE @providerSiteId2 INT = 20000002
	DECLARE @providerSiteId3 INT = 20000003

	DECLARE @edsurn1 INT = 21000002
	DECLARE @edsurn2 INT = 21000003
	DECLARE @edsurn3 INT = 21000004

	--------------------------------------------------------------------------------
	-- Provider
	--
	SET IDENTITY_INSERT [dbo].[Provider] ON

	MERGE [dbo].[Provider] AS dest
	USING (VALUES
		(
		@providerId,
		123, -- UPIN
		@ukprn, -- UKPRN
		'Hopwood Hall College', -- FullName
		'Hopwood Hall College', -- TradingName
		1, -- IsContracted
		'2016-01-31', -- ContractedFrom
		'2016-12-31', -- ContractedTo
		1, -- ProviderStatusTypeID
		0, -- IsNASProvider
		456 -- OriginalUPIN
		)
	) AS src (
		ProviderId,
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
			ProviderId,
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
			ProviderId,
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

	SET IDENTITY_INSERT [dbo].[Provider] OFF

	SELECT * FROM [dbo].[Provider]
	WHERE UKPRN = @ukprn

	--------------------------------------------------------------------------------
	-- ProviderSite
	--
	SET IDENTITY_INSERT [dbo].[ProviderSite] ON

	MERGE [dbo].[ProviderSite] AS dest
	USING (VALUES
		(
		@providerSiteId1,
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
		@providerSiteId2,
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
		@providerSiteId3,
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
		ProviderSiteId,
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
			ProviderSiteId,
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
			ProviderSiteId,
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

	SET IDENTITY_INSERT [dbo].[ProviderSite] OFF

	SELECT * FROM [dbo].[ProviderSite]
	WHERE EDSURN IN (@edsurn1, @edsurn2, @edsurn3)

	--------------------------------------------------------------------------------
	-- ProviderSiteRelationship
	--
	MERGE [dbo].[ProviderSiteRelationship] AS dest
	USING (VALUES
		(
			@providerId,
			@providerSiteId1,
			1 -- ProviderSiteRelationShipTypeID: Owner
		),
		(
			@providerId,
			@providerSiteId2,
			1 -- ProviderSiteRelationShipTypeID: Owner
		),
		(
			@providerId,
			@providerSiteId3,
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
			ProviderSiteRelationShipTypeID = src.ProviderSiteRelationShipTypeID -- ProviderSiteRelationShipTypeID: Owner
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
	ON psr.ProviderSiteId = ps.ProviderSiteID
	WHERE p.UKPRN = @ukprn

	COMMIT TRANSACTION
END
