-- =============================================
-- Author:		Kate Cookson
-- Create date: 23 July 2008
-- Description:	Remove the time from date so that
--				date comparisons don't return unexpected
--				results
-- =============================================
CREATE FUNCTION [dbo].[fnx_RemoveTime]
(
	@date DATETIME
)
RETURNS DATETIME
AS
BEGIN

	DECLARE @return datetime
	--DateTime is stored internally as a float, with everything to the right of the decimal point
	--being used for time. 
	SET @return = CAST(FLOOR(CAST(@date AS FLOAT))AS DATETIME)

	RETURN @return 

END