/* DROP PROCEDURE [dbo].[uspRetrieveSummaryVacancyXMLDetails2] */
CREATE PROCEDURE [dbo].[uspRetrieveSummaryVacancyXMLDetails2]
    @vacancyReferenceNumber INT          = -1,
    @frameworkCode          VARCHAR(3)   = NULL,
    @occupationCode         VARCHAR(3)   = NULL,
    @countyCode             VARCHAR(3)   = NULL,
    @town                   VARCHAR(255) = NULL,
    @regionCode             VARCHAR(3)   = NULL, -- TODO: This is 6 on in usbRetrieveFullVacancyXMLDetails.sql
    @vacancyPublishedDate   DATETIME     = NULL,
    @locationType           INT          = -1,
    @pageSize               INT          = 25,
    @pageIndex              INT          = 1,
    @totalRecords           INT OUT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- obtain the vacancy live status ID for use in the main query
    DECLARE @liveVacancyStatusID INT =	(
                                            SELECT	VacancyStatusTypeId 
                                            FROM	VacancyStatusType 
                                            WHERE	CodeName = 'Lve' 
                                        );
	
	SELECT *
	INTO   #AllRecords
	FROM   fnGetIdsForApi(@vacancyReferenceNumber, @frameworkCode, @occupationCode, @countyCode, @town, @regionCode, @vacancyPublishedDate, @locationType);

    SELECT 
        vac.VacancyId                   AS 'VacancyId',
        vac.Title                       AS 'Title',
        vac.VacancyReferenceNumber      AS 'VacancyReferenceNumber',
        CASE WHEN vac.EmployerAnonymousName IS NULL THEN emp.TradingName ELSE vac.EmployerAnonymousName END AS 'Employer',
        tp.FullName                     AS 'LearningProvider',
        vac.ShortDescription            AS 'Description',
        apt.ApprenticeshipTypeId        AS 'ApprenticeshipTypeId',
        apt.FullName                    AS 'VacancyType',
        vac.NumberofPositions           AS 'NumberOfVacancies',
        vac.AddressLine1                AS 'AddressLine1', 
        vac.AddressLine2                AS 'AddressLine2', 
        vac.AddressLine3                AS 'AddressLine3', 
        vac.AddressLine4                AS 'AddressLine4', 
        vac.Town                        AS 'Town',
        Cty.FullName                    AS 'County',
        vac.PostCode                    AS 'Postcode',
        COALESCE(fwk.FullName, std.FullName) as 'ApprenticeshipFramework',
        vac.ApplicationClosingDate      AS 'ClosingDateForApplicationsDate', 
        convert(VARCHAR, vac.ApplicationClosingDate, 111) AS 'ClosingDateForApplications', 
        vac.GeocodeEasting              AS 'GeocodeEasting',
        vac.GeocodeNorthing             AS 'GeocodeNorthing',
        vac.Latitude                    AS 'Latitude',
        vac.Longitude                   AS 'Longitude',
        vh.HistoryDate                  AS 'VacancyPublishedDateDate',
        convert(VARCHAR, vh.HistoryDate, 111) AS 'VacancyPublishedDate',
        vac.VacancyId                   AS 'VacancyURL',
        vac.VacancyLocationTypeId       AS 'VacancyLocationTypeId',
		DO.FullName                     AS 'DeliveryOrganisation',
		MO.TradingName                  AS 'VacancyManager',
		la.CodeName                     AS 'LocalAuthority',
		vac.DeliveryOrganisationId      AS 'DeliveryOrganisationId',
		tp.ProviderSiteId               AS 'TrainingProviderId',
		DOP.IsNASProvider               AS 'IsNasProvider',
		DO.TrainingProviderStatusTypeId AS 'DeliveryOrganisationStatusId',
		vac.VacancyManagerId            AS 'VacancyManagerId',
        vac.VacancyManagerAnonymous     AS 'VacancyManagerAnonymous',
		DOR.ProviderID                  AS 'VacancyOwnerOwnerOrgID',			-- These lines added
		VO.ProviderID                   AS 'DeliveryOrganisationOwnerOrgID',	-- as a fix for ITSM5547830.
		tp.TradingName                  AS 'LearningDeliverySiteName'		-- Lynden Davies. 20120704.

    FROM 
        Vacancy vac
        INNER JOIN [VacancyOwnerRelationship] vpr ON  vac.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]
        INNER JOIN [ProviderSite]             tp  ON  vpr.[ProviderSiteID]             = tp.ProviderSiteID
        INNER JOIN Employer                   emp ON  vpr.EmployerId                   = emp.EmployerId
        left outer join ApprenticeshipFramework fwk on vac.ApprenticeshipFrameworkId = fwk.ApprenticeshipFrameworkId
        left outer join [Reference].[Standard] std on vac.StandardId = std.StandardId
        left outer join ApprenticeshipOccupation occ on fwk.ApprenticeshipOccupationId = occ.ApprenticeshipOccupationId
        INNER JOIN ApprenticeshipType         apt ON  vac.ApprenticeshipType           = apt.ApprenticeshipTypeId 
        INNER JOIN VacancyHistory             vh  ON  vh.VacancyId                     = vac.VacancyId 
		                                          AND vh.VacancyHistoryEventSubTypeId = @liveVacancyStatusID
		                                          AND vh.VacancyHistoryId = (
		                                            SELECT	MAX(vh1.VacancyHistoryId)  
		                                            FROM	VacancyHistory vh1
		                                            WHERE	vh1.VacancyId                    = vac.VacancyId 
		                                            AND     vh1.VacancyHistoryEventSubTypeId = @liveVacancyStatusID
		                                          )
		INNER JOIN Provider                   VO  ON  Vac.ContractOwnerID = VO.ProviderID
		LEFT OUTER JOIN SectorSuccessRates    ssr ON  VO.ProviderID = ssr.ProviderID 
			                                      AND occ.ApprenticeshipOccupationId = ssr.SectorID
        LEFT OUTER JOIN County                cty ON  vac.CountyId = cty.CountyId 
        LEFT OUTER JOIN AdditionalQuestion    aq1 ON  vac.vacancyId = aq1.vacancyId 
                                                  AND aq1.QuestionId = 1           
        LEFT OUTER JOIN AdditionalQuestion    aq2 ON  vac.vacancyId = aq2.vacancyId 
                                                  AND aq2.QuestionId = 2           
        LEFT OUTER JOIN ProviderSiteFramework tpf ON  tpf.ProviderSiteRelationshipID = vpr.[ProviderSiteID] 
		                                          AND vac.apprenticeshipframeworkid = tpf.frameworkid
        LEFT OUTER JOIN 
            (	
                SELECT	Vacancyid , 
                        MAX(CASE WHEN FullName = 'Future prospects'            THEN vtf.[Value] END) AS FutureProspects,
                        MAX(CASE WHEN FullName = 'Training to be provided'     THEN vtf.[Value] END) AS TrainingToBeProvided,
                        MAX(CASE WHEN FullName = 'Skills required'             THEN vtf.[Value] END) AS SkillsRequired,
                        MAX(CASE WHEN FullName = 'Qualifications Required'     THEN vtf.[Value] END) AS QualificationRequired,
                        MAX(CASE WHEN FullName = 'Personal qualities'          THEN vtf.[Value] END) AS PersonalQualities,
                        MAX(CASE WHEN FullName = 'Reality check'               THEN vtf.[Value] END) AS RealityCheck,
                        MAX(CASE WHEN FullName = 'Other important information' THEN vtf.[Value] END) AS OtherImportantInformation
                FROM    VacancyTextFieldValue vtfv 
                INNER JOIN VacancyTextField vtf ON vtf.Field = vtfv.vacancytextfieldValueId 
                GROUP BY vacancyid
			) AS vt ON vt.VacancyId = vac.VacancyId
		LEFT OUTER JOIN dbo.LocalAuthority                la   ON  vac.LocalAuthorityId          = la.LocalAuthorityId		
		--INNER JOIN dbo.LSCRegion reg ON la.LSCRegionId = reg.LSCRegionId
		LEFT OUTER JOIN dbo.LocalAuthorityGroupMembership LAGM ON  LA.LocalAuthorityId           = LAGM.LocalAuthorityID
		LEFT OUTER JOIN dbo.LocalAuthorityGroup           LAG  ON  LAGM.LocalAuthorityGroupID    = LAG.LocalAuthorityGroupID
		LEFT OUTER JOIN dbo.LocalAuthorityGroupType       LAGT ON  LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
		LEFT OUTER JOIN dbo.ProviderSite                  DO   ON  DO.ProviderSiteId             = vac.DeliveryOrganisationId
		LEFT OUTER JOIN ProviderSiteRelationship          DOR  ON  DOR.ProviderSiteId            = vac.DeliveryOrganisationID
		LEFT OUTER JOIN ProviderSiteRelationshipType      DORT ON  DORT.ProviderSiteRelationshipTypeID = DOR.ProviderSiteRelationshipTypeID
		LEFT OUTER JOIN Provider                          DOP  ON  DOP.ProviderId                = DOR.ProviderId
		LEFT OUTER JOIN dbo.ProviderSite                  MO   ON  MO.ProviderSiteId             = vac.VacancyManagerId

    WHERE	vac.Vacancyid IN (
				SELECT VacancyId
				FROM   #AllRecords
				ORDER BY Sort_Id
				OFFSET   (@pageIndex-1) * @pageSize ROWS FETCH NEXT @pageSize ROWS ONLY
			)
	AND     lagt.LocalAuthorityGroupTypeName      = 'Region'
	AND     DORT.ProviderSiteRelationshipTypeName = 'Owner'

	ORDER BY VacancyReferenceNumber;

    SELECT @totalRecords = COUNT(1) FROM #AllRecords;

	SET NOCOUNT OFF;
END;