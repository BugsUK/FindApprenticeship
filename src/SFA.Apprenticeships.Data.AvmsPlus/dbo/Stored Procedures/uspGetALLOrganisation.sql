CREATE PROCEDURE [dbo].[uspGetALLOrganisation]  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  

	SET NOCOUNT ON;  

	Declare @shortnameother char(3)
	Set @shortnameother = 'OTH'
	  
	SELECT	OrganisationId,
			CodeName,
			ShortName,
			FullName
	from	Organisation  
	ORDER BY CASE WHEN upper(CodeName) = @shortnameother THEN 1 ELSE 0 END, FullName
		
END