namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mappers
{
    using Candidate.Mappers;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class TraineeshipApplicationViewModelMapperTests
    {
        [Test]
        public void ShouldCreateMap()
        {
            new TraineeshipCandidateWebMappers().Mapper.AssertConfigurationIsValid();
        }
    }
}