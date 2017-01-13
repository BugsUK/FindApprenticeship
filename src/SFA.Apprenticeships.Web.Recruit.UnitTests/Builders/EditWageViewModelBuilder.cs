namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Builders
{
    using System;
    using Common.Mappers.Resolvers;
    using Domain.Entities.Vacancies;
    using Raa.Common.ViewModels.Vacancy;
    using Raa.Common.ViewModels.VacancyManagement;

    public class EditWageViewModelBuilder
    {
        private readonly WageType _wageType;
        private readonly decimal? _amount;
        private readonly decimal? _amountLowerBound;
        private readonly decimal? _amountUpperBound;
        private WageUnit _wageUnit = WageUnit.Weekly;

        public EditWageViewModelBuilder(WageType wageType)
        {
            _wageType = wageType;
            if (_wageType == WageType.Custom)
            {
                _amount = 200;
                _amountLowerBound = 200;
            }
            if (_wageType == WageType.CustomRange)
            {
                _amount = 200;
                _amountLowerBound = 200;
                _amountUpperBound = 220;
            }
        }

        public EditWageViewModel Build()
        {
            var existingWage = new Wage(_wageType, _amount, _amountLowerBound, _amountUpperBound, "", _wageUnit, 30, "");
            var converter = new WageToWageViewModelConverter();
            var wageViewModel = converter.Convert(existingWage);

            return new EditWageViewModel
            {
                Type = _wageType,
                Amount = _amount,
                AmountLowerBound = _amountLowerBound,
                AmountUpperBound = _amountUpperBound,
                Unit = _wageUnit,
                ExistingWage = existingWage,
                PossibleStartDate = DateTime.UtcNow.Date.AddMonths(2),
                VacancyReferenceNumber = 1,
                VacancyApplicationsState = VacancyApplicationsState.NoApplications,
                Classification = wageViewModel.Classification,
                CustomType = wageViewModel.CustomType,
                RangeUnit = _wageUnit
            };
        }
    }
}