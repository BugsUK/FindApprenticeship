CREATE PROCEDURE [dbo].[uspBulkVacancyInsert] 
	@SystemType INT, 
	@RequestorId INT, 
	@TrainingProviderEdsUrn INT, 
	@EmployerEdsUrn INT, 
	@Title NVARCHAR (100), 
	@VacancyReferenceNumber INT, 
	@ShortDescription NVARCHAR (256), 
	@Description NVARCHAR (MAX), 
	@NumberofPositions SMALLINT, 
	@ContactName NVARCHAR (100), 
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
	@ApplicationClosingDate DATETIME, 
	@AddressLine1 NVARCHAR (50), 
	@AddressLine2 NVARCHAR (50), 
	@AddressLine3 NVARCHAR (50), 
	@AddressLine4 NVARCHAR (50), 
	@AddressLine5 NVARCHAR (50), 
	@Town NVARCHAR (40), 
	@CountyName NVARCHAR(150), 
	@Postcode NVARCHAR (8), 
	@GeocodeEasting DECIMAL (22, 11), 
	@GeocodeNorthing DECIMAL (22, 11), 
	@Longitude DECIMAL (22, 11), 
	@Latitude DECIMAL (22, 11), 
	@ApprenticeshipFrameworkCode NVARCHAR(3), 
	@ApprenticeshipType INT, 
	@WeeklyWage MONEY, 
	@InterviewsFromDate DATETIME, 
	@ExpectedStartDate DATETIME, 
	@ExpectedDuration NVARCHAR (50), 
	@WorkingWeek NVARCHAR (50), 
	@NumberOfViews INT, 
	@EmployerAnonymousName NVARCHAR (255), 
	@EmployerDescription NVARCHAR (MAX), 
	@EmployersWebsite NVARCHAR (255), 
	@ApplyOutsideNAVMS BIT, 
	@EmployersApplicationInstructions NVARCHAR (4000), 
	@EmployersRecruitmentWebsite NVARCHAR (256), 
	@UserName VARCHAR (50), 
	@EmployerImageId INT, 
	@VacancyLocationType INT, 
	--5.1
	@ContractOwnerUKPRN INT, 
	@VacancyManagerUrn INT, 
	@DeliveryOrganisationUrn INT, 
	@IsSmallEmployerWageIncentive BIT, 
	@IsVacancyManagerAnonymous BIT, 
	@version NVARCHAR(5),
	@localAuthority nvarchar(8),
	--5.1 end
	@refNo INT OUTPUT, 
	@errorCode NVARCHAR(1000) OUTPUT, 
	@vacancyId INT OUTPUT
AS  
BEGIN        
SET NOCOUNT ON
	 
