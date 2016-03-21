namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy
{
    using Providers;

    public class QAActionResult<T> where T: IPartialVacancyViewModel
    {
        public QAActionResult(QAActionResultCode qaActionResultCode, T viewModel = default(T))
        {
            Code = qaActionResultCode;
            ViewModel = viewModel;
        }

        public QAActionResultCode Code { get; private set; }

        public T ViewModel { get; private set; }
    }
}