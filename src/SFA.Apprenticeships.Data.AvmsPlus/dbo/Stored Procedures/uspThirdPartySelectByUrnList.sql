CREATE PROCEDURE [dbo].[uspThirdPartySelectByUrnList]
@urnList VARCHAR (4000)
AS
BEGIN      
 -- SET NOCOUNT ON added to prevent extra result sets from      
 -- interfering with SELECT statements.      
 SET NOCOUNT ON;      
 
    SELECT       
    T.ID as "ThirdPartyId"
	,T.ThirdPartyName       
    ,T.EDSURN       
    ,T.AddressLine1      
    ,T.AddressLine2      
    ,T.AddressLine3      
    ,T.AddressLine4      
    ,T.AddressLine5
    ,T.Town      
    ,County.FullName as "County"
    ,T.CountyId      
    ,T.PostCode      
    ,T.Longitude      
    ,T.Latitude      
 FROM       
 dbo.ThirdParty T   
 Left Outer Join County on County.CountyId = T.CountyId    
 Inner Join (SELECT * from dbo.fnx_SplitListToTable(@urnList)) as TEMP on TEMP.ID = EdsUrn

END