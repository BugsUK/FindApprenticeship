namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.VacancySearch
{
    using Common.IoC;
    using SFA.Infrastructure.Interfaces;
    using FluentAssertions;
    using Infrastructure.Elastic.Common.IoC;
    using Infrastructure.VacancySearch.Configuration;
    using Logging.IoC;
    using NUnit.Framework;
    using StructureMap;

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

            var config = container.GetInstance<IConfigurationService>().Get<SearchFactorConfiguration>();

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

            var employerFactors = config.EmployerFactors;
            employerFactors.Should().NotBeNull();
            employerFactors.Boost.Should().Be(5d);
            employerFactors.Fuzziness.Should().Be(1);
            employerFactors.FuzzyPrefix.Should().Be(1);
            employerFactors.MatchAllKeywords.Should().BeTrue();
            employerFactors.PhraseProximity.Should().NotHaveValue();
            employerFactors.MinimumMatch.Should().Be("100%");

            //Not in config so should all be defaults
            var desciptionfactors = config.DescriptionFactors;
            desciptionfactors.Should().NotBeNull();
            desciptionfactors.Boost.Should().Be(1.0d);
            desciptionfactors.Fuzziness.Should().Be(1);
            desciptionfactors.FuzzyPrefix.Should().Be(1);
            desciptionfactors.MatchAllKeywords.Should().BeFalse();
            desciptionfactors.PhraseProximity.Should().Be(2);
            desciptionfactors.MinimumMatch.Should().Be("2<75%");
        }
    }
}
