CREATE PROCEDURE [dbo].[uspCandidateSchoolInfoNotificationSend]
AS
BEGIN
	DECLARE @MessageEventId INT, @UserTypeId INT, @RecipientTypeId INT
	DECLARE @Sender INT
	SET @Sender = 0

	DECLARE @DateTime Datetime
	SET @DateTime = GETDATE()

	Select @MessageEventId = MessageEventId FROM MessageEvent WHERE CodeName = 'SYS'
	Select @UserTypeId = UserTypeId FROM UserType WHERE CodeName = 'SUP'
	Select @RecipientTypeId = UserTypeId FROM UserType WHERE CodeName = 'CAN'


	INSERT INTO [dbo].[Message]
							(
								  [Sender],
								  [SenderType],
								  [Recipient],
								  [RecipientType],
								  [MessageDate],
								  [MessageEventId],
								  [Text],
								  [Title]                 
							) 
						  (
								  SELECT      Distinct                
										@Sender AS 'Sender', 
										@UserTypeId AS 'SenderType',
										c.CandidateId AS 'Recipient',
										@RecipientTypeId AS 'RecipientType',
										@DateTime AS 'MessageDate',
										@MessageEventId  AS 'MessageEventId',
										'We have updated the information held in the system that relates to school names.  If you have previously been unable to select your last school attended from the dropdown list when completing your profile, and had to type it in yourself, please search again and see if your last school attended is now showing.  This will only take a minute and ensure your profile information is up to date.' AS 'Text',
										'Please check your school information' AS 'Title'  
								  FROM [dbo].[Candidate] c
										INNER JOIN dbo.SchoolAttended sa on c.CandidateId = sa.CandidateId                                  
								  WHERE
										sa.SchoolId IS NULL                 
							)
	                        
	                  

END