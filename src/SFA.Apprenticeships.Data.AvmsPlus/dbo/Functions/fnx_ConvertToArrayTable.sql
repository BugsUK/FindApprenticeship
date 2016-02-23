CREATE FUNCTION [dbo].[fnx_ConvertToArrayTable]
(	
	@arrayString nvarchar(4000),
	@Delimiter nvarchar(1)
)
RETURNS @results TABLE 
( [value] varchar(4000) )
AS 
BEGIN
IF (@arrayString IS NOT NULL AND LEN(@arrayString) >= 3)
BEGIN
	Insert into @results
	SELECT 
		SUBSTRING(@arraystring, Numbers.n + 1, CHARINDEX(@Delimiter, @arraystring,Numbers.n + 1) - Numbers.n - 1) as [Value]
	FROM 
		[dbo].[fnx_Numbers](len(@arrayString) -1) Numbers
	WHERE
		SUBSTRING(@arraystring, Numbers.n, 1) = @Delimiter
		AND
		LEN(SUBSTRING(@arraystring, Numbers.n + 1,CHARINDEX(@Delimiter, @arraystring, Numbers.n + 1) - Numbers.n - 1)) > 0 
END
RETURN 
END