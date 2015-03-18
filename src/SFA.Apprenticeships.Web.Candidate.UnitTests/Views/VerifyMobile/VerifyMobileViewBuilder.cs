namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.VerifyMobile
{
    using Candidate.ViewModels.Account;
    using Candidate.Views.Account;
    using HtmlAgilityPack;
    using RazorGenerator.Testing;

    public class VerifyMobileViewBuilder : ViewBuilderBase
    {
         private VerifyMobileViewModel _viewModel;

        public VerifyMobileViewBuilder With(VerifyMobileViewModel viewModel)
        {
            _viewModel = viewModel;
            return this;
        }

        public VerifyMobile Build()
        {
            var view = new VerifyMobile { ViewData = { Model = _viewModel } };
            return view;
        }

        public HtmlDocument Render()
        {
            var view = Build();
            var result = view.RenderAsHtml(_viewModel);
            return result;
        }
    }
}