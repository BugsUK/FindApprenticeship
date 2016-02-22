namespace SFA.Apprenticeships.Web.Raa.Common.Converters
{
    using Domain.Entities.Raa.Parties;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;

    public static class EmployerViewModelConverter
    {
        public static EmployerResultViewModel ConvertToResult(this EmployerViewModel employer)
        {
            var viewModel = new EmployerResultViewModel
            {
                EmployerId = employer.EmployerId,
                EmployerName = employer.Name,
                Address = employer.Address
            };

            return viewModel;
        }

        public static EmployerResultViewModel ConvertToResult(this Employer employer)
        {
            var viewModel = new EmployerResultViewModel
            {
                EmployerId = employer.EmployerId,
                EmployerName = employer.Name,
                Address = employer.Address.Convert()
            };

            return viewModel;
        }

        public static EmployerViewModel Convert(this Employer employer)
        {
            var viewModel = new EmployerViewModel
            {
                EmployerId = employer.EmployerId,
                Name = employer.Name,
                Address = employer.Address.Convert()
            };

            return viewModel;
        }
    }
}