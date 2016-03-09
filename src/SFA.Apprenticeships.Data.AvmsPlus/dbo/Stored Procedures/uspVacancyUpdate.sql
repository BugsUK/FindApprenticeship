CREATE PROCEDURE [dbo].[uspVacancyUpdate]        
         
 @VacancyId int,
 @MasterVacancyId int,
 @VacancyReferenceNumber int,        
 @RelationShipId int,      
 @ContactName nvarchar(100),        
 @VacancyStatusId int,        
 @AddressLine1 nvarchar(40),        
 @AddressLine2 nvarchar(40),        
 @AddressLine3 nvarchar(40),        
 @AddressLine4 nvarchar(40),        
 @Town nvarchar(40),        
 @CountyId int,        
 @PostCode nvarchar(8), 
 @LocalAuthorityId int,       
 @GeocodeEasting decimal(30, 15),        
 @GeocodeNorthing decimal(30, 15),        
 @Longitude decimal(20, 15),        
 @Latitude decimal(20, 15),        
 @ApprenticeshipFrameworkId int,        
 @Title nvarchar(100),        
 @ApprenticeshipType tinyint,        
 @ShortDescription nvarchar(256),        
 @Description nvarchar(MAX),        
 @WagesDuringApprenticeship money,        
 @Keywords nvarchar(200),        
 @NumberofPositions smallint,        
 @ApplicationClosingDate datetime,        
 @InterviewsFromDate datetime,        
 @ExpectedStartDate datetime,        
 @ExpectedDuration nvarchar(50),        
 @HoursOfWork nvarchar(50),        
 @NumberOfViews int,        
 @EmployerAnonymousName nvarchar(255),   
 @EmployerDescription  nvarchar(MAX),     
 @EmployersWebsite nvarchar(255),        
 @MaxNumberofApplications int,        
 @EmployersApplicationInstructions nvarchar(4000),  
 @ApplyOutsideNAVMS bit,      
 @EmployersRecruitmentWebsite nvarchar(256),        
 @BeingSupportedBy nvarchar(50),        
 @FutureProspectsValue nvarchar(4000),        
 @Trainingtobeprovided nvarchar(4000),        
 @SkillRequired nvarchar(4000),        
 @QualificationRequired nvarchar(4000),        
 @PersonalQualities nvarchar(4000),        
 @RealityCheck nvarchar(4000),        
 @Otherimportantinformation nvarchar(4000),    
 @Question1 nvarchar(4000),        
 @Question2 nvarchar(4000),        
 @LockedForSupportUntil datetime = null,        
 @VacancyStatus nvarchar(200) = null,
 @UserName Varchar(50),
 @VacancyLocationTypeId int,    
 @DeliveryOrganisationID int,
 @ContractOwnerID int, 
 @SmallEmployerWageIncentive bit,     -- CCR11983
 @VacancyManagerAnonymous bit,
 @WageType int,
 @WageText nvarchar(50)
AS        
BEGIN        
 SET NOCOUNT ON        
 BEGIN TRY        
       
/**************************** Histoty Entry*****************************************/
declare @statusId int    

select @statusId = VacancyStatusId from Vacancy         
where VacancyId = @VacancyId  
if (isnull(@statusId,0)!=@VacancyStatusId) 
Begin
        
	 declare @VacancyHistoryEventTypeId int 
	 declare @Comment Varchar(200) 
	  
	 SET @VacancyHistoryEventTypeId = 1 
	 SET @Comment = 'Status Change'  
	  
	--This insert statement has to be executed before Update statement becuase update statement on vacancy table will fire a trigger which will need a value   
	--from vacancyhistory table.   
	INSERT into [dbo].[vacancyhistory] (VacancyId,Username,VacancyHistoryEventTypeId,VacancyHistoryEventSubTypeId,HistoryDate,Comment)  
	 VALUES (@VacancyId,@UserName,@VacancyHistoryEventTypeId,@VacancyStatusId,GetDate(),@Comment)  
