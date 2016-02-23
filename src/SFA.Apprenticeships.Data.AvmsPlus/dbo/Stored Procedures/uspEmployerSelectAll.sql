CREATE PROCEDURE [dbo].[uspEmployerSelectAll]      
AS  
BEGIN  
 SET NOCOUNT ON  
   
 SELECT  
-- [employer].[BusinessDescription] AS 'BusinessDescription',  
-- [employer].[EdsID] AS 'EdsID',  
-- [employer].[EmployerId] AS 'EmployerId',  
-- [employer].[FullName] AS 'FullName',  
-- [employer].[IndustrySector] AS 'IndustrySector',  
-- [employer].[InformationPageLink] AS 'InformationPageLink',  
-- [employer].[IsContractHolder] AS 'IsContractHolder',  
-- [employer].[LscResponsibilityDetails] AS 'LscResponsibilityDetails',  
-- [employer].[NumberofEmployees] AS 'NumberofEmployees',  
-- [employer].[PrimaryContact] AS 'PrimaryContact',  
-- [employer].[RegisteredAddress] AS 'RegisteredAddress',  
-- [employer].[Status] AS 'Status',  
-- [employer].[TradingName] AS 'TradingName'  
[employer].[EmployerId] as 'EmployerId',  
[employer].[EdsUrn] as 'EdsUrn',  
[employer].[FullName] as 'FullName',  
[employer].[TradingName] as 'TradingName',  
[employer].[AddressLine1] as 'AddressLine1',  
[employer].[AddressLine2] as 'AddressLine2',  
[employer].[AddressLine3] as 'AddressLine3',  
[employer].[AddressLine4] as 'AddressLine4',  
[employer].[Town] as 'Town',  
County.FullName as 'County',      
LAG.FullName as 'Region',
[employer].[PostCode] as 'PostCode',  
[employer].[Longitude] as 'Longitude',  
[employer].[Latitude] as 'Latitude',  
[employer].[GeocodeEasting] as 'GeocodeEasting',  
[employer].[GeocodeNorthing] as 'GeocodeNorthing',  
[employer].[PrimaryContact] as 'PrimaryContact',  
--[employer].[BusinessDescription] as 'BusinessDescription',  
--[employer].[WebPage] as 'WebPage',  
--[employer].[LscResponsibilityDetails] as 'LscResponsibilityDetails',  
[employer].[NumberofEmployeesAtSite] as 'NumberofEmployeesAtSite', 
 [employer].[NumberOfEmployeesInGroup] as 'NumberOfEmployeesInGroup', 
--[employer].[Status] as 'Status',  
--[employer].[IsContractHolder] as 'IsContractHolder',  
--[employer].[IndustrySector] as 'IndustrySector',  
[employer].[OwnerOrgnistaion] as 'OwnerOrgnistaion',  
[employer].[CompanyRegistrationNumber] as 'CompanyRegistrationNumber',  
'' as 'SicId',  
[employer].[TotalVacanciesPosted] as 'TotalVacanciesPosted'  
FROM [dbo].[Employer] [employer]
 INNER JOIN dbo.LocalAuthority LA ON [employer].LocalAuthorityId = LA.LocalAuthorityId
 INNER JOIN dbo.LocalAuthorityGroupMembership LAGM ON LA.LocalAuthorityId = LAGM.LocalAuthorityID
 INNER JOIN dbo.LocalAuthorityGroup LAG ON LAGM.LocalAuthorityGroupID = LAG.LocalAuthorityGroupID
 INNER JOIN dbo.LocalAuthorityGroupType LAGT ON LAG.LocalAuthorityGroupTypeID = LAGT.LocalAuthorityGroupTypeID
 AND LocalAuthorityGroupTypeName = N'Region'  
 Left Outer Join County on County.CountyId = [Employer].CountyId    
-- left outer join LSCRegion on LSCRegion.LSCRegionId = [Employer].LSCRegionId    
   
 SET NOCOUNT OFF  
END