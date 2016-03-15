CREATE TABLE [dbo].[Vacancy] (
    [VacancyId]                        INT              IDENTITY (-1, -1) NOT FOR REPLICATION NOT NULL,
    [VacancyOwnerRelationshipId]       INT              NOT NULL,
    [VacancyReferenceNumber]           INT              NULL,
    [ContactName]                      NVARCHAR (MAX)   NULL,
    [VacancyStatusId]                  INT              NOT NULL,
    [AddressLine1]                     NVARCHAR (MAX)    NULL,
    [AddressLine2]                     NVARCHAR (MAX)    NULL,
    [AddressLine3]                     NVARCHAR (MAX)    NULL,
    [AddressLine4]                     NVARCHAR (MAX)    NULL,
    [AddressLine5]                     NVARCHAR (MAX)    NULL,
    [Town]                             NVARCHAR (MAX)    NULL,
    [CountyId]                         INT              NULL,
    [PostCode]                         NVARCHAR (MAX)     NULL,
    [LocalAuthorityId]                 INT              NULL,
    [GeocodeEasting]                   INT              NULL,
    [GeocodeNorthing]                  INT              NULL,
    [Longitude]                        DECIMAL (13, 10) NULL,
    [Latitude]                         DECIMAL (13, 10) NULL,
    [ApprenticeshipFrameworkId]        INT              CONSTRAINT [DF_Vacancy_ApprenticeshipFrameworkId] DEFAULT ((0)) NULL,
    [Title]                            NVARCHAR (MAX)   NULL,
    [ApprenticeshipType]               INT              NULL,
    [ShortDescription]                 NVARCHAR (MAX)   NULL,
    [Description]                      NVARCHAR (MAX)   NULL,
    [WeeklyWage]                       MONEY            NULL,
    [WageType]                         INT              CONSTRAINT [DFT_WageType] DEFAULT ((1)) NOT NULL,
    [WageText]                         NVARCHAR (MAX)    NULL,
    [NumberofPositions]                SMALLINT         NULL,
    [ApplicationClosingDate]           DATETIME         NULL,
    [InterviewsFromDate]               DATETIME         NULL,
    [ExpectedStartDate]                DATETIME         NULL,
    [ExpectedDuration]                 NVARCHAR (MAX)    NULL,
    [WorkingWeek]                      NVARCHAR (MAX)    NULL,
    [NumberOfViews]                    INT              NULL,
    [EmployerAnonymousName]            NVARCHAR (MAX)   NULL,
    [EmployerDescription]              NVARCHAR (MAX)   NULL,
    [EmployersWebsite]                 NVARCHAR (MAX)   NULL,
    [MaxNumberofApplications]          INT              NULL,
    [ApplyOutsideNAVMS]                BIT              NULL,
    [EmployersApplicationInstructions] NVARCHAR (MAX)   NULL,
    [EmployersRecruitmentWebsite]      NVARCHAR (MAX)   NULL,
    [BeingSupportedBy]                 NVARCHAR (MAX)   NULL,
    [LockedForSupportUntil]            DATETIME         NULL,
    [NoOfOfflineApplicants]            INT              NULL,
    [MasterVacancyId]                  INT              NULL,
    [VacancyLocationTypeId]            INT              NULL,
    [NoOfOfflineSystemApplicants]      INT              NULL,
    [VacancyManagerID]                 INT              NULL,
    [DeliveryOrganisationID]           INT              NULL,
    [ContractOwnerID]                  INT              NULL,
    [SmallEmployerWageIncentive]       BIT              CONSTRAINT [DF_Vacancy_SmallEmployerWageIncentive] DEFAULT ((0)) NOT NULL,
    [OriginalContractOwnerId]          INT              NULL,
    [VacancyManagerAnonymous]          BIT              CONSTRAINT [DFT_VacancyManagerAnonymous] DEFAULT ((0)) NOT NULL,
	-- NEW FIELDS
	[VacancyGuid]					   UNIQUEIDENTIFIER NOT NULL,
	[ContactEmail]					   NVARCHAR (MAX)   NULL,
	[ContactNumber]					   NVARCHAR (MAX)   NULL,
	[SubmissionCount]				   INT				NULL,
	[StartedToQADateTime]			   DATETIME			NULL,
	[StandardId]					   INT				NULL,
	[HoursPerWeek]                     DECIMAL (10, 2)  NULL,
	[AdditionalLocationInformation]    NVARCHAR (MAX)   NULL,
	[WageUnitId]					   INT				NULL,
	[DurationTypeId]				   INT				NULL,
	[DurationValue]					   INT				NULL,
	[QAUserName]					   NVARCHAR (MAX)	NULL,
	[TrainingTypeId]				   INT				NULL,
	[VacancyTypeId]					   INT				NULL,
	[SectorId]						   INT				NULL,
	[UpdatedDateTime]				   DATETIME			NULL,
	[EditedInRaa]					   BIT				NOT NULL DEFAULT 0
	

    CONSTRAINT [PK_Vacancy_1] PRIMARY KEY CLUSTERED ([VacancyId] ASC),
	CONSTRAINT [CK_FrameworkId_StandardId] CHECK ([ApprenticeshipFrameworkId] IS NOT NULL AND [StandardId] IS NULL OR [ApprenticeshipFrameworkId] IS NULL AND [StandardId] IS NOT NULL OR [ApprenticeshipFrameworkId] IS NULL AND [StandardId] IS NULL),
    CONSTRAINT [FK_Vacancy_ApprenticeshipFramework] FOREIGN KEY ([ApprenticeshipFrameworkId]) REFERENCES [dbo].[ApprenticeshipFramework] ([ApprenticeshipFrameworkId]),
    CONSTRAINT [FK_Vacancy_ApprenticeshipType] FOREIGN KEY ([ApprenticeshipType]) REFERENCES [dbo].[ApprenticeshipType] ([ApprenticeshipTypeId]),
    CONSTRAINT [FK_Vacancy_County] FOREIGN KEY ([CountyId]) REFERENCES [dbo].[County] ([CountyId]),
    CONSTRAINT [FK_Vacancy_LocalAuthority] FOREIGN KEY ([LocalAuthorityId]) REFERENCES [dbo].[LocalAuthority] ([LocalAuthorityId]),
    CONSTRAINT [FK_Vacancy_MasterVacancyId] FOREIGN KEY ([MasterVacancyId]) REFERENCES [dbo].[Vacancy] ([VacancyId]),
    CONSTRAINT [FK_Vacancy_Provider_As_ContractOwner] FOREIGN KEY ([ContractOwnerID]) REFERENCES [dbo].[Provider] ([ProviderID]),
    CONSTRAINT [FK_Vacancy_ProviderSite_As_DeliveryOrg] FOREIGN KEY ([DeliveryOrganisationID]) REFERENCES [dbo].[ProviderSite] ([ProviderSiteID]),
    -- RESTORE THIS AFTER MIGRATION -> CONSTRAINT [FK_Vacancy_ProviderSite_As_VacancyManager] FOREIGN KEY ([VacancyManagerID]) REFERENCES [dbo].[ProviderSite] ([ProviderSiteID]),
    -- RESTORE THIS -> CONSTRAINT [FK_Vacancy_VacancyOwnerRelationship] FOREIGN KEY ([VacancyOwnerRelationshipId]) REFERENCES [dbo].[VacancyOwnerRelationship] ([VacancyOwnerRelationshipId]),
    CONSTRAINT [FK_Vacancy_VacancyStatusType] FOREIGN KEY ([VacancyStatusId]) REFERENCES [dbo].[VacancyStatusType] ([VacancyStatusTypeId]),
	CONSTRAINT [FK_Vacancy_DurationType] FOREIGN KEY ([DurationTypeId]) REFERENCES [dbo].[DurationType] ([DurationTypeId]),
    CONSTRAINT [uq_idx_vacancy] UNIQUE NONCLUSTERED ([VacancyReferenceNumber] ASC)
);

