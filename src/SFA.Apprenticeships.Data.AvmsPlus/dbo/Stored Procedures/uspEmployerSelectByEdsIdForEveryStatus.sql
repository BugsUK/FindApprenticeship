CREATE PROCEDURE [dbo].[uspEmployerSelectByEdsIdForEveryStatus]  
 @EdsId int  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
SELECT  
	 [employer].[EdsUrn]  AS 'EdsUrn',  
	 [employer].[EmployerId] AS 'EmployerId',  
	 isnull([employer].[FullName],'') AS 'FullName',  
	 isnull([employer].[PrimaryContact],'') AS 'PrimaryContact',  
	 --isnull([employer].[Status],'') AS 'Status',  : Filed is deleted as per new requirement. 
	 isnull([employer].[TradingName],'') AS 'TradingName',  
	 [employer].[TotalVacanciesPosted] AS 'TotalVacanciesPosted',  
	 isnull([employer].[OwnerOrgnistaion],'') as 'OwnerOrgnistaion',  
	 isnull([employer].[CompanyRegistrationNumber],'') as  'CompanyRegistrationNumber',  
	 isnull([employer].[AddressLine1],'') as 'AddressLine1',  
	 isnull([employer].[AddressLine2],'') as 'AddressLine2',  
	 isnull([employer].[AddressLine3],'') as 'AddressLine3',  
	 isnull([employer].[AddressLine4],'') as 'AddressLine4',  
	 isnull([employer].[Town],'') as 'Town',  
	 [EmployerCounty].[FullName] as 'County',  
	 [employer].CountyId, 
	 --LAG.[FullName] as 'Region',  
	 --LAG.LocalAuthorityGroupID as 'RegionId', 
	 vwLA.GeographicRegionID 'GeographicRegionID',
	 vwLA.GeographicCodeName 'GeographicCodeName',
	 vwLA.GeographicShortName 'GeographicShortName',
	 vwLA.GeographicFullName 'GeographicFullName', 
	 isnull([employer].[PostCode],'') as 'PostCode',  
	 [employer].[Longitude] as 'Longitude',  
	 [employer].[Latitude] as 'Latitude',  
	 [employer].[GeocodeEasting] as 'GeocodeEasting',  
	 [employer].[GeocodeNorthing] as 'GeocodeNorthing',  
	 [person].[PersonId] as 'PersonId',    
	 isnull([person].[Title],'') as 'PersonTitle',  
	 isnull([person].[OtherTitle],'') as 'PersonOtherTitle',    
	 isnull([person].[FirstName],'') as 'PersonFirst',    
	 isnull([person].[MiddleNames],'') as 'PersonMiddle',    
	 isnull([person].[Surname],'') as 'PersonSurname',    
	 isnull([EC].[JobTitle],'') as 'PersonJobTitle',    
	 isnull([person].[LandlineNumber],'') as 'PersonLandlineNumber',    
	 isnull([person].[MobileNumber],'') as 'PersonMobileNumber',   
	 isnull([person].[Email],'') as 'PersonEmail',     
	 isnull([CP].[FullName],'') as 'PersonContactPreference',    
	 isnull([personType].[FullName],'') as 'PersonType',    
	 isnull([EC].[Availability],'') as 'PersonAvailability',     
	 isnull([ec].[AddressLine1],'') as 'PersonAddressLine1',    
	 isnull([ec].[AddressLine2],'') as 'PersonAddressLine2',    
	 isnull([ec].[AddressLine3],'') as 'PersonAddressLine3',    
	 isnull([ec].[AddressLine4] ,'')as 'PersonAddressLine4',   
	 isnull([ec].[Town],'') as 'PersonTown',  
	 isnull([County].[FullName],'') as 'PersonCounty',
	 isnull([ec].[CountyId], 0) as 'PersonCountyId',
	 isnull([ec].[PostCode],'') as 'PersonPostcode',  
	 [ec].[FaxNumber] as 'PersonFaxNumber',
	 ISNULL(EmployerStatusTypeId,1) as 'EmployerStatus'   
FROM [dbo].[Employer] [employer]
JOIN [vwRegionsAndLocalAuthority] vwLA 
ON [employer].LocalAuthorityId = vwLA.LocalAuthorityId   
LEFT JOIN dbo.EmployerContact ec ON ec.EmployerContactId = [employer].[PrimaryContact]  
LEFT JOIN [dbo].[Person] [person] ON [person].[PersonId] = ec.PersonId    
LEFT JOIN [dbo].[County] [County] ON [County].[CountyId] = ec.[CountyId]  
LEFT JOIN [dbo].[County] [EmployerCounty] ON [EmployerCounty].[CountyId] = [Employer].[CountyId]  
LEFT JOIN [dbo].[ContactPreferenceType] CP ON CP.[ContactPreferenceTypeId] = ec.[ContactPreferenceTypeId]  
LEFT JOIN PersonType on person.personTypeId=personType.PersonTypeId  
--left join LSCRegion on [LSCRegion].[LSCRegionId] = [employer].[LSCRegionId]
WHERE [employer].[EdsUrn]=@EdsId 
END