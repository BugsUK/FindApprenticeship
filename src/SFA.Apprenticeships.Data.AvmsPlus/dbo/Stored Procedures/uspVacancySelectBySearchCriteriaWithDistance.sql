CREATE PROCEDURE [dbo].[uspVacancySelectBySearchCriteriaWithDistance]
	@SearchType varchar(30),
	@apprenticeOccupation int = 0,
	@apprenticeFramework int = 0,
	@postCode varchar(20) = null,
	@location varchar(100) = null,
	@vacPostedSince datetime =  null,
	@Employer varchar(100) = null,
	@trainingProvider varchar(100) = null,
	@keyWord varchar(200) = null,
	@VacancyReferenceNumber varchar(100) = null,
	@Latitude decimal(28,15) = null,
	@Longitude decimal(28,15) = null,
	@Distancefrom decimal(28,15) = null,
	@WeeklyWagesFrom int = null,
	@WeeklyWagesTo int = null,
	@PageNo int = 1,
	@PageSize int = 10,
	@SortByField nvarchar(100)='ClosingDate',
	@SortOrder bit = 1
AS
BEGIN
	SET NOCOUNT ON
			--SET @Distancefrom =  @Distancefrom * 1609;
			
			DECLARE @DispRowFrom int
			DECLARE @DispRowTo int

			-- temporary measure until @location (argument) is changed to an int
			DECLARE @locationID int
			SET @locationID=CAST(@location as int)

			 if @apprenticeOccupation = 0 or @apprenticeOccupation = -1
				set @apprenticeOccupation = null

			if @apprenticeFramework = 0 or @apprenticeFramework = -1
				set @apprenticeFramework = null

			
			if @locationID = -1
			BEGIN
				SET @locationID = NULL
			END
			
			if @PageNo = 1
			BEGIN
				SET @DispRowFrom = 1
				SET @DispRowTo = @DispRowFrom + @PageSize
			END

			if @PageNo > 1
			BEGIN
				SET @DispRowFrom = (@PageNo * @PageSize) + 1
				SET @DispRowTo = (@DispRowFrom + @PageSize)
			END

			/*********set the order by**********************************************/
			if @SortByField = 'EmployerName'
			Begin 
				if @SortOrder = 1 BEGIN set  @SortByField = 'EmployerName Asc' END
				if @SortOrder = 0 BEGIN  set  @SortByField = 'EmployerName desc' END
			End
			if @SortByField = 'ClosingDate'
			Begin 
				if @SortOrder = 1 BEGIN set  @SortByField = 'ApplicationClosingDate Asc' END
				if @SortOrder = 0 BEGIN  set  @SortByField = 'ApplicationClosingDate desc' END
			End
			/***********************************************************************/

			/**************Total Number of Rows*************************************/
			declare @TotalRows int
			
			/***********************************************************************/
		
			if @SearchType = 'Occupation' 
			BEGIN

				select @TotalRows= count(1) 
				FROM [dbo].[vacancySearch] with (nolock)
					WHERE  ([ApprenticeshipFrameworkId] in (@apprenticeFramework)  or  @apprenticeFramework is null) AND 
						   --[Vacancy].[PostCode] like isnull(@postCode,'%') + '%'  AND  
						   --[CountyId] Like isnull(@locationID,'%') AND   
							([CountyId] = @locationId OR @locationID IS NULL OR @locationID=-1) AND
						   ([WeeklyWage] >= isnull(@WeeklyWagesFrom,0) AND [WeeklyWage] <= isnull(@WeeklyWagesTo,100000)) AND     
						   dbo.fnx_CalcDistance(@Latitude,@Longitude,[vacancySearch].[Latitude],[vacancySearch].[Longitude]) <= @Distancefrom AND
						   [VacancyPostedDate] >= isnull(@vacPostedSince,'01-01-1900')

				SELECT *,@TotalRows AS TotalRows   FROM
				(
					SELECT distinct
							ROW_NUMBER() OVER( ORDER BY 
								Case when @SortByField='EmployerName Asc'  then [EmployerName]  End ASC,
								Case when @SortByField='EmployerName desc'  then [EmployerName]  End DESC,
								Case when @SortByField='ApplicationClosingDate Asc'  then [ApplicationClosingDate]  End ASC,
								Case when @SortByField='ApplicationClosingDate desc'  then [ApplicationClosingDate]  End DESC
								) as RowNum, 
 							[vacancyid] AS 'vacancyid',
							[Title] AS 'Title',
							[EmployerName] As 'Employer',
							[Town] AS 'Town',
							[ShortDescription] AS 'ShortDescription',
							[VacancyPostedDate] AS 'Vacancy Posted Date', 
							[ApprenticeshipType] As 'ApprenticeShipType',
							'Framework' As 'Framework',
							[ApplicationClosingDate] AS 'ClosingDate'
							FROM [dbo].[vacancySearch] with (nolock)
							WHERE 
								  ([ApprenticeshipFrameworkId] in (@apprenticeFramework)  or  @apprenticeFramework is null) AND 
								   --[Vacancy].[PostCode] like isnull(@postCode,'%') + '%'  AND  
								  ([CountyId] = @locationId OR @locationID IS NULL OR @locationID=-1) AND
								   ([WeeklyWage] >= isnull(@WeeklyWagesFrom,0) AND [WeeklyWage] <= isnull(@WeeklyWagesTo,100000)) AND     
								   dbo.fnx_CalcDistance(@Latitude,@Longitude,[vacancySearch].[Latitude],[vacancySearch].[Longitude]) <= @Distancefrom AND
								   [VacancyPostedDate] >= isnull(@vacPostedSince,'01-01-1900')
						   

				) AS D
				WHERE RowNum >= @DispRowFrom AND
					  RowNum < @DispRowTo
			END


			if @SearchType = 'Keyword'
			BEGIN

				select @TotalRows= count(1) 
				FROM [dbo].[vacancySearch] with (nolock)
						 
						WHERE 
							([Title] like '%' + isnull(@keyWord,'%') + '%' OR [ShortDescription] like '%' + isnull(@keyWord,'%') + '%' OR [Description] like '%' + isnull(@keyWord,'%') + '%')AND 
							--[Vacancy].[PostCode] like isnull(@postCode,'%')  + '%' AND
							--[CountyID] like isnull(@location,'%') AND  
							([CountyId] = @locationId OR @locationID IS NULL OR @locationID=-1) AND
							([WeeklyWage] >= isnull(@WeeklyWagesFrom,0) AND [WeeklyWage] <= isnull(@WeeklyWagesTo,100000)) AND
							dbo.fnx_CalcDistance(@Latitude,@Longitude,[Latitude],[Longitude]) <= @Distancefrom AND
							[VacancyPostedDate] >= isnull(@vacPostedSince,'01-01-1900')

					
       
				SELECT *,@TotalRows AS TotalRows   FROM
				(
					SELECT distinct
							ROW_NUMBER() OVER( ORDER BY 
								Case when @SortByField='EmployerName Asc'  then [EmployerName]  End ASC,
								Case when @SortByField='EmployerName desc'  then [EmployerName]  End DESC,
								Case when @SortByField='ApplicationClosingDate Asc'  then [ApplicationClosingDate]  End ASC,
								Case when @SortByField='ApplicationClosingDate desc'  then [ApplicationClosingDate]  End DESC
								) as RowNum,
							[vacancyid] AS 'vacancyid',
							[Title] AS 'Title',
							[EmployerName] As 'Employer',
							[Town] AS 'Town',
							[ShortDescription] AS 'ShortDescription',
							[VacancyPostedDate] AS 'Vacancy Posted Date' ,
							[ApprenticeshipType] As 'ApprenticeShipType',
							'Framework' As 'Framework',
							[ApplicationClosingDate] AS 'ClosingDate'
						FROM [dbo].[vacancySearch] with (nolock)
						WHERE ([Title] like '%' + isnull(@keyWord,'%') + '%' OR [ShortDescription] like '%' + isnull(@keyWord,'%') + '%' OR [Description] like '%' + isnull(@keyWord,'%') + '%')AND 
							--[CountyID] like isnull(@location,'%') AND 
							([CountyId] = @locationId OR @locationID IS NULL OR @locationID=-1) AND 
							([WeeklyWage] >= isnull(@WeeklyWagesFrom,0) AND [WeeklyWage] <= isnull(@WeeklyWagesTo,100000)) AND
							dbo.fnx_CalcDistance(@Latitude,@Longitude,[Latitude],[Longitude]) <= @Distancefrom AND
							[VacancyPostedDate] >= isnull(@vacPostedSince,'01-01-1900')
				) AS D
				WHERE RowNum >= @DispRowFrom AND
					  RowNum < @DispRowTo
					  
			END


			if @SearchType = 'Employer'
			BEGIN

				select @TotalRows= count(1) 
				FROM [dbo].[vacancysearch] with (nolock)
					WHERE [Employername] like isnull(@Employer,'%') + '%' AND 
							--[Vacancy].[PostCode] like isnull(@postCode,'%')  + '%' AND
							--[CountyId] like isnull(@location,'%') AND 
							([CountyId] = @locationId OR @locationID IS NULL OR @locationID=-1) AND
							([WeeklyWage] >= isnull(@WeeklyWagesFrom,0) AND [WeeklyWage] <= isnull(@WeeklyWagesTo,100000)) AND
							dbo.fnx_CalcDistance(@Latitude,@Longitude,[vacancySearch].[Latitude],[vacancySearch].[Longitude]) <= @Distancefrom AND						
							[VacancyPostedDate] >= isnull(@vacPostedSince,'01-01-1900') 
      

				SELECT *,@TotalRows AS TotalRows   FROM
				(
					SELECT distinct
							ROW_NUMBER() OVER( ORDER BY 
								Case when @SortByField='EmployerName Asc'  then [EmployerName]  End ASC,
								Case when @SortByField='EmployerName desc'  then [EmployerName]  End DESC,
								Case when @SortByField='ApplicationClosingDate Asc'  then [ApplicationClosingDate]  End ASC,
								Case when @SortByField='ApplicationClosingDate desc'  then [ApplicationClosingDate]  End DESC
								) as RowNum,
							[vacancyid] AS 'vacancyid',
							[Title] AS 'Title',
							[EmployerName] As 'Employer',
							[Town] AS 'Town',
							[ShortDescription] AS 'ShortDescription',
							[VacancyPostedDate] AS 'Vacancy Posted Date' ,
							[ApprenticeshipType] As 'ApprenticeShipType',
							'Framework' As 'Framework',
							[ApplicationClosingDate] AS 'ClosingDate'
					FROM [dbo].[vacancySearch] with (nolock)
							
					WHERE [Employername] like isnull(@Employer,'%') + '%' AND 
							--[Vacancy].[PostCode] like isnull(@postCode,'%')  + '%' AND
							--[CountyId] like isnull(@location,'%') AND  
							([CountyId] = @locationId OR @locationID IS NULL OR @locationID=-1) AND
							([WeeklyWage] >= isnull(@WeeklyWagesFrom,0) AND [WeeklyWage] <= isnull(@WeeklyWagesTo,100000)) AND
							dbo.fnx_CalcDistance(@Latitude,@Longitude,[vacancySearch].[Latitude],[vacancySearch].[Longitude]) <= @Distancefrom AND						
							[VacancyPostedDate] >= isnull(@vacPostedSince,'01-01-1900') 
				) AS D
				WHERE RowNum >= @DispRowFrom AND
					  RowNum < @DispRowTo
			END


			if @SearchType = 'TrainingProvider' 
			BEGIN
				select @TotalRows= count(1) 
				FROM [dbo].[vacancySearch] with (nolock)
					WHERE 	--[Vacancy].[PostCode] like isnull(@postCode,'%')  + '%' AND
							-- [CountyID] like isnull(@location,'%') AND 
							([CountyId] = @locationId OR @locationID IS NULL OR @locationID=-1) AND
							 [VacancyOwnerName] like isnull(@TrainingProvider,'%')AND   
							([WeeklyWage] >= isnull(@WeeklyWagesFrom,0) AND [WeeklyWage] <= isnull(@WeeklyWagesTo,100000)) AND
							dbo.fnx_CalcDistance(@Latitude,@Longitude,[vacancySearch].[Latitude],[vacancySearch].[Longitude]) <= @Distancefrom AND	
							[VacancyPostedDate] >= isnull(@vacPostedSince,'01-01-1900') 


				SELECT *,@TotalRows AS TotalRows FROM
				(
					SELECT distinct
							ROW_NUMBER() OVER( ORDER BY 
								Case when @SortByField='EmployerName Asc'  then [EmployerName]  End ASC,
								Case when @SortByField='EmployerName desc'  then [EmployerName]  End DESC,
								Case when @SortByField='ApplicationClosingDate Asc'  then [ApplicationClosingDate]  End ASC,
								Case when @SortByField='ApplicationClosingDate desc'  then [ApplicationClosingDate]  End DESC
								) as RowNum,
							[vacancyid] AS 'vacancyid',
							[Title] AS 'Title',
							[EmployerName] As 'Employer',
							[Town] AS 'Town',
							[ShortDescription] AS 'ShortDescription',
							[VacancyPostedDate] AS 'Vacancy Posted Date' ,
							[ApprenticeshipType] As 'ApprenticeShipType',
							'Framework' As 'Framework',
							[ApplicationClosingDate] AS 'ClosingDate'
							
					FROM [dbo].[vacancySearch] with (nolock)
					WHERE 	--[Vacancy].[PostCode] like isnull(@postCode,'%')  + '%' AND
							 --[CountyID] like isnull(@location,'%') AND
							([CountyId] = @locationId OR @locationID IS NULL OR @locationID=-1) AND
							 [VacancyOwnerName] like isnull(@TrainingProvider,'%')AND   
							([WeeklyWage] >= isnull(@WeeklyWagesFrom,0) AND [WeeklyWage] <= isnull(@WeeklyWagesTo,100000)) AND
							dbo.fnx_CalcDistance(@Latitude,@Longitude,[vacancySearch].[Latitude],[vacancySearch].[Longitude]) <= @Distancefrom AND	
							[VacancyPostedDate] >= isnull(@vacPostedSince,'01-01-1900') 

				) AS D
				WHERE RowNum >= @DispRowFrom AND
					  RowNum < @DispRowTo
			END

			IF @SearchType = 'VacancyReferenceNumber'
			BEGIN
				select @TotalRows= count(1) 
				FROM [dbo].[vacancySearch] with (nolock)
					WHERE [VacancyReferenceNumber] = @VacancyReferenceNumber
						
				SELECT *,@TotalRows AS TotalRows FROM
				(
					SELECT distinct
							ROW_NUMBER() OVER( ORDER BY 
									Case when @SortByField='EmployerName Asc'  then [EmployerName]  End ASC,
									Case when @SortByField='EmployerName desc'  then [EmployerName]  End DESC,
									Case when @SortByField='ApplicationClosingDate Asc'  then [ApplicationClosingDate]  End ASC,
									Case when @SortByField='ApplicationClosingDate desc'  then [ApplicationClosingDate]  End DESC
									) as RowNum,
							[vacancyid] AS 'vacancyid',
							[Title] AS 'Title',
							[EmployerName] As 'Employer',
							[Town] AS 'Town',
							[ShortDescription] AS 'ShortDescription',
							[VacancyPostedDate] AS 'Vacancy Posted Date' ,
							[ApprenticeshipType] As 'ApprenticeShipType',
							'Framework' As 'Framework',
							[ApplicationClosingDate] AS 'ClosingDate'
					FROM [dbo].[vacancySearch] with (nolock)
						WHERE [VacancyReferenceNumber] = @VacancyReferenceNumber
							
				) AS D
				WHERE RowNum >= @DispRowFrom AND
					  RowNum < @DispRowTo
	
			END

	SET NOCOUNT OFF
END