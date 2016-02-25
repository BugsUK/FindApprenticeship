CREATE PROCEDURE [dbo].[uspApplicationUpdate]
@ApplicationId INT, @candidateId INT, @vacancyId INT, @ApplicationStatusTypeId INT, @Allocatedto VARCHAR (200), @UnsuccessfulReasonID INT, @UnsuccessfulComments VARCHAR (100), @WithdrawalReasonID INT, @WithdrawalComments VARCHAR (100), @WithdrawalAcknowledged BIT
AS
BEGIN    
  SET NOCOUNT ON    
     
  BEGIN TRY  
  
	DECLARE @ReasonID int
	DECLARE @ReasonComment varchar(100)
	
	IF @UnsuccessfulReasonID<>0
		SET @ReasonID=@UnsuccessfulReasonID
	ELSE IF @WithdrawalReasonID<>0
		SET @ReasonID=@WithdrawalReasonID
	ELSE
		SET @ReasonID=0

	IF @UnsuccessfulComments<>''
		SET @ReasonComment=@UnsuccessfulComments
	ELSE IF @WithdrawalComments<>''
		SET @ReasonComment=@WithdrawalComments
	ELSE
		SET @ReasonComment=''

    /** Histoty Entry   **/  
    DECLARE @statusId int      
  
    /** Determine previous status **/  
    SELECT @statusId = ApplicationStatusTypeId  
    FROM Application           
    WHERE ApplicationId = @ApplicationId  
    
    /** Check if this is a status change **/  
    IF (ISNULL(@statusId,0) != @ApplicationStatusTypeId)   
    BEGIN  
      DECLARE @VacancyHistoryEventTypeId int  
      DECLARE @Comment VARCHAR(200)  
      
      /** Insert status change history record **/  
      INSERT INTO APPLICATIONHISTORY(ApplicationId, UserName,  
        ApplicationHistoryEventDate, ApplicationHistoryEventTypeId,  
        ApplicationHistoryEventSubTypeId ,Comment)  
      VALUES (@ApplicationId,NULL,getdate(),1,@ApplicationStatusTypeId,@ReasonComment)  
    END    
  
    /** Do the application update **/
	
		
	 
    UPDATE [dbo].[Application]     
    SET    
      [CandidateId] = @candidateId,    
      [VacancyId] = @vacancyId,    
      [ApplicationStatusTypeId] = @ApplicationStatusTypeId,  
      [AllocatedTo] = @Allocatedto,  
      [UnsuccessfulReasonId] = @ReasonID,     
      [OutcomeReasonOther] = @ReasonComment,  
      WithdrawalAcknowledged = @WithdrawalAcknowledged  
    WHERE ApplicationId = @ApplicationId        
  
  END TRY    
    
  BEGIN CATCH    
    EXEC RethrowError;    
  END CATCH    
  
  SET NOCOUNT OFF    
END