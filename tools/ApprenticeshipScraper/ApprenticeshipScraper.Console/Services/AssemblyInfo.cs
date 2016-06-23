namespace ApprenticeshipScraper.CmdLine.Services
{
    using System;
    using System.IO;
    using System.Reflection;

    public static class AssemblyInfo
    {
        public static string GetExeName()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            return Path.GetFileName(codeBase);
        }

        public static Version GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version;
        }
    }
}