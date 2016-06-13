CREATE PROCEDURE [dbo].[uspTrainingProviderInsert]


@UKPRN INT, 
@UPIN INT, 
@fullName  NVARCHAR (255), 
@tradingName  NVARCHAR (255),
@isContracted BIT, 
@StartDate datetime,
@EndDate datetime,
@trainingProviderId INT OUTPUT

AS
BEGIN      
SET NOCOUNT ON      
   
   
       
BEGIN TRY      
IF NOT EXISTS( select UKPRN from [dbo].[Provider] where UKPRN = @UKPRN AND ProviderStatusTypeID <> 2 )    
BEGIN    
INSERT INTO [Provider]	
           ([UPIN]
           ,[UKPRN]
           ,[FullName]
           ,[TradingName]
           ,[IsContracted]
           ,[ContractedFrom]
           ,[ContractedTo]
           ,[OriginalUPIN]
           )
     VALUES
     (		@UPIN
           ,@UKPRN    
           ,@fullName
           ,@tradingName
           ,@isContracted
           ,@startDate
           ,@enddate
           ,@UPIN
           )  
    
SET @trainingProviderId = SCOPE_IDENTITY()      
END    
    
ELSE    
    
BEGIN    
SELECT @trainingProviderId = ProviderID from [dbo].[Provider] where UKPRN = @UKPRN AND ProviderStatusTypeID <> 2 
  
UPDATE [dbo].[Provider]    
   SET [UPIN] = ISNULL(@UPIN,[UPIN])   
      ,[UKPRN] = ISNULL(@UKPRN,[UKPRN])    
      ,[FullName] = ISNULL(@fullName,[FullName])  
      ,[TradingName] =ISNULL(@tradingName,[TradingName])  
      ,[IsContracted] = ISNULL(@IsContracted,[IsContracted]) 
      ,[ContractedFrom] = ISNULL(@startDate,[ContractedFrom]) 
      ,[ContractedTo]    = ISNULL(@enddate,[ContractedTo]) 
 WHERE ProviderID=@trainingProviderId        
  
END    
      
END TRY      
      
BEGIN CATCH      
 EXEC RethrowError;      
END CATCH      
          
SET NOCOUNT OFF      
    
END