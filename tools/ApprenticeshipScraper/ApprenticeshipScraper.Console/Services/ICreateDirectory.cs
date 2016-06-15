namespace ApprenticeshipScraper.CmdLine.Services
{
    using ApprenticeshipScraper.CmdLine.Models;

    public interface ICreateDirectory
    {
        void CreateDirectoryIfMissing(string name);

        string FindStepFolder(ApplicationArguments arguments, string stepName);
    }
}