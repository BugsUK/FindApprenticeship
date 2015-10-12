namespace SFA.Apprenticeships.Web.Recruit.Converters
{
    using Domain.Entities.Organisations;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;

    public static class EmployerViewModelConverter
    {
        public static EmployerResultViewModel ConvertToResult(this Employer employer)
        {
            var viewModel = new EmployerResultViewModel
            {
                Ern = employer.Ern,
                //EmployerId = 
                EmployerName = employer.Name,
                Address = employer.Address.Convert()
            };

            return viewModel;
        }

        public static EmployerViewModel Convert(this Employer employer)
        {
            var viewModel = new EmployerViewModel
            {
                ProviderSiteErn = employer.ProviderSiteErn,
                Ern = employer.Ern,
                Name = employer.Name,
                Description = employer.Description,
                WebsiteUrl = employer.WebsiteUrl,
                IsWebsiteUrlWellFormed = employer.IsWebsiteUrlWellFormed,
                Address = employer.Address.Convert()
            };

            return viewModel;
        }
    }
}