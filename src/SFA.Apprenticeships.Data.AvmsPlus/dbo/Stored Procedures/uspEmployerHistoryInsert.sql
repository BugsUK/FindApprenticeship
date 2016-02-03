CREATE PROCEDURE [dbo].[uspEmployerHistoryInsert]  
    @comment varchar(4000) = NULL,  
 @date datetime,  
 @employerHistoryId int ,  
 @employerId int,  
 @event smallint,  
 @userName nvarchar(50)  
AS  
BEGIN  
 SET NOCOUNT ON  
   
 BEGIN TRY  
    INSERT INTO [dbo].[EmployerHistory] ([Comment], [Date], [EmployerId], [Event], [UserName])  
 VALUES (@comment, @date, @employerId, @event, @userName)  
    SET @employerHistoryId = SCOPE_IDENTITY()  
    END TRY  
  
    BEGIN CATCH  
  EXEC RethrowError;  
 END CATCH  
      
    SET NOCOUNT OFF  
END