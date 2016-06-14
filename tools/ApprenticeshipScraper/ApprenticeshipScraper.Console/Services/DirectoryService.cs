namespace ApprenticeshipScraper.CmdLine.Services
{
    using System.IO;

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
    }
}