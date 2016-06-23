namespace ApprenticeshipScraper.CmdLine
{
    using System.Linq;

    using ApprenticeshipScraper.CmdLine.Services;
    using ApprenticeshipScraper.CmdLine.Services.Logger;
    using ApprenticeshipScraper.CmdLine.Settings;
    using ApprenticeshipScraper.CmdLine.Steps;

    using TinyIoC;

    public static class IoC
    {
        public static TinyIoCContainer RegisterDependencies()
        {
            var container = TinyIoCContainer.Current;
            container.Register<ICreateDirectory, DirectoryService>();
            container.Register<IUrlResolver, UrlResolver>();
            container.Register<IStepLogger, TimedStepLogger>();
            container.Register<IGlobalSettings, GlobalSettings>();
            container.Register<IThreadSafeStepLogger, ThreadSafeStepLogger>();
            container.Register<IRetryWebRequests, WebRequestRetryService>();
            container.RegisterMultiple<IStep>(
                new[]
                    {
                        typeof(ApprenticeshipResultsScraper),
                        typeof(TraineeResultsScraper),
                        typeof(TraineeDetailsScraper),
                        typeof(ApprenticeshipDetailsScraper)
                    });
            return container;
        }
    }
}