GO
CREATE NONCLUSTERED INDEX [idx_Vacancy_ApprenticeshipFrameworkId]
    ON [dbo].[Vacancy]([ApprenticeshipFrameworkId] ASC)
    INCLUDE([VacancyId], [VacancyStatusId]);


GO
CREATE NONCLUSTERED INDEX [idx_Vacancy_ContractOwnerID]
    ON [dbo].[Vacancy]([ContractOwnerID] ASC);


GO
CREATE NONCLUSTERED INDEX [idx_Vacancy_DeliveryOrganisationID]
    ON [dbo].[Vacancy]([DeliveryOrganisationID] ASC);


GO
CREATE NONCLUSTERED INDEX [idx_Vacancy_VacancyManagerID]
    ON [dbo].[Vacancy]([VacancyManagerID] ASC);


GO
CREATE NONCLUSTERED INDEX [idx_Vacancy_VacancyOwnerRelationshipId]
    ON [dbo].[Vacancy]([VacancyOwnerRelationshipId] ASC)
    INCLUDE([ApprenticeshipFrameworkId], [Title], [VacancyManagerID], [ApplicationClosingDate], [VacancyStatusId], [NumberofPositions], [ApplyOutsideNAVMS]);


GO
CREATE NONCLUSTERED INDEX [idx_Vacancy_EditedInRaa]
	ON [dbo].[Vacancy]([EditedInRaa] ASC)


