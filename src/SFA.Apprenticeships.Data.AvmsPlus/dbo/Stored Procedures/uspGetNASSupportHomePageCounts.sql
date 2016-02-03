CREATE PROCEDURE [dbo].[uspGetNASSupportHomePageCounts]       
 @LSCRegion int = 0,      
 @DaysNotProgressed int = 60,   
 @nasSupportThresholdForUnSuccessfulCandidate INT = 3,  
 @daysFromClosingDateForVacancyNotFilled INT = 10,                  
 @daysFromClosingDateFor0ApplicationVacancies INT = 10,                  
 @numberOfDaysForFilledVacanciesWithOpenApplications INT = 60,                
 @noApplicationsOnly BIT = 0,  
 @DaysEmployerWithoutTrainingProvider int = -1    
AS      
BEGIN      
      
 SET NOCOUNT ON    
     
 declare @EmployersRequiringLearningProviderAssignment int      
 declare @VacanciesPendingApproval int      
 declare @ApplicationsNotProgressed int      
 declare @CandidatesWithUnsuccessfulApplications int      
 declare @ConfirmedCandidatesWithoutILRDetails int      
 declare @FilledVacanciesWithOpenApplications int      
 declare @VacanciesApproachingClosingDateWith0Applications int      
 declare @VacanciesNotFilledAfter60DaysFromClosingDate int      
      
 /*** replace set statements with relevent exec call when sp completed ***/  
 exec uspGetEmployersWithoutTrainingPartnersCount @LSCRegion, @DaysEmployerWithoutTrainingProvider, @EmployersRequiringLearningProviderAssignment output     
 exec uspGetPendingVacanciesCount @LSCRegion, @VacanciesPendingApproval output      
 exec uspGetNotProgressedApplicationsCount @LSCRegion, @DaysNotProgressed, @ApplicationsNotProgressed output      
     
 --Below code is commented out because NAS alert funtionality for unsuccessfull candidate is removed in sprint 2 (149-03)  
 /***************Get the cnt for Unsuccessful*****************/    
     
-- DECLARE @Total INT     
-- EXEC uspCandidateSelectByReferralPoints @nasSupportThresholdForUnSuccessfulCandidate,@LSCRegion,1,-1,0,'CandidateName',@Total OUT    
--     
-- set @CandidatesWithUnsuccessfulApplications = @Total      
     
 /************************************************************/    
     
     
 set @ConfirmedCandidatesWithoutILRDetails = 0      
   
 --EXEC @FilledVacanciesWithOpenApplications = uspVacancyGetFilledWithOpenApplicationsCount @LSCRegion,@daysFromClosingDateForVacancyNotFilled,@daysFromClosingDateFor0ApplicationVacancies,@numberOfDaysForFilledVacanciesWithOpenApplications,@noApplicationsOnly  
 ---Modified to take care of Managing Area  
 EXEC @FilledVacanciesWithOpenApplications = uspVacancyGetFilledWithOpenApplicationsCount @LSCRegion,@numberOfDaysForFilledVacanciesWithOpenApplications  
   
 --EXEC @VacanciesApproachingClosingDateWith0Applications = uspVacancyGetClosingCount @LSCRegion,@daysFromClosingDateForVacancyNotFilled,@daysFromClosingDateFor0ApplicationVacancies,@numberOfDaysForFilledVacanciesWithOpenApplications,@noApplicationsOnly  
    
 ---Modified to take care of Managing Area  
 EXEC @VacanciesApproachingClosingDateWith0Applications = uspVacancyGetClosingCount @LSCRegion,@daysFromClosingDateFor0ApplicationVacancies     
    
 --EXEC @VacanciesNotFilledAfter60DaysFromClosingDate = uspVacancyGetClosedUnfilledCount @LSCRegion,@daysFromClosingDateForVacancyNotFilled,@daysFromClosingDateFor0ApplicationVacancies,@numberOfDaysForFilledVacanciesWithOpenApplications,@noApplicationsOnly   
 ---Modified to take care of Managing Area  
 EXEC @VacanciesNotFilledAfter60DaysFromClosingDate = uspVacancyGetClosedUnfilledCount @LSCRegion,@daysFromClosingDateForVacancyNotFilled  
   
      
 select @EmployersRequiringLearningProviderAssignment as 'EmployersRequiringLearningProviderAssignment',      
   @VacanciesPendingApproval as 'VacanciesPendingApproval',      
   @ApplicationsNotProgressed as 'ApplicationsNotProgressed',      
   --Below line is commented out because NAS alert funtionality for unsuccessfull candidate is removed in sprint 2 (149-03)  
   --@CandidatesWithUnsuccessfulApplications as 'CandidatesWithUnsuccessfulApplications',          
   @ConfirmedCandidatesWithoutILRDetails as 'ConfirmedCandidatesWithoutILRDetails',      
   @FilledVacanciesWithOpenApplications as 'FilledVacanciesWithOpenApplications',      
   @VacanciesApproachingClosingDateWith0Applications as 'VacanciesApproachingClosingDateWith0Applications',      
   @VacanciesNotFilledAfter60DaysFromClosingDate as 'VacanciesNotFilledAfter60DaysFromClosingDate'      
      
 SET NOCOUNT OFF      
END