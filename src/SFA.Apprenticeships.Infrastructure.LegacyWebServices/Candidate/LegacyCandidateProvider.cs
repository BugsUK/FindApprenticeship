namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.Candidate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Application.Candidate;
    using SFA.Infrastructure.Interfaces;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using GatewayServiceProxy;
    using Newtonsoft.Json;
    using Wcf;
    using Candidate = GatewayServiceProxy.Candidate;
    using CreateCandidateRequest = GatewayServiceProxy.CreateCandidateRequest;
    using ErrorCodes = Apprenticeships.Application.Interfaces.Candidates.ErrorCodes;

    public class LegacyCandidateProvider : ILegacyCandidateProvider
    {
        private readonly static Dictionary<string, string> LegacyCandidateServiceErrorsMap = new Dictionary<string, string>
        {
            {ValidationErrorCodes.InvalidCandidateState, ErrorCodes.CandidateStateError},
            {ValidationErrorCodes.CandidateNotFound, ErrorCodes.CandidateNotFoundError},
            {ValidationErrorCodes.UnknownCandidate, ErrorCodes.CandidateNotFoundError}
        };

        private readonly ILogService _logger;
        private readonly IWcfService<GatewayServiceContract> _service;

        public LegacyCandidateProvider(IWcfService<GatewayServiceContract> service, ILogService logger)
        {
            _service = service;
            _logger = logger;
        }

        #region Create Candidate

        public int CreateCandidate(Domain.Entities.Candidates.Candidate candidate)
        {
            var context = new { candidateId = candidate.EntityId };

            try
            {
                _logger.Debug("Calling Legacy.CreateCandidate for candidate id='{0}'", candidate.EntityId);

                var legacyCandidateId = CreateLegacyCandidate(candidate);

                _logger.Debug("Legacy.CreateCandidate succeeded for candidate id='{0}', legacy candidate id='{1}'", candidate.EntityId, legacyCandidateId);

                return legacyCandidateId;
            }
            catch (DomainException e)
            {
                _logger.Error(e);
                throw;
            }
            catch (BoundaryException e)
            {
                _logger.Error(e, context);
                throw new DomainException(ErrorCodes.CreateCandidateFailed, e, context);
            }
            catch (Exception e)
            {
                _logger.Error(e, context);
                throw;
            }
        }

        private int CreateLegacyCandidate(Domain.Entities.Candidates.Candidate candidate)
        {
            var request = new CreateCandidateRequest { Candidate = MapLegacyCandidate(candidate) };
            var response = default(CreateCandidateResponse);

            _service.Use("SecureService", client => response = client.CreateCandidate(request));

            if (response == null || (response.ValidationErrors != null && response.ValidationErrors.Any()))
            {
                string message;
                string errorCode;

                if (response == null)
                {
                    message = "No response";
                    errorCode = ErrorCodes.CreateCandidateFailed;
                }
                else
                {
                    ParseCreateCandidateResponseValidationError(response, out message, out errorCode);
                }

                throw new DomainException(errorCode, new { message, candidateId = candidate.EntityId });
            }

            return response.CandidateId;
        }

        private static void ParseCreateCandidateResponseValidationError(CreateCandidateResponse response, out string message, out string errorCode)
        {
            foreach (var pair in LegacyCandidateServiceErrorsMap)
            {
                var validationError = response.ValidationErrors.FirstOrDefault(each => each.ErrorCode == pair.Key);

                if (validationError != null)
                {
                    message = string.Format("{0} (ErrorCode='{1}')", validationError.Message, pair.Key);
                    errorCode = pair.Value;
                    return;
                }
            }

            // Failed to parse expected validation error.
            message = string.Format("{0} unexpected validation error(s): {1}", response.ValidationErrors.Count(), JsonConvert.SerializeObject(response, Formatting.None));
            errorCode = ErrorCodes.CreateCandidateFailed;
        }

        #endregion

        #region Update Candidate

        public void UpdateCandidate(Domain.Entities.Candidates.Candidate candidate)
        {
            var context = new { candidateId = candidate.EntityId };

            try
            {
                _logger.Debug("Calling Legacy.UpdateCandidate for candidate id='{0}'", candidate.EntityId);

                UpdateLegacyCandidate(candidate);

                _logger.Debug("Legacy.UpdateCandidate succeeded for candidate id='{0}'", candidate.EntityId);
            }
            catch (DomainException e)
            {
                _logger.Error(e);
                throw;
            }
            catch (BoundaryException e)
            {
                _logger.Error(e, context);
                throw new DomainException(ErrorCodes.UpdateCandidateFailed, e, context);
            }
            catch (Exception e)
            {
                _logger.Error(e, context);
                throw;
            }
        }

        private void UpdateLegacyCandidate(Domain.Entities.Candidates.Candidate candidate)
        {
            var request = new UpdateCandidateRequest { Candidate = MapLegacyCandidate(candidate) };
            request.Candidate.Id = candidate.LegacyCandidateId;
            request.Candidate.IdSpecified = true;

            var response = default(UpdateCandidateResponse);

            _service.Use("SecureService", client => response = client.UpdateCandidate(request));

            if (response == null || (response.ValidationErrors != null && response.ValidationErrors.Any()))
            {
                string message;
                string errorCode;

                if (response == null)
                {
                    message = "No response";
                    errorCode = ErrorCodes.UpdateCandidateFailed;
                }
                else
                {
                    ParseUpdateCandidateResponseValidationError(response, out message, out errorCode);
                }

                throw new DomainException(errorCode, new { message, candidateId = candidate.EntityId });
            }
        }

        private static void ParseUpdateCandidateResponseValidationError(UpdateCandidateResponse response, out string message, out string errorCode)
        {
            foreach (var pair in LegacyCandidateServiceErrorsMap)
            {
                var validationError = response.ValidationErrors.FirstOrDefault(each => each.ErrorCode == pair.Key);

                if (validationError != null)
                {
                    message = string.Format("{0} (ErrorCode='{1}')", validationError.Message, pair.Key);
                    errorCode = pair.Value;
                    return;
                }
            }

            // Failed to parse expected validation error.
            message = string.Format("{0} unexpected validation error(s): {1}", response.ValidationErrors.Count(), JsonConvert.SerializeObject(response, Formatting.None));
            errorCode = ErrorCodes.UpdateCandidateFailed;
        }

        #endregion

        #region Helpers/Mappers

        private static Candidate MapLegacyCandidate(Domain.Entities.Candidates.Candidate candidate)
        {
            var disabilityStatus = MapDisabilityStatus(candidate);
            var ethnicity = MapEthnicity(candidate);
            var gender = MapGender(candidate);

            return new Candidate
            {
                EmailAddress = candidate.RegistrationDetails.EmailAddress,
                FirstName = candidate.RegistrationDetails.FirstName,
                MiddleName = candidate.RegistrationDetails.MiddleNames,
                Surname = candidate.RegistrationDetails.LastName,
                DateOfBirth = candidate.RegistrationDetails.DateOfBirth.Date,
                AddressLine1 = candidate.RegistrationDetails.Address.AddressLine1,
                AddressLine2 = candidate.RegistrationDetails.Address.AddressLine2,
                AddressLine3 = candidate.RegistrationDetails.Address.AddressLine3,
                AddressLine4 = candidate.RegistrationDetails.Address.AddressLine4,
                TownCity = "N/A",
                Postcode = candidate.RegistrationDetails.Address.Postcode,
                LandlineTelephone = candidate.RegistrationDetails.PhoneNumber,
                MobileTelephone = string.Empty,
                Disability = disabilityStatus,
                DisabilitySpecified = disabilityStatus.HasValue,
                EthnicOrigin = ethnicity,
                EthnicOriginSpecified = ethnicity.HasValue,
                EthnicOrginOther = MapEthnicOriginOther(candidate),
                Gender = gender,
                GenderSpecified = gender.HasValue
            };
        }

        private static int? MapDisabilityStatus(Domain.Entities.Candidates.Candidate candidate)
        {
            if (!candidate.MonitoringInformation.DisabilityStatus.HasValue)
            {
                return null;
            }

            switch (candidate.MonitoringInformation.DisabilityStatus.Value)
            {
                case DisabilityStatus.Yes:
                    return 13;

                case DisabilityStatus.No:
                    return 0;
            }

            return 14;
        }

        private static int? MapEthnicity(Domain.Entities.Candidates.Candidate candidate)
        {
            var ethnicity = candidate.MonitoringInformation.Ethnicity;

            if (!ethnicity.HasValue)
            {
                return null;
            }

            var ethnicities = new Dictionary<int, int>
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

            if (ethnicities.ContainsKey(ethnicity.Value))
            {
                return ethnicities[ethnicity.Value];
            }

            return null;
        }

        private static int? MapGender(Domain.Entities.Candidates.Candidate candidate)
        {
            return candidate.MonitoringInformation.Gender.HasValue
                ? (int)candidate.MonitoringInformation.Gender.Value
                : default(int?);
        }

        private static string MapEthnicOriginOther(Domain.Entities.Candidates.Candidate candidate)
        {
            var ethnicity = candidate.MonitoringInformation.Ethnicity;

            if (!ethnicity.HasValue)
            {
                return null;
            }

            switch (ethnicity)
            {
                case 33:
                    return "Gypsy or Irish Traveller";

                case 47:
                    return "Arab";

                case 99:
                    return "Not provided";
            }

            return null;
        }

        #endregion
    }
}