namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Entities;
    using Entities.Mongo;
    using Entities.Sql;
    using SFA.Infrastructure.Interfaces;
    using CandidateSummary = Entities.Sql.CandidateSummary;
    using Candidate = Entities.Sql.Candidate;

    public class CandidateMappers : ICandidateMappers
    {
        private const int CandidateStatusTypeIdPreRegistered = 1;
        private const int CandidateStatusTypeIdActivated = 2;
        // ReSharper disable once UnusedMember.Local
        private const int CandidateStatusTypeIdRegistered = 3;
        private const int CandidateStatusTypeIdSuspended = 4;
        // ReSharper disable once UnusedMember.Local
        private const int CandidateStatusTypeIdPendingDelete = 5;
        private const int CandidateStatusTypeIdDeleted = 6;

        private readonly IDictionary<int, int> _ethnicities = new Dictionary<int, int>
        {
            // Prefer not to say
            {99, 20}, 

            // White
            {31, 16}, // English / Welsh / Scottish / Northern Irish / British
            {32, 17}, // Irish
            {33, 20}, // Gypsy or Irish Traveller
            {34, 18}, // Any other White background

            // Mixed / Multiple ethnic groups
            {35, 13}, // White and Black Caribbean
            {36, 12}, // White and Black African
            {37, 11}, // White and Asian
            {38, 14}, // Any other Mixed / Multiple ethnic background

            // Asian / Asian British
            {39, 3}, // Indian
            {40, 4}, // Pakistani
            {41, 2}, // Bangladeshi
            {42, 19}, // Chinese
            {43, 5}, // Any other Asian background

            // Black / African / Caribbean / Black British
            {44, 7}, // African
            {45, 8}, // Caribbean
            {46, 9}, // Any other Black / African / Caribbean background

            // Other ethnic group
            {47, 20}, // Arab
            {98, 20} // Any other ethnic group
        };

        private readonly ILogService _logService;

        public CandidateMappers(ILogService logService)
        {
            _logService = logService;
        }

        public CandidatePerson MapCandidatePerson(CandidateUser candidateUser, IDictionary<Guid, CandidateSummary> candidateSummaries, IDictionary<string, int> vacancyLocalAuthorities, IDictionary<int, int> localAuthorityCountyIds, bool anonymise)
        {
            try
            {
                var candidateGuid = candidateUser.Candidate.Id;
                var candidateSummary = candidateSummaries.ContainsKey(candidateGuid) ? candidateSummaries[candidateGuid] : new CandidateSummary();
                var address = candidateUser.Candidate.RegistrationDetails.Address;
                var email = candidateUser.Candidate.RegistrationDetails.EmailAddress.ToLower();
                var monitoringInformation = candidateUser.Candidate.MonitoringInformation ?? new MonitoringInformation();

                //TODO: Use PCA to validate these lookups when the service is complete and we have the keys
                var localAuthorityId = GetLocalAuthorityId(address.Postcode, vacancyLocalAuthorities);
                var countyId = localAuthorityCountyIds.ContainsKey(localAuthorityId) ? localAuthorityCountyIds[localAuthorityId] : 0;

                var candidate = new Candidate
                {
                    CandidateId = GetCandidateId(candidateUser, candidateSummary),
                    PersonId = candidateSummary.PersonId,
                    CandidateStatusTypeId = GetCandidateStatusTypeId(candidateUser.User.Status),
                    DateofBirth = candidateUser.Candidate.RegistrationDetails.DateOfBirth,
                    AddressLine1 = anonymise ? "" : address.AddressLine1,
                    AddressLine2 = address.AddressLine2 ?? "",
                    AddressLine3 = address.AddressLine3 ?? "",
                    AddressLine4 = address.AddressLine4 ?? "",
                    AddressLine5 = "",
                    Town = "N/A",
                    CountyId = countyId,
                    Postcode = address.Postcode,
                    LocalAuthorityId = localAuthorityId,
                    Longitude = null,
                    Latitude = null,
                    GeocodeEasting = null,
                    GeocodeNorthing = null,
                    NiReference = "",
                    VoucherReferenceNumber = null,
                    UniqueLearnerNumber = null,
                    UlnStatusId = 0,
                    Gender = GetGender(monitoringInformation.Gender),
                    EthnicOrigin = GetEthnicOrigin(monitoringInformation.Ethnicity),
                    EthnicOriginOther = GetEthnicOriginOther(monitoringInformation.Ethnicity),
                    ApplicationLimitEnforced = false,
                    LastAccessedDate = candidateUser.User.LastLogin ?? candidateUser.Candidate.DateUpdated ?? candidateUser.Candidate.DateCreated,
                    AdditionalEmail = anonymise ? "anonymised@data.com" : email.Length > 50 ? "" : email,
                    Disability = GetDisability(monitoringInformation.DisabilityStatus),
                    DisabilityOther = GetDisabilityOther(monitoringInformation.DisabilityStatus),
                    HealthProblems = "",
                    ReceivePushedContent = false,
                    ReferralAgent = false,
                    DisableAlerts = false,
                    UnconfirmedEmailAddress = "",
                    MobileNumberUnconfirmed = false,
                    DoBFailureCount = null,
                    ForgottenUsernameRequested = false,
                    ForgottenPasswordRequested = false,
                    TextFailureCount = 0,
                    EmailFailureCount = 0,
                    LastAccessedManageApplications = null,
                    ReferralPoints = 0,
                    BeingSupportedBy = "NAS Exemplar",
                    LockedForSupportUntil = null,
                    NewVacancyAlertEmail = null,
                    NewVacancyAlertSMS = null,
                    AllowMarketingMessages = false,
                    ReminderMessageSent = true,
                    CandidateGuid = candidateGuid
                };

                var person = new Person
                {
                    PersonId = candidateSummary.PersonId,
                    Title = 0,
                    OtherTitle = "",
                    FirstName = anonymise ? "Candidate" : candidateUser.Candidate.RegistrationDetails.FirstName,
                    MiddleNames = anonymise ? "" : candidateUser.Candidate.RegistrationDetails.MiddleNames,
                    Surname = anonymise ? candidateGuid.ToString().Replace("-", "") : candidateUser.Candidate.RegistrationDetails.LastName,
                    LandlineNumber = anonymise ? "07999999999" : candidateUser.Candidate.RegistrationDetails.PhoneNumber,
                    MobileNumber = "",
                    Email = anonymise ? "anonymised@data.com" : email,
                    PersonTypeId = 1
                };

                return new CandidatePerson
                {
                    Candidate = candidate,
                    Person = person
                };
            }
            catch (Exception ex)
            {
                //Copes with test data from integration and acceptance tests
                _logService.Error($"Failed to map Candidate with Id {candidateUser.Candidate.Id}", ex);
                return null;
            }
        }

        public Dictionary<string, object> MapCandidateDictionary(Candidate candidate)
        {
            return new Dictionary<string, object>
            {
                {"CandidateId", candidate.CandidateId},
                {"PersonId", candidate.PersonId},
                {"CandidateStatusTypeId", candidate.CandidateStatusTypeId},
                {"DateofBirth", candidate.DateofBirth},
                {"AddressLine1", candidate.AddressLine1},
                {"AddressLine2", candidate.AddressLine2},
                {"AddressLine3", candidate.AddressLine3},
                {"AddressLine4", candidate.AddressLine4},
                {"AddressLine5", candidate.AddressLine5},
                {"Town", candidate.Town},
                {"CountyId", candidate.CountyId},
                {"Postcode", candidate.Postcode},
                {"LocalAuthorityId", candidate.LocalAuthorityId},
                {"Longitude", candidate.Longitude},
                {"Latitude", candidate.Latitude},
                {"GeocodeEasting", candidate.GeocodeEasting},
                {"GeocodeNorthing", candidate.GeocodeNorthing},
                {"NiReference", candidate.NiReference},
                {"VoucherReferenceNumber", candidate.VoucherReferenceNumber},
                {"UniqueLearnerNumber", candidate.UniqueLearnerNumber},
                {"UlnStatusId", candidate.UlnStatusId},
                {"Gender", candidate.Gender},
                {"EthnicOrigin", candidate.EthnicOrigin},
                {"EthnicOriginOther", candidate.EthnicOriginOther},
                {"ApplicationLimitEnforced", candidate.ApplicationLimitEnforced},
                {"LastAccessedDate", candidate.LastAccessedDate},
                {"AdditionalEmail", candidate.AdditionalEmail},
                {"Disability", candidate.Disability},
                {"DisabilityOther", candidate.DisabilityOther},
                {"HealthProblems", candidate.HealthProblems},
                {"ReceivePushedContent", candidate.ReceivePushedContent},
                {"ReferralAgent", candidate.ReferralAgent},
                {"DisableAlerts", candidate.DisableAlerts},
                {"UnconfirmedEmailAddress", candidate.UnconfirmedEmailAddress},
                {"MobileNumberUnconfirmed", candidate.MobileNumberUnconfirmed},
                {"DoBFailureCount", candidate.DoBFailureCount},
                {"ForgottenUsernameRequested", candidate.ForgottenUsernameRequested},
                {"ForgottenPasswordRequested", candidate.ForgottenPasswordRequested},
                {"TextFailureCount", candidate.TextFailureCount},
                {"EmailFailureCount", candidate.EmailFailureCount},
                {"LastAccessedManageApplications", candidate.LastAccessedManageApplications},
                {"ReferralPoints", candidate.ReferralPoints},
                {"BeingSupportedBy", candidate.BeingSupportedBy},
                {"LockedForSupportUntil", candidate.LockedForSupportUntil},
                {"NewVacancyAlertEmail", candidate.NewVacancyAlertEmail},
                {"NewVacancyAlertSMS", candidate.NewVacancyAlertSMS},
                {"AllowMarketingMessages", candidate.AllowMarketingMessages},
                {"ReminderMessageSent", candidate.ReminderMessageSent},
                {"CandidateGuid", candidate.CandidateGuid}
            };
        }

        public Dictionary<string, object> MapPersonDictionary(Person person)
        {
            return new Dictionary<string, object>
            {
                {"PersonId", person.PersonId},
                {"Title", person.Title},
                {"OtherTitle", person.OtherTitle},
                {"FirstName", person.FirstName},
                {"MiddleNames", person.MiddleNames},
                {"Surname", person.Surname},
                {"LandlineNumber", person.LandlineNumber},
                {"MobileNumber", person.MobileNumber},
                {"Email", person.Email},
                {"PersonTypeId", person.PersonTypeId}
            };
        }

        private int GetCandidateId(CandidateUser candidateUser, CandidateSummary candidateSummary)
        {
            var candidate = candidateUser.Candidate;
            if (candidateSummary.CandidateId != 0)
            {
                var candidateId = candidateSummary.CandidateId;
                if (candidate.LegacyCandidateId != 0 && candidate.LegacyCandidateId != candidateId)
                {
                    _logService.Warn($"CandidateId: {candidateId} does not match the LegacyCandidateId: {candidate.LegacyCandidateId} for candidate with Id: {candidate.Id}. This shouldn't change post activation");
                }
                return candidateId;
            }

            return candidate.LegacyCandidateId;
        }

        private int GetCandidateStatusTypeId(int status)
        {
            switch (status)
            {
                case 0:
                    return CandidateStatusTypeIdPreRegistered;
                case 10:
                    return CandidateStatusTypeIdPreRegistered;
                case 20:
                    return CandidateStatusTypeIdActivated;
                case 30:
                    return CandidateStatusTypeIdSuspended;
                case 90:
                    return CandidateStatusTypeIdSuspended;
                case 100:
                    return CandidateStatusTypeIdSuspended;
                case 999:
                    return CandidateStatusTypeIdDeleted;
            }

            return CandidateStatusTypeIdPreRegistered;
        }

        private static int GetLocalAuthorityId(string postCode, IDictionary<string, int> vacancyLocalAuthorities)
        {
            var postCodeKey = postCode.Replace(" ", "");

            for (var i = 0; i < 5; i++)
            {
                if (postCodeKey.Length > 2 || char.IsDigit(postCodeKey.Last()))
                {
                    postCodeKey = postCodeKey.Substring(0, postCodeKey.Length - 1);

                    if (vacancyLocalAuthorities.ContainsKey(postCodeKey))
                    {
                        return vacancyLocalAuthorities[postCodeKey];
                    }
                }
            }

            return 0;
        }

        private static int GetGender(int? gender)
        {
            if (gender.HasValue)
            {
                if (gender == 4)
                    return 0;
                return gender.Value;
            }
            return 0;
        }

        private int GetEthnicOrigin(int? ethnicity)
        {
            if (ethnicity.HasValue)
            {
                if (_ethnicities.ContainsKey(ethnicity.Value))
                {
                    return _ethnicities[ethnicity.Value];
                }
            }
            return 0;
        }

        private static string GetEthnicOriginOther(int? ethnicity)
        {
            if (ethnicity.HasValue)
            {
                switch (ethnicity)
                {
                    case 33:
                        return "Gypsy or Irish Traveller";

                    case 47:
                        return "Arab";

                    case 99:
                        return "Not provided";
                }
            }
            return "";
        }

        private static int GetDisability(int? disabilityStatus)
        {
            if (disabilityStatus.HasValue && disabilityStatus != 2)
            {
                return disabilityStatus == 1 ? 13 : 14;
            }

            return 0;
        }

        private static string GetDisabilityOther(int? disabilityStatus)
        {
            if (disabilityStatus.HasValue && disabilityStatus == 1)
            {
                return "Other";
            }
            return "";
        }
    }
}