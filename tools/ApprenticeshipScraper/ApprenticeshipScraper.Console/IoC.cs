namespace ApprenticeshipScraper.CmdLine
{
    using ApprenticeshipScraper.CmdLine.Services;
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
            container.RegisterMultiple<IStep>(
                new[]
                    {
                        typeof(ApprenticeshipResultsScraper),
                        typeof(ApprenticeshipDetailsScraper),
                        typeof(TraineeResultsScraper),
                        typeof(TraineeDetailsScraper)
                    });
            return container;
        }
    }
}