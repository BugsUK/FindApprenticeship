namespace ApprenticeshipScraper.CmdLine.Extensions
{
    using System;

    public static class ExceptionExtensions
    {
        public static string ToNiceString(this Exception ex)
        {
            var message =
                "Exception type " + ex.GetType() + Environment.NewLine +
                "Exception message: " + ex.Message + Environment.NewLine +
                "Stack trace: " + ex.StackTrace + Environment.NewLine;
            if (ex.InnerException != null)
            {
                message += "---BEGIN InnerException--- " + Environment.NewLine +
                           "Exception type " + ex.InnerException.GetType() + Environment.NewLine +
                           "Exception message: " + ex.InnerException.Message + Environment.NewLine +
                           "Stack trace: " + ex.InnerException.StackTrace + Environment.NewLine +
                           "---END Inner Exception";
            }

            return message;
        }
    }
}