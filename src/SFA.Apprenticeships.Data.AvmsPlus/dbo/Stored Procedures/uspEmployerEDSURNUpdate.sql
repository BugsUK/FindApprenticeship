CREATE PROCEDURE [dbo].[uspEmployerEDSURNUpdate]                                    
(                             
 
@employerId INT, 
@edsUrn INT, 
@fullName NVARCHAR (50), 
@tradingName NVARCHAR (50), 
@addressLine1 NVARCHAR (50), 
@addressLine2 NVARCHAR (50)=NULL, 
@addressLine3 NVARCHAR (50)=NULL, 
@addressLine4 NVARCHAR (50)=NULL, 
@town NVARCHAR (50), 
@county INT, 
@region INT, 
@postcode NVARCHAR (50), 
@longitude DECIMAL (20, 15)=NULL, 
@latitude DECIMAL (20, 15)=NULL, 
@GeocodeEasting DECIMAL (20, 15)=NULL, 
@GeocodeNorthing DECIMAL (20, 15)=NULL, 
@primaryContact INT, 
@businessDescription NVARCHAR (50)=NULL, 
@lscResponsibilityDetails NVARCHAR (200)=NULL, 
@NumberofEmployeesAtSite INT=NULL, 
@OwnerOrganisation NVARCHAR (255)=NULL, 
@companyRegistrationNumber VARCHAR (8)=NULL, 
@sicId INT, 
@totalVacanciesPosted INT
)                              
AS                                
BEGIN                                
                            
SET NOCOUNT ON                                
          
BEGIN TRY  
 UPDATE [dbo].[Employer]   
 SET   
 [EdsUrn] = ISNULL(@edsUrn,[EdsUrn]),   
 [FullName] = ISNULL(@fullName,[FullName]),  
 [TradingName] = ISNULL(@tradingName,[TradingName]),  
 [AddressLine1]=ISNULL(@addressLine1,[AddressLine1]),  
 [AddressLine2]=ISNULL(@addressLine2,[AddressLine2]),  
 [AddressLine3]=ISNULL(@addressLine3,[AddressLine3]),  
 [AddressLine4]=ISNULL(@addressLine4,[AddressLine4]),  
 [Town]=ISNULL(@town,[Town]),  
 [CountyId]=ISNULL(@county,[CountyId]),  
-- [LSCRegionId]=ISNULL(@region,[LSCRegionId]),  
 [Postcode]=ISNULL(@postcode,[PostCode]),  

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
 [TotalVacanciesPosted]= ISNULL(@totalVacanciesPosted,[TotalVacanciesPosted])
 --[EmployerStatusTypeId] = @EmployerStatusTypeId   
   
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