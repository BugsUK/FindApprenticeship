-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnx_CalcDistance] 
(
	-- Add the parameters for the function here
	@lat1 decimal(28,15), 
	@lon1 decimal(28,15), 
	@lat2 decimal(28,15), 
	@lon2 decimal(28,15) 
	--@unit char(1),
	--@distance decimal(28,15) output
)
RETURNS DECIMAL(28,15)
AS
BEGIN
	declare @theta decimal(28,15)
	declare @dist decimal(28,15)

    set @theta = @lon1 - @lon2
	
    set @dist =   Sin((@lat1 * PI() / 180.0)) * Sin((@lat2 * PI() / 180.0)) + Cos((@lat1 * PI() / 180.0)) * Cos((@lat2 * PI() / 180.0)) * Cos((@theta * PI() / 180.0))
    
	set @dist = convert(decimal(28,15),Acos(@dist))

	set @dist = (@dist / PI() * 180.0)
	
    set @dist = @dist * 60 * 1.1515
 
    --set @dist = @dist * 0.8684
	
	return convert(decimal(28,18),@dist)	

END