CREATE FUNCTION [dbo].[fnx_ConditionalFreeText]
(@SearchType VARCHAR (30), @SearchTerm VARCHAR (200))
RETURNS 
    @Results TABLE (
        [Key]  INT NOT NULL,
        [Rank] INT NOT NULL)
AS
Begin
DECLARE @EscapedSearchTerm VARCHAR(1000)
SET @EscapedSearchTerm = RTRIM(LTRIM(REPLACE(@SearchTerm,'&','xxvvzz')))

-- Backup the string for Employer and provider Search
DECLARE @EscapedSearchTermForProvider varchar(1000)
set @EscapedSearchTermForProvider = @EscapedSearchTerm
------------------------------

	DECLARE @searchString VARCHAR(1000)
	DECLARE @QuotedSearchTerm VARCHAR(1000)

/********************************************************************

	The following line has been replaced by a block of code so that the FORMSOF call changes from:-
		FORMSOF(THESAURUS, "fashion media photography art tailor ")
	to 
		FORMSOF(THESAURUS, fashion, media, photography, textiles, art)
	
	The key difference is that the first call will perform a cartesian product of the number of synonyms of each word!!!!

--	SET @searchString = 'FORMSOF(THESAURUS, "' + @EscapedSearchTerm + '") OR "'+ @EscapedSearchTerm +'*"'

	New code Starts : 2009-10-09

*********************************************************************/

	DECLARE @backupstr VARCHAR(1000)
	DECLARE @Token Varchar(100)
	DECLARE @Pos int

	SET @Backupstr = @EscapedSearchTerm + '*'
	set @searchString = 'FORMSOF(THESAURUS'
	WHILE LEN(@EscapedSearchTerm) > 0
      BEGIN

            SET @Pos = CHARINDEX (' ', @EscapedSearchTerm)
            
            if  @Pos > 0
                  begin
                        SET @Token = ltrim(rtrim(SUBSTRING (@EscapedSearchTerm, 0, @pos)))
                        SET @searchString = @searchString + ', '+ @Token 
                        SET @EscapedSearchTerm = LTRIM(RTRIM(REPLACE (@EscapedSearchTerm , @token , '')))
                        continue
                  end
            else
                  SET @searchString = @searchString  + ', '+ @EscapedSearchTerm + ')' --+ 'FORMSOF(THESAURUS, ' + @EscapedSearchTerm + ') OR "' + @Backupstr + '"'
                  SET @EscapedSearchTerm = ''
                  break 
      END


set @EscapedSearchTerm = @Backupstr



/********************************************************************

	New code ends : 2009-10-09

*********************************************************************/


	set @QuotedSearchTerm = '"' + @EscapedSearchTerm + '"'
	IF @SearchType = 'Keyword'
		INSERT INTO @Results([Key], [Rank])
	
		/* Contains Search */ 
		-- Wildcarded to do a search term word begins with search.
		SELECT  coalesce(ct.[key], ft1.[key], ft2.[key]), 
		coalesce(ct.rank, 0) + coalesce(ft1.rank, 0) + (coalesce(ft2.rank, 0) * 1.5)
		FROM CONTAINSTABLE(
			vacancysearch,(
				[Title], 
				[ShortDescription], 
				[Description] , 
				[VacancyOwnerName],
				[EmployerName],
				ApprenticeshipFrameworkName, 
				ApprenticeshipOccupationName, 
				EmployerSearch,
				TrainingProviderSearch,
				DeliveryOrganisationSearch,		
				OtherImportantInformation,	
				RealityCheck
			), 
			@searchString) ct full outer join
		FREETEXTTABLE(vacancysearch,(
				[Title], 
				[ShortDescription], 
				[Description] , 
				[VacancyOwnerName],
				[EmployerName],
				ApprenticeshipFrameworkName, 
				ApprenticeshipOccupationName, 
				EmployerSearch,
				TrainingProviderSearch,
				DeliveryOrganisationSearch,		
				OtherImportantInformation,	
				RealityCheck
			), @EscapedSearchTerm) ft1 on ct.[key] = ft1.[key] full outer join
		FREETEXTTABLE(vacancysearch,(
				[Title], 
				[ShortDescription], 
				[Description] , 
				[VacancyOwnerName],
				[EmployerName],
				ApprenticeshipFrameworkName, 
				ApprenticeshipOccupationName, 
				EmployerSearch,
				TrainingProviderSearch,
				DeliveryOrganisationSearch,		
				OtherImportantInformation,	
				RealityCheck
			), @QuotedSearchTerm) ft2 on ct.[key] = ft2.[key] 
	ELSE IF @SearchType = 'Employer'
		insert into @Results([Key],[Rank] ) 
		select * 
		from FREETEXTTABLE(vacancysearch,(
			[EmployerName], 
			EmployerSearch), 
			@EscapedSearchTermForProvider) 

	ELSE IF @SearchType = 'TrainingProvider'
		insert into @Results([Key],[Rank])
		select [key], 10	-- Force equal rank to order by closing date always.
		from containstable(vacancysearch, (trainingprovidersearch, DeliveryOrganisationSearch), @EscapedSearchTermForProvider)
	RETURN				 
END