DECLARE  @VacancyProvisionRelationshipId INT, @CountyId INT, @FrameworkId INT , @IsNationWideAllowed bit
DECLARE @EmployerID INT 
DECLARE @TrainingProviderId INT 
DECLARE @ContractOwnerId INT 
DECLARE @DeliveryOrganisationId INT 
DECLARE @VacancyManagerId INT 
DECLARE @Version50 NVARCHAR(5)
declare @localAuthorityId int
DECLARE @VacancyTypeId int
	BEGIN TRY 
			
		SET @Version50 = '5.0'
		SET @errorCode = ''
		
		-- Getting Employer & Training Provider ID
		SELECT @EmployerID = EmployerId FROM EMPLOYER WHERE EdsUrn = @EmployerEdsUrn
		SELECT @TrainingProviderId = ProviderSiteID FROM [ProviderSite] WHERE EDSURN = @TrainingProviderEdsUrn
		--5.1
		SELECT @ContractOwnerId = ProviderID FROM [Provider] WHERE UKPRN = @ContractOwnerUKPRN AND ProviderStatusTypeID <> 2
		SELECT @DeliveryOrganisationId = ProviderSiteID FROM [ProviderSite] WHERE EDSURN = @DeliveryOrganisationUrn
		SELECT @VacancyManagerId = ProviderSiteID FROM [ProviderSite] WHERE EDSURN = @VacancyManagerUrn
		if (@localAuthority is null)
			select @localAuthorityId = null
        else
		begin
			select @localAuthorityId = LocalAuthorityId from LocalAuthority where CodeName = @localAuthority
			if (@localAuthorityId is null)
				select @errorCode = @errorCode + ',-10063' --Invalid local authority
        end
		
		--IF (1=2 AND (@SystemType = 2 AND NOT EXISTS (SELECT 1 FROM dbo.TrainingProvider WHERE UKPRN = @RequestorId AND EDSURN = @TrainingProviderEdsUrn)))

		IF (@EmployerID IS NULL)
			SET @errorCode = @errorCode + ',-10045' --EmployerEDSURNDoesNotExist
		IF (@TrainingProviderId IS NULL)
			SET @errorCode = @errorCode + ',-10046' --ProviderEDSURNDoesNotExist		
		--IF (@ContractOwnerId IS NULL)
		--	SET @errorCode = @errorCode + ',-10046' --@ContractOwnerUKPRN
		IF (@DeliveryOrganisationId IS NULL)
			SET @errorCode = @errorCode + ',-10054' --DeliveryOrganisationDoesNotExist		
		IF (@VacancyManagerId IS NULL)
			SET @errorCode = @errorCode + ',-10055' --VacancyManagerDoesNotExist		
						
		--IF (@TrainingProviderId IS NULL AND @EmployerID IS NULL)
		--	SET @errorCode = '-10045,-10046' --ProviderEDSURNDoesNotExist AND EmployerEDSURNDoesNotExist
		SET @errorCode = SUBSTRING(@errorCode,2,LEN(@errorCode))

		IF(@ApprenticeshipType = 4)
			SET @VacancyTypeId = 2
		ELSE IF(@ApprenticeshipType = 0)
			SET @VacancyTypeId = 0
		ELSE
			SET @VacancyTypeId = 1
		

		IF (@errorCode='')
		BEGIN
			-- TODO MMA Join will contacin the third priary key to validate the relationship 5.1
			-- Checking if Training Provider is Authorized to put in this Vacancy
			IF (@SystemType = 2 AND NOT EXISTS (SELECT 1 FROM dbo.[ProviderSite] PS
				JOIN dbo.ProviderSiteRelationShip PSR ON PS.ProviderSiteID = PSR.ProviderSiteID 
				JOIN dbo.Provider P on PSR.ProviderID = P.ProviderID
				WHERE UKPRN = @RequestorId 
					  AND EDSURN = @TrainingProviderEdsUrn
					  AND PSR.ProviderSiteRelationShipTypeID = 1
					  AND P.ProviderStatusTypeID = 1
					  AND PS.TrainingProviderStatusTypeId = 1
					  
				))
				SET @errorCode = '-10033' --LearningProviderNotAuthorised
			ELSE
			BEGIN
			
					--If version 5.0 defaulting the value of 
					IF (@Version50 = @version)
					BEGIN
						SELECT @ContractOwnerId=P.ProviderID,
							   @ContractOwnerUKPRN = p.UKPRN FROM dbo.[ProviderSite] PS
						JOIN dbo.ProviderSiteRelationShip PSR ON PS.ProviderSiteID = PSR.ProviderSiteID 
						JOIN dbo.Provider P on PSR.ProviderID = P.ProviderID
						WHERE PS.EDSURN = @TrainingProviderEdsUrn
						  AND PSR.ProviderSiteRelationShipTypeID = 1
						  AND P.ProviderStatusTypeID = 1
						  AND PS.TrainingProviderStatusTypeId = 1
					END
					
					-- Getting Relationship Id and 	IsNationWideAllowed Flag based on relationships
					SELECT
						@VacancyProvisionRelationshipId = [VacancyOwnerRelationshipId],
						@IsNationWideAllowed = ISNULL(NationWideAllowed,0),
						@EmployerDescription = CASE @EmployerDescription WHEN NULL THEN vpr.EmployerDescription 
																		WHEN '' THEN vpr.EmployerDescription 
																		ELSE @EmployerDescription END
						FROM
						[VacancyOwnerRelationship] vpr
						WHERE
						[ProviderSiteID] = @TrainingProviderId
						AND EmployerId = @EmployerID	
									
				-- Validating if Relationship Exists
				IF @VacancyProvisionRelationshipId IS NULL											
						SET @errorCode = '-10035' -- InvalidRelationshipTPandEMP
				ELSE IF (@VacancyLocationType = (Select VacancyLocationTypeId from VacancyLocationType WHERE CodeName='NAT') AND @IsNationWideAllowed = 0) -- Checking if Nationwise or not
						SET @errorCode = '-10044' -- RelationshipNationwideNotAllowed
				ELSE IF @ContractOwnerId IS NULL
						SET @errorCode = '-10059' -- ContractOwnerUKPRNMandatory
				ELSE IF NOT EXISTS (SELECT P.UKPRN  FROM dbo.[ProviderSite] PS
									JOIN dbo.ProviderSiteRelationShip PSR ON PS.ProviderSiteID = PSR.ProviderSiteID 
									JOIN dbo.Provider P on PSR.ProviderID = P.ProviderID
									WHERE P.UKPRN = @ContractOwnerUKPRN
										  AND PS.EDSURN = @TrainingProviderEdsUrn
									  AND PSR.ProviderSiteRelationShipTypeID IN(1,2)
									  AND P.ProviderStatusTypeID = 1
									  AND PS.TrainingProviderStatusTypeId = 1
									  AND P.IsContracted = 1)
									  
						SET @errorCode = '-10056' -- ContractOwnerUKPRN Not Valid
						
				ELSE IF @TrainingProviderEdsUrn <> @VacancyManagerUrn AND NOT EXISTS (SELECT P.UKPRN  FROM dbo.[ProviderSite] PS
									JOIN dbo.ProviderSiteRelationShip PSR ON PS.ProviderSiteID = PSR.ProviderSiteID 
									JOIN dbo.Provider P on PSR.ProviderID = P.ProviderID
									WHERE PS.EDSURN = @VacancyManagerUrn
									  AND P.UKPRN = @ContractOwnerUKPRN
									  AND PSR.ProviderSiteRelationShipTypeID = 3 --
									  AND P.ProviderStatusTypeID = 1
									  AND PS.TrainingProviderStatusTypeId = 1)
									  
						SET @errorCode = '-10057' -- Vacancy Manager Not Valid		
						
				ELSE IF /*@TrainingProviderEdsUrn <> @VacancyManagerUrn AND */NOT EXISTS (SELECT P.UKPRN  FROM dbo.[ProviderSite] PS
									JOIN dbo.ProviderSiteRelationShip PSR ON PS.ProviderSiteID = PSR.ProviderSiteID 
									JOIN dbo.Provider P on PSR.ProviderID = P.ProviderID
									WHERE PS.EDSURN = @DeliveryOrganisationUrn
									  AND P.UKPRN = @ContractOwnerUKPRN
									  AND PSR.ProviderSiteRelationShipTypeID in (1, 2)
									  AND P.ProviderStatusTypeID = 1
									  AND PS.TrainingProviderStatusTypeId = 1)
									  
						SET @errorCode = '-10058' -- Delivery Organisation Not Valid				
				ELSE
				BEGIN
			
					-- Getting Framework ID for Vacancy	
					SELECT 
						@FrameworkId = ApprenticeshipFrameworkId
					FROM
						dbo.ApprenticeshipFramework 
						JOIN ApprenticeshipOccupation
						ON ApprenticeshipOccupation.ApprenticeshipOccupationId = ApprenticeshipFramework.ApprenticeshipOccupationId
						AND ApprenticeshipOccupation.ApprenticeshipOccupationStatusTypeId = (Select ApprenticeshipOccupationStatusTypeId #
																							 FROM ApprenticeshipOccupationStatusType 
																							 WHERE CodeName='ACT')
						AND ApprenticeshipFramework.ApprenticeshipFrameworkStatusTypeId  = (Select ApprenticeshipOccupationStatusTypeId 
																							FROM ApprenticeshipFrameworkStatusType 
																							WHERE CodeName='ACT')
	  				 WHERE
						ApprenticeshipFramework.CodeName = @ApprenticeshipFrameworkCode 
		
					-- Validating if Framework Exists
					IF @FrameworkId IS NULL		
						SET @errorCode = '-10036' -- IncorrectFrameworkCode
					ELSE
					BEGIN
						-- Checking for county id based on Name & validating if its missing only for standard vacancies
						SELECT @CountyId = CountyId 
						From County WHERE FullName = @CountyName 
					
						IF (@CountyId IS NULL AND @VacancyLocationType = (Select VacancyLocationTypeId 
													from VacancyLocationType 
													WHERE CodeName='STD'))
						SET @errorCode = '-10037' -- CountyInvalid	

					END
				
				END
			END

		END	
		--If no Error do the insert
		IF @errorCode = ''
		BEGIN  
			IF NOT EXISTS(Select VacancyReferenceNumber from Vacancy Where VacancyReferenceNumber = @VacancyReferenceNumber)  
			BEGIN  			  
				IF @VacancyLocationType = 2 or @VacancyLocationType = 3
				BEGIN
				   --Getting Address values for National And Multisite Vacancies from Employer
					SELECT
							@AddressLine1 = AddressLine1,
							@AddressLine2 = AddressLine2,
							@AddressLine3 = AddressLine3,
							@AddressLine4 = AddressLine4,
							@AddressLine5 = AddressLine5,
							@Town = Town,
							@CountyId = CountyId,
							@GeocodeEasting = GeocodeEasting,
							@GeocodeNorthing = GeocodeNorthing,
							@Latitude = Latitude,
							@Longitude = Longitude,
							@Postcode = PostCode
						FROM
							dbo.Employer
						WHERE
							EmployerId = @EmployerID

					END --@VacancyLocationType = 2
					
					-- if employer anonymous name is empty but not null UI treats it as a valid name
					if ('' = ltrim(@EmployerAnonymousName))
						select @EmployerAnonymousName = null;

					--Creating Vacancy
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
						InterviewsFromDate,    
						ExpectedStartDate,    
						ExpectedDuration,    
						WorkingWeek,    
						NumberOfViews,    
						EmployerAnonymousName,    
						EmployerDescription,    
						EmployersWebsite,       
						ApplyOutsideNAVMS,    
						EmployersApplicationInstructions,    
						EmployersRecruitmentWebsite,
						VacancyTypeId,
						VacancyLocationTypeId,
						--5.1
				  	    VacancyManagerID,
						DeliveryOrganisationID,
						OriginalContractOwnerID,
						ContractOwnerID,   
						SmallEmployerWageIncentive,
						VacancyManagerAnonymous,
						LocalAuthorityId,
						VacancyGuid
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
						@FrameworkId,    
						@ApprenticeshipType,    
						@WeeklyWage,    
						@InterviewsFromDate,    
						@ExpectedStartDate,    
						@ExpectedDuration,    
						@WorkingWeek,    
						@NumberOfViews,    
						@EmployerAnonymousName,    
						@EmployerDescription,    
						@EmployersWebsite,    
						@ApplyOutsideNAVMS,    
						@EmployersApplicationInstructions,    
						@EmployersRecruitmentWebsite,
						@VacancyTypeId,
						@VacancyLocationType,
						--5.1
						@VacancyManagerId,
						@DeliveryOrganisationId,
						@ContractOwnerId,
						@ContractOwnerId,
						@IsSmallEmployerWageIncentive, 
						case WHEN @VacancyManagerId = @TrainingProviderId THEN 0 ELSE @IsVacancyManagerAnonymous END, --Its VM=VO then always display
						@localAuthorityId,
						NEWID()
					   )    
				 
					  --Inserting Image Parameters in Relationship Table
					  IF ISNULL(@EmployerImageId,0) <> 0
					  BEGIN
							UPDATE [dbo].[VacancyOwnerRelationship]
							SET EmployerLogoAttachmentId = @EmployerImageId
							WHERE [VacancyOwnerRelationshipId] = @VacancyProvisionRelationshipId
					  END 
					   

					  --Output Parameters
					  SET @refNo = @VacancyReferenceNumber
					  SET @errorCode = '0'
					  
					  --New Vacancy ID
					  SET @vacancyId = SCOPE_IDENTITY()   
		  			
					  --Additional Questions 	
					  INSERT INTO [dbo].[AdditionalQuestion] ([VacancyId], [QuestionId], [Question])        
					  VALUES (@VacancyId, 1, @Question1)         
			
					  --Additional Questions
					  INSERT INTO [dbo].[AdditionalQuestion] ([VacancyId], [QuestionId], [Question])        
					  VALUES(@VacancyId, 2, @Question2)    
			
					  --Vacancy Text Field Values
					  Declare @FieldId int      
						
					  Select @FieldId = vacancytextfieldValueId     
					  from vacancytextfieldValue     
					  where FullName = 'Training to be provided'    
						
					  INSERT INTO [dbo].[vacancytextfield]([VacancyId],[Field],[Value])      
					  VALUES  (@VacancyId,ISNULL(@FieldId, ''),NULLIF(@Trainingtobeprovided,'') )       
						
					  SET @FieldId = 0      
					  Select @FieldId = vacancytextfieldValueId     
					  from vacancytextfieldValue     
					  where FullName = 'Other important information'    
						
					  INSERT INTO [dbo].[vacancytextfield]([VacancyId],[Field],[Value])      
					  VALUES  (@VacancyId,ISNULL(@FieldId, ''),NULLIF(@Otherimportantinformation,'') )       
						
					  SET @FieldId = 0      
						
					  Select @FieldId = vacancytextfieldValueId     
					  from vacancytextfieldValue     
					  where FullName = 'Reality Check'    
						
					  INSERT INTO [dbo].[vacancytextfield]([VacancyId],[Field],[Value])      
					  VALUES  (@VacancyId,ISNULL(@FieldId, ''),NULLIF(@RealityCheck,'') )       
						
					  SET @FieldId = 0      
						
					  Select @FieldId = vacancytextfieldValueId     
					  from vacancytextfieldValue     
					  where FullName = 'Future Prospects'    
						
					  INSERT INTO [dbo].[vacancytextfield]([VacancyId],[Field],[Value])      
					  VALUES  (@VacancyId,ISNULL(@FieldId, ''), NULLIF(@FutureProspectsValue,'') )     
						
					  SET @FieldId = 0      
						
					  Select @FieldId = vacancytextfieldValueId     
					  from vacancytextfieldValue     
					  where FullName = 'Skills Required'    
						
					  INSERT INTO [dbo].[vacancytextfield]([VacancyId],[Field],[Value])      
					  VALUES  (@VacancyId,ISNULL(@FieldId, ''), NULLIF(@SkillRequired,''))
						
						
					  SET @FieldId = 0      
						
					  Select @FieldId = vacancytextfieldValueId     
					  from vacancytextfieldValue     
					  where FullName = 'Qualifications Required'    
						
					  INSERT INTO [dbo].[vacancytextfield]([VacancyId],[Field],[Value])      
					  VALUES  (@VacancyId,ISNULL(@FieldId, ''),NULLIF(@QualificationRequired,'') )     
						
					  SET @FieldId = 0      
					  Select @FieldId = vacancytextfieldValueId     
					  from vacancytextfieldValue     
					  where FullName = 'Personal Qualities'    
						
					  INSERT INTO [dbo].[vacancytextfield]([VacancyId],[Field],[Value])      
					  VALUES  (@VacancyId,ISNULL(@FieldId, ''),NULLIF(@PersonalQualities,'') )        
		  
					/**************************** History Entry*****************************************/  
							  
					  declare @VacancyHistoryEventTypeId int   
					  declare @Comment Varchar(200)   
						 
					  SET @VacancyHistoryEventTypeId = 1  
					  SET @Comment = 'Status Change'    --TODO:Check whatc value should be here
						 
					 --This insert statement has to be executed before Update statement becuase update statement on vacancy table will fire a trigger which will need a value     
					 --from vacancyhistory table.     
					 INSERT into [dbo].[vacancyhistory] (VacancyId,Username,VacancyHistoryEventTypeId,VacancyHistoryEventSubTypeId,HistoryDate,Comment)    
					  VALUES (@VacancyId,@UserName,@VacancyHistoryEventTypeId,@VacancyStatusId,GetDate(),@Comment)    
					  
					/*********************************************************************************************/    
				END  
				ELSE  
				BEGIN
					--Vacancy reference number already exists so return error with correct code
					SET @refNo = NULL 
					SET @errorCode = '-10034' --Vacancy reference number already exists
   
				END  
			END  --	@errorCode = '' 
		
			
			IF @errorCode <> ''  
				SET @refNo = NULL 
 
	END TRY        
	BEGIN CATCH 
	--exception caught   
		SET @refNo = NULL 
		SET @errorCode = -1 --UnknownSystemError
		EXEC RethrowError;        
	END CATCH        
	
 SET NOCOUNT OFF        
END