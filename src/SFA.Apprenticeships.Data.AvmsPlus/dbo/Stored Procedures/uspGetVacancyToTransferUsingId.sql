CREATE PROCEDURE [dbo].[uspGetVacancyToTransferUsingId]
	@vacancyId int,
	@IsChild bit = 1,
	@AcceptableStatus bit = 0
AS
BEGIN

		 
SELECT   DISTINCT
                        ps.ProviderSiteID,  
                        ps.FullName AS TrainingProviderFullName,  
                        ps.TradingName AS TrainingProviderTradingName,  
                        ps.WebPage AS TrainingProviderWebPage,  
                        ps.Town AS TrainingProviderTown,  
                        ps.PostCode AS TrainingProviderPostCode,  
                        ps.TrainingProviderStatusTypeId,  
  
                        LAG.LocalAuthorityGroupID AS ManagingAreaId,  
                        LAG.CodeName AS ManagingAreaCodeName,  
                        LAG.FullName AS ManagingAreaFullName,  
                        LAG.ShortName AS ManagingAreaShortName,  
  
                        emp.EmployerId,  
                        emp.FullName AS EmployerFullName,  
                        emp.TradingName AS EmployerTradingName,  
                        emp.OwnerOrgnistaion AS EmployerOwnerOrganisation,  
                        emp.Town AS EmployerTown,  
                        emp.PostCode AS EmployerPostCode,  
  
                        v.VacancyId,  
                        v.MasterVacancyId AS VacancyMasterVacancyId,  
                        v.PostCode AS VacancyPostCode,  
                        v.ShortDescription AS VacancyShortDescription,  
                        v.Title AS VacancyTitle,  
                        v.VacancyReferenceNumber,  
  
                        vst.VacancyStatusTypeId,  
                        vst.CodeName AS VacancyStatusCodeName,  
                        vst.FullName AS VacancyStatusFullName,  
                        vst.ShortName AS VacancyStatusShortName,  
                        v.Town AS VacancyLocation,  
						v.VacancyLocationTypeId as VacancyLocationTypeId,
                        af.ApprenticeshipFrameworkId,  
                        af.CodeName AS ApprenticeFrameworkCodeName,  
                        af.ShortName AS ApprenticeFrameworkShortName,  
                        af.FullName AS ApprenticeFrameworkFullName,  
                          
                        vlt.CodeName AS VacancyLocationCode,  
                        vlt.ShortName AS VacancyLocationShortName,  
                        -- This is used to display the town on the UI as it uses the   
                        -- VacancyLocationFullName to display town, multi-location or nationwide  
                        v.Town AS VacancyLocationFullName,  
                        
                        p.ProviderId AS ContractOwnwerId,
                        p.TradingName AS ContractOwnerTradingName, 
                        psv.ProviderSiteID AS VacancyManagerId,  
                        psv.TradingName AS VacancyManagerFullName,  
                        psd.ProviderSiteID AS DelivererID,  
                        psd.TradingName AS DelivererFullName,  
                        poc.ProviderId As OriginalContractOwnerId,
                        poc.TradingName As OriginalContractOwnerTradingName,
                        
                        -- Used to make sure multiple and national locations are at the end / beginning  
                        -- and sub sorted by multiple locations then national  
                        1 AS SingleOrMultipleLocationSortOrder,  
                        1 AS SubSingleOrMultipleLocationSortOrder
						                        
                    FROM
						dbo.Vacancy  v 
						INNER JOIN dbo.Provider P ON v.ContractOwnerId = P.ProviderID  
						INNER JOIN dbo.[VacancyOwnerRelationship] rel ON v.[VacancyOwnerRelationshipId] = rel.[VacancyOwnerRelationshipId]  
						INNER JOIN dbo.ProviderSite PS ON rel.ProviderSiteID = PS.ProviderSiteID  
						INNER JOIN dbo.Employer emp ON rel.EmployerID = emp.EmployerID  
						LEFT JOIN dbo.LocalAuthority LA ON v.LocalAuthorityId = LA.LocalAuthorityId  
						INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID  
						INNER JOIN dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID  
						INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID  
								    AND LAGT.LocalAuthorityGroupTypeName = N'Managing Area'  
						INNER JOIN dbo.VacancyStatusType vst ON vst.VacancyStatusTypeId = v.VacancyStatusId  
						LEFT JOIN ApprenticeshipFramework af ON af.ApprenticeshipFrameworkId = v.ApprenticeshipFrameworkId  
						INNER JOIN VacancyLocationType vlt ON vlt.VacancyLocationTypeId = v.VacancyLocationTypeId   
						INNER JOIN ProviderSite PSD ON PSD.ProviderSiteId = v.DeliveryOrganisationID
						INNER JOIN ProviderSite PSV ON PSV.ProviderSiteId = v.VacancyManagerID
						LEFT JOIN Provider POC ON POC.ProviderId = v.OriginalContractOwnerId
					Where 
					((@IsChild = 1 AND v.VacancyId = @vacancyId) OR
					(@IsChild = 0 AND v.MasterVacancyId = @vacancyId))
					AND
					(@AcceptableStatus = 1 OR (@AcceptableStatus = 0 AND vst.CodeName IN ( 'Lve', 'Dft', 'Sub', 'Ref','Cld', 'Com', 'Wdr', 'Pie', 'Del')))



END