GO

CREATE TRIGGER uDeleteVacancySearch
   ON  Vacancy
   AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

		DELETE 
		vacancysearch 
	FROM 
		vacancysearch vs 
		INNER JOIN deleted i on 
			vs.vacancyid = i.vacancyid
	SET NOCOUNT OFF;
END
GO
CREATE TRIGGER [dbo].[uInsertVacancySearch]
    ON [dbo].[Vacancy]
    AFTER INSERT
    AS BEGIN
	SET NOCOUNT ON;
		INSERT INTO VacancySearch(
			EmployerName
			,VacancyId
			,VacancyReferenceNumber
			,[VacancyOwnerName]
		    ,DeliveryOrganisationName	
			,Town 
			,GeocodeEasting
			,GeocodeNorthing
		    ,LocalAuthorityID
			,Latitude
			,Longitude
			,VacancyPostedDate
			,Title
			,ShortDescription
			,Description
			,Status
			,ApplicationClosingDate
			,ApprenticeshipFrameworkId
			,ApprenticeshipFrameworkName
			,ApprenticeshipOccupationName
			,ApprenticeShipType
			,CountyId
			,WeeklyWage
			,WageTYpe
			,ApplicationClosingDateAsInt
			,RealityCheck
			,OtherImportantInformation
			,NationalVacancy
		)

	SELECT 
			coalesce( i.EmployerAnonymousName, e.FullName) FullName1
			,i.VacancyId
			,i.VacancyReferenceNumber
			,tp.TradingName
			,DO.TradingName	
			,i.Town 
			,i.GeocodeEasting
			,i.GeocodeNorthing
			,i.LocalAuthorityID	
			,i.Latitude
			,i.Longitude
			,vh.PostedDate
			,i.Title
			,i.ShortDescription
			,dbo.replaceHTMLTags(i.Description)
			,i.VacancyStatusId
			,i.ApplicationClosingDate
			,i.ApprenticeshipFrameworkId
			,af.ApprenticeshipFrameworkName
			,af.ApprenticeshipOccupationName
			,i.ApprenticeShipType
			,i.CountyId
			,i.weeklywage
			,i.WageType
			,FLOOR(CAST(i.ApplicationClosingDate AS FLOAT)) ApplicationClosingDate1
			,dbo.replaceHTMLTags(vtfr.RealityCheck)
			,dbo.replaceHTMLTags(vtfo.OtherImportantInformation)
			,CASE when vlt.CodeName = 'NAT' then 1 else 0 end CodeName1
	FROM 
		Inserted i 
	INNER JOIN [VacancyOwnerRelationship] vpr 
		ON i.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]
	INNER JOIN Employer e
		ON vpr.EmployerId = e.EmployerId
	INNER JOIN [ProviderSite] tp
		ON vpr.[ProviderSiteID] = tp.ProviderSiteID
	INNER JOIN ProviderSite DO ON i.DeliveryOrganisationID = DO.ProviderSiteID
	INNER JOIN VacancyLocationType vlt
		on i.VacancyLocationTypeId = vlt.VacancyLocationTypeId
	INNER JOIN (SELECT 
					a.ApprenticeshipFrameworkId, 
					a.FullName as ApprenticeshipFrameworkName,
					ao.Fullname as ApprenticeshipOccupationName
				FROM 
					apprenticeshipframework a 
					INNER JOIN apprenticeshipoccupation ao ON 
						a.ApprenticeshipOccupationId = ao.ApprenticeshipOccupationId) as af
		ON i.ApprenticeshipFrameworkId = af.ApprenticeshipFrameworkId
	INNER JOIN (select 
					VacancyId 
					,Max(HistoryDate) as PostedDate 
				from vacancyhistory where VacancyHistoryEventSubTypeId = 2 
		GROUP BY vacancyid) AS vh
		ON vh.vacancyid = i.vacancyid
    INNER JOIN (SELECT 
                    vtf5.vacancyid,
                    vtf5.[value] as RealityCheck
                FROM vacancytextfield vtf5 where vtf5.field = 7) as vtfr
		ON i.vacancyid = vtfr.vacancyid 		
    INNER JOIN (SELECT 
                    vtf7.vacancyid,
                    vtf7.[value] as OtherImportantInformation
                FROM vacancytextfield vtf7 where vtf7.field = 5) as vtfo
		ON i.vacancyid = vtfo.vacancyid 		
	WHERE i.vacancystatusid = 2 
	AND i.Town IS NOT NULL 
	AND i.Title IS NOT NULL 
	AND i.ApprenticeshipType IS NOT NULL
	AND i.ApprenticeshipFrameworkId <> 0


	SET NOCOUNT ON;
