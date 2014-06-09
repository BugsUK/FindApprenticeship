﻿namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IntegrationTests
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using NUnit.Framework;
    using Application.VacancyEtl;
    using Domain.Entities.Vacancy;
    using Common.Wcf;
    using Configuration;
    using VacancySummaryProxy;
    using StructureMap;

    [TestFixture]
    public class VacancyServiceTests
    {
        private ILegacyServicesConfiguration _legacyServicesConfiguration;

        [TestFixtureSetUp]
        public void Setup()
        {
            _legacyServicesConfiguration = ObjectFactory.GetInstance<ILegacyServicesConfiguration>();
        }

        [TestCase]
        public void TheServiceEndpointShouldRespond()
        {
            var vacancySummaryRequest = new VacancySummaryRequest
            {
                ExternalSystemId = _legacyServicesConfiguration.SystemId,
                PublicKey = _legacyServicesConfiguration.PublicKey,
                MessageId = Guid.NewGuid(),
                VacancySearchCriteria = new VacancySearchData
                {
                    PageIndex = 1,
                    VacancyLocationType = VacancyDetailsSearchLocationType.NonNational.ToString()
                }
            };

            var result = default(VacancySummaryResponse);
            var service = ObjectFactory.GetInstance<IWcfService<IVacancySummary>>();
            service.Use(client => { result = client.Get(vacancySummaryRequest); });

            result.Should().NotBeNull();
        }

        [TestCase]
        public void ShouldReturnMappedCollectionFromGetVacancySummary()
        {
            var service = ObjectFactory.GetInstance<IVacancyIndexDataProvider>();
            var result = service.GetVacancySummary(VacancyLocationType.NonNational, 1).ToList();

            result.Should().NotBeNullOrEmpty();
            result.ForEach(r => r.VacancyLocationType.Should().Be(VacancyLocationType.NonNational));
        }
    }
}
