create procedure [dbo].[usp_CandidateDeRegister]

as

begin 
	
				-- Order in which records are deleted from dB
				
				-- Delete from candidatebroadcastmessage
				delete from candidatebroadcastmessage 
				from candidatebroadcastmessage cbm
				inner join candidate c
				on cbm.candidateid = c.candidateid
				where c.candidatestatustypeid = (select CandidateStatusId 
												from CandidateStatus 
												where fullname = 'Pending Delete')


				-- Delete from message
				delete from message 
				from message m
				inner join candidatebroadcastmessage cbm
				on m.messageid = cbm.messageid
				inner join candidate c
				on cbm.candidateid = c.candidateid
				where c.candidatestatustypeid = (select CandidateStatusId 
												from CandidateStatus 
												where fullname = 'Pending Delete')


				-- Delete from WorkExperience where applications exist but have not been SENT
				delete from workexperience 
				from workexperience we
				inner join application a
				on we.applicationid = a.applicationid
				inner join candidate c
				on a.candidateid = c.candidateid
				where a.applicationstatustypeid = (select ApplicationStatusTypeId
													from ApplicationStatusType
													where fullname = 'Unsent')
				and c.candidatestatustypeid = (select CandidateStatusId 
												from CandidateStatus 
												where fullname = 'Pending Delete')

				-- Delete from CAFFields where applications exist but have not been SENT
				delete from caffields  
				from caffields caf
				inner join application a
				on caf.applicationid = a.applicationid
				inner join candidate c
				on a.candidateid = c.candidateid
				where a.applicationstatustypeid = (select ApplicationStatusTypeId
													from ApplicationStatusType
													where fullname = 'Unsent')
				and c.candidatestatustypeid = (select CandidateStatusId 
												from CandidateStatus 
												where fullname = 'Pending Delete')

				-- Delete from ApplicationHistory where applications exist but have not been SENT
				delete from applicationhistory  
				from applicationhistory ah
				inner join application a
				on a.applicationid = ah.applicationid
				inner join candidate c
				on a.candidateid = c.candidateid
				where a.applicationstatustypeid = (select ApplicationStatusTypeId
													from ApplicationStatusType
													where fullname = 'Unsent')
				and c.candidatestatustypeid = (select CandidateStatusId 
												from CandidateStatus 
												where fullname = 'Pending Delete')

				-- Delete from AdditionalAnswer where applications exist but have not been SENT
				delete from additionalanswer  
				from additionalanswer aa
				inner join application a
				on a.applicationid = aa.applicationid
				inner join candidate c
				on a.candidateid = c.candidateid
				where a.applicationstatustypeid = (select ApplicationStatusTypeId
													from ApplicationStatusType
													where fullname = 'Unsent')
				and c.candidatestatustypeid = (select CandidateStatusId 
												from CandidateStatus 
												where fullname = 'Pending Delete')


				-- Nullify the following fields on the application record
				-- where the applicationstatustype is anything other than UNSENT
-- TEST THIS ON THE 2nd RUN
				update application 
				set WithdrawnOrDeclinedReasonId = 0,
					UnsuccessfulReasonId = 0,
					OutcomeReasonOther = NULL,
					NextActionId = 0,
					NextActionOther = NULL,
					AllocatedTo = NULL,
					--CVAttachmentId = 
					BeingSupportedBy = NULL,
					LockedForSupportUntil = NULL,
					WithdrawalAcknowledged = NULL
				from application a
				inner join candidate c
				on a.candidateid = c.candidateid
				where a.applicationstatustypeid <> (select ApplicationStatusTypeId
												from ApplicationStatusType
												where fullname = 'Unsent')
				and c.candidatestatustypeid = (select CandidateStatusId 
												from CandidateStatus 
												where fullname = 'Pending Delete')


				-- Delete from Application where Unsent Applications exist 
				delete from application 
				from application a
				inner join candidate c
				on a.candidateid = c.candidateid
				where a.applicationstatustypeid = (select ApplicationStatusTypeId
												from ApplicationStatusType
												where fullname = 'Unsent')
				and c.candidatestatustypeid = (select CandidateStatusId 
												from CandidateStatus 
												where fullname = 'Pending Delete')		
		
				-- Delete from SearchFrameworks where applications exist for
				-- the candidate picked up in this iteration of the loop
				delete from SearchFrameworks  
				from SearchFrameworks sf
				inner join SavedSearchCriteria ssc
				on sf.savedsearchcriteriaid = ssc.SavedSearchCriteriaId
				inner join candidate c
				on ssc.candidateid = c.candidateid
				and c.candidatestatustypeid = (select CandidateStatusId 
												from CandidateStatus 
												where fullname = 'Pending Delete')		

				-- Delete the related candidated records from SavedSearchCriteria for
				delete from SavedSearchCriteria
				from SavedSearchCriteria ssc
				inner join candidate c
				on ssc.candidateid = c.candidateid
				where c.candidatestatustypeid = (select CandidateStatusId 
												from CandidateStatus 
												where fullname = 'Pending Delete')					

				-- Delete the related candidated records from AlertPreference for
				delete from AlertPreference
				from AlertPreference ap
				inner join candidate c
				on ap.candidateid = c.candidateid
				where c.candidatestatustypeid = (select CandidateStatusId 
												from CandidateStatus 
												where fullname = 'Pending Delete')		

				-- Delete the related candidated records from WatchedVacancy
				delete from WatchedVacancy
				from WatchedVacancy wv
				inner join candidate c
				on wv.candidateid = c.candidateid
				where c.candidatestatustypeid = (select CandidateStatusId 
												from CandidateStatus 
												where fullname = 'Pending Delete')		

				-- Delete the related candidate history records from CandidateHistory
				delete from CandidateHistory
				from CandidateHistory ch
				inner join candidate c
				on ch.candidateid = c.candidateid
				where c.candidatestatustypeid = (select CandidateStatusId 
												from CandidateStatus 
												where fullname = 'Pending Delete')		
	
				-- Delete the related candidate preferences records from CandidatePreferences
				delete from CandidatePreferences
				from CandidatePreferences cp
				inner join candidate c
				on cp.candidateid = c.candidateid
				where c.candidatestatustypeid = (select CandidateStatusId 
												from CandidateStatus 
												where fullname = 'Pending Delete')		

	-- Data to be amended only

			-- Set the Lastname to De-registered
				update person 
				set title = 0,
				 othertitle = NULL,
				 surname = 'De-registered',
				 firstname = NULL,
				 middlenames = NULL,
				 landlinenumber = NULL,
				 mobilenumber = NULL,
				 email = NULL
				from person p, candidate c
				where p.personid = c.personid
				and c.candidateid = c.candidateid
				and c.candidatestatustypeid = (select CandidateStatusId 
												from CandidateStatus 
												where fullname = 'Pending Delete')		


				-- Truncate the postcode to return the outcode only
				update candidate
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
						end,
				AdditionalEmail = ' ',
				UnconfirmedEmailAddress = NULL,
				ReferralPoints = 0,
				Longitude = NULL,
				Latitude = NULL,
				GeocodeEasting = NULL,
				GeocodeNorthing = NULL
				where candidatestatustypeid = (select CandidateStatusId 
												from CandidateStatus 
												where fullname = 'Pending Delete')		
				


				-- Set the Candidate Status to Deleted
				update candidate
				set CandidateStatusTypeId = (select CandidateStatusId
											from candidatestatus
											where fullname = 'Deleted')
				where candidatestatustypeid = (select CandidateStatusId 
												from CandidateStatus 
												where fullname = 'Pending Delete')		
				
end