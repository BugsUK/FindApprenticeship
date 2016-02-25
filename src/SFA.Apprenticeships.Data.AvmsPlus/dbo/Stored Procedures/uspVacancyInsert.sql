CREATE PROCEDURE [dbo].[uspVacancyInsert]
@VacancyProvisionRelationshipId INT, 
@Title NVARCHAR (100), 
@VacancyReferenceNumber INT, 
@ShortDescription NVARCHAR (256), 
@Description NVARCHAR (MAX), 
@NumberofPositions SMALLINT, 
@HoursOfWork NVARCHAR (50), 
@WagesDuringApprenticeship INT, 
@ContactName NVARCHAR (100), 
@SmallEmployerWageIncentive BIT, 
@FutureProspectsValue NVARCHAR (4000), 
@Trainingtobeprovided NVARCHAR (4000), 
@SkillRequired NVARCHAR (4000), 
@QualificationRequired NVARCHAR (4000), 
@PersonalQualities NVARCHAR (4000), 
@RealityCheck NVARCHAR (4000), 
@Otherimportantinformation NVARCHAR (4000), 
@Question1 NVARCHAR (4000), 
@Question2 NVARCHAR (4000), 
@VacancyStatusId INT, 
@ModifiedBy NVARCHAR (50), 
@ApplicationClosingDate DATETIME, 
@AddressLine1 NVARCHAR (50), 
@AddressLine2 NVARCHAR (50), 
@AddressLine3 NVARCHAR (50), 
@AddressLine4 NVARCHAR (50), 
@AddressLine5 NVARCHAR (50), 
@Town NVARCHAR (40), 
@CountyId INT, 
@Postcode NVARCHAR (8), 
@GeocodeEasting DECIMAL (22, 11), 
@GeocodeNorthing DECIMAL (22, 11), 
@Longitude DECIMAL (22, 11), 
@Latitude DECIMAL (22, 11), 
@ApprenticeshipFrameworkId INT, 
@ApprenticeshipType INT, 
@WeeklyWage MONEY,
@WageType INT,
@WageText NVARCHAR(50), 
@Keywords NVARCHAR (200), 
@InterviewsFromDate DATETIME, 
@ExpectedStartDate DATETIME, 
@ExpectedDuration NVARCHAR (50), 
@WorkingWeek NVARCHAR (50), 
@NumberOfViews INT, 
@EmployerAnonymousName NVARCHAR (255), 
@EmployerDescription NVARCHAR (MAX), 
@EmployersWebsite NVARCHAR (255), 
@MaxNumberOfApplications INT, 
@ApplyOutsideNAVMS BIT, 
@EmployersApplicationInstructions NVARCHAR (4000), 
@EmployersRecruitmentWebsite NVARCHAR (50), 
@UserName VARCHAR (50), 
@VacancyManagerId INT, 
@VacancyManagerAnonymous BIT,
@vacancyId INT OUTPUT
AS
BEGIN      
 SET NOCOUNT ON  
  
BEGIN TRY  
IF NOT EXISTS(Select VacancyReferenceNumber from Vacancy Where VacancyReferenceNumber = @VacancyReferenceNumber)
BEGIN



