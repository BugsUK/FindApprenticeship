-- =============================================
-- Author:		Kate Cookson
-- Create date: 20 August 2008
-- Description:	Check to see whether 
-- =============================================
CREATE PROCEDURE [dbo].[uspPersonAndCandidateIsEmailRegistered] 
	-- Add the parameters for the stored procedure here
	@email varchar(100),
	@result bit out
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @count int

	SELECT @count=count(Email) 
	FROM Person Where Email=@email

	SELECT @count = @count + count(UnconfirmedEmailAddress) 
	FROM dbo.Candidate WHERE UnconfirmedEmailAddress=@email

	IF(@count > 0)
		SET @result=1
	ELSE
		SET @result=0

	SELECt @result
END