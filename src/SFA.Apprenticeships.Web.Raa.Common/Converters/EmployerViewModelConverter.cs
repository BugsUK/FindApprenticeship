namespace SFA.Apprenticeships.Web.Raa.Common.Converters
{
    using Domain.Entities.Raa.Parties;
    using ViewModels.Employer;

    public static class EmployerViewModelConverter
    {
        public static EmployerViewModel Convert(this Employer employer)
        {
            var viewModel = new EmployerViewModel
            {
                EmployerId = employer.EmployerId,
                EdsUrn = employer.EdsUrn,
                FullName = employer.FullName,
                TradingName = employer.TradingName,
                Address = employer.Address.Convert(),
                Status = employer.EmployerStatus
            };

            return viewModel;
        }
    }
}