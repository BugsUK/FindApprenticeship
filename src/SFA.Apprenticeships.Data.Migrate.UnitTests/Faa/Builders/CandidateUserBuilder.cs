﻿namespace SFA.Apprenticeships.Data.Migrate.UnitTests.Faa.Builders
{
    using System;
    using Migrate.Faa.Entities.Mongo;

    public class CandidateUserBuilder
    {
        private Guid _candidateId = Guid.NewGuid();
        private int _legacyCandidateId = 456789;
        private int _status = 20;
        private int _gender;
        private int _disabilityStatus;
        private int _ethnicity;
        private string _emailAddress = "Test@TEST.CoM";
        private string _postCode = "B26 2LW";
        private ApplicationTemplate _applicationTemplate;
        private CommunicationPreferences _communicationPreferences;
        private DateTime? _activationDate;

        public CandidateUser Build()
        {
            var candidate = new Candidate
            {
                Id = _candidateId,
                DateCreated = DateTime.Now.AddDays(-7),
                DateUpdated = DateTime.Now,
                LegacyCandidateId = _legacyCandidateId,
                RegistrationDetails = new RegistrationDetails
                {
                    FirstName = "FirstName",
                    MiddleNames = "MiddleNames",
                    LastName = "LastName",
                    DateOfBirth = new DateTime(1978, 10, 23),
                    Address = new Address
                    {
                        AddressLine1 = "Address Line 1",
                        AddressLine2 = "Address Line 2",
                        AddressLine3 = "Address Line 3",
                        AddressLine4 = "Address Line 4",
                        GeoPoint = new GeoPoint
                        {
                            Latitude = 52.8810974569710030,
                            Longitude = -1.7005217601595300
                        },
                        Postcode = _postCode
                    },
                    EmailAddress = _emailAddress,
                    PhoneNumber = "07895123456"
                },
                ApplicationTemplate = _applicationTemplate,
                CommunicationPreferences = _communicationPreferences,
                MonitoringInformation = new MonitoringInformation
                {
                    Gender = _gender,
                    DisabilityStatus = _disabilityStatus,
                    Ethnicity = _ethnicity
                }
            };

            var user = new User
            {
                Id = _candidateId,
                DateCreated = DateTime.Now.AddDays(-7),
                DateUpdated = DateTime.Now,
                Status = _status,
                ActivationDate = _activationDate,
                LastLogin = DateTime.Now.AddHours(-12)
            };

            return new CandidateUser
            {
                Candidate = candidate,
                User = user
            };
        }

        public CandidateUserBuilder WithCandidateId(Guid candidateId)
        {
            _candidateId = candidateId;
            return this;
        }

        public CandidateUserBuilder WithLegacyCandidateId(int legacyCandidateId)
        {
            _legacyCandidateId = legacyCandidateId;
            return this;
        }

        public CandidateUserBuilder WithStatus(int status)
        {
            _status = status;
            if (status >= 20 && _status < 999)
            {
                _activationDate = DateTime.Now.AddDays(-6);
            }
            return this;
        }

        public CandidateUserBuilder WithGender(int gender)
        {
            _gender = gender;
            return this;
        }

        public CandidateUserBuilder WithDisabilityStatus(int disabilityStatus)
        {
            _disabilityStatus = disabilityStatus;
            return this;
        }

        public CandidateUserBuilder WithEthnicity(int ethnicity)
        {
            _ethnicity = ethnicity;
            return this;
        }

        public CandidateUserBuilder WithEmailAddress(string emailAddress)
        {
            _emailAddress = emailAddress;
            return this;
        }

        public CandidateUserBuilder WithPostCode(string postCode)
        {
            _postCode = postCode;
            return this;
        }

        public CandidateUserBuilder WithApplicationTemplate(ApplicationTemplate applicationTemplate)
        {
            _applicationTemplate = applicationTemplate;
            return this;
        }

        public CandidateUserBuilder WithCommunicationPreferences(CommunicationPreferences communicationPreferences)
        {
            _communicationPreferences = communicationPreferences;
            return this;
        }

        public CandidateUserBuilder WithActivated(bool activationDate)
        {
            if (activationDate)
            {
                _activationDate = DateTime.Now.AddDays(-6);
            }
            else
            {
                _activationDate = null;
            }
            return this;
        }
    }
}