namespace SetApplicationStatus.Console
{
    using CommandLine;
    using CommandLine.Text;

    public class CommandLineOptions
    {
        [Option('m', "mongo", Required = true, HelpText = "MongoDB connection string.")]
        public string MongoConnectionString { get; set; }

        [Option('s', "sql", Required = true, HelpText = "SQL connection string.")]
        public string SqlConnectionString { get; set; }

        [Option('p', "ukprn", Required = true, HelpText = "Provider UKPRN.")]
        public string Ukprn { get; set; }

        [Option('u', "update", DefaultValue = false, HelpText = "Update?")]
        public bool Update { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
