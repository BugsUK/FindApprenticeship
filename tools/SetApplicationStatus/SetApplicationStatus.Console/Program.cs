namespace SetApplicationStatus.Console
{
    using System;
    using NLog;

    public class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            // TODO: traineeships

            var options = new CommandLineOptions();

            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                Console.WriteLine(options.GetUsage());
                Environment.ExitCode = 1;
                return;
            }

            Logger.Info("Started");

            {
                var patch = new InProgressApplicationPatch(
                    options.Ukprn,
                    options.SqlConnectionString,
                    options.MongoConnectionString,
                    options.Update);

                patch.Run();
            }

            /*
            {
                var patch = new ApplicationStatusPatch(
                    options.Ukprn,
                    options.SqlConnectionString,
                    options.MongoConnectionString,
                    options.Update);

                patch.Run();
            }
            */

            Logger.Info("Finished");
        }
    }
}
