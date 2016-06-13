CREATE PROCEDURE [dbo].[uspPersonIsEmailRegistered]  
	@email VARCHAR (100), 
	@result BIT OUTPUT  
AS  
BEGIN  
	-- SET NOCOUNT ON added to prevent extra result sets from  
	-- interfering with SELECT statements.  
	SET NOCOUNT ON;  
	DECLARE @count int  

	SELECT @count=count(Email)   
		FROM Person Where Email=@email  

	SELECT @count = @count + count(UnconfirmedEmailAddress)   
		FROM Candidate Where UnconfirmedEmailAddress = @email  

	SELECT @count = @count + count(UnconfirmedEmailAddress)   
		FROM StakeHolder WHERE UnconfirmedEmailAddress = @email  

	IF(@count > 0)  
		SET @result=1  
	ELSE  
		SET @result=0  

	SELECt @result  
END