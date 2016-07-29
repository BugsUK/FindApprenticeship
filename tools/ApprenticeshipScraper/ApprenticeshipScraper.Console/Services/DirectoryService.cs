namespace ApprenticeshipScraper.CmdLine.Services
{
    using System.IO;

    using ApprenticeshipScraper.CmdLine.Models;

    public sealed class DirectoryService : ICreateDirectory
    {
        public void CreateDirectoryIfMissing(string name)
        {
            var directoryInfo = new DirectoryInfo(name);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
        }

        public string FindStepFolder(ApplicationArguments arguments, string stepName)
        {
            return Path.Combine(arguments.Directory,arguments.StartTime.ToString("yyMMdd-HHmm") + "-" + arguments.Site.ToString().ToUpper(), stepName);
        }
    }
}