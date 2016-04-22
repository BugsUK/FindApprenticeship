namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System;
    using System.Collections.Generic;
    using Entities;
    using Entities.Mongo;
    using Entities.Sql;
    using SFA.Infrastructure.Interfaces;
    using Candidate = Entities.Sql.Candidate;

    public class CandidateMappers : ICandidateMappers
    {
        private const int CandidateStatusTypeIdPreRegistered = 1;
        private const int CandidateStatusTypeIdActivated = 2;
        private const int CandidateStatusTypeIdRegistered = 3;
        private const int CandidateStatusTypeIdSuspended = 4;
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

        public CandidatePerson MapCandidatePerson(CandidateUser candidateUser, IDictionary<Guid, int> candidateIds, IDictionary<string, int> personIds)
        {
            var address = candidateUser.Candidate.RegistrationDetails.Address;
            var email = candidateUser.Candidate.RegistrationDetails.EmailAddress.ToLower();
            var personId = personIds.ContainsKey(email) ? personIds[email] : 0;//Unspecified person. Filled in later

            var candidate = new Candidate
            {
                CandidateId = GetCandidateId(candidateUser.Candidate, candidateIds),
                PersonId = personId,
                CandidateStatusTypeId = CandidateStatusTypeIdPreRegistered,
                DateofBirth = candidateUser.Candidate.RegistrationDetails.DateOfBirth,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2 ?? "",
                AddressLine3 = address.AddressLine3 ?? "",
                AddressLine4 = address.AddressLine4 ?? "",
                AddressLine5 = "",
                Town = "N/A",
                CountyId = 0,
                Postcode = address.Postcode,
                LocalAuthorityId = 0,
                Longitude = (decimal)address.GeoPoint.Longitude,
                Latitude = (decimal)address.GeoPoint.Latitude,
                GeocodeEasting = null,
                GeocodeNorthing = null,
                NiReference = "",
                VoucherReferenceNumber = null,
                UniqueLearnerNumber = null,
                UlnStatusId = 0,
                Gender = GetGender(candidateUser.Candidate.MonitoringInformation.Gender),
                EthnicOrigin = GetEthnicOrigin(candidateUser.Candidate.MonitoringInformation.Ethnicity),
                EthnicOriginOther = GetEthnicOriginOther(candidateUser.Candidate.MonitoringInformation.Ethnicity),
                /*ApplicationLimitEnforced = candidateUser.Candidate.,
                LastAccessedDate = candidateUser.Candidate.,
                AdditionalEmail = candidateUser.Candidate.,
                Disability = candidateUser.Candidate.,
                DisabilityOther = candidateUser.Candidate.,
                HealthProblems = candidateUser.Candidate.,
                ReceivePushedContent = candidateUser.Candidate.,
                ReferralAgent = candidateUser.Candidate.,
                DisableAlerts = candidateUser.Candidate.,
                UnconfirmedEmailAddress = candidateUser.Candidate.,
                MobileNumberUnconfirmed = candidateUser.Candidate.,
                DoBFailureCount = candidateUser.Candidate.,
                ForgottenUsernameRequested = candidateUser.Candidate.,
                ForgottenPasswordRequested = candidateUser.Candidate.,
                TextFailureCount = candidateUser.Candidate.,
                EmailFailureCount = candidateUser.Candidate.,
                LastAccessedManageApplications = candidateUser.Candidate.,
                ReferralPoints = candidateUser.Candidate.,
                BeingSupportedBy = candidateUser.Candidate.,
                LockedForSupportUntil = candidateUser.Candidate.,
                NewVacancyAlertEmail = candidateUser.Candidate.,
                NewVacancyAlertSMS = candidateUser.Candidate.,
                AllowMarketingMessages = candidateUser.Candidate.,
                ReminderMessageSent = candidateUser.Candidate.,*/
                CandidateGuid = candidateUser.Candidate.Id
            };

            var person = new Person
            {
                Title = 0,
                OtherTitle = "",
                FirstName = candidateUser.Candidate.RegistrationDetails.FirstName,
                MiddleNames = candidateUser.Candidate.RegistrationDetails.MiddleNames,
                Surname = candidateUser.Candidate.RegistrationDetails.LastName,
                LandlineNumber = candidateUser.Candidate.RegistrationDetails.PhoneNumber,
                MobileNumber = "",
                Email = email,
                PersonTypeId = 1
            };

            return new CandidatePerson
            {
                Candidate = candidate,
                Person = person
            };
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

        private int GetCandidateId(CandidateSummary candidate, IDictionary<Guid, int> candidateIds)
        {
            var candidateId = candidateIds.ContainsKey(candidate.Id) ? candidateIds[candidate.Id] : candidate.LegacyCandidateId;

            if (candidate.LegacyCandidateId != 0 && candidateId != candidate.LegacyCandidateId)
            {
                var message = $"CandidateId: {candidateId} does not match the LegacyCandidateId: {candidate.LegacyCandidateId} for candidate with Id: {candidate.Id}";
                _logService.Error(message);
                throw new Exception(message);
            }

            return candidateId;
        }

        private int GetGender(int? gender)
        {
            if (gender.HasValue)
            {
                if (gender == 4)
                    return 3;
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

        private string GetEthnicOriginOther(int? ethnicity)
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
    }
}