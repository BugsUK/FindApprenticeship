CREATE PROCEDURE [dbo].[uspGetCAFFieldsByCandidateIdAndByApplicationId]
	@candidateId int,
	@applicationId int
AS
BEGIN  
  
 SET NOCOUNT ON  

DECLARE @applicationStatus int
	
	-- get the application status.
	select @applicationStatus = applicationStatusTypeId
	from application 
	where applicationId = @applicationId


	If @applicationId = 0
	 
		SELECT  
			[cAFFields].[CAFFieldsId] AS 'CAFFieldsId', [cAFFields].[CandidateId] AS 'CandidateId',
			[cAFFields].[ApplicationId] AS 'ApplicationId',  [cAFFields].[Field] AS 'Field', 
			[cAFFields].[Value] AS 'Value'  
		FROM [dbo].[CAFFields] [cAFFields]  
			WHERE [CandidateId]= @candidateId
		AND [ApplicationId] Is NULL
	 
	Else -- application id exists

	 BEGIN
			-- Return rows held against UNSENT applications
if @applicationStatus = 1
		SELECT  
			[cAFFields].[CAFFieldsId] AS 'CAFFieldsId', [cAFFields].[CandidateId] AS 'CandidateId',
			[cAFFields].[ApplicationId] AS 'ApplicationId',  [cAFFields].[Field] AS 'Field', 
			[cAFFields].[Value] AS 'Value'  
		FROM [dbo].[CAFFields] [cAFFields]  
			WHERE [CandidateId]= @candidateId
		AND [ApplicationId] Is NULL

else
-- Return rows held against SENT applications

		SELECT  
			[cAFFields].[CAFFieldsId] AS 'CAFFieldsId', [cAFFields].[CandidateId] AS 'CandidateId',
			[cAFFields].[ApplicationId] AS 'ApplicationId',  [cAFFields].[Field] AS 'Field', 
			[cAFFields].[Value] AS 'Value'  
		FROM [dbo].[CAFFields] [cAFFields]  
			WHERE [CandidateId]= @candidateId
		AND [ApplicationId] = @applicationId

END

	 
	SET NOCOUNT OFF  
END