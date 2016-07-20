/*----------------------------------------------------------------------                               
Name  : fnPopulateEthnicTempTable                  
Description :  returns table to join on for selected ethnicyt

                
History:                  
------                  
Date			Version		Author		Comment
10-Nov-2008		1.0			Ian Emery	first version

-------------------------------------------------------------------- */                 

CREATE FUNCTION [dbo].[fnPopulateEthnicTempTable](@EthnicityID int)
   RETURNS @t TABLE (ID int) 
AS

BEGIN
	IF EXISTS (SELECT * FROM CandidateEthnicOrigin WHERE CandidateEthnicOriginID = @EthnicityID AND FullName = 'Please Select')
		INSERT INTO @t SELECT CandidateEthnicOriginID FROM CandidateEthnicOrigin 
			WHERE ShortName = (SELECT ShortName FROM CandidateEthnicOrigin 
				WHERE CandidateEthnicOriginID = @EthnicityID
					)AND FullName <> 'Please Select'
	ELSE
		INSERT INTO @t SELECT @EthnicityID
		
	RETURN  
END


