CREATE PROCEDURE [dbo].[uspEmployerInsert]  
    --@EmployerId INT,   
    @EdsUrn int ,  
    @FullName nvarchar(255) ,  
    @TradingName nvarchar(255) ,  
    @AddressLine1 nvarchar(50) ,  
    @AddressLine2 nvarchar(50) ,  
    @AddressLine3 nvarchar(50) ,  
    @AddressLine4 nvarchar(50) ,  
    @Town nvarchar(50) ,  
    @County int,  
   -- @Region int,   
    @PostCode nvarchar(50) ,  
	@localAuthorityId int ,
    @Longitude decimal(20, 15),  
    @Latitude decimal(20, 15) ,  
    @GeocodeEasting decimal(20, 15) ,  
    @GeocodeNorthing decimal(20, 15),  
    @PrimaryContact INT ,  
    --@BusinessDescription nvarchar(50),    field is deleted 
    --@WebPage nvarchar(100) ,  
    --@LscResponsibilityDetails nvarchar(255) ,  
    @NumberofEmployeesAtSite int ,  
    --@Status nvarchar(30) ,  
--    @IsContractHolder bit ,  
--    @IndustrySector nvarchar(25) ,  
    @OwnerOrgnistaion varchar(255),   
    @CompanyRegistrationNumber varchar(8) ,  
    @SicId INT ,  
    @TotalVacanciesPosted int ,  
    @Employerid int out ,
     @EmployerStatusTypeId int,
	 @DisableAllowed bit, 
	 @TrackingAllowed bit
  
AS  
BEGIN  
 SET NOCOUNT ON  
   
 BEGIN TRY  
 
--    INSERT INTO [dbo].[Employer] ([BusinessDescription], [EdsID], [EmployerId], [FullName], [IndustrySector], [InformationPageLink], [IsContractHolder], [LscResponsibilityDetails], [NumberofEmployees], [PrimaryContact], [RegisteredAddress], [Status], [TradingName])  
-- VALUES (@businessDescription, @edsID, @employerId, @fullName, @industrySector, @informationPageLink, @isContractHolder, @lscResponsibilityDetails, @numberofEmployees, @primaryContact, @registeredAddress, @status, @tradingName)  
  
--EmployerId removed on 21/Jul/2008 after changes on guid to int  
  
  
  
    INSERT INTO [dbo].[Employer]  
    (  
                  EdsUrn,     
                  FullName,     
                  TradingName,    
                  AddressLine1,
                  AddressLine2,  
                  AddressLine3,
                  AddressLine4,
                  Town,
                  CountyId,
               --   LSCRegionId,
                  PostCode, 
                  LocalAuthorityId, 
                  Longitude,     
                  Latitude,     
                  GeocodeEasting,     
                  GeocodeNorthing,     
                  PrimaryContact,  
                  NumberofEmployeesAtSite,  
                  OwnerOrgnistaion,   
                  CompanyRegistrationNumber,          
                  TotalVacanciesPosted,    
   				  [EmployerStatusTypeId],
				  DisableAllowed,
   				  TrackingAllowed
	)
    VALUES  
            (  
                @EdsUrn,
                @FullName,    
                @TradingName,   
                @AddressLine1,    
                @AddressLine2,  
                @AddressLine3,    
                @AddressLine4,    
                @Town,    
                @County,    
            --    @Region,     
                @PostCode, 
                @localAuthorityId ,
                @Longitude,    
                @Latitude,    
                @GeocodeEasting,    
                @GeocodeNorthing,    
                @PrimaryContact,  
                @NumberofEmployeesAtSite, 
                @OwnerOrgnistaion,   
                @CompanyRegistrationNumber,        
                @TotalVacanciesPosted,
                @EmployerStatusTypeId,
				@DisableAllowed,
                @TrackingAllowed      
            )  
  
        SET @Employerid = SCOPE_IDENTITY()  
        
   
  
    END TRY  
  
    BEGIN CATCH  
  EXEC RethrowError;  
 END CATCH  
      
    SET NOCOUNT OFF  
END