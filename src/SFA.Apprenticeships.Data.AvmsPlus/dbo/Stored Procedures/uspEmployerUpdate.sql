CREATE PROCEDURE [dbo].[uspEmployerUpdate]  
 @employerId INT,  
 @edsUrn int,  
 @fullName nvarchar(255),  
 @tradingName nvarchar(255),  
 @addressLine1 nvarchar(50),  
 @addressLine2 nvarchar(50) = NULL,  
 @addressLine3 nvarchar(50) = NULL,  
 @addressLine4 nvarchar(50) = NULL,  
 @town nvarchar(50),  
 @county int,  
 @postcode nvarchar(50),  
 @longitude decimal(20,15) = NULL,  
 @latitude decimal(20,15) = NULL,  
 @GeocodeEasting decimal(20,15) = NULL,  
 @GeocodeNorthing decimal (20,15) = NULL,  
 @primaryContact INT,   
 @businessDescription nvarchar(50) = NULL,  
 --@webpage nvarchar(100) = NULL,  
 @lscResponsibilityDetails nvarchar(200) = NULL,  
 @NumberofEmployeesAtSite int = NULL,  
-- @status nvarchar(30) = NULL,  
-- @isContractHolder bit = NULL,  
-- @industrySector nvarchar(25) = NULL,  
 @OwnerOrganisation nvarchar(255) = NULL,  
 @companyRegistrationNumber varchar(8) = NULL,  
 @sicId INT,  
 @totalVacanciesPosted int,
 @EmployerStatusTypeId int,
 @DisableAllowed bit, 
 @TrackingAllowed bit
  
   
AS  
BEGIN  
  
 --The [dbo].[Employer] table doesn't have a timestamp column. Optimistic concurrency logic cannot be generated  
 SET NOCOUNT ON  
  
 BEGIN TRY  
 UPDATE [dbo].[Employer]   
 SET   
 --[EdsUrn] = ISNULL(@edsUrn,[EdsUrn]),   
 [FullName] = ISNULL(@fullName,[FullName]),  
 [TradingName] = ISNULL(@tradingName,[TradingName]),  
 [AddressLine1]=ISNULL(@addressLine1,[AddressLine1]),  
 [AddressLine2]=ISNULL(@addressLine2,[AddressLine2]),  
 [AddressLine3]=ISNULL(@addressLine3,[AddressLine3]),  
 [AddressLine4]=ISNULL(@addressLine4,[AddressLine4]),  
 [Town]=ISNULL(@town,[Town]),  
 [CountyId]=ISNULL(@county,[CountyId]),  
 [PostCode]=ISNULL(@postcode,[PostCode]),  

 [Longitude]=ISNULL(@longitude,[Longitude]),  
 [Latitude]=ISNULL(@latitude,[Latitude]),  
 [GeocodeEasting]=ISNULL(@geocodeEasting,[GeocodeEasting]),  
 [GeocodeNorthing]=ISNULL(@geocodeNorthing,[GeocodeNorthing]),  
 [PrimaryContact] = ISNULL(@primaryContact,[PrimaryContact]),    
 --[BusinessDescription] = ISNULL(@businessDescription,[BusinessDescription]),   
 --[Webpage]= ISNULL(@webpage,[Webpage]),  
 --[LscResponsibilityDetails] = ISNULL(@lscResponsibilityDetails,[LscResponsibilityDetails]),    
 [NumberofEmployeesAtSite] = ISNULL(@NumberofEmployeesAtSite,[NumberofEmployeesAtSite]),   
 --[Status] = ISNULL(@status,[Status]),   
 --[IsContractHolder] = ISNULL(@isContractHolder,[IsContractHolder]),   
 --[IndustrySector] = ISNULL(@industrySector,[IndustrySector]),   
 [OwnerOrgnistaion] = ISNULL(@OwnerOrganisation,[OwnerOrgnistaion]),  
 [CompanyRegistrationNumber] = ISNULL(@companyRegistrationNumber,[CompanyRegistrationNumber]),  
 [TotalVacanciesPosted]= ISNULL(@totalVacanciesPosted,[TotalVacanciesPosted]),  
 [EmployerStatusTypeId] = @EmployerStatusTypeId,   
 [DisableAllowed] = @DisableAllowed,
 [TrackingAllowed] = @TrackingAllowed  
  
  
 WHERE [EmployerId]=@employerId  
  
 IF @@ROWCOUNT = 0  
 BEGIN  
  RAISERROR('Concurrent update error. Updated aborted.', 16, 2)  
 END  
    END TRY  
  
    BEGIN CATCH  
  EXEC RethrowError;  
 END CATCH   
  
 SET NOCOUNT OFF  
END