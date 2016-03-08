CREATE PROCEDURE [dbo].[uspGetLocalAuthorityGroupById]
	@LocalAuthorityGroupID INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [LocalAuthorityGroupID], 
		   [CodeName], 
		   [ShortName],
		   [FullName],
		   [LocalAuthorityGroupTypeID],
		   [LocalAuthorityGroupPurposeID], 
		   [ParentLocalAuthorityGroupID] 
		   FROM [LocalAuthorityGroup] 
		   WHERE [LocalAuthorityGroupID] = @LocalAuthorityGroupID
END