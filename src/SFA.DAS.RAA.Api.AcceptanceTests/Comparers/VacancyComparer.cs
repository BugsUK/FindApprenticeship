namespace SFA.DAS.RAA.Api.AcceptanceTests.Comparers
{
    using System.Collections.Generic;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;

    public class VacancyComparer : IEqualityComparer<Vacancy>
    {
        public bool Equals(Vacancy x, Vacancy y)
        {
            if (ReferenceEquals(null, x)) return false;
            if (ReferenceEquals(null, y)) return false;
            if (ReferenceEquals(x, y)) return true;
            return string.Equals(x.AdditionalLocationInformation, y.AdditionalLocationInformation) 
                && string.Equals(x.AdditionalLocationInformationComment, y.AdditionalLocationInformationComment) 
                && string.Equals(x.ApprenticeshipLevelComment, y.ApprenticeshipLevelComment) 
                && string.Equals(x.ClosingDateComment, y.ClosingDateComment) 
                && string.Equals(x.ContactDetailsComment, y.ContactDetailsComment) 
                && string.Equals(x.ContactEmail, y.ContactEmail) 
                && string.Equals(x.ContactName, y.ContactName) 
                && string.Equals(x.ContactNumber, y.ContactNumber) 
                && string.Equals(x.CreatedByProviderUsername, y.CreatedByProviderUsername) 
                && string.Equals(x.DesiredQualifications, y.DesiredQualifications) 
                && string.Equals(x.DesiredQualificationsComment, y.DesiredQualificationsComment) 
                && string.Equals(x.DesiredSkills, y.DesiredSkills) 
                && string.Equals(x.DesiredSkillsComment, y.DesiredSkillsComment) 
                && string.Equals(x.DurationComment, y.DurationComment) 
                && x.EditedInRaa == y.EditedInRaa 
                && string.Equals(x.EmployerDescription, y.EmployerDescription) 
                && string.Equals(x.EmployerDescriptionComment, y.EmployerDescriptionComment) 
                && string.Equals(x.EmployerWebsiteUrl, y.EmployerWebsiteUrl) 
                && string.Equals(x.EmployerWebsiteUrlComment, y.EmployerWebsiteUrlComment) 
                && string.Equals(x.FirstQuestion, y.FirstQuestion) 
                && string.Equals(x.FirstQuestionComment, y.FirstQuestionComment) 
                && string.Equals(x.FrameworkCodeNameComment, y.FrameworkCodeNameComment) 
                && string.Equals(x.FutureProspects, y.FutureProspects) 
                && string.Equals(x.FutureProspectsComment, y.FutureProspectsComment) 
                && x.LastEditedById == y.LastEditedById 
                && string.Equals(x.LocalAuthorityCode, y.LocalAuthorityCode) 
                && string.Equals(x.LocationAddressesComment, y.LocationAddressesComment) 
                && string.Equals(x.LongDescription, y.LongDescription) 
                && string.Equals(x.LongDescriptionComment, y.LongDescriptionComment) 
                && string.Equals(x.NumberOfPositionsComment, y.NumberOfPositionsComment) 
                && string.Equals(x.OfflineApplicationInstructions, y.OfflineApplicationInstructions) 
                && string.Equals(x.OfflineApplicationInstructionsComment, y.OfflineApplicationInstructionsComment) 
                && string.Equals(x.OfflineApplicationUrl, y.OfflineApplicationUrl) 
                && string.Equals(x.OfflineApplicationUrlComment, y.OfflineApplicationUrlComment) 
                && x.OfflineVacancyType == y.OfflineVacancyType 
                && string.Equals(x.OtherInformation, y.OtherInformation) 
                && string.Equals(x.PersonalQualities, y.PersonalQualities) 
                && string.Equals(x.PersonalQualitiesComment, y.PersonalQualitiesComment) 
                && string.Equals(x.PossibleStartDateComment, y.PossibleStartDateComment) 
                && string.Equals(x.SecondQuestion, y.SecondQuestion) 
                && string.Equals(x.SecondQuestionComment, y.SecondQuestionComment) 
                && string.Equals(x.SectorCodeNameComment, y.SectorCodeNameComment) 
                && string.Equals(x.ShortDescriptionComment, y.ShortDescriptionComment) 
                && string.Equals(x.StandardIdComment, y.StandardIdComment) 
                && string.Equals(x.ThingsToConsider, y.ThingsToConsider) 
                && string.Equals(x.ThingsToConsiderComment, y.ThingsToConsiderComment) 
                && string.Equals(x.TitleComment, y.TitleComment) 
                && string.Equals(x.TrainingProvided, y.TrainingProvided) 
                && string.Equals(x.TrainingProvidedComment, y.TrainingProvidedComment) 
                && x.VacancySource == y.VacancySource 
                && string.Equals(x.WageComment, y.WageComment) 
                && string.Equals(x.WorkingWeekComment, y.WorkingWeekComment) 
                && string.Equals(x.AnonymousEmployerDescriptionComment, y.AnonymousEmployerDescriptionComment) 
                && string.Equals(x.AnonymousEmployerReasonComment, y.AnonymousEmployerReasonComment) 
                && string.Equals(x.AnonymousAboutTheEmployerComment, y.AnonymousAboutTheEmployerComment) 
                && x.CreatedDateTime.Equals(y.CreatedDateTime)
                && Equals(x.Address, y.Address) 
                && string.Equals(x.AnonymousAboutTheEmployer, y.AnonymousAboutTheEmployer) 
                && x.ApplicantCount == y.ApplicantCount 
                && x.ApprenticeshipLevel == y.ApprenticeshipLevel 
                && x.ClosingDate.Equals(y.ClosingDate) 
                && x.ContractOwnerId == y.ContractOwnerId 
                && x.CreatedDate.Equals(y.CreatedDate) 
                && x.DateFirstSubmitted.Equals(y.DateFirstSubmitted) 
                && x.DateQAApproved.Equals(y.DateQAApproved) 
                && x.DateStartedToQA.Equals(y.DateStartedToQA) 
                && x.DateSubmitted.Equals(y.DateSubmitted) 
                && x.DeliveryOrganisationId == y.DeliveryOrganisationId 
                && x.Duration == y.Duration 
                && x.DurationType == y.DurationType 
                && string.Equals(x.EmployerAnonymousName, y.EmployerAnonymousName) 
                && string.Equals(x.EmployerAnonymousReason, y.EmployerAnonymousReason) 
                && x.EmployerId == y.EmployerId 
                && string.Equals(x.EmployerLocation, y.EmployerLocation) 
                && string.Equals(x.EmployerName, y.EmployerName) 
                && string.Equals(x.ExpectedDuration, y.ExpectedDuration) 
                && string.Equals(x.FrameworkCodeName, y.FrameworkCodeName) 
                && x.IsAnonymousEmployer == y.IsAnonymousEmployer 
                && x.NewApplicationCount == y.NewApplicationCount 
                && x.NoOfOfflineApplicants == y.NoOfOfflineApplicants 
                && x.NumberOfPositions == y.NumberOfPositions 
                && x.OfflineVacancy == y.OfflineVacancy 
                && x.OriginalContractOwnerId == y.OriginalContractOwnerId 
                && x.ParentVacancyId == y.ParentVacancyId 
                && x.PossibleStartDate.Equals(y.PossibleStartDate) 
                && string.Equals(x.ProviderTradingName, y.ProviderTradingName) 
                && string.Equals(x.QAUserName, y.QAUserName) 
                && x.RegionalTeam == y.RegionalTeam 
                && string.Equals(x.SectorCodeName, y.SectorCodeName) 
                && string.Equals(x.ShortDescription, y.ShortDescription) 
                && x.StandardId == y.StandardId 
                && x.Status == y.Status 
                && x.SubmissionCount == y.SubmissionCount 
                && string.Equals(x.Title, y.Title) 
                && x.TrainingType == y.TrainingType 
                && x.UpdatedDateTime.Equals(y.UpdatedDateTime) 
                && x.VacancyGuid.Equals(y.VacancyGuid) 
                && x.VacancyId == y.VacancyId 
                && x.VacancyLocationType == y.VacancyLocationType 
                && x.VacancyManagerId == y.VacancyManagerId 
                && x.VacancyOwnerRelationshipId == y.VacancyOwnerRelationshipId 
                && x.VacancyReferenceNumber == y.VacancyReferenceNumber 
                && x.VacancyType == y.VacancyType 
                && Equals(x.Wage, y.Wage) 
                && string.Equals(x.WorkingWeek, y.WorkingWeek);
        }

        public int GetHashCode(Vacancy obj)
        {
            var hashCode = (obj.AdditionalLocationInformation != null ? obj.AdditionalLocationInformation.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.AdditionalLocationInformationComment != null ? obj.AdditionalLocationInformationComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.ApprenticeshipLevelComment != null ? obj.ApprenticeshipLevelComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.ClosingDateComment != null ? obj.ClosingDateComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.ContactDetailsComment != null ? obj.ContactDetailsComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.ContactEmail != null ? obj.ContactEmail.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.ContactName != null ? obj.ContactName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.ContactNumber != null ? obj.ContactNumber.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.CreatedByProviderUsername != null ? obj.CreatedByProviderUsername.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.DesiredQualifications != null ? obj.DesiredQualifications.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.DesiredQualificationsComment != null ? obj.DesiredQualificationsComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.DesiredSkills != null ? obj.DesiredSkills.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.DesiredSkillsComment != null ? obj.DesiredSkillsComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.DurationComment != null ? obj.DurationComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ obj.EditedInRaa.GetHashCode();
            hashCode = (hashCode * 397) ^ (obj.EmployerDescription != null ? obj.EmployerDescription.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.EmployerDescriptionComment != null ? obj.EmployerDescriptionComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.EmployerWebsiteUrl != null ? obj.EmployerWebsiteUrl.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.EmployerWebsiteUrlComment != null ? obj.EmployerWebsiteUrlComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.FirstQuestion != null ? obj.FirstQuestion.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.FirstQuestionComment != null ? obj.FirstQuestionComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.FrameworkCodeNameComment != null ? obj.FrameworkCodeNameComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.FutureProspects != null ? obj.FutureProspects.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.FutureProspectsComment != null ? obj.FutureProspectsComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ obj.LastEditedById;
            hashCode = (hashCode * 397) ^ (obj.LocalAuthorityCode != null ? obj.LocalAuthorityCode.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.LocationAddressesComment != null ? obj.LocationAddressesComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.LongDescription != null ? obj.LongDescription.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.LongDescriptionComment != null ? obj.LongDescriptionComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.NumberOfPositionsComment != null ? obj.NumberOfPositionsComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.OfflineApplicationInstructions != null ? obj.OfflineApplicationInstructions.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.OfflineApplicationInstructionsComment != null ? obj.OfflineApplicationInstructionsComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.OfflineApplicationUrl != null ? obj.OfflineApplicationUrl.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.OfflineApplicationUrlComment != null ? obj.OfflineApplicationUrlComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ obj.OfflineVacancyType.GetHashCode();
            hashCode = (hashCode * 397) ^ (obj.OtherInformation != null ? obj.OtherInformation.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.PersonalQualities != null ? obj.PersonalQualities.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.PersonalQualitiesComment != null ? obj.PersonalQualitiesComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.PossibleStartDateComment != null ? obj.PossibleStartDateComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.SecondQuestion != null ? obj.SecondQuestion.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.SecondQuestionComment != null ? obj.SecondQuestionComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.SectorCodeNameComment != null ? obj.SectorCodeNameComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.ShortDescriptionComment != null ? obj.ShortDescriptionComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.StandardIdComment != null ? obj.StandardIdComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.ThingsToConsider != null ? obj.ThingsToConsider.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.ThingsToConsiderComment != null ? obj.ThingsToConsiderComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.TitleComment != null ? obj.TitleComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.TrainingProvided != null ? obj.TrainingProvided.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.TrainingProvidedComment != null ? obj.TrainingProvidedComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (int)obj.VacancySource;
            hashCode = (hashCode * 397) ^ (obj.WageComment != null ? obj.WageComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.WorkingWeekComment != null ? obj.WorkingWeekComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.AnonymousEmployerDescriptionComment != null ? obj.AnonymousEmployerDescriptionComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.AnonymousEmployerReasonComment != null ? obj.AnonymousEmployerReasonComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.AnonymousAboutTheEmployerComment != null ? obj.AnonymousAboutTheEmployerComment.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ obj.CreatedDateTime.GetHashCode();
            hashCode = (hashCode * 397) ^ (obj.Address != null ? obj.Address.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.AnonymousAboutTheEmployer != null ? obj.AnonymousAboutTheEmployer.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ obj.ApplicantCount;
            hashCode = (hashCode * 397) ^ (int)obj.ApprenticeshipLevel;
            hashCode = (hashCode * 397) ^ obj.ClosingDate.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.ContractOwnerId;
            hashCode = (hashCode * 397) ^ obj.CreatedDate.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.DateFirstSubmitted.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.DateQAApproved.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.DateStartedToQA.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.DateSubmitted.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.DeliveryOrganisationId.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.Duration.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)obj.DurationType;
            hashCode = (hashCode * 397) ^ (obj.EmployerAnonymousName != null ? obj.EmployerAnonymousName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.EmployerAnonymousReason != null ? obj.EmployerAnonymousReason.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ obj.EmployerId;
            hashCode = (hashCode * 397) ^ (obj.EmployerLocation != null ? obj.EmployerLocation.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.EmployerName != null ? obj.EmployerName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.ExpectedDuration != null ? obj.ExpectedDuration.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.FrameworkCodeName != null ? obj.FrameworkCodeName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ obj.IsAnonymousEmployer.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.NewApplicationCount;
            hashCode = (hashCode * 397) ^ obj.NoOfOfflineApplicants;
            hashCode = (hashCode * 397) ^ obj.NumberOfPositions.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.OfflineVacancy.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.OriginalContractOwnerId;
            hashCode = (hashCode * 397) ^ obj.ParentVacancyId.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.PossibleStartDate.GetHashCode();
            hashCode = (hashCode * 397) ^ (obj.ProviderTradingName != null ? obj.ProviderTradingName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.QAUserName != null ? obj.QAUserName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (int)obj.RegionalTeam;
            hashCode = (hashCode * 397) ^ (obj.SectorCodeName != null ? obj.SectorCodeName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.ShortDescription != null ? obj.ShortDescription.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ obj.StandardId.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)obj.Status;
            hashCode = (hashCode * 397) ^ obj.SubmissionCount;
            hashCode = (hashCode * 397) ^ (obj.Title != null ? obj.Title.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (int)obj.TrainingType;
            hashCode = (hashCode * 397) ^ obj.UpdatedDateTime.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.VacancyGuid.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.VacancyId;
            hashCode = (hashCode * 397) ^ (int)obj.VacancyLocationType;
            hashCode = (hashCode * 397) ^ obj.VacancyManagerId.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.VacancyOwnerRelationshipId;
            hashCode = (hashCode * 397) ^ obj.VacancyReferenceNumber;
            hashCode = (hashCode * 397) ^ (int)obj.VacancyType;
            hashCode = (hashCode * 397) ^ (obj.Wage != null ? obj.Wage.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (obj.WorkingWeek != null ? obj.WorkingWeek.GetHashCode() : 0);

            return hashCode;
        }
    }
}