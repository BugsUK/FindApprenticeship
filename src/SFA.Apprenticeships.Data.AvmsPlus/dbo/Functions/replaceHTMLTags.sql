--select dbo.replaceHTMLTags('<em>sd</em>asd asdasd asd asdas  <div><em>sdasd asdasd asd asdas </em> <div>')
CREATE FUNCTION [dbo].[replaceHTMLTags] (@VAL NVARCHAR(MAX))
RETURNS NVARCHAR(MAX)
AS
BEGIN
	
		SET @VAL = REPLACE(@VAL,'<em>','') 
		SET @VAL = REPLACE(@VAL,'<ul>','') 
		SET @VAL = REPLACE(@VAL,'<li>','') 
		SET @VAL = REPLACE(@VAL,'<strong>','') 
		SET @VAL = REPLACE(@VAL,'<span>','') 
		SET @VAL = REPLACE(@VAL,'<div>','') 
		SET @VAL = REPLACE(@VAL,'</em>','') 
		SET @VAL = REPLACE(@VAL,'</ul>','') 
		SET @VAL = REPLACE(@VAL,'</li>','') 
		SET @VAL = REPLACE(@VAL,'</strong>','') 
		SET @VAL = REPLACE(@VAL,'</span>','') 
		SET @VAL = REPLACE(@VAL,'</div>','') 
		SET @VAL = REPLACE(@VAL,'<br />',' ') 
		SET @VAL = REPLACE(@VAL,'<span style="text-decoration: underline">','') 


RETURN 
(@VAL);
END