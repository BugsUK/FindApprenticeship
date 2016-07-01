/* DROP FUNCTION [dbo].[[fnGetIdsForApi]] */
CREATE FUNCTION [dbo].[fnGetIdsForApi]
(
    @vacancyReferenceNumber int = -1,
    @frameworkCode varchar(3) = null,
    @occupationCode varchar(3) = null,
    @countyCode varchar(3) = null,
    @town varchar(255) = null,
    @regionCode varchar(6) = null,
    @vacancyPublishedDate datetime = null,
    @locationType int = -1
)
RETURNS @Result TABLE
(
	VacancyId INT,
	VacancyReferenceNumber VARCHAR(100)
)
As
Begin
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


			INSERT @Result
          select
                vac.VacancyId, VacancyReferenceNumber
            from 
                
                Vacancy vac
                join ApprenticeshipFramework fwk on vac.ApprenticeshipFrameworkId = fwk.ApprenticeshipFrameworkId
                join ApprenticeshipOccupation occ on fwk.ApprenticeshipOccupationId = occ.ApprenticeshipOccupationId
                join VacancyHistory vh on vh.VacancyId = vac.VacancyId 
                    and vh.VacancyHistoryEventSubTypeId = @liveVacancyStatusID
                    and vh.VacancyHistoryId =	(
                                                select	max(vh1.VacancyHistoryId)  
                                                from	VacancyHistory vh1
                                                where	vh1.VacancyId = vac.VacancyId 
                                                        and	vh1.VacancyHistoryEventSubTypeId = @liveVacancyStatusID
                                            )
				left outer join dbo.LocalAuthority la ON vac.LocalAuthorityId = la.LocalAuthorityId		
				left outer join dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID
				left outer join dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
				left outer join dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
				left outer join dbo.ProviderSite DO on DO.ProviderSiteId = vac.DeliveryOrganisationId
				left outer join ProviderSiteRelationship DOR on DOR.ProviderSiteId = vac.DeliveryOrganisationID
				left outer join ProviderSiteRelationshipType DORT on DORT.ProviderSiteRelationshipTypeID = DOR.ProviderSiteRelationshipTypeID
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

	RETURN
End