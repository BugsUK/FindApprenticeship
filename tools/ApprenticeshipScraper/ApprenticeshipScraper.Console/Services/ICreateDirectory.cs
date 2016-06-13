namespace ApprenticeshipScraper.CmdLine.Services
{
    public interface ICreateDirectory
    {
        void CreateDirectoryIfMissing(string name);
    }
}