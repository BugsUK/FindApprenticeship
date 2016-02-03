CREATE PROCEDURE  [dbo].[uspGetALLManagingAreas]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
       SELECT 	 
	   maView.[ManagingAreaId] AS 'ManagingAreaId'
      ,maView.[ManagingAreaCodeName] AS 'ManagingAreaCodeName'
      ,maView.[ManagingAreaShortName] AS 'ManagingAreaShortName'
      ,maView.[ManagingAreaFullName] AS 'ManagingAreaIdFullName'
      ,maView.ManagingAreaTypeId AS 'ManagingAreaTypeId'      
      ,Division.[DivisionID] AS 'DivisionId'
      ,Division.[DivisionCode] AS 'DivisionCodeName'
      ,Division.[DivisionShortName] AS 'DivisionShortName'
      ,Division.[DivisionFullName] AS 'DivisionFullName'  
      ,Division.[DivisionTypeId] AS 'DivisionTypeId'       
  FROM dbo.vwManagingAreas maView
  INNER JOIN dbo.vwDivisions Division ON Division.[DivisionID] = maView.[DivisionId] 
  ORDER BY maView.[ManagingAreaFullName]   

 END