namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Settings
{
    using Candidate.ViewModels.Account;
    using Candidate.Views.Account;
    using HtmlAgilityPack;
    using RazorGenerator.Testing;

    public class SettingsViewBuilder : ViewBuilderBase
    {
        private SettingsViewModel _viewModel;

        public SettingsViewBuilder With(SettingsViewModel viewModel)
        {
            _viewModel = viewModel;
            return this;
        }

        public Settings Build()
        {
            var view = new Settings {ViewData = {Model = _viewModel}};
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