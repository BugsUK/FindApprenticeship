-- =============================================
-- Author:		Manish
-- Create date: 16 sep 2008
-- Description:	It will return the list of SIC Codes for an employer
-- =============================================
CREATE FUNCTION [dbo].[fnx_GetSICCodes]
(
	@EmployerId int
)
RETURNS varchar(200)
AS
BEGIN

	DECLARE @SICCodes Varchar(200)
	select @SICCODes = COALESCE(@SICCodes + ', ', '') + CAST(sic.SICCode as varchar(20)) from EMPLOYER emp 
	inner join EMPLOYERSICCODES empsic on emp.employerid=empsic.employerid inner 
	join SICCODE sic on empsic.sicid=sic.siccodeid where emp.EmployerId = @EmployerId

	RETURN @SICCodes
	
END