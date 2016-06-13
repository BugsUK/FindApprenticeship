﻿namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.LegacyWebServices
{
    using System;
    using Application.Candidate;
    using Common.Configuration;
    using Common.IoC;
    using Domain.Entities.Candidates;
    using Domain.Entities.Locations;
    using Domain.Entities.Users;
    using FluentAssertions;
    using Infrastructure.LegacyWebServices.IoC;
    using Infrastructure.Monitor.IoC;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;

    public class LegacyCandidateProviderIntegrationTests
    {
        private ILegacyCandidateProvider _legacyCandidateProvider;

        [SetUp]
        public void SetUp()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry(new LegacyWebServicesRegistry(new ServicesConfiguration { ServiceImplementation = ServicesConfiguration.Legacy, VacanciesSource = ServicesConfiguration.Legacy }, new CacheConfiguration()));
                x.AddRegistry(new VacancySourceRegistry(new CacheConfiguration(), new ServicesConfiguration { ServiceImplementation = ServicesConfiguration.Legacy }));
            });

            _legacyCandidateProvider = container.GetInstance<ILegacyCandidateProvider>();
        }

        [Test, Category("Integration")]
        public void ShouldCreateCandidateUsingLegacyCandidateProvider()
        {
            var candidate = new Candidate
            {
                EntityId = Guid.NewGuid(),
                RegistrationDetails = new RegistrationDetails
                {
                    FirstName = "FirstName",
                    LastName = "LastName",
                    EmailAddress = string.Format("nas.exemplar+{0}@gmail.com", Guid.NewGuid()),
                    DateOfBirth = new DateTime(1980, 06, 15),
                    PhoneNumber = "01221234567",
                    Address = new Address
                    {
                        AddressLine1 = "103 Crawley Drive",
                        AddressLine3 = "Hemel Hempstead",
                        AddressLine4 = "Hertfordhsire",
                        Postcode = "HP2 6AL",
                        AddressLine2 = "Hemel Hempstead"
                    },
                }
            };

            var result = _legacyCandidateProvider.CreateCandidate(candidate);

            result.Should().BeGreaterThan(0);
        }
    }
}