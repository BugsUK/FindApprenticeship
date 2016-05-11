--/*----------------------------------------------------------------------                               
--Name  : fnReportSplitListToTable                  
--Description :  returns A table from a comma seprated list
--
--                
--History:                  
----------                  
--Date			Version		Author			Comment
--09-SEP-2008		1.0		Ian Emery		first version
------------------------------------------------------------------------ */          
CREATE FUNCTION [dbo].[fnReportSplitListToTable](@text varchar(8000))
   RETURNS @words TABLE (
      --pos smallint primary key,
      ID  varchar(8000)
   )
AS
BEGIN
   DECLARE
      @pos smallint,
      @i smallint,
      @j smallint,
      @s varchar(8000)
	 -- @cnt integer

   --set @cnt=1
   SET @pos = 1
   WHILE @pos <= LEN(@text) 
   BEGIN 
      --SET @i = CHARINDEX(' ', @text, @pos)
      --SET @j = CHARINDEX(',', @text, @pos)
      SET @i = CHARINDEX(',', @text, @pos)
      IF @i > 0 OR @j > 0
      BEGIN
         --IF @i = 0 OR (@j > 0 AND @j < @i)
            --SET @i = @j

         IF @i > @pos
         BEGIN
            -- @i now holds the earliest delimiter in the string
            SET @s = SUBSTRING(@text, @pos, @i - @pos)

            INSERT INTO @words
            VALUES ( @s)
			--set @cnt = @cnt + 1
         END 
         SET @pos = @i + 1

         WHILE @pos < LEN(@text) 
            AND SUBSTRING(@text, @pos, 1) IN (',')--(' ', ',')
            SET @pos = @pos + 1 
      END 
      ELSE 
      BEGIN 
         INSERT INTO @words 
         VALUES ( SUBSTRING(@text, @pos, LEN(@text) - @pos + 1))

         SET @pos = LEN(@text) + 1 
      END 
   END 
   RETURN 
END