INSERT INTO [dbo].[vacancy]   
   (   
    [VacancyOwnerRelationshipId],     
    Title,      
    VacancyReferenceNumber,      
    ShortDescription,      
    Description,      
    NumberofPositions,      
    ContactName,      
    VacancyStatusId,  
    ApplicationClosingDate,  
    AddressLine1,  
    AddressLine2,  
    AddressLine3,  
    AddressLine4,  
    AddressLine5,  
    Town,  
    CountyId,  
    Postcode,  
    GeocodeEasting,  
    GeocodeNorthing,  
    Longitude,  
    Latitude,  
    ApprenticeshipFrameworkId,  
    ApprenticeshipType,  
    WeeklyWage,
	WageType,
	WageText,  
    InterviewsFromDate,  
    ExpectedStartDate,  
    ExpectedDuration,  
    WorkingWeek,  
    NumberOfViews,  
    EmployerAnonymousName,  
    EmployerDescription,  
    EmployersWebsite,  
    MaxNumberOfApplications,  
    ApplyOutsideNAVMS,  
    EmployersApplicationInstructions,  
    EmployersRecruitmentWebsite,
	VacancyManagerId,
	SmallEmployerWageIncentive,     -- CCR11983
	VacancyManagerAnonymous
   )      
  VALUES   
   (    
    @VacancyProvisionRelationshipId,  
    @Title,  
    @VacancyReferenceNumber,  
    @ShortDescription,  
    @Description,  
    @NumberofPositions,  
    @ContactName,  
    @VacancyStatusId,  
    @ApplicationClosingDate,  
    @AddressLine1,  
    @AddressLine2,  
    @AddressLine3,  
    @AddressLine4,  
    @AddressLine5,  
    @Town,  
    @CountyId,  
    @Postcode,  
    @GeocodeEasting,  
    @GeocodeNorthing,  
    @Longitude,  
    @Latitude,  
    @ApprenticeshipFrameworkId,  
    @ApprenticeshipType,  
    @WeeklyWage,  
	@WageType,
	@WageText,
    @InterviewsFromDate,  
    @ExpectedStartDate,  
    @ExpectedDuration,  
    @WorkingWeek,  
    @NumberOfViews,  
    @EmployerAnonymousName,  
    @EmployerDescription,  
    @EmployersWebsite,  
    @MaxNumberOfApplications,  
    @ApplyOutsideNAVMS,  
    @EmployersApplicationInstructions,  
    @EmployersRecruitmentWebsite,
	@VacancyManagerId,
	@SmallEmployerWageIncentive,
	@VacancyManagerAnonymous     -- CCR11983  
   )  
  
  SET @vacancyId = SCOPE_IDENTITY()      
  
  INSERT INTO [dbo].[AdditionalQuestion] ([VacancyId], [QuestionId], [Question])      
   VALUES (@VacancyId, 1, @Question1)       
  
  INSERT INTO [dbo].[AdditionalQuestion] ([VacancyId], [QuestionId], [Question])      
   VALUES(@VacancyId, 2, @Question2)  
  
  Declare @FieldId int    
  
  Select @FieldId = vacancytextfieldValueId   
  from vacancytextfieldValue   
  where FullName = 'Training to be provided'  
  
  INSERT INTO [dbo].[vacancytextfield]([VacancyId],[Field],[Value])    
  VALUES  (@VacancyId,ISNULL(@FieldId, ''),@Trainingtobeprovided )     
  
  SET @FieldId = 0    
  Select @FieldId = vacancytextfieldValueId   
  from vacancytextfieldValue   
  where FullName = 'Other important information'  
  
  INSERT INTO [dbo].[vacancytextfield]([VacancyId],[Field],[Value])    
  VALUES  (@VacancyId,ISNULL(@FieldId, ''),@Otherimportantinformation )     
  
  SET @FieldId = 0    
  
  Select @FieldId = vacancytextfieldValueId   
  from vacancytextfieldValue   
  where FullName = 'Reality Check'  
  
  INSERT INTO [dbo].[vacancytextfield]([VacancyId],[Field],[Value])    
  VALUES  (@VacancyId,ISNULL(@FieldId, ''),@RealityCheck )     
  
  SET @FieldId = 0    
  
  Select @FieldId = vacancytextfieldValueId   
  from vacancytextfieldValue   
  where FullName = 'Future Prospects'  
  
  INSERT INTO [dbo].[vacancytextfield]([VacancyId],[Field],[Value])    
  VALUES  (@VacancyId,ISNULL(@FieldId, ''),@FutureProspectsValue )   
  
  SET @FieldId = 0    
  
  Select @FieldId = vacancytextfieldValueId   
  from vacancytextfieldValue   
  where FullName = 'Skills Required'  
  
  INSERT INTO [dbo].[vacancytextfield]([VacancyId],[Field],[Value])    
  VALUES  (@VacancyId,ISNULL(@FieldId, ''),@SkillRequired )   
  
  
  SET @FieldId = 0    
  
  Select @FieldId = vacancytextfieldValueId   
  from vacancytextfieldValue   
  where FullName = 'Qualifications Required'  
  
  INSERT INTO [dbo].[vacancytextfield]([VacancyId],[Field],[Value])    
  VALUES  (@VacancyId,ISNULL(@FieldId, ''),@QualificationRequired )   
  
  SET @FieldId = 0    
  Select @FieldId = vacancytextfieldValueId   
  from vacancytextfieldValue   
  where FullName = 'Personal Qualities'  
  
  INSERT INTO [dbo].[vacancytextfield]([VacancyId],[Field],[Value])    
  VALUES  (@VacancyId,ISNULL(@FieldId, ''),@PersonalQualities )      


/**************************** Histoty Entry*****************************************/
        
	 declare @VacancyHistoryEventTypeId int 
	 declare @Comment Varchar(200) 
	  
	 SET @VacancyHistoryEventTypeId = 1
	 SET @Comment = 'Status Change'  
	  
	--This insert statement has to be executed before Update statement becuase update statement on vacancy table will fire a trigger which will need a value   
	--from vacancyhistory table.   
	INSERT into [dbo].[vacancyhistory] (VacancyId,Username,VacancyHistoryEventTypeId,VacancyHistoryEventSubTypeId,HistoryDate,Comment)  
	 VALUES (@VacancyId,@UserName,@VacancyHistoryEventTypeId,@VacancyStatusId,GetDate(),@Comment)  

/*********************************************************************************************/  
END
ELSE
BEGIN
    SELECT @vacancyId = VacancyId From  Vacancy Where VacancyReferenceNumber = @VacancyReferenceNumber
END
END TRY      
 BEGIN CATCH      
  EXEC RethrowError;      
 END CATCH      
  
 SET NOCOUNT OFF      
END