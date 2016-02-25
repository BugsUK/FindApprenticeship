
CREATE Procedure [dbo].[uspRetrieveSummaryVacancyXMLDetails]
    @vacancyReferenceNumber int = -1,
    @frameworkCode varchar(3) = null,
    @occupationCode varchar(3) = null,
    @countyCode varchar(3) = null,
    @town varchar(255) = null,
    @regionCode varchar(3) = null,
    @vacancyPublishedDate datetime = null,
    @locationType int = -1,
    @pageSize int = 25,
    @pageIndex int = 1,
    @totalRecords int out
As
Begin

    SET NOCOUNT ON
    
    -- obtain the vacancy live status ID for use in the main query
    declare @liveVacancyStatusID int =	(
                                            select	VacancyStatusTypeId 
                                            from	VacancyStatusType 
                                            where	CodeName = 'Lve' 
                                        )

    -- obtain Apprenticeship Framework Id from parameter supplied			
    declare @frameworkId int = null
    IF @frameworkCode IS NOT NULL and @frameworkCode <> ''
        select	@frameworkId = ApprenticeshipFrameworkId
        from	ApprenticeshipFramework
        where	CodeName = @frameworkCode
	else
		select @frameworkId = -1

    -- obtain Occupation Id from parameter supplied			
    declare @occupationId int =	null
    IF @occupationCode IS NOT NULL and @occupationCode <> ''
        select	@occupationId = ApprenticeshipOccupationId
        from	ApprenticeshipOccupation
        where	Codename = @occupationCode
	else
		select @occupationId = -1

    -- obtain County Id from parameter supplied			
    declare @countyId int = null
    IF @countyCode IS NOT NULL and @countyCode <> ''
        select  @countyId = CountyId
        from    County
        where   CodeName = @countyCode
	else
		select @countyId = -1

    -- calculate row numbers
    declare @startRowNo int = ((@PageIndex-1)* @PageSize)+1
    declare @endRowNo int = (@PageIndex * @PageSize);
