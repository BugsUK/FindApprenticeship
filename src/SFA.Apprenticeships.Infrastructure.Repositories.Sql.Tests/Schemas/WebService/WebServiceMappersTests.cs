namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Schemas.WebService
{
    using System;
    using Domain.Entities.WebServices;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;
    using Sql.Schemas.WebService;
    using WebServiceConsumer = Sql.Schemas.WebService.Entities.WebServiceConsumer;

    [TestFixture]
    public class WebServiceMappersTests
    {
        private IMapper _mapper;
        private WebServiceConsumer _source;

        [SetUp]
        public void SetUp()
        {
            _mapper = new WebServiceMappers();

            _source = new Fixture()
                .Build<WebServiceConsumer>()
                .With(each => each.WebServiceConsumerTypeCode, "P")
                .With(each => each.AllowReferenceDataService, false)
                .With(each => each.AllowVacancyUploadService, false)
                .With(each => each.AllowVacancyDetailService, false)
                .With(each => each.AllowVacancySummaryService, false)
                .Create();
        }

        [Test]
        public void ShouldCreateMap()
        {
            new WebServiceMappers().Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void ShouldMapWebConsumer()
        {
            // Act.
            var target = _mapper.Map<WebServiceConsumer, Domain.Entities.WebServices.WebServiceConsumer>(_source);

            // Assert.
            target.Should().NotBeNull();

            target.ExternalSystemId.Should().Be(_source.ExternalSystemId);
            target.ExternalSystemPassword.Should().Be(_source.ExternalSystemPassword);

            target.AllowReferenceDataService.Should().BeFalse();
            target.AllowVacancyUploadService.Should().BeFalse();
            target.AllowVacancySummaryService.Should().BeFalse();
            target.AllowVacancyDetailService.Should().BeFalse();
        }

        [Test]
        public void ShouldMapWebConsumerAllowReferenceDataService()
        {
            // Arrange.
            _source.AllowReferenceDataService = true;

            // Act.
            var target = _mapper.Map<WebServiceConsumer, Domain.Entities.WebServices.WebServiceConsumer>(_source);

            // Assert.
            target.AllowReferenceDataService.Should().BeTrue();
        }

        [Test]
        public void ShouldMapWebConsumerAllowVacancyUploadService()
        {
            // Arrange.
            _source.AllowVacancyUploadService = true;

            // Act.
            var target = _mapper.Map<WebServiceConsumer, Domain.Entities.WebServices.WebServiceConsumer>(_source);

            // Assert.
            target.AllowVacancyUploadService.Should().BeTrue();
        }

        [Test]
        public void ShouldMapWebConsumerAllowVacancySummaryService()
        {
            // Arrange.
            _source.AllowVacancySummaryService = true;

            // Act.
            var target = _mapper.Map<WebServiceConsumer, Domain.Entities.WebServices.WebServiceConsumer>(_source);

            // Assert.
            target.AllowVacancySummaryService.Should().BeTrue();
        }

        [Test]
        public void ShouldMapWebConsumerAllowVacancyDetailService()
        {
            // Arrange.
            _source.AllowVacancyDetailService = true;

            // Act.
            var target = _mapper.Map<WebServiceConsumer, Domain.Entities.WebServices.WebServiceConsumer>(_source);

            // Assert.
            target.AllowVacancyDetailService.Should().BeTrue();
        }

        [TestCase("P", WebServiceConsumerType.Provider)]
        [TestCase("E", WebServiceConsumerType.Employer)]
        [TestCase("T", WebServiceConsumerType.ThirdParty)]
        public void ShouldMapWebServiceConsumerType(string webServiceConsumerTypeCode, WebServiceConsumerType webServiceConsumerType)
        {
            // Arrange.
            _source.WebServiceConsumerTypeCode = webServiceConsumerTypeCode;

            // Act.
            var target = _mapper.Map<WebServiceConsumer, Domain.Entities.WebServices.WebServiceConsumer>(_source);

            // Assert.
            target.WebServiceConsumerType.Should().Be(webServiceConsumerType);
        }

        [Test]
        public void ShouldThrowIfWebServiceConsumerTypeIsUnknown()
        {
            // Arrange.
            _source.WebServiceConsumerTypeCode = "?";

            // Act.
            Action action = () => _mapper.Map<WebServiceConsumer, Domain.Entities.WebServices.WebServiceConsumer>(_source);

            // Assert.
            action.ShouldThrow<Exception>();
        }
    }
}
