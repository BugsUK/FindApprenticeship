CREATE PROCEDURE [dbo].[uspThirdPartySelectByThirdPartyId]  
 @ThirdPartyId int  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
SELECT  
 [ThirdParty].[EDSURN]  AS 'EdsUrn',  
 [ThirdParty].[ID] AS 'ThirdPartyId',  
 isnull([ThirdParty].[ThirdPartyName],'') AS 'ThirdPartyName',  
 isnull([ThirdParty].[AddressLine1],'') as 'AddressLine1',  
 isnull([ThirdParty].[AddressLine2],'') as 'AddressLine2',  
 isnull([ThirdParty].[AddressLine3],'') as 'AddressLine3',  
 isnull([ThirdParty].[AddressLine4],'') as 'AddressLine4',  
 isnull([ThirdParty].[AddressLine5],'') as 'AddressLine5',  
 isnull([ThirdParty].[Town],'') as 'Town',  
 [ThirdParty].[CountyId],
 [County].[FullName] as 'County',  
 isnull([ThirdParty].[PostCode],'') as 'PostCode'
 
FROM [dbo].[ThirdParty]
LEFT JOIN [dbo].[County] [County] ON [County].[CountyId] = [ThirdParty].[CountyId]  
WHERE [ThirdParty].[ID]=@ThirdPartyId

END