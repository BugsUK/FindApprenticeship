CREATE PROCEDURE  [dbo].[uspGetALLApprenticeshipFrameworkAndOccupation]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Insert statements for procedure here
	SELECT   
		  AF.CodeName as 'FrameworkCodeName',  
		  AF.ShortName as 'FrameworkShortName',  
		  AF.FullName as 'FrameworkFullName',  
		  AO.Codename as 'OccupationCodeName',  
		  AO.ShortName as 'OccupationShortName',  
		  AO.FullName as 'OccupationFullName'  
 FROM ApprenticeshipFramework AF  
		  INNER JOIN ApprenticeshipOccupation AO  
		  ON AF.ApprenticeshipOccupationId = AO.ApprenticeshipOccupationId  
		  JOIN ApprenticeshipFrameworkStatusType Afs
		  ON Afs.ApprenticeshipFrameworkStatusTypeId = AF.ApprenticeshipFrameworkStatusTypeId 
		  AND Afs.CodeName ='ACT'
		  JOIN ApprenticeshipOccupationStatusType Aost
		  ON Aost.ApprenticeshipOccupationStatusTypeId = AO.ApprenticeshipOccupationStatusTypeId 
		  AND Aost.CodeName ='ACT'  
END