End  
/*********************************************************************************************/  
        
 UPDATE [dbo].[vacancy] SET
  MasterVacancyId = @MasterVacancyId,
  [VacancyOwnerRelationshipId] = @RelationShipId,      
  Title = @Title,        
  VacancyReferenceNumber = @VacancyReferenceNumber,        
  ShortDescription = @ShortDescription,        
  Description = @Description,        
  NumberofPositions = @NumberofPositions,        
  WorkingWeek = @HoursOfWork,        
  WeeklyWage = @WagesDuringApprenticeship,        
  ContactName = @ContactName,        
  PostCode = @PostCode,
  LocalAuthorityId = @LocalAuthorityId,        
  GeocodeEasting = @GeocodeEasting,        
  GeocodeNorthing = @GeocodeNorthing,        
  Longitude = @Longitude,        
  Latitude = @Latitude,        
  AddressLine1 = @AddressLine1,        
  AddressLine2 = @AddressLine2,        
  AddressLine3 = @AddressLine3,        
  CountyId = @CountyId,        
  ApprenticeshipType = @ApprenticeshipType,        
  ApprenticeshipFrameworkId = @ApprenticeshipFrameworkId,        
  ExpectedDuration = @ExpectedDuration,     
  EmployerDescription=@EmployerDescription,       
  EmployersWebsite = @EmployersWebsite,        
  EmployerAnonymousName = @EmployerAnonymousName,        
  ApplicationClosingDate = @ApplicationClosingDate,        
  InterviewsFromDate = @InterviewsFromDate,        
  ExpectedStartDate = @ExpectedStartDate,     
  VacancyStatusId = @VacancyStatusId,        
  AddressLine4 = @AddressLine4,        
  Town = @Town,        
  NumberOfViews = @NumberOfViews,        
  MaxNumberofApplications = @MaxNumberofApplications,        
  EmployersApplicationInstructions = @EmployersApplicationInstructions, 
  ApplyOutsideNAVMS=@ApplyOutsideNAVMS,         
  EmployersRecruitmentWebsite = @EmployersRecruitmentWebsite,        
  BeingSupportedBy = @BeingSupportedBy,        
  LockedForSupportUntil =@LockedForSupportUntil,
  VacancyLocationTypeId = @VacancyLocationTypeId,
  DeliveryOrganisationID = @DeliveryOrganisationID,
  ContractOwnerID = @ContractOwnerID,
  SmallEmployerWageIncentive = @SmallEmployerWageIncentive,     -- CCR11983
  VacancyManagerAnonymous = @VacancyManagerAnonymous,
  WageType = @WageType,
  WageText = @WageText
 WHERE        
  VacancyId = @VacancyId        
        

IF EXISTS(
  SELECT [Field] FROM [dbo].[vacancytextfield] WHERE        
  [VacancyId] =  @VacancyId   
  AND [Field] =  (SELECT vacancytextfieldValueId   
  FROM vacancytextfieldValue   
  WHERE CodeName = 'FP')) 

  BEGIN
	  UPDATE [dbo].[vacancytextfield] SET [Value] = @FutureProspectsValue WHERE        
	  [VacancyId] =  @VacancyId   
	  AND [Field] =  (SELECT  vacancytextfieldValueId   
	  FROM vacancytextfieldValue   
	  WHERE CodeName = 'FP')  
  END   
ELSE IF (len(@FutureProspectsValue) > 0 )
  BEGIN
	  INSERT INTO [dbo].[vacancytextfield] ([Field],[Value],VacancyID)
	  SELECT  vacancytextfieldValueId ,@FutureProspectsValue, @VacancyId 
	  FROM vacancytextfieldValue   
	  WHERE CodeName = 'FP'
  END
      

