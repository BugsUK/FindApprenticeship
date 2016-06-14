namespace ApprenticeshipScraper.CmdLine.Extensions
{
    internal static class ObjectExtensions
    {
        public static string NiceName(this object value)
        {
            return value.GetType().Name.SplitCamelCase();
        }
    }
}