BEGIN
        WITH tempVacancyDetails as
        (    
            select 
                vac.VacancyId as 'VacancyId',
                vac.Title as 'Title',
                vac.VacancyReferenceNumber as 'VacancyReferenceNumber',
                case when vac.EmployerAnonymousName is null then emp.TradingName else vac.EmployerAnonymousName end as 'Employer',
                tp.FullName as 'LearningProvider',
                vac.ShortDescription as 'Description',
                apt.ApprenticeshipTypeId as 'ApprenticeshipTypeId',
                apt.FullName as 'VacancyType',
                vac.NumberofPositions as 'NumberOfVacancies',
                vac.AddressLine1 as 'AddressLine1', 
                vac.AddressLine2 as 'AddressLine2', 
                vac.AddressLine3 as 'AddressLine3', 
                vac.AddressLine4 as 'AddressLine4', 
                vac.Town as 'Town',
                Cty.FullName as 'County',
                vac.PostCode as 'Postcode',
                fwk.FullName as 'ApprenticeshipFramework',
                vac.ApplicationClosingDate as 'ClosingDateForApplicationsDate', 
                convert(varchar, vac.ApplicationClosingDate, 111) as 'ClosingDateForApplications', 
                vac.GeocodeEasting as 'GeocodeEasting',
                vac.GeocodeNorthing as 'GeocodeNorthing',
                vac.Latitude as 'Latitude',
                vac.Longitude as 'Longitude',
                vh.HistoryDate as 'VacancyPublishedDateDate',
                convert(varchar, vh.HistoryDate, 111) as 'VacancyPublishedDate',
                vac.VacancyId as 'VacancyURL',
                vac.VacancyLocationTypeId as 'VacancyLocationTypeId',
				DO.FullName as 'DeliveryOrganisation',
				MO.TradingName as 'VacancyManager',
				la.CodeName as 'LocalAuthority',
				vac.DeliveryOrganisationId as 'DeliveryOrganisationId',
				tp.ProviderSiteId as 'TrainingProviderId',
				DOP.IsNasProvider as 'IsNasProvider',
				DO.TrainingProviderStatusTypeId as 'DeliveryOrganisationStatusId',
				vac.VacancyManagerId as 'VacancyManagerId',
				vac.VacancyManagerAnonymous as 'VacancyManagerAnonymous', 
                ROW_NUMBER() OVER (ORDER BY vac.VacancyReferenceNumber) AS 'RowNumber',
				DOR.ProviderID as 'VacancyOwnerOwnerOrgID',			-- These lines added
				VO.ProviderID as 'DeliveryOrganisationOwnerOrgID',	-- as a fix for ITSM5547830.
				tp.TradingName as 'LearningDeliverySiteName'		-- Lynden Davies. 20120704.

            from 
                
                Vacancy vac
                inner join [VacancyOwnerRelationship] vpr
                    on vac.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]
                inner join [ProviderSite] tp
                    on vpr.[ProviderSiteID] = tp.ProviderSiteID
                inner join Employer emp
                    on vpr.EmployerId = emp.EmployerId
                inner join ApprenticeshipFramework fwk
                    on vac.ApprenticeshipFrameworkId = fwk.ApprenticeshipFrameworkId
                inner join ApprenticeshipOccupation occ
                    on fwk.ApprenticeshipOccupationId = occ.ApprenticeshipOccupationId
                inner join ApprenticeshipType apt
                    on vac.ApprenticeshipType = apt.ApprenticeshipTypeId 
                inner join VacancyHistory vh
                    on vh.VacancyId = vac.VacancyId 
                    and vh.VacancyHistoryEventSubTypeId = @liveVacancyStatusID
                    and vh.VacancyHistoryId =	(
                                                select	max(vh1.VacancyHistoryId)  
                                                from	VacancyHistory vh1
                                                where	vh1.VacancyId = vac.VacancyId 
                                                        and	vh1.VacancyHistoryEventSubTypeId = @liveVacancyStatusID
                                            )
				INNER JOIN Provider VO ON Vac.ContractOwnerID = VO.ProviderID
				left outer JOIN SectorSuccessRates ssr on VO.ProviderID = ssr.ProviderID 
					and occ.ApprenticeshipOccupationId = ssr.SectorID
                left outer join County cty
                    on vac.CountyId = cty.CountyId 
                left outer join AdditionalQuestion as aq1 
                    on vac.vacancyId = aq1.vacancyId 
                    and aq1.QuestionId = 1           
                left outer join AdditionalQuestion as aq2 
                    on vac.vacancyId = aq2.vacancyId 
                    and aq2.QuestionId = 2           
                left outer join [ProviderSiteFramework] tpf 
                    on tpf.ProviderSiteRelationshipID = vpr.[ProviderSiteID] 
                    and vac.apprenticeshipframeworkid = tpf.frameworkid
	
                left outer join 
                    (	
                        select	vacancyid , 
                                Max(case when FullName = 'Future prospects' then vtf.[Value] end) as FutureProspects,
                                Max(case when FullName = 'Training to be provided' then vtf.[Value] end) as TrainingToBeProvided,
                                Max(case when FullName = 'Skills required' then vtf.[Value] end) as SkillsRequired,
                                Max(case when FullName = 'Qualifications Required' then vtf.[Value] end) as QualificationRequired,
                                Max(case when FullName = 'Personal qualities' then vtf.[Value] end) as PersonalQualities,
                                Max(case when FullName = 'Reality check' then vtf.[Value] end) as RealityCheck,
                                Max(case when FullName = 'Other important information' then vtf.[Value] end) as OtherImportantInformation
                        from	
                            vacancytextfieldValue vtfv 
                            inner join vacancytextfield vtf 
                                on vtf.Field = vtfv.vacancytextfieldValueId 
                        group by vacancyid
                    ) as vt on vt.VacancyId = vac.VacancyId 
					left outer join dbo.LocalAuthority la ON vac.LocalAuthorityId = la.LocalAuthorityId		
					--inner join dbo.LSCRegion reg ON la.LSCRegionId = reg.LSCRegionId
				    left outer join dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID
                    left outer join dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
                    left outer join dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
					left outer join dbo.ProviderSite DO on DO.ProviderSiteId = vac.DeliveryOrganisationId
					left outer join ProviderSiteRelationship DOR on DOR.ProviderSiteId = vac.DeliveryOrganisationID
					left outer join ProviderSiteRelationshipType DORT on DORT.ProviderSiteRelationshipTypeID = DOR.ProviderSiteRelationshipTypeID
					left outer join Provider DOP on DOP.ProviderId = DOR.ProviderId
 					left outer join dbo.ProviderSite MO on MO.ProviderSiteId = vac.VacancyManagerId             
            where 
             
                (vac.VacancyStatusId = @liveVacancyStatusID)
                and
                (vac.VacancyReferenceNumber = @vacancyReferenceNumber or @vacancyReferenceNumber = -1) 
				and 
				(lagt.LocalAuthorityGroupTypeName = N'Region')
				and
				DORT.ProviderSiteRelationshipTypeName = 'Owner'
                and
                (vac.ApprenticeshipFrameworkId = @frameworkId or @frameworkId = -1)
                and
                (occ.ApprenticeshipOccupationId = @occupationId or @occupationId = -1)
                and
                (vh.HistoryDate > @vacancyPublishedDate or @vacancyPublishedDate is null)
                and
                (
					(@locationType = 1 and vac.VacancyLocationTypeId = 3) 
					or
					((@locationType = 0 and vac.VacancyLocationTypeId in (1,2)
					and
					(vac.CountyId = @countyId or @countyId = -1)
					and
					(vac.Town = @town or @town is null or @town = '')
					and
					(LAG.CodeName = @regionCode or @regionCode IS NULL or @regionCode = '')))
				)
        )
            select 
                VacancyId,
                Title,
                VacancyReferenceNumber,
                Employer,
                LearningProvider,
                Description,
                ApprenticeshipTypeId,
                VacancyType,
                NumberOfVacancies,
                AddressLine1, 
                AddressLine2, 
                AddressLine3, 
                AddressLine4, 
                Town,
                County,
                Postcode,
                ApprenticeshipFramework,
                ClosingDateForApplicationsDate,
                ClosingDateForApplications, 
                GeocodeEasting,
                GeocodeNorthing,
                Latitude,
                Longitude,
                VacancyPublishedDateDate,
                VacancyPublishedDate,
                VacancyURL,
                VacancyLocationTypeId,
				DeliveryOrganisation,
				VacancyManager,
				LocalAuthority,
				DeliveryOrganisationId,
				TrainingProviderId,
				IsNasProvider,
				DeliveryOrganisationStatusId,
				VacancyManagerId,
				VacancyManagerAnonymous,
				VacancyOwnerOwnerOrgID,			-- These lines added
				DeliveryOrganisationOwnerOrgID,	-- as a fix for ITSM5547830.
				LearningDeliverySiteName		-- Lynden Davies. 20120704.

        from 

            tempVacancyDetails

        where RowNumber between @startRowNo and @endRowNo

        order by RowNumber

        select 

            @totalRecords = COUNT(1)

        from 
                Vacancy vac
                inner join [VacancyOwnerRelationship] vpr
                    on vac.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]
                inner join [ProviderSite] tp
                    on vpr.[ProviderSiteID] = tp.ProviderSiteID
                inner join Employer emp
                    on vpr.EmployerId = emp.EmployerId
                inner join ApprenticeshipFramework fwk
                    on vac.ApprenticeshipFrameworkId = fwk.ApprenticeshipFrameworkId
                inner join ApprenticeshipOccupation occ
                    on fwk.ApprenticeshipOccupationId = occ.ApprenticeshipOccupationId
                inner join ApprenticeshipType apt
                    on vac.ApprenticeshipType = apt.ApprenticeshipTypeId 
                inner join VacancyHistory vh
                    on vh.VacancyId = vac.VacancyId 
                    and vh.VacancyHistoryEventSubTypeId = @liveVacancyStatusID
                    and vh.VacancyHistoryId =	(
                                                select	max(vh1.VacancyHistoryId)  
                                                from	VacancyHistory vh1
                                                where	vh1.VacancyId = vac.VacancyId 
                                                        and	vh1.VacancyHistoryEventSubTypeId = @liveVacancyStatusID
                                            )
				INNER JOIN Provider VO ON Vac.ContractOwnerID = VO.ProviderID
				left outer JOIN SectorSuccessRates ssr on VO.ProviderID = ssr.ProviderID 
					and occ.ApprenticeshipOccupationId = ssr.SectorID
                left outer join County cty
                    on vac.CountyId = cty.CountyId 
                left outer join AdditionalQuestion as aq1 
                    on vac.vacancyId = aq1.vacancyId 
                    and aq1.QuestionId = 1           
                left outer join AdditionalQuestion as aq2 
                    on vac.vacancyId = aq2.vacancyId 
                    and aq2.QuestionId = 2           
                left outer join [ProviderSiteFramework] tpf 
                    on tpf.ProviderSiteRelationshipID = vpr.[ProviderSiteID] 
                    and vac.apprenticeshipframeworkid = tpf.frameworkid
	
                left outer join 
                    (	
                        select	vacancyid , 
                                Max(case when FullName = 'Future prospects' then vtf.[Value] end) as FutureProspects,
                                Max(case when FullName = 'Training to be provided' then vtf.[Value] end) as TrainingToBeProvided,
                                Max(case when FullName = 'Skills required' then vtf.[Value] end) as SkillsRequired,
                                Max(case when FullName = 'Qualifications Required' then vtf.[Value] end) as QualificationRequired,
                                Max(case when FullName = 'Personal qualities' then vtf.[Value] end) as PersonalQualities,
                                Max(case when FullName = 'Reality check' then vtf.[Value] end) as RealityCheck,
                                Max(case when FullName = 'Other important information' then vtf.[Value] end) as OtherImportantInformation
                        from	
                            vacancytextfieldValue vtfv 
                            inner join vacancytextfield vtf 
                                on vtf.Field = vtfv.vacancytextfieldValueId 
                        group by vacancyid
                    ) as vt on vt.VacancyId = vac.VacancyId 
					left outer join dbo.LocalAuthority la ON vac.LocalAuthorityId = la.LocalAuthorityId		
					--inner join dbo.LSCRegion reg ON la.LSCRegionId = reg.LSCRegionId
				    left outer join dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID
                    left outer join dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
                    left outer join dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
					left outer join dbo.ProviderSite DO on DO.ProviderSiteId = vac.DeliveryOrganisationId
					left outer join ProviderSiteRelationship DOR on DOR.ProviderSiteId = vac.DeliveryOrganisationID
					left outer join ProviderSiteRelationshipType DORT on DORT.ProviderSiteRelationshipTypeID = DOR.ProviderSiteRelationshipTypeID
					left outer join Provider DOP on DOP.ProviderId = DOR.ProviderId
 					left outer join dbo.ProviderSite MO on MO.ProviderSiteId = vac.VacancyManagerId             
            where 
             
                (vac.VacancyStatusId = @liveVacancyStatusID)
                and
                (vac.VacancyReferenceNumber = @vacancyReferenceNumber or @vacancyReferenceNumber = -1) 
				and 
				(lagt.LocalAuthorityGroupTypeName = N'Region')
				and
				DORT.ProviderSiteRelationshipTypeName = 'Owner'
                and
                (vac.ApprenticeshipFrameworkId = @frameworkId or @frameworkId = -1)
                and
                (occ.ApprenticeshipOccupationId = @occupationId or @occupationId = -1)
                and
                (vh.HistoryDate > @vacancyPublishedDate or @vacancyPublishedDate is null)
                and
                (
					(@locationType = 1 and vac.VacancyLocationTypeId = 3) 
					or
					((@locationType = 0 and vac.VacancyLocationTypeId in (1,2)
					and
					(vac.CountyId = @countyId or @countyId = -1)
					and
					(vac.Town = @town or @town is null or @town = '')
					and
					(LAG.CodeName = @regionCode or @regionCode IS NULL or @regionCode = '')))
				)
 
 END           
 SET NOCOUNT OFF

End