// ReSharper disable InconsistentNaming
namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies
{
    using System;

    public class Vacancy : VacancySummary, ICreatableEntity, IUpdatableEntity, ICloneable
    {
        public string AdditionalLocationInformation { get; set; }
        public string AdditionalLocationInformationComment { get; set; }
        public string ApprenticeshipLevelComment { get; set; }
        public string ClosingDateComment { get; set; }
        public string ContactDetailsComment { get; set; }
        public string ContactEmail { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string CreatedByProviderUsername { get; set; }
        public string DesiredQualifications { get; set; }
        public string DesiredQualificationsComment { get; set; }
        public string DesiredSkills { get; set; }
        public string DesiredSkillsComment { get; set; }
        public string DurationComment { get; set; }
        public bool EditedInRaa { get; set; }
        public string EmployerDescription { get; set; }
        public string EmployerDescriptionComment { get; set; }
        public string EmployerWebsiteUrl { get; set; }
        public string EmployerWebsiteUrlComment { get; set; }
        public string FirstQuestion { get; set; }
        public string FirstQuestionComment { get; set; }
        public string FrameworkCodeNameComment { get; set; }
        public string FutureProspects { get; set; }
        public string FutureProspectsComment { get; set; }
        public int LastEditedById { get; set; }
        public string LocalAuthorityCode { get; set; }
        public string LocationAddressesComment { get; set; }
        public string LongDescription { get; set; }
        public string LongDescriptionComment { get; set; }
        public string NumberOfPositionsComment { get; set; }
        public string OfflineApplicationInstructions { get; set; }
        public string OfflineApplicationInstructionsComment { get; set; }
        public string OfflineApplicationUrl { get; set; }
        public string OfflineApplicationUrlComment { get; set; }
        public OfflineVacancyType? OfflineVacancyType { get; set; }
        public string OtherInformation { get; set; }
        public string PersonalQualities { get; set; }
        public string PersonalQualitiesComment { get; set; }
        public string PossibleStartDateComment { get; set; }
        public string SecondQuestion { get; set; }
        public string SecondQuestionComment { get; set; }
        public string SectorCodeNameComment { get; set; }
        public string ShortDescriptionComment { get; set; }
        public string StandardIdComment { get; set; }
        public string ThingsToConsider { get; set; }
        public string ThingsToConsiderComment { get; set; }

        public string TitleComment { get; set; }
        public string TrainingProvided { get; set; }
        public string TrainingProvidedComment { get; set; }
        public VacancySource VacancySource { get; set; }
        public string WageComment { get; set; }
        public string WorkingWeekComment { get; set; }
        public string AnonymousEmployerDescriptionComment { get; set; }
        public string AnonymousEmployerReasonComment { get; set; }
        public string AnonymousAboutTheEmployerComment { get; set; }

        public object Clone()
        {
            return new Vacancy
            {
                VacancyReferenceNumber = VacancyReferenceNumber,
                VacancyOwnerRelationshipId = VacancyOwnerRelationshipId,
                VacancyGuid = VacancyGuid,
                Title = Title,
                TitleComment = TitleComment,
                ShortDescription = ShortDescription,
                ShortDescriptionComment = ShortDescriptionComment,
                WorkingWeek = WorkingWeek,
                Wage = Wage,
                ExpectedDuration = ExpectedDuration,
                DurationType = DurationType,
                Duration = Duration,
                ClosingDate = ClosingDate,
                PossibleStartDate = PossibleStartDate,
                LongDescription = LongDescription,
                DesiredSkills = DesiredSkills,
                DesiredSkillsComment = DesiredSkillsComment,
                FutureProspects = FutureProspects,
                FutureProspectsComment = FutureProspectsComment,
                PersonalQualities = PersonalQualities,
                PersonalQualitiesComment = PersonalQualitiesComment,
                ThingsToConsider = ThingsToConsider,
                ThingsToConsiderComment = ThingsToConsiderComment,
                DesiredQualifications = DesiredQualifications,
                DesiredQualificationsComment = DesiredQualificationsComment,
                FirstQuestion = FirstQuestion,
                SecondQuestion = SecondQuestion,
                EmployerDescription = EmployerDescription,
                EmployerWebsiteUrl = EmployerWebsiteUrl,
                OfflineVacancy = OfflineVacancy,
                OfflineVacancyType = OfflineVacancyType,
                OfflineApplicationUrl = OfflineApplicationUrl,
                OfflineApplicationUrlComment = OfflineApplicationUrlComment,
                OfflineApplicationInstructions = OfflineApplicationInstructions,
                OfflineApplicationInstructionsComment = OfflineApplicationInstructionsComment,
                NoOfOfflineApplicants = NoOfOfflineApplicants,
                DateSubmitted = DateSubmitted,
                DateFirstSubmitted = DateFirstSubmitted,
                DateStartedToQA = DateStartedToQA,
                QAUserName = QAUserName,
                DateQAApproved = DateQAApproved,
                SubmissionCount = SubmissionCount,
                VacancyManagerId = VacancyManagerId,
                LastEditedById = LastEditedById,
                ParentVacancyId = ParentVacancyId,
                TrainingType = TrainingType,
                ApprenticeshipLevel = ApprenticeshipLevel,
                ApprenticeshipLevelComment = ApprenticeshipLevelComment,
                FrameworkCodeName = FrameworkCodeName,
                FrameworkCodeNameComment = FrameworkCodeNameComment,
                StandardId = StandardId,
                StandardIdComment = StandardIdComment,
                SectorCodeName = SectorCodeName,
                SectorCodeNameComment = SectorCodeNameComment,
                Status = Status,
                WageComment = WageComment,
                ClosingDateComment = ClosingDateComment,
                DurationComment = DurationComment,
                LongDescriptionComment = LongDescriptionComment,
                PossibleStartDateComment = PossibleStartDateComment,
                WorkingWeekComment = WorkingWeekComment,
                FirstQuestionComment = FirstQuestionComment,
                SecondQuestionComment = SecondQuestionComment,
                AdditionalLocationInformation = AdditionalLocationInformation,
                VacancyLocationType = VacancyLocationType,
                NumberOfPositions = NumberOfPositions,
                EmployerDescriptionComment = EmployerDescriptionComment,
                EmployerWebsiteUrlComment = EmployerWebsiteUrlComment,
                LocationAddressesComment = LocationAddressesComment,
                NumberOfPositionsComment = NumberOfPositionsComment,
                AdditionalLocationInformationComment = AdditionalLocationInformationComment,
                TrainingProvided = TrainingProvided,
                TrainingProvidedComment = TrainingProvidedComment,
                ContactName = ContactName,
                ContactNumber = ContactNumber,
                ContactEmail = ContactEmail,
                ContactDetailsComment = ContactDetailsComment,
                VacancyType = VacancyType,
                Address = Address,
                ContractOwnerId = ContractOwnerId,
                OriginalContractOwnerId = OriginalContractOwnerId,
                EditedInRaa = EditedInRaa,
                AnonymousEmployerReasonComment = AnonymousEmployerReasonComment,
                AnonymousEmployerDescriptionComment = AnonymousEmployerDescriptionComment,
                AnonymousAboutTheEmployerComment = AnonymousAboutTheEmployerComment
            };
        }

        public DateTime CreatedDateTime { get; set; }

        protected bool Equals(Vacancy other)
        {
            return string.Equals(AdditionalLocationInformation, other.AdditionalLocationInformation) && string.Equals(AdditionalLocationInformationComment, other.AdditionalLocationInformationComment) && string.Equals(ApprenticeshipLevelComment, other.ApprenticeshipLevelComment) && string.Equals(ClosingDateComment, other.ClosingDateComment) && string.Equals(ContactDetailsComment, other.ContactDetailsComment) && string.Equals(ContactEmail, other.ContactEmail) && string.Equals(ContactName, other.ContactName) && string.Equals(ContactNumber, other.ContactNumber) && string.Equals(CreatedByProviderUsername, other.CreatedByProviderUsername) && string.Equals(DesiredQualifications, other.DesiredQualifications) && string.Equals(DesiredQualificationsComment, other.DesiredQualificationsComment) && string.Equals(DesiredSkills, other.DesiredSkills) && string.Equals(DesiredSkillsComment, other.DesiredSkillsComment) && string.Equals(DurationComment, other.DurationComment) && EditedInRaa == other.EditedInRaa && string.Equals(EmployerDescription, other.EmployerDescription) && string.Equals(EmployerDescriptionComment, other.EmployerDescriptionComment) && string.Equals(EmployerWebsiteUrl, other.EmployerWebsiteUrl) && string.Equals(EmployerWebsiteUrlComment, other.EmployerWebsiteUrlComment) && string.Equals(FirstQuestion, other.FirstQuestion) && string.Equals(FirstQuestionComment, other.FirstQuestionComment) && string.Equals(FrameworkCodeNameComment, other.FrameworkCodeNameComment) && string.Equals(FutureProspects, other.FutureProspects) && string.Equals(FutureProspectsComment, other.FutureProspectsComment) && LastEditedById == other.LastEditedById && string.Equals(LocalAuthorityCode, other.LocalAuthorityCode) && string.Equals(LocationAddressesComment, other.LocationAddressesComment) && string.Equals(LongDescription, other.LongDescription) && string.Equals(LongDescriptionComment, other.LongDescriptionComment) && string.Equals(NumberOfPositionsComment, other.NumberOfPositionsComment) && string.Equals(OfflineApplicationInstructions, other.OfflineApplicationInstructions) && string.Equals(OfflineApplicationInstructionsComment, other.OfflineApplicationInstructionsComment) && string.Equals(OfflineApplicationUrl, other.OfflineApplicationUrl) && string.Equals(OfflineApplicationUrlComment, other.OfflineApplicationUrlComment) && OfflineVacancyType == other.OfflineVacancyType && string.Equals(OtherInformation, other.OtherInformation) && string.Equals(PersonalQualities, other.PersonalQualities) && string.Equals(PersonalQualitiesComment, other.PersonalQualitiesComment) && string.Equals(PossibleStartDateComment, other.PossibleStartDateComment) && string.Equals(SecondQuestion, other.SecondQuestion) && string.Equals(SecondQuestionComment, other.SecondQuestionComment) && string.Equals(SectorCodeNameComment, other.SectorCodeNameComment) && string.Equals(ShortDescriptionComment, other.ShortDescriptionComment) && string.Equals(StandardIdComment, other.StandardIdComment) && string.Equals(ThingsToConsider, other.ThingsToConsider) && string.Equals(ThingsToConsiderComment, other.ThingsToConsiderComment) && string.Equals(TitleComment, other.TitleComment) && string.Equals(TrainingProvided, other.TrainingProvided) && string.Equals(TrainingProvidedComment, other.TrainingProvidedComment) && VacancySource == other.VacancySource && string.Equals(WageComment, other.WageComment) && string.Equals(WorkingWeekComment, other.WorkingWeekComment) && string.Equals(AnonymousEmployerDescriptionComment, other.AnonymousEmployerDescriptionComment) && string.Equals(AnonymousEmployerReasonComment, other.AnonymousEmployerReasonComment) && string.Equals(AnonymousAboutTheEmployerComment, other.AnonymousAboutTheEmployerComment) && CreatedDateTime.Equals(other.CreatedDateTime);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vacancy) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (AdditionalLocationInformation != null ? AdditionalLocationInformation.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (AdditionalLocationInformationComment != null ? AdditionalLocationInformationComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ApprenticeshipLevelComment != null ? ApprenticeshipLevelComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ClosingDateComment != null ? ClosingDateComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ContactDetailsComment != null ? ContactDetailsComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ContactEmail != null ? ContactEmail.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ContactName != null ? ContactName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ContactNumber != null ? ContactNumber.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (CreatedByProviderUsername != null ? CreatedByProviderUsername.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (DesiredQualifications != null ? DesiredQualifications.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (DesiredQualificationsComment != null ? DesiredQualificationsComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (DesiredSkills != null ? DesiredSkills.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (DesiredSkillsComment != null ? DesiredSkillsComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (DurationComment != null ? DurationComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ EditedInRaa.GetHashCode();
                hashCode = (hashCode*397) ^ (EmployerDescription != null ? EmployerDescription.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (EmployerDescriptionComment != null ? EmployerDescriptionComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (EmployerWebsiteUrl != null ? EmployerWebsiteUrl.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (EmployerWebsiteUrlComment != null ? EmployerWebsiteUrlComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (FirstQuestion != null ? FirstQuestion.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (FirstQuestionComment != null ? FirstQuestionComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (FrameworkCodeNameComment != null ? FrameworkCodeNameComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (FutureProspects != null ? FutureProspects.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (FutureProspectsComment != null ? FutureProspectsComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ LastEditedById;
                hashCode = (hashCode*397) ^ (LocalAuthorityCode != null ? LocalAuthorityCode.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (LocationAddressesComment != null ? LocationAddressesComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (LongDescription != null ? LongDescription.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (LongDescriptionComment != null ? LongDescriptionComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (NumberOfPositionsComment != null ? NumberOfPositionsComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (OfflineApplicationInstructions != null ? OfflineApplicationInstructions.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (OfflineApplicationInstructionsComment != null ? OfflineApplicationInstructionsComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (OfflineApplicationUrl != null ? OfflineApplicationUrl.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (OfflineApplicationUrlComment != null ? OfflineApplicationUrlComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ OfflineVacancyType.GetHashCode();
                hashCode = (hashCode*397) ^ (OtherInformation != null ? OtherInformation.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (PersonalQualities != null ? PersonalQualities.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (PersonalQualitiesComment != null ? PersonalQualitiesComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (PossibleStartDateComment != null ? PossibleStartDateComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (SecondQuestion != null ? SecondQuestion.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (SecondQuestionComment != null ? SecondQuestionComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (SectorCodeNameComment != null ? SectorCodeNameComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ShortDescriptionComment != null ? ShortDescriptionComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (StandardIdComment != null ? StandardIdComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ThingsToConsider != null ? ThingsToConsider.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ThingsToConsiderComment != null ? ThingsToConsiderComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (TitleComment != null ? TitleComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (TrainingProvided != null ? TrainingProvided.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (TrainingProvidedComment != null ? TrainingProvidedComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (int) VacancySource;
                hashCode = (hashCode*397) ^ (WageComment != null ? WageComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (WorkingWeekComment != null ? WorkingWeekComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (AnonymousEmployerDescriptionComment != null ? AnonymousEmployerDescriptionComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (AnonymousEmployerReasonComment != null ? AnonymousEmployerReasonComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (AnonymousAboutTheEmployerComment != null ? AnonymousAboutTheEmployerComment.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ CreatedDateTime.GetHashCode();
                return hashCode;
            }
        }
    }
}
