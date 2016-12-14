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
            return string.Equals(AdditionalLocationInformation, y.AdditionalLocationInformation) && string.Equals(AdditionalLocationInformationComment, y.AdditionalLocationInformationComment) && string.Equals(ApprenticeshipLevelComment, y.ApprenticeshipLevelComment) && string.Equals(ClosingDateComment, y.ClosingDateComment) && string.Equals(ContactDetailsComment, y.ContactDetailsComment) && string.Equals(ContactEmail, y.ContactEmail) && string.Equals(ContactName, y.ContactName) && string.Equals(ContactNumber, y.ContactNumber) && string.Equals(CreatedByProviderUsername, y.CreatedByProviderUsername) && string.Equals(DesiredQualifications, y.DesiredQualifications) && string.Equals(DesiredQualificationsComment, y.DesiredQualificationsComment) && string.Equals(DesiredSkills, y.DesiredSkills) && string.Equals(DesiredSkillsComment, y.DesiredSkillsComment) && string.Equals(DurationComment, y.DurationComment) && EditedInRaa == y.EditedInRaa && string.Equals(EmployerDescription, y.EmployerDescription) && string.Equals(EmployerDescriptionComment, y.EmployerDescriptionComment) && string.Equals(EmployerWebsiteUrl, y.EmployerWebsiteUrl) && string.Equals(EmployerWebsiteUrlComment, y.EmployerWebsiteUrlComment) && string.Equals(FirstQuestion, y.FirstQuestion) && string.Equals(FirstQuestionComment, y.FirstQuestionComment) && string.Equals(FrameworkCodeNameComment, y.FrameworkCodeNameComment) && string.Equals(FutureProspects, y.FutureProspects) && string.Equals(FutureProspectsComment, y.FutureProspectsComment) && LastEditedById == y.LastEditedById && string.Equals(LocalAuthorityCode, y.LocalAuthorityCode) && string.Equals(LocationAddressesComment, y.LocationAddressesComment) && string.Equals(LongDescription, y.LongDescription) && string.Equals(LongDescriptionComment, y.LongDescriptionComment) && string.Equals(NumberOfPositionsComment, y.NumberOfPositionsComment) && string.Equals(OfflineApplicationInstructions, y.OfflineApplicationInstructions) && string.Equals(OfflineApplicationInstructionsComment, y.OfflineApplicationInstructionsComment) && string.Equals(OfflineApplicationUrl, y.OfflineApplicationUrl) && string.Equals(OfflineApplicationUrlComment, y.OfflineApplicationUrlComment) && OfflineVacancyType == y.OfflineVacancyType && string.Equals(OtherInformation, y.OtherInformation) && string.Equals(PersonalQualities, y.PersonalQualities) && string.Equals(PersonalQualitiesComment, y.PersonalQualitiesComment) && string.Equals(PossibleStartDateComment, y.PossibleStartDateComment) && string.Equals(SecondQuestion, y.SecondQuestion) && string.Equals(SecondQuestionComment, y.SecondQuestionComment) && string.Equals(SectorCodeNameComment, y.SectorCodeNameComment) && string.Equals(ShortDescriptionComment, y.ShortDescriptionComment) && string.Equals(StandardIdComment, y.StandardIdComment) && string.Equals(ThingsToConsider, y.ThingsToConsider) && string.Equals(ThingsToConsiderComment, y.ThingsToConsiderComment) && string.Equals(TitleComment, y.TitleComment) && string.Equals(TrainingProvided, y.TrainingProvided) && string.Equals(TrainingProvidedComment, y.TrainingProvidedComment) && VacancySource == y.VacancySource && string.Equals(WageComment, y.WageComment) && string.Equals(WorkingWeekComment, y.WorkingWeekComment) && string.Equals(AnonymousEmployerDescriptionComment, y.AnonymousEmployerDescriptionComment) && string.Equals(AnonymousEmployerReasonComment, y.AnonymousEmployerReasonComment) && string.Equals(AnonymousAboutTheEmployerComment, y.AnonymousAboutTheEmployerComment) && CreatedDateTime.Equals(y.CreatedDateTime);
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
            hashCode = (hashCode * 397) ^ (Address != null ? Address.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (AnonymousAboutTheEmployer != null ? AnonymousAboutTheEmployer.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ ApplicantCount;
            hashCode = (hashCode * 397) ^ (int)ApprenticeshipLevel;
            hashCode = (hashCode * 397) ^ ClosingDate.GetHashCode();
            hashCode = (hashCode * 397) ^ ContractOwnerId;
            hashCode = (hashCode * 397) ^ CreatedDate.GetHashCode();
            hashCode = (hashCode * 397) ^ DateFirstSubmitted.GetHashCode();
            hashCode = (hashCode * 397) ^ DateQAApproved.GetHashCode();
            hashCode = (hashCode * 397) ^ DateStartedToQA.GetHashCode();
            hashCode = (hashCode * 397) ^ DateSubmitted.GetHashCode();
            hashCode = (hashCode * 397) ^ DeliveryOrganisationId.GetHashCode();
            hashCode = (hashCode * 397) ^ Duration.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)DurationType;
            hashCode = (hashCode * 397) ^ (EmployerAnonymousName != null ? EmployerAnonymousName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (EmployerAnonymousReason != null ? EmployerAnonymousReason.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ EmployerId;
            hashCode = (hashCode * 397) ^ (EmployerLocation != null ? EmployerLocation.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (EmployerName != null ? EmployerName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (ExpectedDuration != null ? ExpectedDuration.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (FrameworkCodeName != null ? FrameworkCodeName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ IsAnonymousEmployer.GetHashCode();
            hashCode = (hashCode * 397) ^ NewApplicationCount;
            hashCode = (hashCode * 397) ^ NoOfOfflineApplicants;
            hashCode = (hashCode * 397) ^ NumberOfPositions.GetHashCode();
            hashCode = (hashCode * 397) ^ OfflineVacancy.GetHashCode();
            hashCode = (hashCode * 397) ^ OriginalContractOwnerId;
            hashCode = (hashCode * 397) ^ ParentVacancyId.GetHashCode();
            hashCode = (hashCode * 397) ^ PossibleStartDate.GetHashCode();
            hashCode = (hashCode * 397) ^ (ProviderTradingName != null ? ProviderTradingName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (QAUserName != null ? QAUserName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (int)RegionalTeam;
            hashCode = (hashCode * 397) ^ (SectorCodeName != null ? SectorCodeName.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (ShortDescription != null ? ShortDescription.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ StandardId.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)Status;
            hashCode = (hashCode * 397) ^ SubmissionCount;
            hashCode = (hashCode * 397) ^ (Title != null ? Title.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (int)TrainingType;
            hashCode = (hashCode * 397) ^ UpdatedDateTime.GetHashCode();
            hashCode = (hashCode * 397) ^ VacancyGuid.GetHashCode();
            hashCode = (hashCode * 397) ^ VacancyId;
            hashCode = (hashCode * 397) ^ (int)VacancyLocationType;
            hashCode = (hashCode * 397) ^ VacancyManagerId.GetHashCode();
            hashCode = (hashCode * 397) ^ VacancyOwnerRelationshipId;
            hashCode = (hashCode * 397) ^ VacancyReferenceNumber;
            hashCode = (hashCode * 397) ^ (int)VacancyType;
            hashCode = (hashCode * 397) ^ (Wage != null ? Wage.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (WorkingWeek != null ? WorkingWeek.GetHashCode() : 0);

            return hashCode;
        }
    }
}