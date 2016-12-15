/*----------------------------------------------------------------------                  
Name  : ReportGetVacancyTracker                 
Description :  Returns Actioned Vacancy by Serco between the given dates.
                
History:                  
--------                  
Date			Version		Author			Comment
26-Aug-2008		1.0			Femma Ashraf	first version
05-Oct-2008		1.1			Ian Emery		removed parameter @type
---------------------------------------------------------------------- */ 

CREATE PROCEDURE [dbo].[GetVacancyTracker]
	@param1 int = 0,
	@param2 int
AS
	SELECT @param1, @param2
RETURN 0
