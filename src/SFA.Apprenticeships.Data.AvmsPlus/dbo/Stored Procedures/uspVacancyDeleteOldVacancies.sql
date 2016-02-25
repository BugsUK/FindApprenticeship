CREATE PROCEDURE [dbo].[uspVacancyDeleteOldVacancies]      
AS      
BEGIN      
	SET NOCOUNT ON

	IF OBJECT_ID('tempdb..#vacancyIds') IS NOT NULL DROP TABLE #vacancyIds
	CREATE TABLE #vacancyIds ( VacancyId INT PRIMARY KEY )

	BEGIN

		DECLARE @archiveDate DATETIME


		DECLARE @postedInErrorLimit INT, @withdrawnLimit INT, @draftLimit INT, @referredLimit INT, @completedLimit INT

		SELECT @postedInErrorLimit= CAST(ParameterValue as INT) FROM SystemParameters WHERE ParameterName = 'MaxAgeVacanciesPostedInError'
		SELECT @withdrawnLimit= CAST(ParameterValue as INT) FROM SystemParameters WHERE ParameterName = 'MaxAgeVacanciesWithdrawn'
		SELECT @draftLimit= CAST(ParameterValue as INT) FROM SystemParameters WHERE ParameterName = 'MaxAgeVacanciesDraft'
		SELECT @completedLimit= CAST(ParameterValue as INT) FROM SystemParameters WHERE ParameterName = 'MaxAgeVacanciesCompleted'
		SELECT @referredLimit= CAST(ParameterValue as INT) FROM SystemParameters WHERE ParameterName = 'MaxAgeVacanciesReferred'


	------------------------------------------------------------------------------------------------
	-- Posted in Error START
	-- delete after 6 months of being posted in error
	------------------------------------------------------------------------------------------------

		-- Set archive date
		SET @archiveDate = DATEADD( day, -@postedInErrorLimit, GETDATE() )

		-- Collect vacancyIds to delete based on date;
		-- ie the last time anything happened to this record was n years ago
		INSERT INTO #vacancyIds ( VacancyId )
		SELECT v.VacancyId
		FROM dbo.Vacancy v
			INNER JOIN dbo.VacancyStatusType vst ON v.VacancyStatusId = vst.VacancyStatusTypeId
		WHERE vst.FullName = 'Posted in Error'
		  AND NOT EXISTS
			(
				SELECT *
				FROM dbo.VacancyHistory vh
				WHERE v.VacancyId = vh.VacancyId
				  AND vh.HistoryDate > @archiveDate
			)

	-- Posted in Error END
	------------------------------------------------------------------------------------------------



	------------------------------------------------------------------------------------------------
	-- Withdrawn START
	-- delete after 365 days of being set to withdrawn – in case of any legal challenge from a candidate
	------------------------------------------------------------------------------------------------

		SET @archiveDate = DATEADD( day, -@withdrawnLimit, GETDATE() )

		INSERT INTO #vacancyIds ( VacancyId )
		SELECT v.VacancyId
		FROM dbo.Vacancy v
			INNER JOIN dbo.VacancyStatusType vst ON v.VacancyStatusId = vst.VacancyStatusTypeId
		WHERE vst.FullName = 'Withdrawn'
		  AND NOT EXISTS
			(
				SELECT *
				FROM dbo.VacancyHistory vh
				WHERE v.VacancyId = vh.VacancyId
				  AND vh.HistoryDate > @archiveDate
			)

	-- Withdrawn END
	------------------------------------------------------------------------------------------------



	------------------------------------------------------------------------------------------------
	-- Draft START
	-- delete after 6 months of being saved as draft
	------------------------------------------------------------------------------------------------

		SET @archiveDate = DATEADD( day, -@draftLimit, GETDATE() )

		INSERT INTO #vacancyIds ( VacancyId )
		SELECT v.VacancyId
		FROM dbo.Vacancy v
			INNER JOIN dbo.VacancyStatusType vst ON v.VacancyStatusId = vst.VacancyStatusTypeId
		WHERE vst.FullName = 'Draft'
		  AND NOT EXISTS
			(
				SELECT *
				FROM dbo.VacancyHistory vh
				WHERE v.VacancyId = vh.VacancyId
				  AND vh.HistoryDate > @archiveDate
			)

	-- Draft END
	------------------------------------------------------------------------------------------------



	------------------------------------------------------------------------------------------------
	-- Referred START
	-- delete after 12 months of being saved as Referred
	------------------------------------------------------------------------------------------------

		SET @archiveDate = DATEADD( day, -@referredLimit, GETDATE() )

		INSERT INTO #vacancyIds ( VacancyId )
		SELECT v.VacancyId
		FROM dbo.Vacancy v
			INNER JOIN dbo.VacancyStatusType vst ON v.VacancyStatusId = vst.VacancyStatusTypeId
		WHERE vst.FullName = 'Referred'
		  AND NOT EXISTS
			(
				SELECT *
				FROM dbo.VacancyHistory vh
				WHERE v.VacancyId = vh.VacancyId
				  AND vh.HistoryDate > @archiveDate
			)

	-- Referred END
	------------------------------------------------------------------------------------------------



	------------------------------------------------------------------------------------------------
	-- Completed START
	-- delete after 2 years from the date vacancy is set to completed
	------------------------------------------------------------------------------------------------

		SET @archiveDate = DATEADD( day, -@completedLimit, GETDATE() )

		INSERT INTO #vacancyIds ( VacancyId )
		SELECT v.VacancyId
		FROM dbo.Vacancy v
			INNER JOIN dbo.VacancyStatusType vst ON v.VacancyStatusId = vst.VacancyStatusTypeId
		WHERE vst.FullName = 'Completed'
		  AND NOT EXISTS
			(
				SELECT *
				FROM dbo.VacancyHistory vh
				WHERE v.VacancyId = vh.VacancyId
				  AND vh.HistoryDate > @archiveDate
			)

	-- Completed END
	------------------------------------------------------------------------------------------------

			BEGIN TRY
				BEGIN TRAN

				------------------------------------------------------------------------------------------------
				-- uspVacancyDelete Additions START
				------------------------------------------------------------------------------------------------

				DELETE aa
				FROM dbo.AdditionalAnswer aa
					INNER JOIN dbo.[Application] ap ON aa.ApplicationId = ap.ApplicationId
				WHERE EXISTS
					(
					SELECT *
					FROM #vacancyIds t
					WHERE ap.VacancyId = t.VacancyId
					)


				DELETE ea
				FROM dbo.ExternalApplication ea
				WHERE EXISTS
					(
					SELECT *
					FROM #vacancyIds t
					WHERE ea.VacancyId = t.VacancyId
					)

				
				DELETE ah
				FROM dbo.ApplicationHistory ah
					INNER JOIN dbo.[Application] ap ON ah.ApplicationId = ap.ApplicationId
				WHERE EXISTS
					(
					SELECT *
					FROM #vacancyIds t
					WHERE ap.VacancyId = t.VacancyId
					)

				
				DELETE caf
				FROM dbo.CAFFields caf
					INNER JOIN dbo.[Application] ap ON caf.ApplicationId = ap.ApplicationId
				WHERE EXISTS
					(
					SELECT *
					FROM #vacancyIds t
					WHERE ap.VacancyId = t.VacancyId
					)


				DELETE vs
				FROM dbo.VacancySearch vs
				WHERE EXISTS
					(
					SELECT *
					FROM #vacancyIds t
					WHERE vs.VacancyId = t.VacancyId
					)


				DELETE vl
				FROM dbo.VacancyLocation vl
				WHERE EXISTS
					(
					SELECT *
					FROM #vacancyIds t
					WHERE vl.VacancyId = t.VacancyId
					)

				-- uspVacancyDelete Additions END
				------------------------------------------------------------------------------------------------


				------------------------------------------------------------------------------------------------
				-- uspVacancyDelete START
				------------------------------------------------------------------------------------------------
	
				DELETE v
				FROM dbo.VacancyTextField v
				WHERE EXISTS
					(
					SELECT *
					FROM #vacancyIds t
					WHERE v.VacancyId = t.VacancyId
					)

				DELETE v
				FROM [dbo].[AdditionalQuestion] v
				WHERE EXISTS
					(
					SELECT *
					FROM #vacancyIds t
					WHERE v.VacancyId = t.VacancyId
					)

				DELETE v
				FROM [dbo].[subVacancy] v
				WHERE EXISTS
					(
					SELECT *
					FROM #vacancyIds t
					WHERE v.VacancyId = t.VacancyId
					)

				

				DELETE a
				FROM [dbo].[Application] a
				WHERE EXISTS
					(
					SELECT *
					FROM #vacancyIds t
					WHERE a.VacancyId = t.VacancyId
					)


				DELETE v
				FROM [dbo].VacancyHistory v
				WHERE EXISTS
					(
					SELECT *
					FROM #vacancyIds t
					WHERE v.VacancyId = t.VacancyId
					)


				DELETE v
				FROM [dbo].[WatchedVacancy] v
				WHERE EXISTS
					(
					SELECT *
					FROM #vacancyIds t
					WHERE v.VacancyId = t.VacancyId
					)


				DELETE v
				FROM [dbo].[VacancyReferralComments] v
				WHERE EXISTS
					(
					SELECT *
					FROM #vacancyIds t
					WHERE v.VacancyId = t.VacancyId
					)


				UPDATE v
				SET MasterVacancyId = NULL
				FROM [dbo].[Vacancy] v
				WHERE EXISTS
					(
					SELECT *
					FROM #vacancyIds t
					WHERE v.MasterVacancyId = t.VacancyId
					)

				-- Status summary
				SELECT v.VacancyStatusId, vst.FullName, COUNT(*) AS records
				FROM dbo.Vacancy v
					INNER JOIN dbo.VacancyStatusType vst ON v.VacancyStatusId = vst.VacancyStatusTypeId
				WHERE EXISTS
					(
					SELECT *
					FROM #vacancyIds t
					WHERE v.VacancyId = t.VacancyId
					)
				GROUP BY v.VacancyStatusId, vst.FullName
				ORDER BY v.VacancyStatusId


				-- Finally, do the dbo.Vacancy delete
				DELETE v
				FROM [dbo].[Vacancy] v
				WHERE EXISTS
					(
					SELECT *
					FROM #vacancyIds t
					WHERE v.VacancyId = t.VacancyId
					)

				-- uspVacancyDelete END
				------------------------------------------------------------------------------------------------

				-- Everything has been successful; commit the transaction
				COMMIT TRAN

			END TRY

			BEGIN CATCH

				DECLARE @errorMessage NVARCHAR(2048) = ERROR_MESSAGE()
				RAISERROR( @errorMessage, 16, 1 )

				IF @@trancount > 0
				BEGIN
					RAISERROR ( 'Error handler rolling back ...', 16, 1 )
					ROLLBACK
				END

			END CATCH
	END

	-- Finally block
	IF @@trancount > 0
	BEGIN
		RAISERROR ( 'Finally handler rolling back ...', 16, 1 )
		ROLLBACK
	END

	SET NOCOUNT OFF      
	RETURN 0
END