﻿/* DROP Procedure [dbo].[uspRetrieveFullVacancyXMLDetails2] */
CREATE Procedure [dbo].[uspRetrieveFullVacancyXMLDetails2]
    @vacancyReferenceNumber int = -1,
    @frameworkCode varchar(3) = null,
    @occupationCode varchar(3) = null,
    @countyCode varchar(3) = null,
    @town varchar(255) = null,
    @regionCode varchar(6) = null,
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

    BEGIN
		SELECT *
		INTO   #AllRecords
		FROM   fnGetIdsForApi(@vacancyReferenceNumber, @frameworkCode, @occupationCode, @countyCode, @town, @regionCode, @vacancyPublishedDate, @locationType)

             select 

                vac.VacancyId as 'VacancyId',
                case when vac.EmployerAnonymousName is null then emp.TradingName else vac.EmployerAnonymousName end as 'Employer',
                tp.FullName as 'LearningProvider',
                vac.ShortDescription as 'ShortDescription',
                vac.Description as 'Description',
                vac.Title as 'Title',
                vac.EmployerDescription as 'EmployerDescription',
                vac.AddressLine1 as 'AddressLine1', 
                vac.AddressLine2 as 'AddressLine2', 
                vac.AddressLine3 as 'AddressLine3', 
                vac.AddressLine4 as 'AddressLine4', 
                vac.Town as 'Town',
                Cty.FullName as 'County',
                vac.PostCode as 'Postcode',
                vac.WorkingWeek as 'WorkingWeek',
                vac.WeeklyWage as 'WeeklyWage',
                vac.NumberofPositions as 'NumberOfVacancies',
                vac.VacancyReferenceNumber as 'VacancyReferenceNumber',
                vac.ApplicationClosingDate as 'ClosingDateForApplicationsDate', 
                convert(varchar, vac.ApplicationClosingDate, 111) as 'ClosingDateForApplications', 
                vac.InterviewsFromDate as 'InterviewBeginFromDate', 
                convert(varchar, vac.InterviewsFromDate, 111) as 'InterviewBeginFrom', 
                vac.ExpectedStartDate as 'PossibleStartDateDate', 
                convert(varchar, vac.ExpectedStartDate, 111) as 'PossibleStartDate', 
                -- check re stripping out of inline style information...... check uat db
                isnull(vt.TrainingToBeProvided,'') as 'TrainingToBeProvided',
                tp.CandidateDescription as 'LearningProviderDescription',
                tp.ContactDetailsForCandidate as 'ContactDetails',
                apt.ApprenticeshipTypeId as 'ApprenticeshipTypeId',
                apt.FullName as 'VacancyType',
                fwk.FullName as 'ApprenticeshipFramework',
                CASE WHEN ssr.New = 1 THEN NULL ELSE ssr.PassRate END as 'LearningProviderSectorPassRate',
                vac.ExpectedDuration as 'ExpectedDuration',
                isnull(vt.SkillsRequired,'') as 'SkillsRequired',        
                isnull(vt.QualificationRequired,'') as 'QualificationsRequired',  
                isnull(vt.PersonalQualities,'') as 'PersonalQualities',    
                isnull(vt.FutureProspects,'') as 'FutureProspects',
                isnull(vt.OtherImportantInformation,'') as 'OtherImportantInformation',
                isnull(aq1.Question,'') as 'AdditionalApplicationFormQuestion1',
                isnull(aq2.Question,'') as 'AdditionalApplicationFormQuestion2',
                vh.HistoryDate as 'VacancyPublishedDateDate',
                convert(varchar, vh.HistoryDate, 111) as 'VacancyPublishedDate',
                vac.GeocodeEasting as 'GeocodeEasting',
                vac.GeocodeNorthing as 'GeocodeNorthing',
                vac.Latitude as 'Latitude',
                vac.Longitude as 'Longitude',
                vac.VacancyId as 'VacancyURL',
                vac.EmployersWebsite as 'EmployersWebsite',
                 vac.VacancyLocationTypeId as 'VacancyLocationTypeId',
				tp.TradingName as 'VacancyOwner',
				vo.TradingName as 'ContractOwner',
				DO.FullName as 'DeliveryOrganisation',
				MO.TradingName as 'VacancyManager',
				la.CodeName as 'LocalAuthority',
				vac.DeliveryOrganisationId as 'DeliveryOrganisationId',
				tp.ProviderSiteId as 'TrainingProviderId',
				DOP.IsNASProvider as 'IsNasProvider',
				DO.TrainingProviderStatusTypeId as 'DeliveryOrganisationStatusId',
				vac.VacancyManagerId as 'VacancyManagerId',
				vac.VacancyManagerAnonymous as 'VacancyManagerAnonymous', 
				vac.WageType as 'WageType',
				vac.WageText as 'WageText',
				vac.SmallEmployerWageIncentive as 'SmallEmployerWageIncentive',
				DOR.ProviderID as 'VacancyOwnerOwnerOrgID',			-- These lines added
				VO.ProviderID as 'DeliveryOrganisationOwnerOrgID',	-- as a fix for ITSM5547830.
				tp.TradingName as 'LearningDeliverySiteName'		-- Lynden Davies. 20120704.
            from 
                
                Vacancy vac
                join [VacancyOwnerRelationship] vpr on vac.[VacancyOwnerRelationshipId] = vpr.[VacancyOwnerRelationshipId]
                join [ProviderSite] tp on vpr.[ProviderSiteID] = tp.ProviderSiteID
                join Employer emp on vpr.EmployerId = emp.EmployerId
                join ApprenticeshipFramework fwk on vac.ApprenticeshipFrameworkId = fwk.ApprenticeshipFrameworkId
                join ApprenticeshipOccupation occ on fwk.ApprenticeshipOccupationId = occ.ApprenticeshipOccupationId
                join ApprenticeshipType apt on vac.ApprenticeshipType = apt.ApprenticeshipTypeId 
                join VacancyHistory vh on vh.VacancyId = vac.VacancyId 
                    and vh.VacancyHistoryEventSubTypeId = @liveVacancyStatusID
                    and vh.VacancyHistoryId =	(
                                                select	max(vh1.VacancyHistoryId)  
                                                from	VacancyHistory vh1
                                                where	vh1.VacancyId = vac.VacancyId 
                                                        and	vh1.VacancyHistoryEventSubTypeId = @liveVacancyStatusID
                                            )
				join Provider VO ON Vac.ContractOwnerID = VO.ProviderID
				left outer join SectorSuccessRates ssr on VO.ProviderID = ssr.ProviderID 
					and occ.ApprenticeshipOccupationId = ssr.SectorID
                left outer join County cty on vac.CountyId = cty.CountyId 
                left outer join AdditionalQuestion as aq1 on vac.vacancyId = aq1.vacancyId 
                    and aq1.QuestionId = 1           
                left outer join AdditionalQuestion as aq2 on vac.vacancyId = aq2.vacancyId 
                    and aq2.QuestionId = 2           
                left outer join [ProviderSiteFramework] tpf on tpf.ProviderSiteRelationshipID = vpr.[ProviderSiteID] 
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
				left outer join dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID
				left outer join dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
				left outer join dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
				--inner join dbo.LSCRegion reg ON la.LSCRegionId = reg.LSCRegionId		
				left outer join dbo.ProviderSite DO on DO.ProviderSiteId = vac.DeliveryOrganisationId
				left outer join ProviderSiteRelationship DOR on DOR.ProviderSiteId = vac.DeliveryOrganisationID
				left outer join ProviderSiteRelationshipType DORT on DORT.ProviderSiteRelationshipTypeID = DOR.ProviderSiteRelationshipTypeID
				left outer join Provider DOP on DOP.ProviderId = DOR.ProviderId
 				left outer join dbo.ProviderSite MO on MO.ProviderSiteId = vac.VacancyManagerId
            where              
				(lagt.LocalAuthorityGroupTypeName = N'Region')
				and
				DORT.ProviderSiteRelationshipTypeName = 'Owner'
				and vac.Vacancyid IN (Select VacancyId From #AllRecords ORDER BY VacancyReferenceNumber OFFSET (@pageIndex-1) * @pageSize  ROWS FETCH NEXT @pageSize ROWS ONLY)

			ORDER BY VacancyReferenceNumber

        select @totalRecords = COUNT(1) FROM #AllRecords
	END

    SET NOCOUNT OFF

End