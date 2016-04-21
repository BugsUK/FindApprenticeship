namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities.Sql
{
    using System;

    public class Candidate
    {
        public int CandidateId { get; set; }
        public int PersonId { get; set; }
        public int CandidateStatusTypeId { get; set; }
        public DateTime DateofBirth { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public string Town { get; set; }
        public int CountyId { get; set; }
        public string Postcode { get; set; }
        public int LocalAuthorityId { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public int? GeocodeEasting { get; set; }
        public int? GeocodeNorthing { get; set; }
        public string NiReference { get; set; }
        public int? VoucherReferenceNumber { get; set; }
        public long? UniqueLearnerNumber { get; set; }
        public int UlnStatusId { get; set; }
        public int Gender { get; set; }
        public int EthnicOrigin { get; set; }
        public string EthnicOriginOther { get; set; }
        public bool ApplicationLimitEnforced { get; set; }
        public DateTime? LastAccessedDate { get; set; }
        public string AdditionalEmail { get; set; }
        public int Disability { get; set; }
        public string DisabilityOther { get; set; }
        public string HealthProblems { get; set; }
        public bool ReceivePushedContent { get; set; }
        public bool ReferralAgent { get; set; }
        public bool DisableAlerts { get; set; }
        public string UnconfirmedEmailAddress { get; set; }
        public bool MobileNumberUnconfirmed { get; set; }
        public int? DoBFailureCount { get; set; }
        public bool ForgottenUsernameRequested { get; set; }
        public bool ForgottenPasswordRequested { get; set; }
        public short TextFailureCount { get; set; }
        public short EmailFailureCount { get; set; }
        public DateTime? LastAccessedManageApplications { get; set; }
        public short ReferralPoints { get; set; }
        public string BeingSupportedBy { get; set; }
        public DateTime? LockedForSupportUntil { get; set; }
        public bool? NewVacancyAlertEmail { get; set; }
        public bool? NewVacancyAlertSMS { get; set; }
        public bool? AllowMarketingMessages { get; set; }
        public bool ReminderMessageSent { get; set; }
        public Guid CandidateGuid { get; set; }
    }
}