CREATE PROCEDURE [dbo].[uspTrainingProvidersSelectByEmployerId]   
 @EmployerId INT  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
 SELECT   
  Provider.FullName  
,VM.StatusTypeId as Status  
,VST.FullName as StatusId  
, Provider.AddressLine1  
, Provider.AddressLine2  
, Provider.AddressLine3  
, Provider.AddressLine4  
, Provider.CountyId  
, Provider.PostCode  
, Provider.Longitude  
, Provider.Latitude  
, Provider.GeocodeEasting  
, Provider.GeocodeNorthing  
, Provider.ProviderSiteID  
 FROM dbo.[VacancyOwnerRelationship] VM INNER JOIN dbo.[ProviderSite] Provider  
 ON VM.[ProviderSiteID] = Provider.ProviderSiteID  
    left outer join VacancyProvisionRelationshipStatusType VST on VST.VacancyProvisionRelationshipStatusTypeId = VM.StatusTypeId  
 WHERE EmployerId=@EmployerId  
END