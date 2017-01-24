namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.IntegrationTests.Schemas.dbo.Vacancy
{
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions.Equivalency;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using System;
    using DomainVacancy = Domain.Entities.Raa.Vacancies.Vacancy;

    [TestFixture]
    public class TestBase
    {
        private const string FramworkCodeNameFramework1 = "260";
        private const int StandardIdStandard1 = 1;

        protected DomainVacancy CreateValidDomainVacancy()
        {
            var fixture = new Fixture();

            fixture.Customizations.Add(
                new StringGenerator(() =>
                    Guid.NewGuid().ToString().Substring(0, 10)));

            var result = fixture.Build<DomainVacancy>()
                .With(av => av.VacancyId, 0)
                .With(av => av.Status, VacancyStatus.Submitted)
                .With(av => av.DateSubmitted, null)
                .With(av => av.QAUserName, null)
                .With(av => av.DateStartedToQA, DateTime.MinValue)
                .With(av => av.DeliveryOrganisationId, null)
                .With(av => av.DateQAApproved, null)
                .With(av => av.VacancyReferenceNumber, Guid.NewGuid().GetHashCode())
                .With(av => av.VacancyLocationType, VacancyLocationType.SpecificLocation)
                .With(av => av.ParentVacancyId, null)
                .With(av => av.UpdatedDateTime, null)
                .With(av => av.RegionalTeam, RegionalTeam.NorthWest)
                .With(av => av.IsAnonymousEmployer, null)
                .With(av => av.NewApplicationCount, 0)
                .With(av => av.ApplicantCount, 0)
                .With(av => av.ProviderTradingName, null)
                .With(av => av.CreatedDate, DateTime.Today)
                .With(av => av.IsMultiLocation, null)
                .Create();

            result.Address = new PostalAddress
            {
                Postcode = "CV1 2WT",
                County = "West Midlands"
            };

            result.LocalAuthorityCode = "00CC";
            if (result.FrameworkCodeName != null && result.FrameworkCodeName.GetHashCode() % 2 == 1)
            {
                result.TrainingType = TrainingType.Frameworks;
                result.FrameworkCodeName = FramworkCodeNameFramework1;
                result.StandardId = null;
                result.SectorCodeName = null;
            }
            else
            {
                result.TrainingType = TrainingType.Standards;
                result.FrameworkCodeName = null;
                result.StandardId = StandardIdStandard1;
                result.SectorCodeName = "ALB";
            }

            return result;
        }

        protected EquivalencyAssertionOptions<DomainVacancy> ForShallowSave(EquivalencyAssertionOptions<DomainVacancy> options)
        {
            return options.Excluding(v => v.FrameworkCodeName);
        }
    }
}
