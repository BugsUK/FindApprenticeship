namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using NUnit.Framework;
    using Raa.Common.Validators.Vacancy;

    public class VacancyViewModelValidatorTests
    {
        private VacancyViewModelValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new VacancyViewModelValidator();
        }
    }
}