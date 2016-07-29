namespace ApprenticeshipScraper.CmdLine.Extensions
{
    using System.Diagnostics;

    public static class StopwatchExtensions
    {
        public static string ElapsedWithoutMs(this Stopwatch stopwatch)
        {
            return string.Format("{0:hh\\:mm\\:ss}", stopwatch?.Elapsed);
        }
        public static string ShortElapsed(this Stopwatch stopwatch)
        {
            return string.Format("{0:mm\\:ss\\.fff}", stopwatch?.Elapsed);
        }
    }
}