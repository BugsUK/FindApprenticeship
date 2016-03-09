CREATE PROCEDURE [dbo].[uspCandidateDeregistrationControlSelect]
AS
BEGIN  
  
 SET NOCOUNT ON  

 DECLARE @MaxRecords INT
 SELECT @MaxRecords = (select cast(parametervalue as int)
									from SystemParameters
									where ParameterName = 'DeRegisterBatchMAXRecordsToProcess')

   
 SELECT TOP(@MaxRecords)
 [CandidateDeRegistrationControl].[CandidateDeRegistrationControlId] AS 'CandidateDeRegistrationControlId',  
 [CandidateDeRegistrationControl].[CandidateId] AS 'CandidateId',  
 [CandidateDeRegistrationControl].[isDeletedFromAdam] AS 'isDeletedFromAdam',  
 [CandidateDeRegistrationControl].[isDeleteFromAOL] AS 'isDeleteFromAOL',
 [CandidateDeRegistrationControl].[RoleId] AS 'RoleId',
 ISNULL([CandidateDeRegistrationControl].[isHardDelete],0) AS 'IsHardDelete'

 FROM [dbo].[CandidateDeRegistrationControl]  
  
 SET NOCOUNT OFF  
END