END
GO
CREATE TRIGGER [dbo].[uUpdateVacancySearch]
    ON [dbo].[Vacancy]
    AFTER UPDATE
    AS BEGIN
SET NOCOUNT ON;
	
	--If status is set be anything other then 'Live' then the record is to be removed from vacancysearch table.
	DELETE 
		vacancysearch 
	FROM 
		vacancysearch vs 
		INNER JOIN inserted i on 
			vs.vacancyid = i.vacancyid 
			and 
			i.vacancystatusid <> 2
	
	--If status is set to Live then if the record already exists then it is to be updated 
	UPDATE vacancysearch
		SET
			EmployerName			=	coalesce( i.EmployerAnonymousName, e.FullName)
			,VacancyId				=	i.VacancyId
			,VacancyReferenceNumber	=	i.VacancyReferenceNumber
			,[VacancyOwnerName]	=	tp.TradingName
			,DeliveryOrganisationName = DO.TradingName	
			,Town					=	i.Town 
			,GeocodeEasting			=	i.GeocodeEasting
			,GeocodeNorthing		=	i.GeocodeNorthing
			,Latitude				=	i.Latitude
			,Longitude				=	i.Longitude
			,LocalAuthorityID       =   i.LocalAuthorityID	
			,VacancyPostedDate		=	vh.PostedDate
			,Title					=	i.Title
			,ShortDescription		=	i.ShortDescription
			,Description			=	dbo.replaceHTMLTags(i.Description)
			,Status					=	i.VacancyStatusId
			,ApplicationClosingDate	=	i.ApplicationClosingDate
			,ApprenticeshipFrameworkId=	i.ApprenticeshipFrameworkId
			,ApprenticeshipFrameworkName =	af.ApprenticeshipFrameworkName
			,ApprenticeshipOccupationName =	af.ApprenticeshipOccupationName
			,ApprenticeShipType		=	i.ApprenticeShipType
			,CountyId				=	i.CountyId
			,WeeklyWage				=	i.weeklywage
			,WageType				= i.WageType
			,ApplicationClosingDateAsInt = FLOOR(CAST(i.ApplicationClosingDate AS FLOAT))
			,RealityCheck			= 	dbo.replaceHTMLTags(vtfr.RealityCheck)
			,OtherImportantInformation = dbo.replaceHTMLTags(vtfo.OtherImportantInformation)
			,NationalVacancy        =   CASE when vlt.CodeName = 'NAT' then 1 else 0 end
	from vacancysearch vs
	INNER JOIN Inserted i --INNER JOIN will get only those rows that have been updated and exist in vacancy search
		on i.VacancyId = vs.VacancyId
	INNER JOIN [VacancyOwnerRelationship] vpr 
		on i.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]
	INNER JOIN Employer e
		on vpr.EmployerId = e.EmployerId
	INNER JOIN [ProviderSite] tp
		on vpr.[ProviderSiteID] = tp.ProviderSiteID
	JOIN ProviderSite DO ON i.DeliveryOrganisationID = DO.ProviderSiteID
	INNER JOIN VacancyLocationType vlt
		on i.VacancyLocationTypeId = vlt.VacancyLocationTypeId
	INNER JOIN (SELECT 
					a.ApprenticeshipFrameworkId, 
					a.FullName as ApprenticeshipFrameworkName,
					ao.Fullname as ApprenticeshipOccupationName
				FROM 
					apprenticeshipframework a 
					INNER JOIN apprenticeshipoccupation ao ON 
						a.ApprenticeshipOccupationId = ao.ApprenticeshipOccupationId) as af
		ON i.ApprenticeshipFrameworkId = af.ApprenticeshipFrameworkId
	INNER JOIN (select 
					VacancyId 
					,Max(HistoryDate) as PostedDate 
				from vacancyhistory where VacancyHistoryEventSubTypeId = 2 
		GROUP BY vacancyid) AS vh
		ON vh.vacancyid = i.vacancyid
    INNER JOIN (SELECT 
                    vtf5.vacancyid,
                    vtf5.[value] as RealityCheck
                FROM vacancytextfield vtf5 where vtf5.field = 7) as vtfr
		ON i.vacancyid = vtfr.vacancyid 		
    INNER JOIN (SELECT 
                    vtf7.vacancyid,
                    vtf7.[value] as OtherImportantInformation
                FROM vacancytextfield vtf7 where vtf7.field = 5) as vtfo
		ON i.vacancyid = vtfo.vacancyid
	WHERE i.vacancystatusid = 2 
	--These should not be null when the vacancy is published, but they are not constrained currently
	AND i.town IS NOT NULL 
	AND i.Title IS NOT NULL 
	AND i.apprenticeshiptype IS NOT NULL
	AND i.apprenticeshipframeworkid <> 0
	--else inserted in VacancySearch.
	INSERT INTO VacancySearch(
			EmployerName
			,VacancyId
			,VacancyReferenceNumber
			,[VacancyOwnerName]
			,DeliveryOrganisationName		
			,Town 
			,GeocodeEasting
			,GeocodeNorthing
			,Latitude
			,Longitude
		    ,LocalAuthorityID	
			,VacancyPostedDate
			,Title
			,ShortDescription
			,Description
			,Status
			,ApplicationClosingDate
			,ApprenticeshipFrameworkId
			,ApprenticeshipFrameworkName
			,ApprenticeshipOccupationName
			,ApprenticeShipType
			,CountyId
			,WeeklyWage
			,WageType
			,ApplicationClosingDateAsInt
			,RealityCheck
			,OtherImportantInformation
			,NationalVacancy
		)
	SELECT 
			coalesce( i.EmployerAnonymousName, e.FullName)
			,i.VacancyId
			,i.VacancyReferenceNumber
			,tp.TradingName
			,do.TradingName		
			,i.Town 
			,i.GeocodeEasting
			,i.GeocodeNorthing
			,i.Latitude
			,i.Longitude
			,i.LocalAuthorityID	
			,vh.PostedDate
			,i.Title
			,i.ShortDescription
			,dbo.replaceHTMLTags(i.Description)
			,i.VacancyStatusId
			,i.ApplicationClosingDate
			,i.ApprenticeshipFrameworkId
			,af.ApprenticeshipFrameworkName
			,af.ApprenticeshipOccupationName
			,i.ApprenticeShipType
			,i.CountyId
			,i.weeklywage
			,i.WageTYpe
			,FLOOR(CAST(i.ApplicationClosingDate AS FLOAT))
			,dbo.replaceHTMLTags(vtfr.RealityCheck)
			,dbo.replaceHTMLTags(vtfo.OtherImportantInformation)
			,CASE when vlt.CodeName = 'NAT' then 1 else 0 end
	FROM 
		Inserted i 
	INNER JOIN [VacancyOwnerRelationship] vpr 
		ON i.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]
	INNER JOIN Employer e
		ON vpr.EmployerId = e.EmployerId
	INNER JOIN [ProviderSite] tp
		ON vpr.[ProviderSiteID] = tp.ProviderSiteID
		JOIN ProviderSite DO ON i.DeliveryOrganisationID = DO.ProviderSiteID
	INNER JOIN VacancyLocationType vlt
		on i.VacancyLocationTypeId = vlt.VacancyLocationTypeId
	INNER JOIN (SELECT 
					a.ApprenticeshipFrameworkId, 
					a.FullName as ApprenticeshipFrameworkName,
					ao.Fullname as ApprenticeshipOccupationName
				FROM 
					apprenticeshipframework a 
					INNER JOIN apprenticeshipoccupation ao ON 
						a.ApprenticeshipOccupationId = ao.ApprenticeshipOccupationId) as af
		ON i.ApprenticeshipFrameworkId = af.ApprenticeshipFrameworkId
	INNER JOIN (select 
					VacancyId 
					,Max(HistoryDate) as PostedDate 
				from vacancyhistory where VacancyHistoryEventSubTypeId = 2 
		GROUP BY vacancyid) AS vh
		ON vh.vacancyid = i.vacancyid
    INNER JOIN (SELECT 
                    vtf5.vacancyid,
                    vtf5.[value] as RealityCheck
                FROM vacancytextfield vtf5 where vtf5.field = 5) as vtfr
		ON i.vacancyid = vtfr.vacancyid 		
    INNER JOIN (SELECT 
                    vtf7.vacancyid,
                    vtf7.[value] as OtherImportantInformation
                FROM vacancytextfield vtf7 where vtf7.field = 7) as vtfo
		ON i.vacancyid = vtfo.vacancyid 		
	LEFT OUTER JOIN vacancysearch vs ON i.VacancyId = vs.VacancyId
	WHERE i.vacancystatusid = 2 
	AND i.town IS NOT NULL 
	AND i.Title IS NOT NULL 
	AND i.apprenticeshiptype IS NOT NULL
	AND i.apprenticeshipframeworkid <> 0
	AND vs.vacancyid is null
END