IF EXISTS(
  SELECT [Field] FROM [dbo].[vacancytextfield] WHERE        
  [VacancyId] =  @VacancyId   
  AND [Field] =  (SELECT vacancytextfieldValueId   
  FROM vacancytextfieldValue   
  WHERE CodeName = 'TBP'))

  BEGIN
	  UPDATE [dbo].[vacancytextfield] SET [Value] = @Trainingtobeprovided WHERE        
	  [VacancyId] =  @VacancyId   
	  AND [Field] =  (SELECT  vacancytextfieldValueId   
	  FROM vacancytextfieldValue   
	  WHERE CodeName = 'TBP')  
  END   
ELSE IF (len(@Trainingtobeprovided) > 0 )
  BEGIN
	  INSERT INTO [dbo].[vacancytextfield] ([Field],[Value],VacancyID)
	  SELECT  vacancytextfieldValueId ,@Trainingtobeprovided, @VacancyId 
	  FROM vacancytextfieldValue   
	  WHERE CodeName = 'TBP'
  END
      
IF EXISTS(
  SELECT [Field] FROM [dbo].[vacancytextfield] WHERE        
  [VacancyId] =  @VacancyId   
  AND [Field] =  (SELECT vacancytextfieldValueId   
  FROM vacancytextfieldValue   
  WHERE CodeName = 'SR'))

  BEGIN
	  UPDATE [dbo].[vacancytextfield] SET [Value] = @SkillRequired WHERE        
	  [VacancyId] =  @VacancyId   
	  AND [Field] =  (SELECT  vacancytextfieldValueId   
	  FROM vacancytextfieldValue   
	  WHERE CodeName = 'SR')  
  END   
ELSE IF (LEN(@SkillRequired) > 0 )
  BEGIN
	  INSERT INTO [dbo].[vacancytextfield] ([Field],[Value],VacancyID)
	  SELECT  vacancytextfieldValueId ,@SkillRequired, @VacancyId 
	  FROM vacancytextfieldValue   
	  WHERE CodeName = 'SR'
  END 

 
IF EXISTS(
  SELECT [Field] FROM [dbo].[vacancytextfield] WHERE        
  [VacancyId] =  @VacancyId   
  AND [Field] =  (SELECT vacancytextfieldValueId   
  FROM vacancytextfieldValue   
  WHERE CodeName = 'QR'))

  BEGIN
	  UPDATE [dbo].[vacancytextfield] SET [Value] = @QualificationRequired WHERE        
	  [VacancyId] =  @VacancyId   
	  AND [Field] =  (SELECT  vacancytextfieldValueId   
	  FROM vacancytextfieldValue   
	  WHERE CodeName = 'QR')  
  END   
ELSE IF (LEN(@QualificationRequired) > 0)
  BEGIN
	  INSERT INTO [dbo].[vacancytextfield] ([Field],[Value],VacancyID)
	  SELECT  vacancytextfieldValueId ,@QualificationRequired, @VacancyId 
	  FROM vacancytextfieldValue   
	  WHERE CodeName = 'QR'
  END 
          
IF EXISTS(
  SELECT [Field] FROM [dbo].[vacancytextfield] WHERE        
  [VacancyId] =  @VacancyId   
  AND [Field] =  (SELECT vacancytextfieldValueId   
  FROM vacancytextfieldValue   
  WHERE CodeName = 'PQ'))

  BEGIN
	  UPDATE [dbo].[vacancytextfield] SET [Value] = @PersonalQualities WHERE        
	  [VacancyId] =  @VacancyId   
	  AND [Field] =  (SELECT  vacancytextfieldValueId   
	  FROM vacancytextfieldValue   
	  WHERE CodeName = 'PQ')  
  END   
ELSE IF (LEN(@PersonalQualities) > 0)
  BEGIN
	  INSERT INTO [dbo].[vacancytextfield] ([Field],[Value],VacancyID)
	  SELECT  vacancytextfieldValueId ,@PersonalQualities, @VacancyId 
	  FROM vacancytextfieldValue   
	  WHERE CodeName = 'PQ'
  END  


