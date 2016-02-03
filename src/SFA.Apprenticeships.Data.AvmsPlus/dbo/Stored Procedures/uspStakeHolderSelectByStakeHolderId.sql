CREATE PROCEDURE [dbo].[uspStakeHolderSelectByStakeHolderId]
@StakeHolderId INT
AS
SET NOCOUNT ON;    

	Select 
		S.PersonId, P.Title, P.OtherTitle, P.FirstName, P.MiddleNames, 
		P.Surname, P.LandlineNumber, P.MobileNumber, P.PersonTypeId, P.Email,
		S.AddressLine1, S.AddressLine2, S.AddressLine3, 
		S.AddressLine4, S.AddressLine5, S.StakeHolderStatusId, 
		S.OrganisationId, S.OrganisationOther, S.Postcode, S.Town, 
		S.CountyId, S.UnconfirmedEmailAddress, S.EmailAlertSent, S.StakeHolderId,
		S.[LastAccessedDate]
	From	StakeHolder S, Person P
	Where	S.PersonId = P.PersonId 
	And		S.StakeHolderId = @StakeHolderId