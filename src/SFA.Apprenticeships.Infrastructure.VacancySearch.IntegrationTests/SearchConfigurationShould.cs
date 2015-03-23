namespace SFA.Apprenticeships.Infrastructure.VacancySearch.IntegrationTests
{
    using Common.IoC;
    using Domain.Interfaces.Configuration;
    using Elastic.Common.IoC;
    using FluentAssertions;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;
    using VacancySearch.Configuration;

    [TestFixture]
    public class SearchConfigurationShould
    {
        [Test]
        public void LoadWithValuesSetFromConfig()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<ElasticsearchCommonRegistry>();
            });

            var config = container.GetInstance<IConfigurationService>().Get<SearchFactorConfiguration>(SearchFactorConfiguration.SearchTermFactorsName);

            config.Should().NotBeNull();
            config.JobTitleFactors.Enabled.Should().BeTrue();
            config.DescriptionFactors.Enabled.Should().BeTrue();
            config.EmployerFactors.Enabled.Should().BeTrue();

            var jobFactors = config.JobTitleFactors;
            jobFactors.Should().NotBeNull();
            jobFactors.Boost.Should().Be(1.5d);
            jobFactors.Fuzziness.Should().Be(1);
            jobFactors.FuzzyPrefix.Should().Be(1);
            jobFactors.MatchAllKeywords.Should().BeTrue();
            jobFactors.PhraseProximity.Should().NotHaveValue();
            jobFactors.MinimumMatch.Should().Be("100%");

            var descriptionFactors = config.DescriptionFactors;
            descriptionFactors.Should().NotBeNull();
            descriptionFactors.Boost.Should().Be(5d);
            descriptionFactors.Fuzziness.Should().Be(1);
            descriptionFactors.FuzzyPrefix.Should().Be(1);
            descriptionFactors.MatchAllKeywords.Should().BeTrue();
            descriptionFactors.PhraseProximity.Should().NotHaveValue();
            descriptionFactors.MinimumMatch.Should().Be("100%");

            //Not in config so should all be defaults
            var employerFactors = config.EmployerFactors;
            employerFactors.Should().NotBeNull();
            employerFactors.Boost.Should().Be(1.0d);
            employerFactors.Fuzziness.Should().Be(1);
            employerFactors.FuzzyPrefix.Should().Be(1);
            employerFactors.MatchAllKeywords.Should().BeFalse();
            employerFactors.PhraseProximity.Should().Be(2);
            employerFactors.MinimumMatch.Should().Be("2<75%");
        }
    }
}
