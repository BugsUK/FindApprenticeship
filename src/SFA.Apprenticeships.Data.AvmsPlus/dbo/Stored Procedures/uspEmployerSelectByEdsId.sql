CREATE PROCEDURE [dbo].[uspEmployerSelectByEdsId]  
 @EdsId int  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
SELECT    
	 [Employer].[EdsUrn]  AS 'EdsUrn',    
	 [Employer].[EmployerId] AS 'EmployerId',    
	 isnull([Employer].[FullName],'') AS 'FullName',    
	 isnull([Employer].[PrimaryContact],'') AS 'PrimaryContact',    
	 --isnull([employer].[Status],'') AS 'Status',  : Filed is deleted as per new requirement.   
	 isnull([Employer].[TradingName],'') AS 'TradingName',    
	 [Employer].[TotalVacanciesPosted] AS 'TotalVacanciesPosted',    
	 isnull([Employer].[OwnerOrgnistaion],'') as 'OwnerOrgnistaion',    
	 isnull([Employer].[CompanyRegistrationNumber],'') as  'CompanyRegistrationNumber',    
	 isnull([Employer].[AddressLine1],'') as 'AddressLine1',    
	 isnull([Employer].[AddressLine2],'') as 'AddressLine2',    
	 isnull([Employer].[AddressLine3],'') as 'AddressLine3',    
	 isnull([Employer].[AddressLine4],'') as 'AddressLine4',    
	 isnull([Employer].[Town],'') as 'Town',    
	 [EmployerCounty].[FullName] as 'County',    
	 [Employer].CountyId,   
	 --LAG.[FullName] as 'Region',    
	 --LAG.LocalAuthorityGroupID as 'RegionId', 
	 vwLA.GeographicRegionID 'GeographicRegionID',
	 vwLA.GeographicCodeName 'GeographicCodeName',
	 vwLA.GeographicShortName 'GeographicShortName',
	 vwLA.GeographicFullName 'GeographicFullName',  
	 isnull([employer].[PostCode],'') as 'PostCode',    
	 [Employer].[Longitude] as 'Longitude',    
	 [Employer].[Latitude] as 'Latitude',    
	 [Employer].[GeocodeEasting] as 'GeocodeEasting',    
	 [Employer].[GeocodeNorthing] as 'GeocodeNorthing',    
	 [person].[PersonId] as 'PersonId',      
	 isnull([Person].[Title],'') as 'PersonTitle',    
	 isnull([Person].[OtherTitle],'') as 'PersonOtherTitle',      
	 isnull([Person].[FirstName],'') as 'PersonFirst',      
	 isnull([Person].[MiddleNames],'') as 'PersonMiddle',      
	 isnull([Person].[Surname],'') as 'PersonSurname',      
	 isnull([EC].[JobTitle],'') as 'PersonJobTitle',      
	 isnull([Person].[LandlineNumber],'') as 'PersonLandlineNumber',      
	 isnull([Person].[MobileNumber],'') as 'PersonMobileNumber',     
	 isnull([Person].[Email],'') as 'PersonEmail',       
	 isnull([CP].[FullName],'') as 'PersonContactPreference',      
	 isnull([PersonType].[FullName],'') as 'PersonType',      
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
	 ISNULL(EmployerStatusTypeId,1) as 'EmployerStatus',   
	 DisableAllowed,  
	TrackingAllowed   
FROM [dbo].[Employer] [Employer]
JOIN [vwRegionsAndLocalAuthority] vwLA 
ON [employer].LocalAuthorityId = vwLA.LocalAuthorityId  
	LEFT JOIN dbo.employercontact ec ON ec.EmployerContactId = [employer].[PrimaryContact]  
	LEFT JOIN [dbo].[Person] [person] ON [person].[PersonId] = ec.PersonId    
	LEFT JOIN [dbo].[County] [County] ON [County].[CountyId] = ec.[CountyId]  
	LEFT JOIN [dbo].[County] [EmployerCounty] ON [EmployerCounty].[CountyId] = [Employer].[CountyId]  
	LEFT JOIN [dbo].[ContactPreferenceType] CP ON CP.[ContactPreferenceTypeId] = ec.[ContactPreferenceTypeId]  
	LEFT JOIN PersonType on person.personTypeId=personType.PersonTypeId  
--left join LSCRegion on [LSCRegion].[LSCRegionId] = [employer].[LSCRegionId]
WHERE [employer].[EdsUrn]=@EdsId  
AND [employer].[EmployerStatusTypeId] != 2  
END