IF EXISTS(
  SELECT [Field] FROM [dbo].[vacancytextfield] WHERE        
  [VacancyId] =  @VacancyId   
  AND [Field] =  (SELECT vacancytextfieldValueId   
  FROM vacancytextfieldValue   
  WHERE CodeName = 'RC'))

  BEGIN
	  UPDATE [dbo].[vacancytextfield] SET [Value] = @RealityCheck WHERE        
	  [VacancyId] =  @VacancyId   
	  AND [Field] =  (SELECT  vacancytextfieldValueId   
	  FROM vacancytextfieldValue   
	  WHERE CodeName = 'RC')  
  END   
ELSE IF (LEN(@RealityCheck) > 0)
  BEGIN
	  INSERT INTO [dbo].[vacancytextfield] ([Field],[Value],VacancyID)
	  SELECT  vacancytextfieldValueId ,@RealityCheck, @VacancyId 
	  FROM vacancytextfieldValue   
	  WHERE CodeName = 'RC'
  END  
      
IF EXISTS(
  SELECT [Field] FROM [dbo].[vacancytextfield] WHERE        
  [VacancyId] =  @VacancyId   
  AND [Field] =  (SELECT vacancytextfieldValueId   
  FROM vacancytextfieldValue   
  WHERE CodeName = 'OII'))

  BEGIN
	  UPDATE [dbo].[vacancytextfield] SET [Value] = @Otherimportantinformation WHERE        
	  [VacancyId] =  @VacancyId   
	  AND [Field] =  (SELECT  vacancytextfieldValueId   
	  FROM vacancytextfieldValue   
	  WHERE CodeName = 'OII')  
  END   
ELSE IF (LEN(@Otherimportantinformation) > 0)
  BEGIN
	  INSERT INTO [dbo].[vacancytextfield] ([Field],[Value],VacancyID)
	  SELECT  vacancytextfieldValueId ,@Otherimportantinformation, @VacancyId 
	  FROM vacancytextfieldValue   
	  WHERE CodeName = 'OII'
  END  

   
IF EXISTS(
  SELECT [Question] FROM [dbo].[AdditionalQuestion] WHERE        
  [VacancyId] =  @VacancyId   
  AND QuestionId = 1)
  BEGIN
	  UPDATE [dbo].[AdditionalQuestion] SET [Question] = @Question1 WHERE         
	  [VacancyId] = @VacancyId AND QuestionId = 1   
  END   
ELSE IF (LEN(@Question1) > 0)
  BEGIN
	  INSERT INTO [dbo].[AdditionalQuestion] ([QuestionId],[Question],VacancyID)
	  VALUES(1, @Question1, @VacancyId )
  END  
       
IF EXISTS(
  SELECT [Question] FROM [dbo].[AdditionalQuestion] WHERE        
  [VacancyId] =  @VacancyId   
  AND QuestionId = 2)
  BEGIN
	  UPDATE [dbo].[AdditionalQuestion] SET [Question] = @Question2 WHERE         
  [VacancyId] = @VacancyId AND QuestionId = 2  
  END   
ELSE IF (LEN(@Question2) > 0)
  BEGIN
	  INSERT INTO [dbo].[AdditionalQuestion] ([QuestionId],[Question],VacancyID)
	  VALUES(2, @Question2, @VacancyId )
  END        
    
  --AVMS Rel 5a CONT 28/29 
  UPDATE	VACANCY
  SET		OriginalContractOwnerId = @ContractOwnerID
  WHERE		VacancyId = @VacancyId AND OriginalContractOwnerId IS NULL 
      
--        
-- IF @@ROWCOUNT = 0        
-- BEGIN        
--  RAISERROR('Concurrent update error. Updated aborted.', 16, 2)        
-- END        
    END TRY        
        
    BEGIN CATCH        
  EXEC RethrowError;        
 END CATCH         
        
    SET NOCOUNT OFF        
END