CREATE PROCEDURE [dbo].[uspCandidateDeRegister]
@candidateId INT,
@isHardDelete bit

AS
BEGIN 
	SET NOCOUNT ON 	
	BEGIN TRANSACTION
	BEGIN TRY
				-- Order in which records are deleted from dB

				-- Delete from message
				delete from message 
				from message m
				where m.Recipient = @candidateId
				
				-- Delete from candidatebroadcastmessage
				delete from candidatebroadcastmessage 
				where candidateid = @candidateId
				
				-- Delete from WorkExperience where applications exist but have not been SENT
				delete from workexperience 
				from workexperience we
				where candidateid = @candidateId
																	
				-- Delete from CAFFields where applications exist but have not been SENT
				delete from caffields  
				from caffields caf
				where candidateid = @candidateId
				
				-- Delete from ApplicationHistory where applications exist but have not been SENT
				delete from applicationhistory  
				from applicationhistory ah
				inner join application a
				on a.applicationid = ah.applicationid
				where a.candidateid = @candidateId

				-- Delete from AdditionalAnswer where applications exist but have not been SENT
				delete from additionalanswer  
				from additionalanswer aa
				inner join application a
				on a.applicationid = aa.applicationid
				where a.candidateid = @candidateId
				
				-- Delete from subvacancy where Unsent Applications exist
				delete from subvacancy
				from subvacancy s
				inner join application a
				on s.allocatedapplicationid = a.applicationid
				where a.candidateid = @candidateId
				
				-- Delete from Application where Unsent Applications exist 
				delete from application 
				where candidateid = @candidateId

				-- Delete from SearchFrameworks where applications exist for
				-- the candidate picked up in this iteration of the loop
				delete from SearchFrameworks  
				from SearchFrameworks sf
				inner join SavedSearchCriteria ssc
				on sf.savedsearchcriteriaid = ssc.SavedSearchCriteriaId
				where ssc.candidateid = @candidateId
				
				-- Delete the related candidated records from SavedSearchCriteria for
				delete from SavedSearchCriteria
				from SavedSearchCriteria 
				where candidateid = @candidateId

				-- Delete the related candidated records from AlertPreference for
				delete from AlertPreference
				from AlertPreference 
				where candidateid = @candidateId
				
				-- Delete the related candidated records from WatchedVacancy
				delete from WatchedVacancy
				from WatchedVacancy 
				where candidateid = @candidateId

				-- Delete the external applications
				delete from ExternalApplication
				where candidateid = @candidateId

				-- Delete the Education Result
				delete from EducationResult
				where candidateid = @candidateId

				-- Delete the School Attended
				delete from SchoolAttended
				where candidateid = @candidateId
							
									
			--If Pre-Registered then do Hard Delete
			IF (@isHardDelete = 1)							
			BEGIN
				-- Delete the related candidate history records from CandidateHistory
				DELETE FROM CandidateHistory
				FROM CandidateHistory 
				WHERE candidateid = @candidateId
				
				-- Delete the related candidate preferences records from CandidatePreferences
				DELETE FROM CandidatePreferences
				FROM CandidatePreferences 
				where candidateid = @candidateId
			
				-- Delete the person record, but make sure you get
				-- the correct personid before deleting the candidate record
				-- which must be the order to ensure RI is not screwed.
				DECLARE @personid  INT

				SELECT @personid = personid 
				from candidate c
				where c.candidateid = @candidateid

				DELETE FROM candidate 
				FROM candidate c
				WHERE c.candidateid = @candidateid

				-- Delete the person record
				DELETE FROM person
				WHERE personid = @personid
				
			END
			-- If Not Pre-Registered then do Soft Delete
			ELSE
			BEGIN
				
				-- Set the Lastname to De-registered etc
				update person 
				set title = 0,
				 othertitle = NULL,
				 surname = 'De-registered',
				 firstname = NULL,
				 middlenames = NULL,
				 landlinenumber = NULL,
				 mobilenumber = NULL,
				 email = NULL
				from person p
				inner join candidate c
				on p.personid = c.personid
				inner join CandidateDeRegistrationControl cdr
				on c.candidateid = cdr.candidateid
				where cdr.candidateid = @candidateId
	
		
				-- Truncate the postcode to return the outcode only
				UPDATE candidate
				set 
				AddressLine1 = ' ',
				AddressLine2 = NULL,
				AddressLine3 = NULL,
				AddressLine4 = NULL,
				AddressLine5 = NULL,
				Town = ' ',
				CountyId = 0,
				postcode = 
						case
							when len(postcode) = 6
							then substring(ltrim(rtrim(postcode)),1,2)
								when len(postcode) = 7
								then substring(ltrim(rtrim(postcode)),1,3)
									when len(postcode) = 8
									then substring(ltrim(rtrim(postcode)),1,4)
							else ' ' 
						end,
				AdditionalEmail = ' ',
				UnconfirmedEmailAddress = NULL,
				ReferralPoints = 0,
				Longitude = NULL,
				Latitude = NULL,
				GeocodeEasting = NULL,
				GeocodeNorthing = NULL,
				NewVacancyAlertEmail = NULL,
				NewVacancyAlertSMS = NULL,
				CandidateStatusTypeId = (select CandidateStatusId
											from candidatestatus
											where fullname = 'Deleted')
				
				from candidate c
				inner join CandidateDeRegistrationControl cdr
				on c.candidateid = cdr.candidateid
				where cdr.candidateid = @candidateId		
				
				--print  ' udpate candidate ' + cast(@@rowcount as varchar(200))


			END
				
				
		DELETE FROM CandidateDeRegistrationControl 
		WHERE candidateid = @candidateId
		AND roleId = 1
		
		COMMIT TRANSACTION				
	END TRY
	BEGIN CATCH
		IF (@@TRANCOUNT > 0)
			ROLLBACK TRAN;

		EXEC RethrowError;      
	END CATCH
end