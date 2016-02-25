Create PROCEDURE [dbo].[uspGetUkprnForUpin]      
  	@Upin varchar(8000)
AS  
  
BEGIN  
  SET NOCOUNT ON  
   
  DECLARE @SQL varchar(8000)

  SET @SQL = 'select Upin,ukprn from [dbo].[MasterUPIN] where ukprn IN (' + @Upin + ')'

  EXEC(@SQL)
  
  SET NOCOUNT OFF  
END