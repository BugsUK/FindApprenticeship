CREATE PROCEDURE [dbo].[uspSystemParametersSelectAll]

AS
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
    -- Insert statements for procedure here  
 SELECT SystemParametersId,
		ParameterName,
		ParameterType,
		ParameterValue,
		Editable,
		LowerLimit,
		UpperLimit,
		Description 
 FROM SystemParameters  
 
SET NOCOUNT OFF; 
END